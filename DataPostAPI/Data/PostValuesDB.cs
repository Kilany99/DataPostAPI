using DataPostAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DataPostAPI.Data
{
    public class PostValuesDB
    {
        public static string PostValuesToDB(PostedDataModel postedData)
        {
            string connetionString = null;
            string sql = null;

            // All the info required to reach your db. See connectionstrings.com
            connetionString = @"Data Source=DESKTOP-P944USQ\SQLEXPRESS01;Initial Catalog=RAISDB;Integrated Security=True;";

            // Prepare a proper parameterized query 
            sql = "insert into PostedData ([CrimeScreenshot], [AnomalyDateTime] , [ActionType] , [ActionPriority], [CameraZoneID]) values(@first,@second,@third,@fourth,@fifth)";

            // Create the connection (and be sure to dispose it at the end)
            using (SqlConnection cnn = new SqlConnection(connetionString))
            {
                try
                {
                    cnn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, cnn))
                    {
                        // Create and set the parameters values 
                        cmd.Parameters.Add("@first", SqlDbType.NVarChar).Value = postedData.CrimeScreenshot;
                        cmd.Parameters.Add("@second", SqlDbType.NVarChar).Value = postedData.AnomalyDateTime;
                        cmd.Parameters.Add("@third", SqlDbType.NVarChar).Value = postedData.AnomalyType;
                        cmd.Parameters.Add("@fourth", SqlDbType.NVarChar).Value = postedData.ActionPriority;
                        cmd.Parameters.Add("@fifth", SqlDbType.NVarChar).Value = postedData.ZoneID;

                        // Let's ask the db to execute the query
                        int rowsAdded = cmd.ExecuteNonQuery();
                        if (rowsAdded > 0)
                            return ("Row inserted!!");
                        else
                            // Well this should never really happen
                            return ("No row inserted");

                    }
                }
                catch (Exception ex)
                {
                    // We should log the error somewhere, 
                    // for this example let's just show a message
                    return ("ERROR:" + ex.Message);
                }

            }
        }
    }
}
