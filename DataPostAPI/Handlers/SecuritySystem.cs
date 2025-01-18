using DataPostAPI.Enums;
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
            await _notificationService.SendNotification(
                "Security Guard",
                $"Anomaly detected: {anomalyEvent.AnomalyType} at {anomalyEvent.Timestamp}"
            );
        }
    }
}
