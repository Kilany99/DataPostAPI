using DataPostAPI.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace DataPostAPI.Models
{
    public class AnomalyEvent
    {
        [Key]
        public string EventId { get; set; }
        public string CameraId { get; set; }
        public DateTime Timestamp { get; set; }
        public string AnomalyType { get; set; }
        public int ZoneId { get; set; }
        public AnomalyPriority Priority { get; set; }
        public string Description { get; set; }
    }
}
