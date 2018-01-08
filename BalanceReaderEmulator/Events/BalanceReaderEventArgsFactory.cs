using PCSC.Iso7816;
using System;
using System.Linq;

namespace BalanceReaderEmulator {

    namespace Events {

        public class BalanceReaderEventArgsFactory {

            public BalanceReaderEventArgs create(ResponseApdu responseApdu) {
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

                switch (dataFormat) {
                    case 1:
                        return new AlphanumericRecordEventArgs(recordNumber, moreData, data, digitCount, delay);
                    case 2:
                        return new HexadecimalRecordEventArgs(recordNumber, moreData, data, digitCount, delay);
                    case 4:
                        return new DecimalRecordEventArgs(recordNumber, moreData, data, digitCount, decimalPoint, delay);
                    case 8:
                        return new SignedDecimalRecordEventArgs(recordNumber, moreData, data, digitCount, decimalPoint, delay);
                    default:
                        return null;
                }
            }
        }
    }
}
