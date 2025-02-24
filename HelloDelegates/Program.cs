


namespace HelloDelegates
{
    internal class Program
    {
        // Function Deklaration fuer einen Function Type den wir spaeter benutzen werden
        public delegate void Hello(string name);

        static void Main(string[] args)
        {
            HelloDelegates();

            Console.WriteLine("\nAction Samples");
            ActionSamples();

            Console.ReadKey();
        }

        #region Delegates
        private static void HelloDelegates()
        {
            Hello hello = new Hello(HelloDe);

            // Ausfuehrung des Delegates
            hello("Bob");

            // Mit += koennen wir weitere function pointers hinzufuegen
            hello += HelloDe;
            hello("Alice");

            hello += HelloEn;
            hello += HelloEn;
            hello("Luca");

            hello -= HelloEn;
            hello -= HelloEn;
            hello("Barbara");

            hello -= HelloDe;
            hello -= HelloDe;
            // hello("Hans"); // hello ist null

            // Immer ein Null-Check vor Ausfuehrung des Delegates durchfuehren
            if (hello != null)
            {
                hello("Hans");
            }
            hello?.Invoke("Jochen");
        }

        private static void HelloDe(string name)
        {
            Console.WriteLine($"Hallo, mein Name ist {name}.");
        }

        private static void HelloEn(string name)
        {
            Console.WriteLine($"Hello, my Name is {name}.");
        }
        #endregion

        private static void ActionSamples()
        {
            var printNumber = new Action<int, int>(PrintRandomNumber);

            printNumber(10, 1);

            var addNumbers = new Func<int, int, int>((x, y) => x + y);
            var result = addNumbers(1, 2);
            Console.WriteLine("1 + 2 = " + result);

            bool isEven(int number) => number % 2 == 0;

            var numbers = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var firstEvenNumber = numbers.ToList().Find(isEven);
            Console.WriteLine("First even number: " + firstEvenNumber);
        }

        private static void PrintRandomNumber(int max, int min)
        {
            Console.WriteLine("Random number " + Random.Shared.Next(min, max));
        }
    }
}
