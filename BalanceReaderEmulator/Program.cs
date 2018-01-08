using System;

namespace BalanceReaderEmulator {

    class Program {

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
            Console.Write('\r' + eventArgs.ToString());
        }
    }
}
