using System;
using System.Linq;

namespace BalanceReaderEmulator {

    namespace Events {

        public class SignedDecimalRecordEventArgs : DecimalRecordEventArgs {

            public SignedDecimalRecordEventArgs(byte recordNumber, bool moreData, byte[] data, byte digitCount, byte decimalPoint, byte delay)
                : this(recordNumber, moreData, data.Length == 4 ? BitConverter.ToInt32(data.Reverse().ToArray(), 0) : BitConverter.ToInt64(data.Reverse().ToArray(), 0), digitCount, decimalPoint, delay) { }

            public SignedDecimalRecordEventArgs(byte recordNumber, bool moreData, long number, byte digitCount, byte decimalPoint, byte delay)
                : base(recordNumber, moreData, (ulong)Math.Abs(number), digitCount, decimalPoint, delay) {

                Record[0] = number >= 0 ? '+' : '-';
            }
        }
    }
}
