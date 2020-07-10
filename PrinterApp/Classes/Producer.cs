using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace PrinterApp.Classes
{
    public class Producer
    {
        private readonly string _name;
        private readonly ConcurrentQueue<PrintJob> _printJobQueue;

        public Producer(string name, ConcurrentQueue<PrintJob> printJobQueue)
        {
            _name = name;
            _printJobQueue = printJobQueue;
        }

        public void ProduceFiles(Dictionary<string, int> files, ManualResetEvent waitHandle)
        {
            foreach (var file in files)
            {
                Console.WriteLine($"#{_name}#: produzindo arquivo '{file.Key}', número de páginas {file.Value}.");
                _printJobQueue.Enqueue(new PrintJob(file.Key, file.Value));
                waitHandle.Set();
                WaitProducingFile();
            }
        }

        private void WaitProducingFile()
        {
            Thread.Sleep(new Random().Next(1, 5) * 1000);
        }
    }
}
