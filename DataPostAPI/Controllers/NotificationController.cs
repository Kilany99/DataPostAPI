using DataPostAPI.Models;
using DataPostAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataPostAPI.Controllers
{
    [Route("api/notification")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        
        protected readonly INotificationService _notificationService;
        
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [Route("send")]
        [HttpPost]
        public async Task<ResponseModel> SendNotification([FromForm] string zone, [FromForm] string anomalyType)
        {
            NotificationModel nm = Data.SendNotification.GetDeviceIDFromDB(Int32.Parse(zone), anomalyType);
            Console.WriteLine(nm.DeviceId.ToString());
            var result = await _notificationService.SendNotification(nm);
            return result;
        }
    }
}
