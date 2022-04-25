using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DataPostAPI.Models
{
    public class Client
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Client ID")]
        public int ClientId { get; set; }

        [Required]
        [Column(TypeName = "varchar(15)")]
        [Display(Name = "Client Username")]
        [MaxLength(8)]
        public string? ClientName { get; set; }

        [Required]
        [Column(TypeName = "varchar(20)")]
        [Display(Name = "Client Password")]
        [MaxLength(16)]
        public string? Password { get; set; }
        [ForeignKey("Camera")]
        [Display(Name = "Zone Name")]
        [NotMapped]
        public int ZoneID { get; set; }

        [Column(TypeName = "varchar(40)")]
        public string? DeviceToken { get; set; }

    }
}
