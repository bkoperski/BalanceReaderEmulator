using System;

namespace BalanceReaderEmulator {

    namespace Events {

        public class BalanceReaderEventArgs : EventArgs {

            public char[] Record { get; protected set; }
            public byte RecordNumber { get; }
            public bool MoreData { get; }
            public byte Delay { get; }

            public BalanceReaderEventArgs(byte recordNumber, bool moreData, byte delay) {
                RecordNumber = recordNumber;
                MoreData = moreData;
                Delay = delay;
            }

            override public string ToString() {
                return new string(Record);
            }
        }
    }
}
