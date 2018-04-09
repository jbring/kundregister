using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace KundRegister
{

    //class HämtaData
    //{

        //private string conString = @"Server = (localdb)\mssqllocaldb; Database = KundregisterBlogg; Trusted_Connection = True";

        //public List<Register> Get(string hämta)
        //{
        //    var sql = @"SELECT [Förnamn], [Efternamn], [Telefonnummer], [Email] 
        //            FROM Kundregister";


        //    using (SqlConnection connection = new SqlConnection(conString))
        //    using (SqlCommand command = new SqlCommand(sql, connection))
        //    {
        //        connection.Open();
        //        command.Parameters.Add(new SqlParameter("Title", hämta));

        //        SqlDataReader reader = command.ExecuteReader();
        //        var list = new List<Register>();

        //        while (reader.Read())
        //        {
        //            var bp = new Register()
        //            {
        //                förNamn = reader.GetString(0),
        //                efterNamn = reader.GetString(1),
        //                telefonnummer = reader.GetString(2),
        //                email = reader.GetString(3),

        //            };

        //            list.Add(bp);
        //        }
        //        foreach (var kund in list)
        //        {

        //        }

        //    }

            

      //}
    }

