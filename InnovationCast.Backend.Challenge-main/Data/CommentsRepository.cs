using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Backend.Challenge.Dtos;
using Backend.Challenge.Helpers;
using Backend.Challenge.Interfaces;
using Backend.Challenge.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Challenge.Data
{
    
    public class CommentsRepository : ICommentsRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public CommentsRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<PagedList<CommentResponseDto>> GetCommentsFromEntityAsync(CommentsParams commentsParams)
        {
            var query = _context.Comments.AsQueryable();

            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(commentsParams.TimeZoneId);

            query = query.Where(m => m.EntityIdentifier == commentsParams.EntityIdentifier).OrderByDescending(m => m.PublishDate);      

            return await PagedList<CommentResponseDto>.CreateAsync(query.ProjectTo<CommentResponseDto>(_mapper
                .ConfigurationProvider).AsNoTracking(), 
                    commentsParams.PageNumber, commentsParams.PageSize); 
        }

        public async Task<ActionResult<CommentResponseDto>> AddCommentAsync(AddCommentDto addCommentDto)
        {
            var comment = _mapper.Map<AppComment>(addCommentDto);

            comment.PublishDate = DateTime.UtcNow; //save with universal datetime

            var result = await _context.Comments.AddAsync(comment);

            await _context.SaveChangesAsync();

            if (comment.Id >0)
            {
                return _mapper.Map<CommentResponseDto>(comment);
            }else
                return null;

        }

        public async Task<PagedList<CommentResponseDto>> GetNewCommentsFromEntityAsync(NewCommentsParams commentsParams)
        {
            var query = _context.Comments.AsQueryable();

            //filter comments and show only the comments that do not exist on the list that was passed by the frontend, this comments should be the new comments
            query = query.Where(m => m.EntityIdentifier == commentsParams.EntityIdentifier).Where(m => !commentsParams.CommentsIds.Contains(m.Id)).OrderByDescending(m => m.PublishDate);

            return await PagedList<CommentResponseDto>.CreateAsync(query.ProjectTo<CommentResponseDto>(_mapper
                .ConfigurationProvider).AsNoTracking(), 
                    commentsParams.PageNumber, commentsParams.PageSize); 
        }
    }
}