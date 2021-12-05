using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Challenge.Dtos
{
    public class CommentResponseDto
    {
        public int Id { get; set;}
        public string Author { get; set; }
        public DateTime PublishDate { get; set; }
        public string MarkedUpText { get; set; }

    }
}