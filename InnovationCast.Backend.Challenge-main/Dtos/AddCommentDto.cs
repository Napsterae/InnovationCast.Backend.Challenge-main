using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Challenge.Dtos
{
    public class AddCommentDto
    {
        [Required]
        public string EntityIdentifier { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public string MarkedUpText { get; set; }
    }
}