
namespace TPL_Multitasking
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //CreateTaskSamples();

            //CreateTaskWaitSample();

            //CreateTaskCancellationToken();

            //HandleExceptionsWithinTasks();

            //TaskContinuationSample();

            ParallelSample();

            Console.ReadKey();
        }

        #region Tasks erstellen
        private static void CreateTaskSamples()
        {
            var task = new Task(ShowRandomNumber);
            task.Start();

            // ab .Net 4.0
            Task.Factory.StartNew(ShowRandomNumber);

            // ab .Net 4.5
            Task.Run(ShowRandomNumber);

            var taskWithArg = new Task(ShowRandomNumber, 42);
            taskWithArg.Start();

            var taskWithResult = new Task<int>(() => CreateRandomNumber(100));
            taskWithResult.Start();

            if (taskWithResult.IsCompleted)
            {
                Console.WriteLine($"Created random number {taskWithResult.Result} from thread {Thread.CurrentThread.ManagedThreadId}");
            }
        }

        private static void ShowRandomNumber() => ShowRandomNumber(100);

        private static void ShowRandomNumber(object max)
        {
            int number = CreateRandomNumber(max);
            Console.WriteLine($"Random number is {number} from thread {Thread.CurrentThread.ManagedThreadId}");
        }

        private static int CreateRandomNumber(object max)
        {
            Thread.Sleep(1000);

            int number = Random.Shared.Next(1, (int)max);
            return number;
        }
        #endregion

        #region Task Wait All/Any
        private static void CreateTaskWaitSample()
        {
            static void WaitOneSecond()
            {
                Thread.Sleep(1000);
                Console.WriteLine($"Waited one second from thread {Thread.CurrentThread.ManagedThreadId}");
            }

            var task = new Task(WaitOneSecond);
            task.Start();

            // Warte hier auf den Task und blockiere den Main Thread
            task.Wait();

            // Auf mehrere Tasks warten
            IEnumerable<Task> someTasks = CreateTasks((_) => WaitOneSecond(), 10);

            // Warten bis alle Tasks abgearbeitet wurden
            Task.WaitAll(someTasks.ToArray());

            // Warten bis mindestens ein Task abgearbeitet wurde
            Task.WaitAny(someTasks.ToArray());
        }

        private static IEnumerable<Task> CreateTasks(Action<object?> action, int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return Task.Factory.StartNew(action, i);
            }
        }
        #endregion

        #region Task Cancellation
        private static void CreateTaskCancellationToken()
        {
            var cts = new CancellationTokenSource();

            var task = new Task(RunTaskWithCancellationToken, cts.Token);

            task.Start();
            Thread.Sleep(500);

            cts.Cancel(); // Auf der Source das Cancel Signal senden
        }

        private static void RunTaskWithCancellationToken(object arg)
        {
            if (arg is CancellationToken token)
            {
                for (int i = 0; i < 20; i++)
                {
                    try
                    {
                        token.ThrowIfCancellationRequested();
                        Console.WriteLine($"Running task {i} with id {Thread.CurrentThread.ManagedThreadId}");
                        Thread.Sleep(100);
                    }
                    catch (OperationCanceledException)
                    {
                        Console.WriteLine($"Task {i} was canceled with id {Thread.CurrentThread.ManagedThreadId}");
                        break;
                    }
                }
            }
        }
        #endregion

        #region Task AggregateException
        private static void HandleExceptionsWithinTasks()
        {
            try
            {
                var tasks = CreateTasks((i) =>
                {
                    Thread.Sleep(200);
                    throw new NotImplementedException($"Not implemented #{i} from thread {Thread.CurrentThread.ManagedThreadId}");
                }, 3);

                Task.WaitAll(tasks.ToArray());
            }
            catch (AggregateException aggregates)
            {
                foreach (Exception ex in aggregates.InnerExceptions)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        #endregion

        #region Tasks verketten
        private static void TaskContinuationSample()
        {
            var task = new Task(() =>
            {
                Console.WriteLine($"Task 1 started with id {Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(1000);
                Console.WriteLine($"Task 1 finished with id {Thread.CurrentThread.ManagedThreadId}");
            });

            task.ContinueWith(t => Console.WriteLine("Always"));
            task.ContinueWith(t => Console.WriteLine("Task okay"), TaskContinuationOptions.NotOnFaulted);
            task.Start();

            var taskWithError = new Task(() =>
            {
                throw new InvalidProgramException("Execution failed");
            });

            taskWithError.ContinueWith(t =>
            {
                Console.WriteLine("Task error: " + t.Exception.InnerException.Message);
            }, TaskContinuationOptions.OnlyOnFaulted);

            taskWithError.Start();

            var taskWithResult = new Task<IEnumerable<int>>(() =>
            {
                Console.WriteLine("Task 2 started");
                Thread.Sleep(1000);
                Console.WriteLine("Task 2 finished");
                return [5, 9, 32];
            });

            taskWithResult.ContinueWith(t =>
            {
                Console.WriteLine($"Previous Task 2 result: {string.Join(", ", t.Result)}");
            }, TaskContinuationOptions.OnlyOnRanToCompletion);
            
            taskWithResult.Start();
        }
        #endregion

        #region Parallel
        private static void ParallelSample()
        {
            // Wird parallel ausgefuehrt in beliebiger Reihenfolge
            //Parallel.For(0, 100_000, i =>
            //{
            //    Console.WriteLine($"Processing {i} from thread {Thread.CurrentThread.ManagedThreadId}");
            //});

            Parallel.Invoke(() => Count(), Count, Count, Count, () => Console.Beep());
            
            void Count()
            {
                for ( int i = 0; i < 100; i++)
                {
                    Console.WriteLine($"Processing {i} from thread {Thread.CurrentThread.ManagedThreadId}");
                }
            }
        }

        #endregion
    }
}
