using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using DataPostAPI.Services;
using DataPostAPI.Helpers;
using DataPostAPI.Models;

namespace DataPostAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private IClientService _clientService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public ClientsController(
            IClientService clientService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _clientService = clientService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticateModel model)
        {
            var client = _clientService.Authenticate(model.Username, model.Password);

            if (client == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, client.ClientId.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info and authentication token
            return Ok(new
            {
                Id = client.ClientId,
                Username = client.ClientName,
                FirstName = client.FirstName,
                LastName = client.LastName,
                ZoneId = client.ZoneID,
                Token = tokenString
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterModelClient model)
        {
            // map model to entity
            var client = _mapper.Map<Client>(model);

            try
            {
                // create client
                _clientService.Create(client, model.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var clients = _clientService.GetAll();
            var model = _mapper.Map<IList<ClientModel>>(clients);
            return Ok(model);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var client = _clientService.GetById(id);
            var model = _mapper.Map<ClientModel>(client);
            return Ok(model);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UpdateModelClient model)
        {
            // map model to entity and set id
            var client = _mapper.Map<Client>(model);
            client.ClientId = id;

            try
            {
                // update user 
                _clientService.Update(client, model.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _clientService.Delete(id);
            return Ok();
        }
    }
}
