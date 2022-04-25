using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DataPostAPI.Models
{
    public class PostedDataModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Posted Data Id")]
        public int PostedDataId { get; set; }

        [Required]
        [Column(TypeName = "varchar(300)")]
        [Display(Name = "Anomaly Screenshot")]
        public string? CrimeScreenshot { get; set; }

        [Required]
        [Column(TypeName = "varchar(150)")]
        [Display(Name = "Anomaly Date and Time")]
        public string? AnomalyDateTime { get; set; }

        [Required]
        [Column(TypeName = "varchar(150)")]
        [Display(Name = "Anomaly type")]
        [MaxLength(100)]
        public string? AnomalyType { get; set; }

        [Required]
        [Column(TypeName = "varchar(15)")]
        [Display(Name = "Action Priority")]
        [MaxLength(10)]
        public string? ActionPriority { get; set; }

        [ForeignKey("Camera")]
        [Display(Name = "Zone ID")]
        [NotMapped]
        public int ZoneID { get; set; }
        
        public string respone { get; set; }
    }
}
