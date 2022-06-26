using DataPostAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataPostAPI.Data;
namespace DataPostAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientAnomalyData : ControllerBase
    {
        [HttpGet]
        [Route("{id}")]
        public async Task<List<PostedDataModel>> Get(string id)
        {

            List<PostedDataModel> values = GetValuesDB.GetAllValuesFromDB(id);
            return values;
            
        }

    }
}
