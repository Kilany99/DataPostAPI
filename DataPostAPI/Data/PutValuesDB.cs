using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DataPostAPI.Data
{
    public class PutValuesDB
    {
        public static string PutValuesToDB(string Value, string Id)
        {
            

            string connetionString = null;
            string sql = null;
            // All the info required to reach your db. See connectionstrings.com
            connetionString = @"Data Source=DESKTOP-P944USQ\SQLEXPRESS01;Initial Catalog=RAISDB;Integrated Security=True;";

            // Prepare a proper parameterized query 
            sql = "UPDATE Client SET DeviceToken = @value WHERE ClientName = @id ";
            using (SqlConnection cnn = new SqlConnection(connetionString))
            {
                try
                {
                    cnn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, cnn))
                    {
                        cmd.Parameters.Add("value", SqlDbType.VarChar).Value = Value;
                        cmd.Parameters.Add("id", SqlDbType.VarChar).Value = Id;
                        int rowsAdded = cmd.ExecuteNonQuery();
                        if (rowsAdded > 0)
                            return ("Row updated!!");
                        else
                            return ("No row updated!!");
                    }
                }
                catch (Exception ex)
                {
                    
                    return ("ERROR:" + ex.Message);
                }

            }

            
        }
    }
}
