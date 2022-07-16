using System.ComponentModel.DataAnnotations;

namespace DataPostAPI.Models
{
    public class RegisterModelClient
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Clientname { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public int ZoneID { get; set; }

    }
}
