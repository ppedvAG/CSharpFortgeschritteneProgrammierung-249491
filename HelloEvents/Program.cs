namespace HelloEvents
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var course = new Course();
            course.CourseStarted += (s, e) => Console.WriteLine("Course started");
            course.CourseCompleted += (s, e) =>
            {
                Console.WriteLine(e.contents);
                Console.WriteLine("Course completed");
            };

            Console.WriteLine("Press any key to start your course");
            _ = Console.ReadKey();

            course.Start();

            _ = Console.ReadKey();
        }
    }

    public class Course
    {
        public record CourseCompletedEventArgs(string contents);

        public event EventHandler CourseStarted;

        public event EventHandler<CourseCompletedEventArgs> CourseCompleted;

        public void Start()
        {
            // Wenn wir ein Event aufrufen uebergeben wir in der Regen den "sender"
            // welches der this Kontext dieser Klasse ist
            CourseStarted?.Invoke(this, EventArgs.Empty);

            // 2 Sekunden warten
            Thread.Sleep(2000);

            var args = new CourseCompletedEventArgs("How to use custom events in C#.");
            CourseCompleted?.Invoke(this, args);
        }
    }
}
