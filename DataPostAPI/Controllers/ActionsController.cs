using AutoMapper;
using DataPostAPI.Data;
using DataPostAPI.Helpers;
using DataPostAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace DataPostAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActionsController : ControllerBase
    {
       
        private ClientContext _context;

        public ActionsController(ClientContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IEnumerable<Models.Action> GetAll()
        {
            return _context.Actions;
            
        }


    }
}
