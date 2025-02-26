namespace Lab_Konto
{
    public class KontoWithMutex : IKonto
    {
        public string Type { get; init; }
        public int Balance { get; set; } = 0;
        public int TransactionCount { get; set; } = 0;

        private readonly Mutex _mutex = new Mutex();

        public int ThreadId => Environment.CurrentManagedThreadId;

        public void Deposite(int value)
        {
            _mutex.WaitOne();
            Balance += value;
            TransactionCount++;
            Console.WriteLine($"{Type}[{ThreadId}] #{TransactionCount,2}:\t{Balance:C}");
            _mutex.ReleaseMutex();
        }

        public void Withdraw(int value)
        {
            Deposite(-value);
        }
    }
}