namespace Lab.ExceptionHandling.Loesung
{
    class Term
    {
        public string Eingabe { get; set; }
        public int Zahl1 { get; set; }
        public int Zahl2 { get; set; }
        public Rechenoperation Operation { get; set; }

        public Term(string term)
        {
            Eingabe = term;
            Operation = GetRechenoperation();

            //SplitTerm kann Null zurückgeben
            string[] zahlen = SplitTerm();

            //Parsing kann FormatExceptions und OverflowExceptions verursachen
            Zahl1 = int.Parse(zahlen[0]);
            Zahl2 = int.Parse(zahlen[1]);
        }

        private Rechenoperation GetRechenoperation()
        {
            if (Eingabe.Contains('+'))
                return Rechenoperation.Addition;
            else if (Eingabe.Contains('-'))
                return Rechenoperation.Subtraktion;
            else if (Eingabe.Contains('*'))
                return Rechenoperation.Multiplikation;
            else if (Eingabe.Contains('/'))
                return Rechenoperation.Division;
            else
                return 0;
        }

        private string[] SplitTerm()
        {
            switch (Operation)
            {
                case Rechenoperation.Addition:
                    return Eingabe.Split('+');
                case Rechenoperation.Subtraktion:
                    return Eingabe.Split('-');
                case Rechenoperation.Multiplikation:
                    return Eingabe.Split('*');
                case Rechenoperation.Division:
                    return Eingabe.Split('/');
            }
            return null; //Null-Rückgabe kann später eine NullReferenceException verursachen
        }
    }
}

