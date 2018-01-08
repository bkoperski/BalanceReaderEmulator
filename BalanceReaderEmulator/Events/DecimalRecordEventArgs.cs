using System;
using System.Linq;

namespace BalanceReaderEmulator {

    namespace Events {

        public class DecimalRecordEventArgs : BalanceReaderEventArgs {

            public DecimalRecordEventArgs(byte recordNumber, bool moreData, byte[] data, byte digitCount, byte decimalPoint, byte delay)
                : this(recordNumber, moreData, data.Length == 4 ? BitConverter.ToUInt32(data.Reverse().ToArray(), 0) : BitConverter.ToUInt64(data.Reverse().ToArray(), 0), digitCount, decimalPoint, delay) { }

            public DecimalRecordEventArgs(byte recordNumber, bool moreData, ulong number, byte digitCount, byte decimalPoint, byte delay) : base(recordNumber, moreData, delay) {

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
    }
}
