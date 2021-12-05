using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Backend.Challenge.Dtos;
using Backend.Challenge.ServiceModels;
using Backend.Challenge.Interfaces;
using Backend.Challenge.Helpers;
using Backend.Challenge.Entities;

namespace Backend.Challenge.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommentsController : Controller
    {
        private readonly ICommentsRepository _CommentsRep;
        private readonly IMapper _mapper;
        const int rowsPerPage = 10;

        public CommentsController(ICommentsRepository CommentRep,IMapper mapper)
        {
             _CommentsRep = CommentRep;
             _mapper = mapper;
        }

        [HttpGet]
        public async  Task<ActionResult<IEnumerable<CommentResponseDto>>> GetComments([FromQuery] CommentsParams commentsParams)
        { 
            var comments = await _CommentsRep.GetCommentsFromEntityAsync(commentsParams);
            return Ok(comments);

            /*var lista = await _context.Comments.Where(m => m.EntityIdentifier.Equals(entity)).Skip(page * rowsPerPage).Take(rowsPerPage).ToListAsync();
            return Ok(_mapper.Map<CommentResponseDto>(lista));*/
        }

        [HttpGet("new-comments")]
        public async Task<ActionResult<CommentResponseDto>> GetNewComments([FromQuery] NewCommentsParams commentsParams)
        {
            var comments = await _CommentsRep.GetNewCommentsFromEntityAsync(commentsParams);
            return Ok(comments);

        }
 
        [HttpPost("add-comment")]
        public async Task<ActionResult<CommentResponseDto>> AddComment(AddCommentDto addCommentDto)
        {
            var result = await _CommentsRep.AddCommentAsync(addCommentDto);

            if (!(result is null))
                return result;
            else
                return BadRequest("Comment wrong format!");
        } 



    } 
}