namespace HelloGenerics.Sample2
{
    public interface IPerson : ISleep, IEat, IWorker
    {

    }

    public interface ISleep
    {
        void TakeANap(int seconds);
    }

    public interface IEat
    {
        string FavoriteFood { get; set; }

        void Eat();
    }

    public interface IWorker
    {
        TResult DoWork<T, TResult>(T value);
    }
}
