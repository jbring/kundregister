namespace KundRegister
{
    class TelefonLista
    {
        public int PersonID;
        public string Förnamn;
        public string Efternamn;
        public string TelefonNummer;
        public string TelefonType;
        public int Etikett;

        public TelefonLista(int personID, string förnamn, string efternamn, string telefonNummer, string telefonType, int etikett)
        {
            PersonID = personID;
            Förnamn = förnamn;
            Efternamn = efternamn;
            TelefonNummer = telefonNummer;
            TelefonType = telefonType;
            Etikett = etikett;
        }
        public TelefonLista(string telefonNummer, string telefonType, int etikett)
        {
            TelefonNummer = telefonNummer;
            TelefonType = telefonType;
            Etikett = etikett;
        }

        public TelefonLista(int personID, string förnamn, string efternamn, string telefonNummer, string telefonType)
        {
            PersonID = personID;
            Förnamn = förnamn;
            Efternamn = efternamn;
            TelefonNummer = telefonNummer;
            TelefonType = telefonType;
        }
    }
}