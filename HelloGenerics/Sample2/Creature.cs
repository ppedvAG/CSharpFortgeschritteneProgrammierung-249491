namespace HelloGenerics.Sample2
{
    public class Creature : ISleep, IEat
    {
        public string FavoriteFood {  get; set; }

        public void Eat()
        {
            Console.WriteLine("Ich esse am liebsten " + FavoriteFood + ".");
        }

        public void TakeANap(int seconds)
        {
            Console.WriteLine($"Pause von {seconds} Sekunden.");
        }

        public void Exist()
        {
            Console.WriteLine("Ich lebe.");
        }
    }
}
