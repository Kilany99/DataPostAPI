using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DataPostAPI.Data
{
    public class GetValuesDB
    {
        private readonly static string ConnectionString = @"Data Source=DESKTOP-P944USQ\SQLEXPRESS01;Initial Catalog=RAISDB;Integrated Security=True;";

        public static List<string> GetValuesFromDB(int zoneID)
        {
            List<string> ValuesFromDB = new List<string>();
            string LocationqueryString = "SELECT * FROM PostedData WHERE " +
                "PostedDataId=(SELECT max(PostedDataId) FROM PostedData) AND CameraZoneID=@zoneID;";
            using (SqlConnection Locationconnection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(LocationqueryString, Locationconnection);
                try
                {
                    Locationconnection.Open();
                    SqlDataReader Locationreader = command.ExecuteReader();
                    while (Locationreader.Read())
                    {
                        for (int i = 0; i <= Locationreader.FieldCount - 1; i++)
                        {
                            ValuesFromDB.Add(Locationreader[i].ToString());
                        }
                    }
                    Locationreader.Close();
                    return ValuesFromDB;
                }
                catch (Exception ex)
                {
                    List<string> res = new List<string>();
                    res[0] = ex.ToString();
                    return res;

                }
            }

        }

        public static List<string> GetValuesFromDB()
        {
            List<string> ValuesFromDB = new List<string>();
            string LocationqueryString = "SELECT * FROM PostedData WHERE PostedDataId=(SELECT max(PostedDataId) FROM PostedData);";
            using (SqlConnection Locationconnection =
                                     new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(LocationqueryString, Locationconnection);
                try
                {
                    Locationconnection.Open();
                    SqlDataReader Locationreader = command.ExecuteReader();
                    while (Locationreader.Read())
                    {
                        for (int i = 0; i <= Locationreader.FieldCount - 1; i++)
                        {
                            ValuesFromDB.Add(Locationreader[i].ToString());
                        }
                    }
                    Locationreader.Close();
                    return ValuesFromDB;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }

        }
    }
}
