using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class Topic
    {
        public int Id { get; set; }  

        public string Name { get; set; }  
        public string Description { get; set; }  
        public int FeedId { get; set; } 

        public Feed Feed { get; set; }  
    }
}
