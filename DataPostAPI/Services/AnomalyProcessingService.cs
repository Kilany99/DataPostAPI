using DataPostAPI.Handlers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System;
using DataPostAPI.Data;
using DataPostAPI.Models;
using DataPostAPI.Enums;

namespace DataPostAPI.Services
{
    public class AnomalyProcessingService : BackgroundService
    {
        private readonly ISecuritySystem _securitySystem;
        private readonly INotificationService _notificationService;
        private readonly ILogger<AnomalyProcessingService> _logger;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public AnomalyProcessingService(
            ISecuritySystem securitySystem,
            INotificationService notificationService,
            ILogger<AnomalyProcessingService> logger,
            IConfiguration configuration,
            HttpClient httpClient)
        {
            _securitySystem = securitySystem;
            _notificationService = notificationService;
            _logger = logger;
            _configuration = configuration;
            _httpClient = httpClient;
        }

        private async Task ProcessEvent(AnomalyEvent anomalyEvent)
        {
            try
            {
                _logger.LogInformation($"Starting to process anomaly event: {anomalyEvent.EventId}");
                var model = SendNotification.GetDeviceIDFromDB(anomalyEvent.ZoneId, anomalyEvent.AnomalyType);
                // Create notification model
                var notification = new NotificationModel
                {
                    Title = GetNotificationTitle(anomalyEvent),
                    Body = GetNotificationBody(anomalyEvent),
                    DeviceId = model.DeviceId, 
                    IsAndroiodDevice = true 
                };

                switch (anomalyEvent.Priority)
                {
                    case AnomalyPriority.Critical:
                        await HandleCriticalEvent(anomalyEvent, notification);
                        break;

                    case AnomalyPriority.High:
                        await HandleHighPriorityEvent(anomalyEvent, notification);
                        break;

                    case AnomalyPriority.Medium:
                        await HandleMediumPriorityEvent(anomalyEvent, notification);
                        break;

                    case AnomalyPriority.Low:
                        await HandleLowPriorityEvent(anomalyEvent, notification);
                        break;

                    default:
                        _logger.LogWarning($"Unhandled priority level for event: {anomalyEvent.EventId}");
                        break;
                }

                await LogEventProcessing(anomalyEvent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error processing anomaly event {anomalyEvent.EventId}");
                await HandleProcessingError(anomalyEvent, ex);
            }
        }

        private async Task HandleCriticalEvent(AnomalyEvent anomalyEvent, NotificationModel notification)
        {
            // Execute all actions in parallel for fastest response
            var tasks = new List<Task>
            {
                _securitySystem.LockdownFacility(),
                _securitySystem.TriggerAlarm(),
                SendEmergencyNotifications(notification),
                NotifyLawEnforcement(anomalyEvent),
                ActivateEmergencyProtocol(anomalyEvent)
            };

            await Task.WhenAll(tasks);
        }

        private async Task HandleHighPriorityEvent(AnomalyEvent anomalyEvent, NotificationModel notification)
        {
            await _securitySystem.TriggerAlarm();
            await _securitySystem.LockSpecificZone(anomalyEvent.CameraId);
            await SendPriorityNotification(notification);
        }

        private async Task HandleMediumPriorityEvent(AnomalyEvent anomalyEvent, NotificationModel notification)
        {
            await _securitySystem.IncreaseSurveillance(anomalyEvent.CameraId);
            await SendStandardNotification(notification);
        }

        private async Task HandleLowPriorityEvent(AnomalyEvent anomalyEvent, NotificationModel notification)
        {
            await SendStandardNotification(notification);
        }

        private async Task SendEmergencyNotifications(NotificationModel notification)
        {
            try
            {
                // Send to Firebase Cloud Messaging
                var fcmToken = _configuration["FCM:ServerKey"];
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("key", fcmToken);

                var fcmNotification = new
                {
                    to = notification.DeviceId,
                    priority = "high",
                    notification = new
                    {
                        title = notification.Title,
                        body = notification.Body,
                        sound = "emergency_alert.wav"
                    },
                    data = new
                    {
                        priority = "critical",
                        click_action = "OPEN_SECURITY_SCREEN"
                    }
                };

                var json = JsonConvert.SerializeObject(fcmNotification);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("https://fcm.googleapis.com/fcm/send", content);
                response.EnsureSuccessStatusCode();

                _logger.LogInformation($"Emergency notification sent successfully to device: {notification.DeviceId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send emergency notification");
                // Implement fallback notification method
                await SendFallbackNotification(notification);
            }
        }

        private async Task SendPriorityNotification(NotificationModel notification)
        {
            // Similar to SendEmergencyNotifications but with different priority and sound
        }

        private async Task SendStandardNotification(NotificationModel notification)
        {
            // Similar to SendEmergencyNotifications but with normal priority
        }

        private string GetNotificationTitle(AnomalyEvent anomalyEvent)
        {
            return anomalyEvent.Priority switch
            {
                AnomalyPriority.Critical => $"CRITICAL ALERT: {anomalyEvent.AnomalyType}",
                AnomalyPriority.High => $"HIGH PRIORITY: {anomalyEvent.AnomalyType}",
                AnomalyPriority.Medium => $"Security Alert: {anomalyEvent.AnomalyType}",
                AnomalyPriority.Low => $"Notice: {anomalyEvent.AnomalyType}",
                _ => $"Security Alert: {anomalyEvent.AnomalyType}"
            };
        }

        private string GetNotificationBody(AnomalyEvent anomalyEvent)
        {
            return $"Location: Camera {anomalyEvent.CameraId}\n" +
                   $"Time: {anomalyEvent.Timestamp:HH:mm:ss}\n" +
                   $"Details: {anomalyEvent.Description}";
        }

        private async Task NotifyLawEnforcement(AnomalyEvent anomalyEvent)
        {
            // Implement law enforcement notification logic
            // This could be through a dedicated API, email, or automated phone call
        }

        private async Task ActivateEmergencyProtocol(AnomalyEvent anomalyEvent)
        {
            // Implement emergency protocol activation
            // This could include activating backup systems, emergency lights, etc.
        }

        private async Task LogEventProcessing(AnomalyEvent anomalyEvent)
        {
            // Log to database or external logging service
            // Implementation here
        }

        private async Task HandleProcessingError(AnomalyEvent anomalyEvent, Exception ex)
        {
            // Implement error handling logic
            // This could include retrying, logging to a special error queue, etc.
        }

        private async Task SendFallbackNotification(NotificationModel notification)
        {
            // Implement fallback notification method (SMS, email, etc.)
            // Implementation here
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }
    }
}
