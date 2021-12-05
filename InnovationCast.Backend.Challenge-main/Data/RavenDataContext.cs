using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Backend.Challenge.Dtos;
using Backend.Challenge.Entities;
using Backend.Challenge.Helpers;
using Backend.Challenge.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;


namespace Backend.Challenge.Data
{
    public class RavenDataContext : ICommentsRepository
    {
        private readonly IMapper _mapper;
        private readonly IDocumentStore _store;

        public RavenDataContext(IMapper Mapper,RavenStore RStore)
        {
            _mapper = Mapper;
            _store = RStore.Store;
        }

        public async Task<PagedList<CommentResponseDto>> GetCommentsFromEntityAsync(CommentsParams commentsParams)
        {
            using (IDocumentSession session = _store.OpenSession())  // Open a session for a default 'Database'
            {
                var query = session
                            .Query<AppComment>()                               // Query for Comments
                            .Where(m => m.EntityIdentifier == commentsParams.EntityIdentifier).OrderByDescending(m => m.PublishDate);                                    // Materialize query

                return await PagedList<CommentResponseDto>.CreateAsync(query.ProjectTo<CommentResponseDto>(_mapper
                .ConfigurationProvider).AsNoTracking(), 
                    commentsParams.PageNumber, commentsParams.PageSize); 
            }
        }

        public async Task<ActionResult<CommentResponseDto>> AddCommentAsync(AddCommentDto addCommentDto)
        {
            var comment = _mapper.Map<AppComment>(addCommentDto);

            comment.PublishDate = DateTime.UtcNow; //save with universal datetime

            using (IDocumentSession session = _store.OpenSession())  // Open a session for a default 'Database'
            {
                await Task.Run(
                    ()=> {
                        session.Store(comment);
                        session.SaveChanges();
                        }
                    );
            }
            if (comment.Id >0)
            {
                return _mapper.Map<CommentResponseDto>(comment);
            }else
                return null;

        }

        public async Task<PagedList<CommentResponseDto>> GetNewCommentsFromEntityAsync(NewCommentsParams Params)
        {
            using (IDocumentSession session = _store.OpenSession())  // Open a session for a default 'Database'
            {
                //filter comments and show only the comments that do not exist on the list that was passed by the frontend, this comments should be the new comments
                var query = session
                            .Query<AppComment>()
                            .Where(m => m.EntityIdentifier == Params.EntityIdentifier)
                            .Where(m => !Params.CommentsIds.Contains(m.Id))
                            .OrderByDescending(m => m.PublishDate);

                return await PagedList<CommentResponseDto>.CreateAsync(query.ProjectTo<CommentResponseDto>(_mapper
                    .ConfigurationProvider).AsNoTracking(), 
                        Params.PageNumber, Params.PageSize); 

            }
        }
    }
}