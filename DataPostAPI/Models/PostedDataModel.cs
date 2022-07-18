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
        public int PostedDataid { get; set; }

        [Required]
        [Column(TypeName = "varchar(MAX)")]
        [Display(Name = "Anomaly Screenshot")]
        public string CrimeScreenshot { get; set; }

        [Required]
        [Column(TypeName = "varchar(150)")]
        [Display(Name = "Anomaly Date and Time")]
        public string AnomalyDatetime { get; set; }

        [Required]
        [Column(TypeName = "varchar(150)")]
        [Display(Name = "Anomaly type")]
        public string AnomalyType { get; set; }

        [Required]
        [Column(TypeName = "varchar(15)")]
        [Display(Name = "Action Priority")]
        public string ActionPriority { get; set; }

        [ForeignKey("Camera")]
        [Display(Name = "Zone ID")]
        public int ZoneId { get; set; }
        
        public string Response { get; set; }
    }
}
