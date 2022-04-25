using DataPostAPI.Data;
using DataPostAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataPostAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        [HttpPost]
        [Route("Login")]
        public async Task<string> Login([FromForm] string userName, [FromForm] string pass)
        {
            Client clientFromDB = LoginDB.LoginToDB(userName);
            if (clientFromDB.ClientName.CompareTo(userName)==0 && clientFromDB.Password.CompareTo(pass)==0)
                return "200";
            else
                return "401";
            

        }
        
    }
}
