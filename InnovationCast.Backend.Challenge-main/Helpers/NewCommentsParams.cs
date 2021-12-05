using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Challenge.Helpers
{
    public class NewCommentsParams : PaginationParams
    {
        [Required]
        public string EntityIdentifier { get; set; }
        [Required]
        public string TimeZoneId { get; set; } = "Greenwich Standard Time";
        [Required]
        public List<int> CommentsIds { get; set; }
    }
}