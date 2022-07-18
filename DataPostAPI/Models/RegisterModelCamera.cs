using System.ComponentModel.DataAnnotations;

namespace DataPostAPI.Models
{
    public class RegisterModelCamera
    {
        [Required]
        public int ZonePriority { get; set; }
        [Required]
        public string ZoneDescription { get; set; }
    }
}
