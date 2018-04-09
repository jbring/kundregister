using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace KundRegister
{
    
    class Register
    {

        public string förNamn;
        public string efterNamn;
        public string telefonnummer;
        public string email;
        
         

        public Register(string förNamn, string efterNamn, string telefonnummer, string email)
        {
            this.förNamn = förNamn;
            this.efterNamn = efterNamn;
            this.telefonnummer = telefonnummer;
            this.email = email;
            
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {

            
            Console.WriteLine(@"Hej, vad vill du göra:
1: Skapa kontakt
2: Ändra kontakt
3. Ta bort kontakt
4. Hämta kundregister
5. Avsluta");


            var input =Convert.ToChar(Console.ReadLine());
            if (input == '1')
                SkapaKontakt();
            else if (input == '2')
                ÄndraKontakt();
            else if (input == '3')
                TaBortKontkat();
            else if (input == '4')
                HämtaData();
                else if (input == '5')
                break;

            }



        }
        public static string conString = @"Server = (localdb)\mssqllocaldb; Database = KundregisterBlogg; Trusted_Connection = True";
        private static void HämtaData()
        {
            var sql = @"SELECT ID, Förnamn, Efternamn, Telefonnummer, Email
                    FROM Kundregister";

            using (SqlConnection connection = new SqlConnection(conString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();
                

                SqlDataReader reader = command.ExecuteReader();
                var list = new List<Register>();

                while (reader.Read())
                {
                    var bp = new Register(reader.GetString(0), reader.GetString(1), reader.GetString(2),
                        reader.GetString(3));
                   

                    list.Add(bp);
                }
                foreach (var person in list)
                {
                    Console.WriteLine($"{person.förNamn}, {person.efterNamn}, {person.email}, {person.telefonnummer}");
                }
            }

            
        }

        private static void ÄndraKontakt()
        {
            Console.WriteLine($"Vem vill du uppdatera?");
            HämtaData();

            var input = Console.ReadLine();

            Console.WriteLine($@"Vad vill du uppdatera? 
1: Förnamn
2: Efternamn
3. Telefonnummer
4: EMail");
            var input2 = Console.ReadLine();
            string coloum = "";
            if (input2 == "1")
                coloum = "Förnamn";
            else if (input2=="2")
                coloum = "Efternamn";
            else if (input2 == "3")
                coloum = "Telefonnummer";
            else if (input2 == "4")
                coloum = "Email";



            Console.WriteLine("Till vad vill du uppdatera?");
            var input3 = Console.ReadLine();

            var sql = $@"UPDATE Kundregister
                    SET {coloum}='{input3}'
                    WHERE ID={input}";

            using (SqlConnection connection = new SqlConnection(conString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();
                command.ExecuteReader();
            }
        }
        

        private static void SkapaKontakt()
        {
            Console.WriteLine("Vänligen skriv förnamn");
            var förnamn = Console.ReadLine();
            Console.WriteLine("Vänligen skriv efternamn");
            var efternamn = Console.ReadLine();
            Console.WriteLine("Vänligen skriv telefonnummer");
            var telefonnummer = Console.ReadLine();
            Console.WriteLine("Vänligen skriv email");
            var email = Console.ReadLine();


            var sql = @"INSERT INTO Kundregister(Förnamn, Efternamn, Telefonnummer, email)
                    VALUES (Förnamn, Efternamn, Telefonnummer, EMail)";

            using (SqlConnection connection = new SqlConnection(conString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();
                command.Parameters.Add(new SqlParameter("Förnamn", förnamn));
                command.Parameters.Add(new SqlParameter("Efternamn", efternamn));
                command.Parameters.Add(new SqlParameter("Telefonnummer", telefonnummer));
                command.Parameters.Add(new SqlParameter("EMail", email));

                command.ExecuteReader();
            }
        }
        private static void TaBortKontkat()
        {
            Console.WriteLine("Vem vill du radera?");
            var input = Console.ReadLine();
           

            var sql = $@"DELETE FROM Kundregister
                    WHERE ID={input}";

            using (SqlConnection connection = new SqlConnection(conString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();
                command.ExecuteReader();
            }
        }

    }
    }
