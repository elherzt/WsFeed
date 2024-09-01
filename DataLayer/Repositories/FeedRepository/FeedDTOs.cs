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
}
