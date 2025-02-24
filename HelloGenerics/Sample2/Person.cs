namespace HelloGenerics.Sample2
{
    public class Person : Creature, IPerson
    {
        public TResult DoWork<T, TResult>(T value)
        {
            var result = default(TResult);
            Console.WriteLine($"Ich arbeite mit {value} und habe das gemacht: {result}.");
            return result;
        }
    }
}
