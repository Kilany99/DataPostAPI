using DataPostAPI.Enums;
using DataPostAPI.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace DataPostAPI.Controllers
{
    public class AnomalyController : ControllerBase
    {
        private readonly AnomalyDetectionSystem _anomalySystem;
        private readonly SecuritySystem _securitySystem;

        public AnomalyController(
            AnomalyDetectionSystem anomalySystem,
            SecuritySystem securitySystem)
        {
            _anomalySystem = anomalySystem;
            _securitySystem = securitySystem;

            // Subscribe to events
            _anomalySystem.OnAnomalyDetected += async (anomalyEvent) =>
            {
                switch (anomalyEvent.Priority)
                {
                    case AnomalyPriority.Critical:
                        _securitySystem.HandleAlarm(anomalyEvent);
                        _securitySystem.HandleDoorLock(anomalyEvent);
                        await _securitySystem.NotifySecurityGuard(anomalyEvent);
                        break;
                    case AnomalyPriority.High:
                        _securitySystem.HandleAlarm(anomalyEvent);
                        await _securitySystem.NotifySecurityGuard(anomalyEvent);
                        break;
                        // Handle other priorities...
                }
            };
        }

        [HttpPost("detect")]
        public IActionResult DetectAnomaly([FromBody] AnomalyEvent anomalyEvent)
        {
            _anomalySystem.DetectAnomaly(anomalyEvent);
            return Ok();
        }
    }
}
