using System.Linq;
using System.Threading.Tasks;
using Backend.Challenge.Dtos;
using Backend.Challenge.ServiceModels;
using Backend.Challenge.Interfaces;
using Backend.Challenge.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System.Collections.Generic;

namespace Backend.Challenge.Controllers
{   [ApiController]
    [Route("[controller]")]
    public class MainController : Controller
    {
        private readonly ICommentsRepository _context;
        private readonly IMapper _mapper;
        const int rowsPerPage = 10;

        public MainController(ICommentsRepository context,IMapper mapper)
        {
             _context = context;
             _mapper = mapper;
        }

        // This action responds to the url /main/users/42 and /main/users?id=4&id=10
        public GetUserResponse Users(int[] id)
        {
            return new GetUserResponse
            {
                Users = id.ToDictionary(i => i, i => new UserDto
                {
                    Id = i,
                    Username = $"User {i}",
                    Email = $"user-{i}@example.com"
                })
            };
        }

        // TODO: An action to return a paged list of comments
        [HttpGet]
        public async  Task<ActionResult<IEnumerable<CommentResponseDto>>> GetComments([FromQuery] CommentsParams commentsParams)
        { 
            var comments = await _context.GetCommentsFromEntityAsync(commentsParams);
            return Ok(comments);

            /*var lista = await _context.Comments.Where(m => m.EntityIdentifier.Equals(entity)).Skip(page * rowsPerPage).Take(rowsPerPage).ToListAsync();
            return Ok(_mapper.Map<CommentResponseDto>(lista));*/
        }

        // TODO: An action to add a comment
        //[HttpPost("add-comment")]
        //public async 

    }
}
