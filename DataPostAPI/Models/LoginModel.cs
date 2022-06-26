using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataPostAPI.Models
{
    public class LoginModel
    {
        [JsonProperty("userId")]
        public string userId {get;set;}

        [JsonProperty("Password")]
        public string Password { get; set; }


        [JsonProperty("ResponseCode")]
        public string ResponseCode { get; set; }

        [JsonProperty("ResponseMessage")]
        public string ResponseMessage { get; set; }

        
    }
}
