
namespace ExceptionHandling
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // vor int? war die Schreibweise Nullable<int>
            int? number = null; 

            do
            {
                Console.Write("Bitte Zahl eingeben:\t");

                // Wir sehen mit IntelliSense welche Exceptions Console.ReadLine() werfen kann
                var input = Console.ReadLine();

                try
                {

                    number = ReadNumber(input);
                }
                catch (Exception)
                {
                    Console.WriteLine("Unerwarteter Fehler!");
                }
            } while (!number.HasValue);

            // Block-Scope
            {
                // Variable nur innerhalb dieses Blocks gueltig
                string message = "Die eingegebene Zahl ist ";
                Console.WriteLine($"{message}{number}");
            }

            Console.ReadKey();
        }

        private static int? ReadNumber(string? input)
        {
            try
            {
                var result = int.Parse(input);

                if (result % 13 == 0)
                {
                    throw new UnluckyNumberException(result);
                }

                return result;
            }
            catch (UnluckyNumberException ex) when (ex.Number > 13)
            {
                // Das selbe wie 
                //if (ex.Number > 13)
                //{
                //    throw;
                //}
                Console.WriteLine("Die Zahl ist nicht 13 und trotzdem eine Unglueckszahl!");

                // throw schmeisst die Exception an den naechsten Block weiter statt fangen
                throw;
            }
            catch (UnluckyNumberException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch(ArgumentNullException)
            {
                Console.WriteLine("Keine Zahl eingegeben");
            }
            catch(FormatException)
            {
                Console.WriteLine($"{input} ist keine gueltige Zahl");
            }
            catch(Exception ex) 
            {
                Console.WriteLine("Ein unbekannter Fehler ist aufgetreten.");
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.WriteLine("Wird immer ausgefuehrt");
                // ein using-block ist nichts anderes als ein try-finally
            }

            return null;
        }
    }

    public class UnluckyNumberException : Exception
    {
        public int Number { get; }

        public UnluckyNumberException(int number) : base($"Die Zahl {number} ist eine Unglueckszahl") 
        {
            Number = number;
        }
    }
}
