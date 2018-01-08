using PCSC;
using PCSC.Iso7816;

namespace BalanceReaderEmulator {
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
}
