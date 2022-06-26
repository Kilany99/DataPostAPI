using DataPostAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DataPostAPI.Data
{
    public class LoginDB
    {
        private readonly static string ConnectionString = @"Data Source=DESKTOP-P944USQ\SQLEXPRESS01;Initial Catalog=RAISDB;Integrated Security=True;";
        public static Client LoginToDB(string userName)
        {
            
            List<string> ValuesFromDB = new List<string>();
            string LocationqueryString = "SELECT * FROM Client WHERE ClientName= @userName";
            using (SqlConnection Locationconnection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(LocationqueryString, Locationconnection);
                try
                {
                    Locationconnection.Open();
                    command.Parameters.Add("userName", SqlDbType.VarChar).Value = userName;
                    SqlDataReader Locationreader = command.ExecuteReader();
                    if (Locationreader.HasRows)
                    {
                      
                        while (Locationreader.Read())
                        {
                            for (int i = 0; i <= Locationreader.FieldCount - 1; i++)
                            {
                                ValuesFromDB.Add(Locationreader[i].ToString());
                            }
                        }
                        Locationreader.Close();

                        Client client = new Client();
                        client.ClientName = ValuesFromDB[1];
                        client.Password = ValuesFromDB[2];
                        return client;


                    }
                    else 
                    {

                        Client client = new Client();
                        client.ClientName = "not found!";
                        return client;


                    }
                }
                catch (Exception ex)
                {
                    Client client = new Client();
                    client.ClientName = ex.ToString();
                    return client ;
                }
                
            }
        }
    }
}
