using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Challenge.Helpers;
using Backend.Challenge.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Challenge.Interfaces
{
    public interface ICommentsRepository
    {
        Task<PagedList<CommentResponseDto>> GetCommentsFromEntityAsync(CommentsParams Params);
        Task<ActionResult<CommentResponseDto>> AddCommentAsync(AddCommentDto addCommentDto);
        Task<PagedList<CommentResponseDto>> GetNewCommentsFromEntityAsync(NewCommentsParams Params);
    }
}