namespace Labs
{
    public class PrimeMain
    {
        public static void Run(string[] args)
        {
            PrimeComponent pc = new PrimeComponent();
            pc.Prime += (i) => Console.WriteLine($"Primzahl: {i}");
            pc.Prime100 += (i) => Console.WriteLine($"Hundertste Primzahl: {i}");
            pc.NotPrime += (tuple) => Console.WriteLine($"Keine Primzahl: {tuple.current}, teilbar durch {tuple.divider}");
            pc.StartProcess();
        }
    }
}