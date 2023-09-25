using Backend.DTOs;
using Backend.Extensions;
using Backend.Interfaces;
using Backend.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Runtime.CompilerServices;

namespace Backend.Data
{
    public class LikesRepository : ILikesRepository
    {
        private readonly AppDbContext context;

        public LikesRepository(AppDbContext context)
        {
            this.context = context;
        }
        public async Task<UserLike> GetUserLike(string sourceUserId, string targetUserId)
        {
            return await context.Likes.FindAsync(sourceUserId, targetUserId);
        }

        public async Task<IEnumerable> GetUserLikes(string predicate, string userId)
        {
            var users = context.Users.OrderBy(u => u.UserName).AsQueryable();
            var likes = context.Likes.AsQueryable();

            if (predicate == "liked")
            {
                likes = likes.Where(like => like.SourceUserId == userId);
                users = likes.Select(like => like.TargetUser);
            }

            if (predicate == "likedBy")
            {
                likes = likes.Where(like => like.TargetUserId == userId);
                users = likes.Select(like => like.SourceUser);
            }

            return await users.Select(user => new LikeDto
            {
                UserName = user.UserName,
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain).Url,
                KnownAs = user.KnownAs,
                Age = user.DateOfBirth.CalculateAge(),
                City = user.City,
                Id = user.Id,
            }).ToListAsync();
        }

        public async Task<AppUser> GetUserWithLikes(string userId)
        {
            return await context.Users
                .Include(x => x.LikedUsers)
                .FirstOrDefaultAsync(x => x.Id == userId);
        }
    }
}
