using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
---------------------------------------------------
1: Skapa kontakt
2: Uppdatera kontakt
3. Ta bort kontakt
4. Hämta kundregister
5. Hämta telefonlista
6. Avsluta");
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
                    HämtaTelefonlista();
                else if (input == '6')
                    break;
                Console.WriteLine();
            }



        }


        public static string conString =
                @"Server = (localdb)\mssqllocaldb; Database = KundregisterBlogg; Trusted_Connection = True";

        private static void HämtaTelefonlista()
        {
            var sql = @"SELECT ID, Förnamn, Efternamn, Telefonnummer, Etikettnamn
                    FROM Kundregister
                    INNER JOIN Telefonlista ON Kundregister.ID = telefonlista.personid
INNER JOIN etiketter ON telefonlista.etikett = etiketter.etikettid";


            using (SqlConnection connection = new SqlConnection(conString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();


                SqlDataReader reader = command.ExecuteReader();
                var list = new List<TelefonLista>();

                while (reader.Read())
                {
                    var personer = new TelefonLista(reader.GetInt32(0), reader.GetString(1), reader.GetString(2),
                        reader.GetString(3), reader.GetString(4));

                    list.Add(personer);
                }

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine($@"{"",-15}Förnamn:{"",-12}Efternamn:{"",-15}Telefon:{"",-12}Telefonnummer:{"",-5}");
                Console.ResetColor();
                list = list.OrderBy(person => person.PersonID).ToList();
                foreach (var person in list)
                {
                    Console.WriteLine(
                        $@"ID: {person.PersonID,-10} {person.Förnamn,-20} {person.Efternamn,-25} {person.TelefonType,-20} {
                            person.TelefonNummer,-5}");
                }
            }

            Console.WriteLine(@"Vill du uppdatera telefonlistan?
1: Yes
2: No");
            var answer = Console.ReadLine();
            if (answer=="1"||answer=="Yes")
                UppdateraNyaTelefonnummer();

        }

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


                var strings = $"{"",-9}Förnamn:{"",-12}Efternamn:{"",-15}E-Mail:{"",-12}Kundrelation:{"",-5}";
                WriteInGreen(strings);

               

                foreach (var person in list)
                {
                    Console.WriteLine(
                        $@"ID: {person.ID,-5}{person.Förnamn,-20}{person.Efternamn,-20}{person.Email,-25}{
                                person.Kundrealation,-5}");
                }
            }


        }
        public static void WriteInGreen(string input)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($@"{"",-9}Förnamn:{"",-12}Efternamn:{"",-15}E-Mail:{"",-12}Kundrelation:{"",-5}");
            Console.ResetColor();
        }
        private static void ÄndraKontakt()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(@"Vem vill du uppdatera? Skriv Personens ID
-------------------------------------------------------------");
            Console.ResetColor();
            HämtaData();

            Console.Write("Personens ID:");
            var input = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($@"Vad vill du uppdatera? 
----------------------------------------
1: Förnamn
2: Efternamn
3. Telefonnummer
4: EMail
5. Kundrelation");
            Console.ResetColor();
            Console.Write("Skriv nummer:");
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
                case "5":
                    ÄndraKundrelation(input);
                    break;
            }

        }

        private static void ÄndraKundrelation(string input)
        {
            Console.WriteLine(@"Till vad vill du uppdatera till?
1.Nykund
2.Guldkund
3.Leverantör");
            var kundrelation = Convert.ToInt32(Console.ReadLine());
            if (kundrelation == 1)
                kundrelation = 3;
            else if (kundrelation == 2)
                kundrelation = 6;
            else if (kundrelation == 3)
                kundrelation = 5;

            var sql = $@"UPDATE Kundregister
                    SET Kundrelation='{kundrelation}'
                    WHERE ID={input}";

            using (SqlConnection connection = new SqlConnection(conString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();
                command.ExecuteReader();
            }
        }

        public static void UppdateraNyaTelefonnummer()
        {
            HämtaData();
            Console.WriteLine("Vem vill du uppdater? Skriv IDnummer:");
            var personID = Console.ReadLine();

            Console.WriteLine(@"Vilket/Vilka telefonnummer vill du lägga till?
1: Hem
2: Mobil
3: Jobb
4: Nödkontakt");

            var telefonnummer = Console.ReadLine().Split(',');
            var telefonnummers = new Dictionary<int, string>();

            foreach (var del in telefonnummer)
            {
                if (del == "1" || del == "Hem")
                {
                    Console.Write($"Notera Hemnummer: ");
                    telefonnummers.Add(1, Console.ReadLine());
                }

                else if (del == "2" || del == "Mobil")
                {
                    Console.Write($"Notera Mobilnummer: ");
                    telefonnummers.Add(2, Console.ReadLine());
                }
                else if (del == "3" || del == "Jobb")
                {
                    Console.Write($"Notera Jobbnummer: ");
                    telefonnummers.Add(3, Console.ReadLine());
                }
                else if (del == "3" || del == "Nödkontakt")
                {
                    Console.Write($"Notera Nödnummer: ");
                    telefonnummers.Add(4, Console.ReadLine());
                }
            }

            var sql="";
            var phones = telefonnummers.ToList();
            for (int i = 0; i < phones.Count; i++)
            {
                sql = $"INSERT INTO Telefonlista(PersonID,Telefonnummer,Etikett) " +
                $"VALUES(@PersonID{i}, @Telefonnummer{i}, @Etikett{i})";
            }


            
            using (SqlConnection connection = new SqlConnection(conString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();
                for (int i = 0; i < phones.Count; i++)
                {
                    var telefon = phones[i];

                    command.Parameters.Add(new SqlParameter($"PersonID{i}", personID));
                    command.Parameters.Add(new SqlParameter($"Telefonnummer{i}", telefon.Value));
                    command.Parameters.Add(new SqlParameter($"Etikett{i}", telefon.Key));
                }

                command.ExecuteReader();
            }
        }

        private static void SkapaKontakt()
        {
            Console.WriteLine("Vänligen skriv förnamn");
            var förnamn = Console.ReadLine();
            Console.WriteLine("Vänligen skriv efternamn");
            var efternamn = Console.ReadLine();
            Console.WriteLine("Vänligen skriv email");
            var email = Console.ReadLine();
            Console.WriteLine(@"Vänligen skriv kundrealation:
1: Ny kund
2: Leverantör
3: Guldkund");
            var kundrelation = Convert.ToInt32(Console.ReadLine());
            if (kundrelation == 1)
                kundrelation = 3;
            else if (kundrelation == 2)
                kundrelation = 5;
            else if (kundrelation == 3)
                kundrelation = 6;

            Console.WriteLine(@"Vilket/Vilka telefonnummer vill du lägga till?
1: Hem
2: Mobil
3: Jobb
4: Nödkontakt");

            var telefonnummer = Console.ReadLine().Split(',');
            var telefonnummers = new Dictionary<int, string>();

            foreach (var del in telefonnummer)
            {
                if (del == "1" || del == "Hem")
                {
                    Console.Write($"Notera Hemnummer: ");
                    telefonnummers.Add(1, Console.ReadLine());
                }

                else if (del == "2" || del == "Mobil")
                {
                    Console.Write($"Notera Mobilnummer: ");
                    telefonnummers.Add(2, Console.ReadLine());
                }
                else if (del == "3" || del == "Jobb")
                {
                    Console.Write($"Notera Jobbnummer: ");
                    telefonnummers.Add(3, Console.ReadLine());
                }
                else if (del == "3" || del == "Nödkontakt")
                {
                    Console.Write($"Notera Nödnummer: ");
                    telefonnummers.Add(4, Console.ReadLine());
                }
            }

            var sql = @"DECLARE @ID int
                    INSERT INTO Kundregister(Förnamn, Efternamn, email, kundrelation)
                    VALUES (@Förnamn, @Efternamn, @EMail, @kundrelation);
                    SELECT @ID = SCOPE_IDENTITY();";

            var phones = telefonnummers.ToList();
            for (int i = 0; i < phones.Count; i++)
            {
                var telefon = phones[i];
                sql += $"INSERT INTO Telefonlista(PersonID,telefonnummer,Etikett) " +
                $"VALUES(@ID, @Telefonnummer{i}, @Etikett{i})";
            }




            using (SqlConnection connection = new SqlConnection(conString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();
                command.Parameters.Add(new SqlParameter("Förnamn", förnamn));
                command.Parameters.Add(new SqlParameter("Efternamn", efternamn));
                command.Parameters.Add(new SqlParameter("EMail", email));
                command.Parameters.Add(new SqlParameter("Kundrelation", kundrelation));

                for (int i = 0; i < phones.Count; i++)
                {
                    var telefon = phones[i];
                    command.Parameters.Add(new SqlParameter($"Telefonnummer{i}", telefon.Value));
                    command.Parameters.Add(new SqlParameter($"Etikett{i}", telefon.Key));
                }




                command.ExecuteReader();
            }




        }

        public static void TaBortKontkat()
        {
            HämtaData();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Vilket personID du radera?");
            Console.ResetColor();
            var input = Console.ReadLine();


            var sql = $@"
DELETE FROM telefonlista
WHERE PersonID ={input}
DELETE FROM projekt
WHERE PersonID ={input}
DELETE FROM kundregister
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
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Till vad vill du uppdatera?");
            Console.ResetColor();
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
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Vilket telefonnummer vill du uppdatera?");
            Console.ResetColor();

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


            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Vilket vill du ändra?");
            Console.ResetColor();
            var nummerÄndra = Console.ReadLine();
            var nyttNummer = "";
            foreach (var namn in listaNummer)
            {
                if (nummerÄndra != namn.TelefonType) continue;
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("Till vad vill du uppdatera?");
                Console.ResetColor();
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


    }
}

