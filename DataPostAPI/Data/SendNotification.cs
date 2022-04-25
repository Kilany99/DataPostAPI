using DataPostAPI.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DataPostAPI.Data
{
    public class SendNotification
    {
       

        public static  NotificationModel GetDeviceIDFromDB(int zoneId,string anomalyType)
        {
            string connetionString = null;
            string sql = null;
            string zoneID = zoneId.ToString();
            connetionString = @"Data Source=DESKTOP-P944USQ\SQLEXPRESS01;Initial Catalog=RAISDB;Integrated Security=True;";
            sql = "SELECT * FROM Client WHERE CameraZoneID = @zoneID";
            List<string> deviceToken = new List<string>();

            using (SqlConnection conn = new SqlConnection(connetionString) )
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql,conn))
                    {
                        cmd.Parameters.Add("zoneID", SqlDbType.VarChar).Value = zoneID;
                        SqlDataReader Locationreader = cmd.ExecuteReader();
                        while (Locationreader.Read())
                        {
                            for (int i = 0; i <= Locationreader.FieldCount - 1; i++)
                            {
                                deviceToken.Add(Locationreader[i].ToString());
                            }
                        }
                        conn.Close();

                    }

                }
                catch(Exception ex)
                {
                    NotificationModel nm = new NotificationModel();
                    nm.DeviceId = ex.ToString();
                    return nm;
                }
                NotificationModel notificationModel = new NotificationModel();
                notificationModel.DeviceId = deviceToken[3];                   //deviceToken is in the 4th coulmn of the table
                notificationModel.IsAndroiodDevice = true;
                notificationModel.Body = "Anomaly Type :" + anomalyType;
                notificationModel.Title = "Anomaly Detected!";
                return notificationModel;
              
            }

        }
    }
}
