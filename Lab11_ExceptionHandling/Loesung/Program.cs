using System;

namespace Lab.ExceptionHandling.Loesung
{
    public enum Rechenoperation { Addition = 1, Subtraktion, Multiplikation, Division }

    class Program
    {
        static void Main(string[] args)
        {
            //Variable, welche Wiederholung definiert
            bool wiederholen;
            //Schleife für Wiederholung
            do
            {
                wiederholen = true;

                //Eingabe durch Benutzer
                string eingabe = GetEingabe();

                //Try-Block für Code, welche möglicherweise Fehler verursacht
                try
                {
                    Term term = new Term(eingabe);

                    int ergebnis = BerechneTerm(term);

                    Console.WriteLine($"\t={ergebnis}");

                    wiederholen = false;
                }
                //Catch-Blöcke zur Bearbeitung der Fehler
                catch (OverflowException)
                {
                    Console.WriteLine("Eine deiner Zahlen war zu groß oder zu klein.\n");
                }
                catch (FormatException)
                {
                    Console.WriteLine("Du hast ein nicht-erlaubtes Zeichen verwendet.\n");
                }
                catch (DivideByZeroException)
                {
                    Console.WriteLine("Du hast versucht durch 0 zu teilen.\n");
                }
                catch (NullReferenceException)
                {
                    Console.WriteLine("Du hast ein nicht-erlaubtes Rechenzeichen verwendet.\n");
                }
                catch (Exception)
                {

                }
            } while (wiederholen);
        }

        static string GetEingabe()
        {
            Console.WriteLine("Bitte gib einen Term mit zwei Zahlen und einem Grundrechenoperator (+ - * /) ein (z.B.: 25+13):");
            return Console.ReadLine();
        }

        static int BerechneTerm(Term term)
        {
            switch (term.Operation)
            {
                case Rechenoperation.Addition:
                    return term.Zahl1 + term.Zahl2;
                case Rechenoperation.Subtraktion:
                    return term.Zahl1 - term.Zahl2;
                case Rechenoperation.Multiplikation:
                    return term.Zahl1 * term.Zahl2;
                default:
                    //Teilung kann DivideByZeroException verursachen
                    return term.Zahl1 / term.Zahl2;
            }
        }
    }
}

