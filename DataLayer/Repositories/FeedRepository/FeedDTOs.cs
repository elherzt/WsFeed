using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.FeedRepository
{
    public class FeedDTOcreate
    {


        [Required(ErrorMessage = "Name field is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description field is required")]
        public string Description { get; set; }

        public bool IsPrivate { get; set; } = false;
    }

    public class FeedDTOPublicSearch()
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }


    public class FeedDTOResponse
    {
        public FeedDTOResponse()
        {
        }

        public FeedDTOResponse(Feed feed)
        {
            this.Id = feed.Id;
            this.Description = feed.Description;
            this.Name = feed.Name;
            this.IsPrivate = feed.IsPrivate;
            this.TopicsCount = feed.Topics.Count;
            this.Topics = string.Join(", ", feed.Topics.Select(x => x.Name));
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsPrivate { get; set; }
        public int TopicsCount { get; set; }
        public string Topics { get; set; }
    }

    public class FeedDTODelete
    {
        [Required(ErrorMessage = "Id field is required")]
        public int Id { get; set; }
    }
}

