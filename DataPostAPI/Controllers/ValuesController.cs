using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using DataPostAPI.Models;
using System.Data.SqlClient;
using System.Data;
using DataPostAPI.Data;
using DataPostAPI.Services;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net;
using System.Text;
using DataPostAPI.FireBaseDB;

namespace DataPostAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        protected readonly INotificationService _notificationService;
        public ValuesController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        // GET api/values
        [HttpGet]
        [Route("data")]
        public async Task<ActionResult<PostedDataModel>> Get()
        {
            
            List<string> values = GetValuesDB.GetValuesFromDB();
            PostedDataModel pd = new PostedDataModel();
            pd.CrimeScreenshot = values[1];
            pd.AnomalyDateTime = values[2];
            pd.AnomalyType = values[3];
            pd.ActionPriority =values[4];
            pd.ZoneID = Int32.Parse(values[5]);
            return pd;
        }


        // GET api/values/data/5
        [HttpGet("data/{id}")]
        public  ActionResult<PostedDataModel> Get(string id)
        {
            int ID;
            if (id.Contains("Omar"))
                ID = 1;

            else if (id.Contains("Mahmud"))
                ID = 2;
            else if (id.Contains("Yasser"))
                ID = 3;
            else if (id.Contains("Kilany"))
                ID = 4;
            else if (id.Contains("Talat"))
                ID = 5;
            else
                ID = 1;
            Console.WriteLine(ID.ToString());
            List<string> values = GetValuesDB.GetValuesFromDB(ID);
            PostedDataModel pd = new PostedDataModel();
                                                        //I don't need values[0] because it is the ID used by the database
            pd.CrimeScreenshot = values[1];
            pd.AnomalyDateTime = values[2];
            pd.AnomalyType = values[3];
            pd.ActionPriority = values[4];
            pd.ZoneID = Int32.Parse(values[5]);
           
            return pd;
        }


        // POST api/values
        [HttpPost]
        public async Task<string> Post([FromForm] string value, [FromForm] string dt, [FromForm] string zone, [FromForm] string anomalyType, [FromForm] string anomalyPriority)
        {
                    //Anomaly data recive part:

            PostedDataModel pdm = new PostedDataModel();
            pdm.CrimeScreenshot = value;
            pdm.AnomalyDateTime = dt;
            pdm.ZoneID = Int32.Parse(zone);
            pdm.AnomalyType = anomalyType;
            pdm.ActionPriority = anomalyPriority;
            string result = PostValuesDB.PostValuesToDB(pdm);

                    //send notification part:

            NotificationModel nm = Data.SendNotification.GetDeviceIDFromDB(Int32.Parse(zone), anomalyType);
            ResponseModel notificationResult = await _notificationService.SendNotification(nm);
            result += "\n IsSuccess : " + notificationResult.IsSuccess.ToString();
           
                    //send action part:

            int anomalyTypeNumber;
            if (pdm.AnomalyType.Contains("Explosion"))
                anomalyTypeNumber = 2;
            else if (pdm.AnomalyType.Contains("Fighting"))
                anomalyTypeNumber = 4;
            else if (pdm.AnomalyType.Contains("Shoplifting"))
                anomalyTypeNumber = 3;
            else
                anomalyTypeNumber = 1;
            SendActionToESP send = new SendActionToESP();
            FirebaseResponse response = await send.SendActionTypeToESP(anomalyTypeNumber, pdm.ZoneID);

            return result;
        }

        

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<string> Put(string id, [FromForm] string value)
        {
            int ID;
            if (id.Contains("Omar"))
                ID = 1;

            else if (id.Contains("Mahmud"))
                ID = 2;
            else if (id.Contains("Yasser"))
                ID = 3;
            else
                ID = 1;
            string result = PutValuesDB.PutValuesToDB(value, ID.ToString());
            return result;

        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
   
}
    

