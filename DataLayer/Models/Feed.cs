using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class Feed
    {
        public int Id { get; set; }  
        
        public string Name { get; set; }  
        public string Description { get; set; }  
        public bool IsPrivate { get; set; }  
        public int UserId { get; set; }  

        public User User { get; set; }  
        public ICollection<Topic> Topics { get; set; }
        public ICollection<UserFeed> FollowingUsers { get; set; } 
    }
}
