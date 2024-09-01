using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name field is required")]
        [StringLength(70, ErrorMessage = "Name cannot be longer than 70 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email field is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Mail { get; set; }

        [Required(ErrorMessage = "Password field is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 8 characters long")]
        public string Password { get; set; }
        


        public DateTime CreatedDate { get; set; }

        public DateTime? LastLoginDate { get; set; }

        public ICollection<Feed> Feeds { get; set; }
        public ICollection<UserFeed> FollowedFeeds { get; set; }
    }
}
