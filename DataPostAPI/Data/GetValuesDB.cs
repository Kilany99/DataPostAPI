using DataPostAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
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
            string zoneId = zoneID.ToString();
            string LocationqueryString = "SELECT * FROM PostedData WHERE " +
                "PostedDataId=(SELECT max(PostedDataId) FROM PostedData WHERE CameraZoneID=@zoneId);";
            using (SqlConnection Locationconnection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(LocationqueryString, Locationconnection);
                try
                {
                    Locationconnection.Open();
                    command.Parameters.Add("zoneId", SqlDbType.VarChar).Value = zoneId;
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
                    Console.WriteLine(ex.ToString());
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
        public static List<PostedDataModel> GetAllValuesFromDB(string ClientDeviceToken)
        {
            int searchResultID = GetClientIDFromToken(ClientDeviceToken);
            PostedDataModel pdm = new PostedDataModel();
            List<PostedDataModel> results = new List<PostedDataModel>();
            List<string> ValuesFromDB = new List<string>();
            string LocationqueryString = "SELECT * FROM PostedData WHERE ClientId = @searchResultID);";
            using (SqlConnection Locationconnection =
                                     new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(LocationqueryString, Locationconnection);
                try
                {
                    Locationconnection.Open();
                    SqlDataReader Locationreader = command.ExecuteReader();
                    int numberOfRecords = command.ExecuteNonQuery();
                    command.Parameters.Add("searchResultID", SqlDbType.VarChar).Value = searchResultID;
                    while (Locationreader.Read())
                    {
                        
                        for (int i = 0; i <= Locationreader.FieldCount - 1; i++)
                        {
                            ValuesFromDB.Add(Locationreader[i].ToString());
                        }
                     
                    }
                    Locationreader.Close();
                    for(int i=0;i<=ValuesFromDB.Capacity - 1;i++)
                    {
                        if(i%6 ==0)
                        {
                            pdm.PostedDataId= Int32.Parse(ValuesFromDB[i]);
                            pdm.CrimeScreenshot = ValuesFromDB[i + 1];
                            pdm.AnomalyDateTime = ValuesFromDB[i + 2];
                            pdm.AnomalyType = ValuesFromDB[i + 3];
                            pdm.ActionPriority = ValuesFromDB[i + 4];
                            pdm.ZoneID = Int32.Parse(ValuesFromDB[i + 5]);
                            results[i] = pdm;


                        }
                    }
                    return results;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }

        }
        private static int GetClientIDFromToken(string clientToken)
        {
            List<string> ValuesFromDB = new List<string>();
            string LocationqueryString = "SELECT * FROM Client WHERE DeviceToken = @clientToken);";
            using (SqlConnection Locationconnection =
                                     new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(LocationqueryString, Locationconnection);
                try
                {
                    Locationconnection.Open();

                    SqlDataReader Locationreader = command.ExecuteReader();
                    int numberOfRecords = command.ExecuteNonQuery();
                    while (Locationreader.Read())
                    {
                        for (int i = 0; i <= Locationreader.FieldCount - 1; i++)
                        {
                            ValuesFromDB.Add(Locationreader[i].ToString());
                        }
                    }
                    Locationreader.Close();
                    int result = Int32.Parse(ValuesFromDB[0]);
                    Console.WriteLine(result);
                    return result;
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
