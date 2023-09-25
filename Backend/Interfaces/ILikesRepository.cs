using Backend.Model;
using System.Collections;

namespace Backend.Interfaces
{
    public interface ILikesRepository
    {
        Task<UserLike> GetUserLike(string sourceUserId, string targetUserId);
        Task<AppUser> GetUserWithLikes(string userId);
        Task<IEnumerable> GetUserLikes(string predicate,string userId);
    }
}
