using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public DateTime CreatedDate { get; set; }

        public DateTime? LastLoginDate { get; set; }

        public ICollection<Feed> Feeds { get; set; }
        public ICollection<UserFeed> FollowedFeeds { get; set; }
    }
}
