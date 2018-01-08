using PCSC;
using System;

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
}
