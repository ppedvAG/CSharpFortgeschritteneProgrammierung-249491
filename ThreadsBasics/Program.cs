

namespace ThreadsBasics
{
    internal class Program
    {
        const int COUNT_TO = 10;

        static void Main(string[] args)
        {
            //ThreadsStartAndWait();

            //ThreadsWithParameters();

            //ThreadWithCancellationToken();

            ThreadsBackground();

            Console.ReadKey();
        }

        private static void ThreadsStartAndWait()
        {
            var thread1 = new Thread(RunThread);
            var thread2 = new Thread(RunThread);
            var thread3 = new Thread(RunThread);

            thread1.Start();
            thread2.Start();
            thread3.Start();

            // Threads werden parallel ausgeführt

            // Wir koennen auf Threads warten
            thread1.Join();
            thread2.Join();
            thread3.Join();

            // Erst wenn alle drei Threads fertig sind, dann wird der MainUI Thread weiter ausgeführt

            for (int i = 0; i < COUNT_TO; i++)
            {
                Console.WriteLine($"MainUI thread #{i,2} with ID {Thread.CurrentThread.ManagedThreadId}");
            }

            void RunThread()
            {
                for (int i = 0; i < COUNT_TO; i++)
                {
                    Console.WriteLine($"Worker thread #{i,2} with ID {Thread.CurrentThread.ManagedThreadId}");
                }
            }
        }

        private static void ThreadsWithParameters()
        {
            var thread1 = new Thread(RunThreadWithParameter);
            thread1.Start(13);

            object result = null;
            var thread2 = new Thread(RunThreadWithParameter);
            thread2.Start((object r) => result = r);

            thread2.Join();

            Console.WriteLine("Result of thread 2: " + result);


            void RunThreadWithParameter(object? param)
            {
                if (param is int n)
                {
                    for (int i = 0; i < n; i++)
                    {
                        Console.WriteLine($"Worker thread #{i,2} with ID {Thread.CurrentThread.ManagedThreadId}");
                    }
                }
                else if (param is Delegate cb)
                {
                    Thread.Sleep(1000);

                    // Wir benutzen DynamicInvoke da wir die Methoden-Signatur an dieser Stelle nicht kennen
                    cb.DynamicInvoke(42);
                }
            }
        }

        private static void ThreadWithCancellationToken()
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token; // Die Source kann beliebig viele Tokens erzeugen
            var thread = new Thread(RunThreadWithCancellationToken);

            try
            {
                thread.Start(token);

                Thread.Sleep(20);

                cts.Cancel(); // Auf der Source das Cancel Signal senden
            }
            catch (OperationCanceledException)
            {
                // Exception kann nicht hier oben gefangen werden
            }

            void RunThreadWithCancellationToken(object? obj)
            {
                try
                {
                    if (obj is CancellationToken token)
                    {
                        for (int i = 0; i < COUNT_TO; i++)
                        {
                            Console.WriteLine($"Worker thread #{i,2} with ID {Thread.CurrentThread.ManagedThreadId}");
                            Thread.Sleep(2);

                            if (token.IsCancellationRequested)
                            {
                                Console.WriteLine("Cancellation Requested");

                                token.ThrowIfCancellationRequested();
                            }
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Exception im Thread gefangen");
                }
            }
        }

        private static void ThreadsBackground()
        {
            ThreadPool.QueueUserWorkItem(RunThreadWithCallback);
            ThreadPool.QueueUserWorkItem(RunThreadWithCallback);
            ThreadPool.QueueUserWorkItem(RunThreadWithCallback);

            var thread = new Thread(RunThread);
            thread.Start(); // Vordergrundthread

            // Alle Items im Thread Pool werden abgebrochen wenn alle Vordergrundthreads fertig sind

            Thread.Sleep(1000);

            void RunThreadWithCallback(object? obj)
            {
                if (obj is Action<int> callback)
                {
                    for (int i = 0; i < COUNT_TO; i++)
                    {
                        callback(i);
                    }
                }
            }

            void RunThread(object? obj)
            {
                for (int i = 0; i < COUNT_TO; i++)
                {
                    Console.WriteLine($"Worker thread #{i,2} with ID {Thread.CurrentThread.ManagedThreadId}");
                }
            }
        }
    }
}
