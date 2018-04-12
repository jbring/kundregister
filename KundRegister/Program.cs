using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace KundRegister
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($@"Hej, vad vill du göra:
1: Skapa kontakt
2: Ändra kontakt
3. Ta bort kontakt
4. Hämta kundregister
5. Avsluta");
                Console.ResetColor();

                var input = Convert.ToChar(Console.ReadLine());
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
                Console.WriteLine();
            }



        }

        public static string conString =
            @"Server = (localdb)\mssqllocaldb; Database = KundregisterBlogg; Trusted_Connection = True";

        private static void HämtaData()
        {
            var sql = @"SELECT ID, Förnamn, Efternamn, Relationstyp, Email
                    FROM Kundregister
                    INNER JOIN Kundrelation ON Kundregister.Kundrelation = Kundrelation.RelationsID";


            using (SqlConnection connection = new SqlConnection(conString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();


                SqlDataReader reader = command.ExecuteReader();
                var list = new List<Register>();

                while (reader.Read())
                {
                    var bp = new Register(reader.GetString(1), reader.GetString(2), reader.GetString(3),
                        reader.GetString(4),
                        reader.GetInt32(0));


                    list.Add(bp);
                }

                Console.WriteLine($@"{"",-9}Förnamn:{"",-12}Efternamn:{"",-15}E-Mail:{"",-12}Kundrelation:{"",-5}");
                foreach (var person in list)
                {
                    Console.WriteLine(
                        $@"ID: {person.ID,-5}{person.Förnamn,-20}{person.Efternamn,-20}{person.Email,-25}{
                                person.Kundrealation,-5}");
                }
            }


        }

        private static void ÄndraKontakt()
        {
            Console.WriteLine($"Vem vill du uppdatera? Välj Personens ID");
            HämtaData();

            var input = Console.ReadLine();

            Console.WriteLine($@"Vad vill du uppdatera? 
1: Förnamn
2: Efternamn
3. Telefonnummer
4: EMail");
            var input2 = Console.ReadLine();
            string coloum = "";
            switch (input2)
            {
                case "1":
                    coloum = "Förnamn";
                    ÄndraEmailEllerNamn(input, coloum);
                    break;
                case "2":
                    coloum = "Efternamn";
                    ÄndraEmailEllerNamn(input, coloum);
                    break;
                case "3":
                    ÄndraTelefonnummer(input);
                    break;
                case "4":
                    coloum = "EMail";
                    ÄndraEmailEllerNamn(input, coloum);
                    break;
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


            var sql = @"INSERT INTO Kundregister(Förnamn, Efternamn, email)
                    VALUES (@Förnamn, @Efternamn, @EMail)";
                    


            using (SqlConnection connection = new SqlConnection(conString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();
                command.Parameters.Add(new SqlParameter("Förnamn", förnamn));
                command.Parameters.Add(new SqlParameter("Efternamn", efternamn));
                command.Parameters.Add(new SqlParameter("EMail", email));
                
                

                command.ExecuteReader();
                connection.Close();
            }
            var sql1 = @"
                    INSERT INTO Telefonlista(telefonnummer)
                    VALUES (@Telefonnummer)";


            using (SqlConnection connection = new SqlConnection(conString))
            using (SqlCommand command = new SqlCommand(sql1, connection))
            {
                connection.Open();
                
                command.Parameters.Add(new SqlParameter("Telefonnummer", telefonnummer));


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

        private static void ÄndraEmailEllerNamn(string input, string coloum)
        {
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

        private static void ÄndraTelefonnummer(string input)
        {

            Console.WriteLine("Vilket telefonnummer vill du uppdatera?");
            var sql1 = $@"Select ID, Förnamn, Efternamn, Telefonnummer, Etikettnamn, Etikett
                FROM Telefonlista
                INNER JOIN Kundregister on Kundregister.ID = Telefonlista.PersonID
            INNER JOIN Etiketter on EtikettID = Etikett";



            using (SqlConnection connection = new SqlConnection(conString))
            using (SqlCommand command = new SqlCommand(sql1, connection))

            {
                connection.Open();


                SqlDataReader reader = command.ExecuteReader();
                var list = new List<TelefonLista>();

                while (reader.Read())
                {
                    var bp = new TelefonLista(reader.GetInt32(0), reader.GetString(1), reader.GetString(2),
                        reader.GetString(3),
                        reader.GetString(4), reader.GetInt32(5));


                    list.Add(bp);
                }

                UppdateraTelefonnummer(list, input);
            }
        }

        private static void UppdateraTelefonnummer(List<TelefonLista> list, string input)
        {




            var listaNummer = new List<TelefonLista>();

            foreach (var person in list)
            {
                if (Convert.ToInt32(input) == person.PersonID)
                {
                    Console.WriteLine($@"{person.Förnamn} har {person.TelefonType}nummer {person.TelefonNummer} ");
                    var persons = new TelefonLista(person.TelefonNummer, person.TelefonType, person.Etikett);
                    listaNummer.Add(persons);
                }
            }



            Console.WriteLine("Vilket vill du ändra?");
            var nummerÄndra = Console.ReadLine();
            var nyttNummer = "";
            foreach (var namn in listaNummer)
            {
                if (nummerÄndra != namn.TelefonType) continue;
                Console.WriteLine("Till vad vill du uppdatera?");
                nyttNummer = Console.ReadLine();

                var sql = $@"
                            UPDATE Telefonlista
                             SET Telefonnummer ='{nyttNummer}'
                             WHERE Etikett={namn.Etikett} AND PersonID={input}";


                using (SqlConnection connection = new SqlConnection(conString))
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    command.ExecuteReader();
                }
            }
        }



        //Console.WriteLine("Till vad vill du uppdatera?");
        //var input3 = Console.ReadLine();

        //var sql = $@"UPDATE Telefonlista
        //        SET {coloum}='{input3}'
        //        WHERE ID={input}";

        //using (SqlConnection connection = new SqlConnection(conString))
        //using (SqlCommand command = new SqlCommand(sql, connection))
        //{
        //    connection.Open();
        //    command.ExecuteReader();
        //}

    }
}
