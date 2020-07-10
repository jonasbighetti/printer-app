using PrinterApp.Classes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PrinterApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var filesForProducer1 = GetFilesForProducer1();
                var filesForProducer2 = GetFilesForProducer2();

                var printJobQueue = new ConcurrentQueue<PrintJob>();
                var printer = new Printer("Printer", printJobQueue);
                var producer1 = new Producer("Producer 1", printJobQueue);
                var producer2 = new Producer("Producer 2", printJobQueue);
                var waitHandle = new ManualResetEvent(false);

                printer.Start();

                var producer1Task = Task.Factory.StartNew(() =>
                {
                    producer1.ProduceFiles(filesForProducer1, waitHandle);
                });

                var producer2Task = Task.Factory.StartNew(() =>
                {
                    producer2.ProduceFiles(filesForProducer2, waitHandle);
                });

                Task.Run(() =>
                {
                    while (true)
                    {
                        if (printJobQueue.IsEmpty)
                        {
                            if (producer1Task.IsCompleted && producer2Task.IsCompleted)
                            {
                                printer.Halt();
                                waitHandle.Close();
                                break;
                            }

                            printer.WaitNextFile();

                            waitHandle.Reset();
                            if (waitHandle.WaitOne())
                                continue;
                        }
                        else
                        {
                            printer.PrintFile();
                        }
                    }
                });

                Console.ReadKey();
            }
            catch (Exception)
            {
                Console.WriteLine("Ocorreu um erro no servidor de impressão.");
            }
        }

        private static Dictionary<string, int> GetFilesForProducer1() =>
            new Dictionary<string, int>() {
                {"arquivo_1.pdf", 21},
                {"arquivo_2.pdf", 45},
                {"arquivo_3.pdf", 15},
                {"arquivo_4.pdf", 71},
                {"arquivo_5.pdf", 14},
                {"arquivo_6.pdf", 88},
                {"arquivo_7.pdf", 9},
                {"arquivo_8.pdf", 78},
                {"arquivo_9.pdf", 3},
                {"arquivo_10.pdf", 33}
            };

        private static Dictionary<string, int> GetFilesForProducer2() =>
            new Dictionary<string, int>() {
                {"arquivo_11.pdf", 30},
                {"arquivo_12.pdf", 12},
                {"arquivo_13.pdf", 44},
                {"arquivo_14.pdf", 35},
                {"arquivo_15.pdf", 102},
                {"arquivo_16.pdf", 10},
                {"arquivo_17.pdf", 18},
                {"arquivo_18.pdf", 22},
                {"arquivo_19.pdf", 65},
                {"arquivo_20.pdf", 71}
            };
    }
}
