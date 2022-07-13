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
        public async Task<LoginModel> Login([FromBody] LoginModel loginModel)
        {
            Console.WriteLine(loginModel.userId +"\n" + loginModel.Password);
            string userId = loginModel.userId;
            string pass = loginModel.Password;
            Client clientFromDB = LoginDB.LoginToDB(userId);
            if(clientFromDB.ClientName.CompareTo("not found!")!=0)
            {
                if (String.Equals(clientFromDB.ClientName,userId) && String.Equals(clientFromDB.Password, pass))
                {
                    loginModel.ResponseCode = "200";
                    loginModel.ResponseMessage = "Success!";

                    return loginModel;
                }
                else
                {
                    loginModel.ResponseCode = "401";
                    loginModel.ResponseMessage = "not Success! 1";
                    return loginModel;

                }
            }
            else
            {
                loginModel.ResponseCode = "401";
                loginModel.ResponseMessage = "not Success! 2";
                return loginModel;

            }
            

        }
        
    }
}
