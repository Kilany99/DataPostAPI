using System;

namespace DataPostAPI.Enums
{
    public enum AnomalyPriority
    {
        Low = 0,
        Medium = 1,
        High = 2,
        Critical = 3
    }

    public class AnomalyEvent
    {
        public string EventId { get; set; }
        public string CameraId { get; set; }
        public DateTime Timestamp { get; set; }
        public string AnomalyType { get; set; }
        public int ZoneId { get; set; }
        public AnomalyPriority Priority { get; set; }
        public string Description { get; set; }
    }
}
