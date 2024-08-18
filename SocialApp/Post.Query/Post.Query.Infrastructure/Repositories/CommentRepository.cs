﻿using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Entities.Posts;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.Data;

namespace Post.Query.Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly DatabaseContextFactory _contextFactory;

        public CommentRepository(DatabaseContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<CommentDb> GetByIdAsync(Guid commentId)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();

            return await context.Comments.FirstOrDefaultAsync(x => x.CommentId == commentId);
        }

        public async Task UpdateAsync(CommentDb comment)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            context.Comments.Update(comment);

            _ = await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid commentId)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            var comment = await GetByIdAsync(commentId);

            if (comment == null) return;

            context.Comments.Remove(comment);
            _ = await context.SaveChangesAsync();
        }

        public async Task CreateAsync(CommentDb comment)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            context.Comments.Add(comment);

            _ = await context.SaveChangesAsync();
        }
    }
}