using System.Linq;

namespace BalanceReaderEmulator {

    namespace Events {

        public class AlphanumericRecordEventArgs : BalanceReaderEventArgs {

            public AlphanumericRecordEventArgs(byte recordNumber, bool moreData, byte[] data, byte digitCount, byte delay)
                : base(recordNumber, moreData, delay) {
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
    }
}
