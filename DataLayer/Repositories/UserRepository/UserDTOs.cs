using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.UserRepository
{
    public class UserDTOResponse
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name field is required")]
        [StringLength(70, ErrorMessage = "Name cannot be longer than 70 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email field is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Mail { get; set; }
        
    }

    public class UserDTORegister
    {
       
        [Required(ErrorMessage = "Name field is required")]
        [StringLength(70, ErrorMessage = "Name cannot be longer than 70 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email field is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Mail { get; set; }

        [Required(ErrorMessage = "Password field is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 8 characters long")]
        public string Password { get; set; }
    }

    public class UserDTOLogin
    {
        [Required]
        [EmailAddress]
        public string Mail { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
