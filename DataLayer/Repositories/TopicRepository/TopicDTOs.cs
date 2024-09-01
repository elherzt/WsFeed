using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.TopicRepository
{
    public class TopicDTOCreate
    {
        [Required(ErrorMessage = "FeedId field is required")]
        public int FeedId { get; set; }

        [Required(ErrorMessage = "Name field is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description field is required")]
        public string Description { get; set; }
    }

    public class TopicDTOResponse
    {
        public TopicDTOResponse()
        {
        }

        public TopicDTOResponse(Topic topic)
        {
            this.Id = topic.Id;
            this.Name = topic.Name;
            this.Description = topic.Description;
            this.FeedId = topic.FeedId;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int FeedId { get; set; }
    }
}
