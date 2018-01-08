using PCSC;
using PCSC.Iso7816;
using System;

using BalanceReaderEmulator.Events;
using System.Threading;

namespace BalanceReaderEmulator {
    class BalanceReader : IsoReader {
        
        private byte currentRecord;
        private BalanceReaderEventArgsFactory eventFactory;
        public event EventHandler NewRecordEventHandlers;

        public BalanceReader(ISCardContext context, String readerName) : base(context, readerName, SCardShareMode.Exclusive, SCardProtocol.Any, false) {
            currentRecord = 0;
            eventFactory = new BalanceReaderEventArgsFactory();
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
                        var eventArgs = eventFactory.create(responseApdu);
                        NewRecordEventHandlers(this, eventArgs);
                        moreData = eventArgs.MoreData;

                        if (eventArgs.Delay == 0) {
                            return;
                        } else {
                            Thread.Sleep(eventArgs.Delay * 200);
                        }
                    }
                }
                currentRecord++;
            }
        }
    }
}
