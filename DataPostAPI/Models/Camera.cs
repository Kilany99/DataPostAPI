using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DataPostAPI.Models
{
    [Table("Camera", Schema = "dbo")]
    public class Camera
    {

        [Key]
        [Display(Name = "camera zone ID")]
        public int CameraZoneID { get; set; }

        [Required]
        [Display(Name = "Zone Priority")]
        [Column(TypeName = "int")]
        public int ZonePriority { get; set; }

        [Required]
        [Display(Name = "Zone Description")]
        [Column(TypeName = "varchar(150)")]
        public string? ZoneDescription { get; set; }


    }
}
