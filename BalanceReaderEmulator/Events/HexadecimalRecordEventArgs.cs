using System.Collections.Generic;
using System.Linq;

namespace BalanceReaderEmulator {

    namespace Events {

        public class HexadecimalRecordEventArgs : BalanceReaderEventArgs {

            public HexadecimalRecordEventArgs(byte recordNumber, bool moreData, byte[] data, byte digitCount, byte delay) : base(recordNumber, moreData, delay) {
                Record = new char[10];
                var digits = new List<char>();

                foreach (var number in data.Reverse().ToArray()) {
                    digits.Add(ToHexDigit(number % 16));
                    digits.Add(ToHexDigit(number / 16));
                }

                digitCount = (digitCount == 0) ? (byte)10 : digitCount;
                for (byte i = 0; i < digitCount; i++) {
                    var indexFromEnd = (Record.Length - 1) - i;
                    Record[indexFromEnd] = digits[i];
                }
            }

            private char ToHexDigit(int number) {
                return (number < 10) ? (char)('0' + number) : (char)('A' + number - 10);
            }
        }
    }
}
