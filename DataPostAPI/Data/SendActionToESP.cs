using DataPostAPI.FireBaseDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataPostAPI.Data
{
    public class SendActionToESP
    {
        public async Task<FirebaseResponse> SendActionTypeToESP(int id,int zoneNumber)
        {
            FirebaseDB firebaseDB = new FirebaseDB("https://esp8266-27419-default-rtdb.firebaseio.com/");

            // Referring to Node with name "Teams"  
            FirebaseDB firebaseDBTeams = firebaseDB.Node("anomaly"+zoneNumber.ToString());

            var data = "{"+  "int"+": "+id + "}";
           
            FirebaseResponse postResponse = await firebaseDBTeams.Put(data);
            return postResponse;

        }

    }
}
