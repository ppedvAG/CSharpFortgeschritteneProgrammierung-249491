namespace Labs
{
    public class PrimeComponent
    {
        // Der EventHandler ist auch nur ein delegate mit den Argumenten object und EventArgs.
        // Wir koennen statt dem EventHandler Delegate auch den Action Delegate verwenden.
        public event Action<int> Prime;
        public event Action<int> Prime100;
        public event Action<(int current, int divider)> NotPrime;

        public void StartProcess()
        {
            Prime(2);
            for (int i = 3, counter = 0; ; i += 2)
            {
                if (CheckPrime(i))
                {
                    counter++;
                    if (counter % 100 != 0)
                        Prime?.Invoke(i);
                    else
                        Prime100?.Invoke(i);
                }
                Thread.Sleep(50);
            }
        }

        public bool CheckPrime(int num)
        {
            if (num % 2 == 0)
            {
                NotPrime?.Invoke((num, 2));
                return false;
            }

            for (int i = 3; i <= num / 2; i += 2) //Nur bis zu Hälfte gehen, da größere Zahlen nicht teilbar sein können
            {
                if (num % i == 0)
                {
                    NotPrime?.Invoke((num, i));
                    return false;
                }
            }
            return true;
        }
    }
}