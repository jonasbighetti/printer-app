using System;
using System.Collections.Concurrent;
using System.Threading;

namespace PrinterApp.Classes
{
    public class Printer
    {
        private readonly string _name;
        private readonly ConcurrentQueue<PrintJob> _printJobQueue;
        private static readonly int _millisPerPage = 50;

        public Printer(string name, ConcurrentQueue<PrintJob> printJobQueue)
        {
            _name = name;
            _printJobQueue = printJobQueue;
        }

        public void Start()
        {
            Console.WriteLine($"[{_name}]: Ligando a impressora...");
        }

        public void PrintFile()
        {
            if (_printJobQueue.TryPeek(out PrintJob printingFile))
            {
                Console.WriteLine($"[{_name}]: Imprimindo {printingFile.GetJobName()}...");

                WaitPrintingTime(printingFile);

                if (_printJobQueue.TryDequeue(out PrintJob printedFile))
                    Console.WriteLine($"[{_name}]: {printedFile.GetJobName()} OK.");
            }
        }

        public void WaitNextFile()
        {
            Console.WriteLine($"[{_name}]: Esperando por trabalho de impressão...");
        }

        public void Halt()
        {
            Console.WriteLine($"[{_name}]: Desligando a impressora...");
            Console.WriteLine($"[{_name}]: Impressora desligada.");
        }

        private void WaitPrintingTime(PrintJob printingFile)
        {
            Thread.Sleep(_millisPerPage * printingFile.GetNumberOfPages());
        }
    }
}
