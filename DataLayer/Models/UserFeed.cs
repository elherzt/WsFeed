using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class UserFeed
    {
        public int UserId { get; set; }     
        public int FeedId { get; set; }    

        public User User { get; set; }     
        public Feed Feed { get; set; }     
    }

}
