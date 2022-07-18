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
    public class CamerasController : ControllerBase
    {
        private ICameraService _cameraService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public CamerasController(
            ICameraService cameraService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _cameraService = cameraService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }


        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterModelCamera model)
        {
            // map model to entity
            var camera = _mapper.Map<Camera>(model);

            try
            {
                // create client
                _cameraService.Create(camera);
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
            var cameras = _cameraService.GetAll();
            var model = _mapper.Map<IList<Camera>>(cameras);
            return Ok(model);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var camera = _cameraService.GetById(id);
            var model = _mapper.Map<Camera>(camera);
            return Ok(model);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UpdateModelCamera model)
        {
            // map model to entity and set id
            var camera = _mapper.Map<Camera>(model);
            camera.CameraZoneid = id;

            try
            {
                // update camera zone 
                _cameraService.Update(camera);
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
            _cameraService.Delete(id);
            return Ok();
        }
    }
}
