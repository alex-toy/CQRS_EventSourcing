﻿using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.Data;

namespace Post.Query.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly DatabaseContextFactory _contextFactory;

        public PostRepository(DatabaseContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task CreateAsync(PostDb post)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            context.Posts.Add(post);

            _ = await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid postId)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            var post = await GetByIdAsync(postId);

            if (post == null) return;

            context.Posts.Remove(post);
            _ = await context.SaveChangesAsync();
        }

        public async Task<List<PostDb>> ListByAuthorAsync(string author)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            return await context.Posts.AsNoTracking()
                    .Include(i => i.Comments).AsNoTracking()
                    .Where(x => x.Author.Contains(author))
                    .ToListAsync();
        }

        public async Task<PostDb> GetByIdAsync(Guid postId)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            return await context.Posts
                    .Include(i => i.Comments)
                    .FirstOrDefaultAsync(x => x.PostId == postId);
        }

        public async Task<List<PostDb>> GetAllAsync()
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            return await context.Posts.AsNoTracking()
                    .Include(i => i.Comments).AsNoTracking()
                    .ToListAsync();
        }

        public async Task<List<PostDb>> ListWithCommentsAsync()
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            return await context.Posts.AsNoTracking()
                    .Include(i => i.Comments).AsNoTracking()
                    .Where(x => x.Comments != null && x.Comments.Any())
                    .ToListAsync();
        }

        public async Task<List<PostDb>> ListWithLikesAsync(int numberOfLikes)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            return await context.Posts.AsNoTracking()
                    .Include(i => i.Comments).AsNoTracking()
                    .Where(x => x.Likes >= numberOfLikes)
                    .ToListAsync();
        }

        public async Task UpdateAsync(PostDb post)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            context.Posts.Update(post);

            _ = await context.SaveChangesAsync();
        }
    }
}