namespace KundRegister
{
    class Register
    {

        public string Förnamn;
        public string Efternamn;
        public string Kundrealation;
        public string Email;
        public int ID;


        public Register(string förnamn, string efternamn, string kundrealation, string email, int iD)
        {
            Förnamn = förnamn;
            Efternamn = efternamn;
            Kundrealation = kundrealation;
            Email = email;
            ID = iD;
        }
    }
}