using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace erkulSale.webui.Models
{
    public class ResetPasswordModel
    {
        [Required]
        public string Token { get; set; }

        // [Required]
        // [DataType(DataType.EmailAddress)]
        // public string Email { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string RePassword { get; set; }

    }
}