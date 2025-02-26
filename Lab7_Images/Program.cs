namespace Lab_Images;

public class Program
{
	private const int MENU_REFRESH_RATE_IN_MS = 500;
	private const int DEFAULT_WORKER_COUNT = 4;

	private static List<Scanner> Scanners = new();
	private static List<Worker> Workers = new();

	private static bool ProcessRunning = false;

	private static string LastOutput = "Keine";

	private static string OutputPath = "Output";

    static void Main(string[] args)
    {
        bool suspendRefresh = false;

        // Hier wird die Oberflaeche in einem eigenen Thread gezeichnet
		// um den Status laufend aktualisieren zu koennen
        Task.Run(PrintMenu);

        while (true)
        {
            ConsoleKey inputKey = Console.ReadKey(true).Key;

            switch (inputKey)
			{
				case ConsoleKey.D1:
                    WithInputRequest(CreateScanner);
                    break;

                case ConsoleKey.D2:
                    WithInputRequest(AdjustWorkerAmount);
                    break;

                case ConsoleKey.D3:
                    WithInputRequest(AdjustOutputPath);
                    break;

                case ConsoleKey.D4:
                    StartProcess();
                    break;

                case ConsoleKey.D5:
                    PauseProcess();
                    break;

				default:
                    break;
            }

            Console.Clear();
        }
		
		// local function
        void WithInputRequest(Action a)
        {
            suspendRefresh = true;
            a.Invoke();
            suspendRefresh = false;
        }

        void PrintMenu()
        {
            while (true)
            {
				if (suspendRefresh)
					continue;

                Console.SetCursorPosition(0, 0);
                PrintUserInputs();
                PrintStatus();
                Thread.Sleep(MENU_REFRESH_RATE_IN_MS);
            }
        }
    }

    #region Print Methoden
    private static void PrintUserInputs()
	{
		Console.WriteLine("Eingaben: ");
		Console.WriteLine("1: Neuen Scanner erstellen");
		Console.WriteLine("2: Anzahl Worker Tasks anpassen");
		Console.WriteLine("3: Speicherpfad anpassen");
		Console.WriteLine("4: Prozess starten/fortsetzen");
		Console.WriteLine("5: Prozess pausieren");
	}

	private static void PrintStatus()
	{
		if (Scanners.Count != 0)
		{
			Console.WriteLine("\nScanner Liste: ");
			for (int i = 0; i < Scanners.Count; i++)
				Console.WriteLine($"{i}: {Scanners[i].ScanPath}");
		}

		if (Workers.Count != 0)
		{
			Console.WriteLine("\nWorker Liste: ");
			for (int i = 0; i < Workers.Count; i++)
				Console.WriteLine($"{i}: {Workers[i].CurrentPath}");
		}

		Console.WriteLine($"\nSpeicherpfad: {Path.GetFullPath(OutputPath)}");
		Console.WriteLine($"Letzte Meldung: {LastOutput}");
	}
    #endregion

    #region Input Methoden
    private static void CreateScanner()
    {
        Console.Write("Gib einen Pfad zum Scannen ein: ");
        string input = Console.ReadLine();

        var path = Path.Combine(Environment.CurrentDirectory, string.IsNullOrWhiteSpace(input) ? "images" : input);

        if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
            LastOutput = $"Pfad {path} wurde erstellt";
        }

        Scanners.Add(new Scanner(path));
    }

    private static void AdjustWorkerAmount()
	{
		Console.Write($"Gib eine neue Anzahl von Worker-Tasks ein (derzeit {Workers.Count}): ");
		string workerEingabe = Console.ReadLine();
		if (int.TryParse(workerEingabe, out int newWorkers))
		{
			if (newWorkers > 0 && newWorkers != Workers.Count)
			{
				PauseList(Workers);

				int newAmount = newWorkers - Workers.Count;
				for (int i = 0; i < newAmount; i++)
					if (Workers.Count < newWorkers)
						Workers.Add(new Worker(OutputPath, ProcessRunning));

				if (newAmount < 0)
					Workers.RemoveRange(0, Workers.Count - newWorkers);
			}
			else
				LastOutput = "Ungültige Eingabe";
		}
	}

	private static void AdjustOutputPath()
	{
		Console.Write("Gib einen neuen Speicherpfad ein: ");
		string input = Console.ReadLine();

        var path = Path.Combine(Environment.CurrentDirectory, string.IsNullOrWhiteSpace(input) ? "output" : input);

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
			LastOutput = $"Pfad {path} wurde erstellt";
        }

		OutputPath = path;

    }

    private static void StartProcess()
	{
		if (ProcessRunning)
		{
			LastOutput = "Prozess läuft bereits";
			return;
		}

		if (Scanners.Count == 0)
		{
			LastOutput = "Keine Scanner erstellt";
			return;
		}

		Scanners.ForEach(e => e.Continue = true);

		if (Workers.Count == 0)
		{
			for (int i = 0; i < DEFAULT_WORKER_COUNT; i++)
				Workers.Add(new Worker(OutputPath, true));
			LastOutput = "Prozess gestartet mit 4 Worker-Tasks";
		}
		else
		{
			Workers.ForEach(e => e.Continue = true);
			LastOutput = "Prozess gestartet";
		}

		if (!Directory.Exists(OutputPath))
			Directory.CreateDirectory(OutputPath);

		ProcessRunning = true;
	}

	private static void PauseProcess()
	{
		if (!ProcessRunning)
		{
			LastOutput = "Prozess läuft nicht";
			return;
		}

		PauseList(Scanners);
		PauseList(Workers);
		ProcessRunning = false;
		LastOutput = "Prozess pausiert";
	}
	#endregion

	private static void PauseList<T>(List<T> runnables) where T : Runnable
	{
		Console.WriteLine($"Warte auf das Beenden aller {typeof(T).Name}");
		foreach (Runnable r in runnables)
			r.Continue = false;

		while (runnables.Any(e => e.CurrentTask.Status == TaskStatus.Running))
			continue;
    }
}