using DataPostAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Data.SqlClient; // Updated namespace

namespace DataPostAPI.Data
{
    public class GetValuesDB
    {
        private readonly static string ConnectionString = @"Data Source=DESKTOP-P944USQ\SQLEXPRESS01;Initial Catalog=newDB;Integrated Security=True;";

        public static List<string> GetValuesFromDB(int zoneID)
        {
            List<string> ValuesFromDB = new List<string>();
            string zoneId = zoneID.ToString();
            string LocationqueryString = "SELECT * FROM postedDatas WHERE " +
                "PostedDataid=(SELECT max(PostedDataid) FROM postedDatas WHERE ZoneId=@zoneId);";
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
            string LocationqueryString = "SELECT * FROM postedDatas WHERE PostedDataid=(SELECT max(PostedDataid) FROM postedDatas);";
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
                            pdm.PostedDataid= Int32.Parse(ValuesFromDB[i]);
                            pdm.CrimeScreenshot = ValuesFromDB[i + 1];
                            pdm.AnomalyDatetime = ValuesFromDB[i + 2];
                            pdm.AnomalyType = ValuesFromDB[i + 3];
                            pdm.ActionPriority = ValuesFromDB[i + 4];
                            pdm.ZoneId = Int32.Parse(ValuesFromDB[i + 5]);
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
        public static int GetClientIDFromToken(string clientToken)
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


        public static List<string> GetDataFromToken(string token)
        {
            List<string> ValuesFromDB = new List<string>();
            string deviceToken = token;
            string LocationqueryString = "SELECT * FROM postedDatas WHERE " +
                "PostedDataid=(SELECT max(PostedDataid) FROM postedDatas WHERE ZoneId=(SELECT ZoneId FROM Client WHERE DeviceToken = @deviceToken));";

            using (SqlConnection Locationconnection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(LocationqueryString, Locationconnection);
                try
                {
                    Locationconnection.Open();
                    command.Parameters.Add("deviceToken", SqlDbType.VarChar).Value = deviceToken;
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
            
        public static int GetMaxPostedDataIdFromToken(string token)
        {
            List<string> ValuesFromDB = new List<string>();
            string deviceToken = token;
            string LocationqueryString = "SELECT PostedDataid FROM postedDatas WHERE " +
                "PostedDataid=(SELECT max(PostedDataid) FROM postedDatas WHERE Zoneid=(SELECT Zoneid FROM Client WHERE DeviceToken = @deviceToken));";

            using (SqlConnection Locationconnection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(LocationqueryString, Locationconnection);
                try
                {
                    Locationconnection.Open();
                    command.Parameters.Add("deviceToken", SqlDbType.VarChar).Value = deviceToken;
                    SqlDataReader Locationreader = command.ExecuteReader();
                    while (Locationreader.Read())
                    {
                        for (int i = 0; i <= Locationreader.FieldCount - 1; i++)
                        {
                            ValuesFromDB.Add(Locationreader[i].ToString());
                        }
                    }
                    Locationreader.Close();
                    return Int32.Parse(ValuesFromDB[0]);
                }
                catch (Exception ex)
                {
                    throw;

                }
            }
        }
        
    }
}

    

