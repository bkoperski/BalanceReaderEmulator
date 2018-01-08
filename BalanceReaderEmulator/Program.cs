using PCSC;
using PCSC.Iso7816;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace BalanceReaderEmulator {
    class BalanceReaderFactory : IDisposable {
        private ISCardContext _context = null;

        private ISCardContext Context {
            get {
                if (_context == null) {
                    _context = ContextFactory.Instance.Establish(SCardScope.System);
                }

                return _context;
            }
        }

        public string[] AvailableReaders {
            get {
                return Context.GetReaders() ?? new string[0];
            }
        }

        public BalanceReader CreateBalanceReader(String readerName) {
            return new BalanceReader(Context, readerName);
        }

        public void Dispose() {
            Context.Dispose();
        }
    }

    class ReadRecordAPDU : CommandApdu {

        private const byte C = 0x43;
        private const byte h = 0x68;

        public ReadRecordAPDU(byte recordNumber, SCardProtocol protocol) : base(IsoCase.Case4Short, protocol) {
            CLA = 0xC8;
            INS = 0x00;
            P1 = 0x00;
            P2 = 0x00;
            Data = new byte[] {
                recordNumber,
                31, // DataFormat
                10,   // DigitCount
                0,    // Reserved
                1,    // DeviceId
                0,    // DeviceVersion
                C, C, C, C, h, h, h, h, h, h // FormatString
            };
            Le = 0x00;
        }
    }

    public class BalanceReaderEventArgs : EventArgs {

        public char[] Record { get; protected set; }
        public byte RecordNumber { get; }
        public bool MoreData { get; }

        public BalanceReaderEventArgs(byte recordNumber, bool moreData) {
            RecordNumber = recordNumber;
            MoreData = moreData;
        }

        public string ToString() {
            return new string(Record);
        }

        public static BalanceReaderEventArgs create(ResponseApdu responseApdu) {
            var responseData = responseApdu.GetData();

            if (responseData.Length < 6) {
                return null;
            }

            byte recordNumber = responseData[0];
            byte dataFormat = responseData[1];
            byte digitCount = responseData[2];
            byte decimalPoint = responseData[3];
            byte delay = responseData[4];
            bool moreData = responseData[5] == 1;

            byte[] data;
            if (responseData.Length > 6) {
                data = responseData.Skip(6).ToArray();
            } else {
                data = new byte[0];
            }

            Console.WriteLine("-------------------");
            Console.WriteLine("Record number = {0}", recordNumber);
            Console.WriteLine("Data Format = {0}", dataFormat);
            Console.WriteLine("Digit Count = {0}", digitCount);
            Console.WriteLine("Decimal Point = {0}", decimalPoint);
            Console.WriteLine("Delay = {0}", delay);
            BalanceReader.PrintArray(data);

            switch (dataFormat) {
                case 1:
                    return new AlphanumericRecordEventArgs(recordNumber, moreData, data, digitCount);
                case 2:
                    return new HexadecimalRecordEventArgs(recordNumber, moreData);
                case 4:
                    return new DecimalRecordEventArgs(recordNumber, moreData, data, digitCount, decimalPoint);
                case 8:
                    return new SignedDecimalRecordEventArgs(recordNumber, moreData, data, digitCount, decimalPoint);
                default:
                    return null;
            }
        }
    }

    public class AlphanumericRecordEventArgs : BalanceReaderEventArgs {

        public AlphanumericRecordEventArgs(byte recordNumber, bool moreData, byte[] data, byte digitCount) 
            : base(recordNumber, moreData) {
            if (digitCount > 0) {
                data = data.Take(digitCount).ToArray();
            }
            Record = new string(data.Select(c => (char)c).ToArray())
                .Replace('\u00A4', '€')
                .Replace('\u0080', '€')
                .Replace('\u00D5', '€')
                .Replace('\u0024', '$')
                .Replace('\u009C', '\u00A3') // Pound character
                .Replace('\u009D', '\u00A5') // Yen
                .Replace('\u00B0', '\u20A3') // French Franc
                .Replace('\u00B1', '\u20A4') // Lira
                .ToCharArray();
        }
    }

    public class HexadecimalRecordEventArgs : BalanceReaderEventArgs {

        public HexadecimalRecordEventArgs(byte recordNumber, bool moreData) : base(recordNumber, moreData) {
        }
    }

    public class DecimalRecordEventArgs : BalanceReaderEventArgs {

        public DecimalRecordEventArgs(byte recordNumber, bool moreData, byte[] data, byte digitCount, byte decimalPoint) : base(recordNumber, moreData) {
            var number = data.Length == 4 ? BitConverter.ToUInt32(data.Reverse().ToArray(), 0) : BitConverter.ToUInt64(data.Reverse().ToArray(), 0);
            int recordLength = decimalPoint == 0 ? 10 : 11;
            var record = new char[recordLength];

            int digitsPrinted = 0;
            for (int i = recordLength - 1; i >= 0; i--) {
                int charIndexFromRight = record.Count() - i;
                if (decimalPoint != 0 && charIndexFromRight == decimalPoint) {
                    record[i] = '.';
                } else if ((digitCount == 0 && number != 0) || digitsPrinted < digitCount) {
                    record[i] = (char)('0' + number % 10);
                    number /= 10;
                    digitsPrinted++;
                } else {
                    record[i] = ' ';
                }
            }

            Record = record;
        }
    }

    public class SignedDecimalRecordEventArgs : BalanceReaderEventArgs {

        public SignedDecimalRecordEventArgs(byte recordNumber, bool moreData, byte[] data, byte digitCount, byte decimalPoint) : base(recordNumber, moreData) {
            var number = data.Length == 4 ? BitConverter.ToInt32(data.Reverse().ToArray(), 0) : BitConverter.ToInt64(data.Reverse().ToArray(), 0);
            int recordLength = decimalPoint == 0 ? 10 : 11;
            var record = new char[recordLength];
            record[0] = number >= 0 ? '+' : '-';

            int digitsPrinted = 0;
            ulong absoluteNumber = (ulong)(number < 0 ? -number : number);
            for (int i = recordLength - 1; i > 0; i--) {
                int charIndexFromRight = record.Count() - i;
                if (decimalPoint != 0 && charIndexFromRight == decimalPoint) {
                    record[i] = '.';
                } else if ((digitCount == 0 && absoluteNumber != 0) || digitsPrinted < digitCount) {
                    record[i] = (char)('0' + absoluteNumber % 10);
                    absoluteNumber /= 10;
                    digitsPrinted++;
                } else {
                    record[i] = ' ';
                }
            }

            Record = record;
        }
    }

    class BalanceReader : IsoReader {
        
        private byte currentRecord;
        public event EventHandler NewRecordEventHandlers;

        public BalanceReader(ISCardContext context, String readerName) : base(context, readerName, SCardShareMode.Exclusive, SCardProtocol.Any, false) {
            currentRecord = 0;
        }

        public void Run() {
            bool moreData = true;
            while (moreData) {
                moreData = false;
                var apdu = new ReadRecordAPDU(currentRecord, ActiveProtocol);
                var response = Transmit(apdu);
                if (response.Count > 0) {
                    var responseApdu = response[0];
                    if (responseApdu.SW1 == 0x90 || responseApdu.SW1 == 0x61) {
                        var eventArgs = BalanceReaderEventArgs.create(responseApdu);
                        NewRecordEventHandlers(this, eventArgs);
                        moreData = eventArgs.MoreData;
                    }
                }
                currentRecord++;
            }
        }

        public static void PrintArray(byte[] bytes) {
            Console.Write("[");
            foreach (var b in bytes) {
                Console.Write("0x{0:X} (dec: {0}), ", b);
            }
            Console.WriteLine("]");
        }

        static void Main(string[] args) {
            using (var balanceReaderFactory = new BalanceReaderFactory()) {
                var readers = balanceReaderFactory.AvailableReaders;

                Console.WriteLine("Available readers :");
                foreach (var readerName in balanceReaderFactory.AvailableReaders) {
                    Console.WriteLine("\t - " + readerName);
                }
               
                if (readers.Length > 0) {
                    using (var balanceReader = balanceReaderFactory.CreateBalanceReader(readers[0])) {
                        balanceReader.NewRecordEventHandlers += OnNewRecord;
                        balanceReader.Run();
                    }
                }

                Console.ReadKey();
            }
        }

        static void OnNewRecord(object sender, EventArgs eventArgs) {
            //if (eventArgs is AlphanumericRecordEventArgs) {
                Console.WriteLine(eventArgs.ToString());
            //}
        }
    }
}
