using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DataPostAPI.Models
{
    [Table("Action", Schema = "dbo")]
    public class Action
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Action ID")]

        public int ActionId { get; set; }

        [Required]
        [Column(TypeName = "varchar(150)")]
        [Display(Name = "Action type")]
        [MaxLength(100)]
        public string ActionType { get; set; }

        [Required]
        [Column(TypeName = "varchar(15)")]
        [Display(Name = "MCU ID")]
        [MaxLength(8)]
        public string McuId { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Action Date and Time")]
        public string ActionDatetime { get; set; }

        [ForeignKey("Client")]
        public int ClientId { get; set; }

        public virtual Client? client { get; set; }
    }
}
