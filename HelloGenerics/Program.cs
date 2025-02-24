using HelloGenerics.Sample1;
using HelloGenerics.Sample2;
using System.Collections;

namespace HelloGenerics
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Euro Symbol usw. darstellbar machen
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            StackSample();

            Console.WriteLine("\n\nCreature Samples\n============");
            CreatureSample();

            _ = Console.ReadLine();
        }

        private static void StackSample()
        {
            var simpleStack = new Sample1.Stack();

            simpleStack.Push(1);
            simpleStack.Push(2);
            simpleStack.Push(3);

            object item = simpleStack.Pop();
            Console.WriteLine("Item from stack: " + item);

            // Wenn wir mit dem int rechnen wollten, muessen wir es als int casten
            // Moegliches Problem: InvalidCastException kann zur LAUFZEIT auftreten
            item = (int)simpleStack.Pop() + 2;
            Console.WriteLine("Item from stack + 2: " + item);

            // Prinzip Fail-Fast: Fehler sollten sehr schnell sichtbar werden, d.h. am besten zur Compilezeit
            // Mit Generics kann keine InvalidCastException auftreten, weil quasi der Compiler das Problem abfaengt.
            // Wir koennen uns das so vorstellen, dass der Compiler intern ein StackOfIntegers, StackOfStrings, StackOfDateTimes etc. erstellt
            var dateTimeStack = new GenericStack<DateTime>();
            dateTimeStack.Push(DateTime.Now);

            var numberStack = new GenericStack<int>();
            numberStack.Push(1);
            numberStack.Push(2);
            numberStack.Push(3);

            int number = numberStack.Pop();
            var calculates = number + 2;
            Console.WriteLine("Item from dymanic stack + 2: " + item);
        }

        private static void CreatureSample()
        {
            var bunny = new Creature() { FavoriteFood = "🥕🥕🥕" };

            // Wir wollen mit Abstraktionen (d. h. Interfaces) arbeiten weil uns die konkrete
            // Implementierung nicht interessiert, also ob wir eine Creature oder Person haben
            EatSomething(bunny);

            // Hier verwenden wir das "Super-Interface" IPerson, welche mehrere Interfaces kombiniert
            var person = new Person() { FavoriteFood = "🍔🍔🍔" };
            DoWork(person);

            Console.WriteLine("\nContraints verwenden");
            // Um mehrere Interfaces verwenden zu koennen ohne ein "Super-Interface" definieren zu muessen
            // koennen wir sog. Constraints verwenden
            DoDailyStuff(bunny); // Soll schlafen und essen

            Console.WriteLine("\nContraints verwenden und Klassen dynamisch erzeugen");
            var duffyDuck = CreateCreature<Creature>("🍕🍔🍦");
            duffyDuck.Exist();
            EatSomething(duffyDuck);
        }

        private static void DoWork(IPerson person)
        {
            person.Eat();
            person.TakeANap(5);
            person.DoWork<int, string>(42);
        }

        private static void DoDailyStuff<T>(T creature) where T : IEat, ISleep
        {
            creature.Eat();
            creature.TakeANap(5);
        }

        private static void EatSomething(IEat eater)
        {
            eater.Eat();
        }

        // Das Constraint class schraenkt T auf Reference-Types ein, d. h. nur Klassen und keine Primitiven wie int, bool etc.
        // Mit new() muss die class ein __parameterlosen__ Defaultkonstruktor enthalten um erstellt werden zu koennen
        private static T CreateCreature<T>(string favoriteFood) where T : class, IEat, new()
        {
            var result = new T();
            result.FavoriteFood = favoriteFood;

            return result;
        }
    }
}
