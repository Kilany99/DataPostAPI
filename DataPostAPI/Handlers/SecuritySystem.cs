using DataPostAPI.Enums;
using DataPostAPI.Models;
using DataPostAPI.Services;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DataPostAPI.Handlers
{

    public interface ISecuritySystem
    {
        void HandleAlarm(AnomalyEvent anomalyEvent);
        void HandleDoorLock(AnomalyEvent anomalyEvent);
        Task NotifySecurityGuard(AnomalyEvent anomalyEvent);
        Task LockdownFacility();
        Task TriggerAlarm();
        Task LockSpecificZone(string CameraId);
        Task IncreaseSurveillance(string CameraId);
    }
    public class SecuritySystem : ISecuritySystem
    {
        private readonly ILogger<SecuritySystem> _logger;
        private readonly INotificationService _notificationService;

        public SecuritySystem(ILogger<SecuritySystem> logger, INotificationService notificationService)
        {
            _logger = logger;
            _notificationService = notificationService;
        }

        public void HandleAlarm(AnomalyEvent anomalyEvent)
        {
            _logger.LogInformation($"Alarm triggered for {anomalyEvent.AnomalyType}");
            // Trigger alarm logic
        }

        public void HandleDoorLock(AnomalyEvent anomalyEvent)
        {
            _logger.LogInformation($"Locking doors for {anomalyEvent.AnomalyType}");
            // Door locking logic
        }

        public async Task NotifySecurityGuard(AnomalyEvent anomalyEvent)
        {
            var model = Data.SendNotification.GetDeviceIDFromDB(anomalyEvent.ZoneId, anomalyEvent.AnomalyType);
            await _notificationService.SendNotification(model);
        }
        public async Task LockdownFacility()
        {
            throw new System.NotImplementedException();
        }
        public async Task TriggerAlarm()
        {
            throw new System.NotImplementedException();
        }
        public async Task LockSpecificZone(string CameraId)
        {
            throw new System.NotImplementedException();
        }
        public async Task IncreaseSurveillance(string CameraId)
        {  throw new System.NotImplementedException(); }

    }
}
