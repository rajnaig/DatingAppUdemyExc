using AutoMapper;
using AutoMapper.QueryableExtensions;
using Backend.DTOs;
using Backend.Helpers;
using Backend.Interfaces;
using Backend.Model;
using Microsoft.EntityFrameworkCore;
using CloudinaryDotNet.Actions;

namespace Backend.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;

        public UserRepository(AppDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<AppUser> GetUserByIdAsync(string id)
        {
            return await context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetByUsernameAsync(string username)
        {
            return await context.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.UserName == username);
        }

        public Task<MemberDto> GetMemberAsync(string username)
        {
            return context.Users
                .Where(x => x.UserName == username)
                .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
        {
            var query = context.Users.AsQueryable();

            query = query.Where(u => u.UserName != userParams.CurrentUsername);
            query = query.Where(u => u.Gender == userParams.Gender);

            var minDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MaxAge - 1));
            var maxDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MinAge));

            query = query.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);

            query = userParams.OrderBy switch
            {
                "created" => query.OrderByDescending(x => x.Created),
                _ => query.OrderByDescending(u => u.LastActive),
            };


            return await PagedList<MemberDto>
                .CreateAsync(
                    query.AsNoTracking().ProjectTo<MemberDto>(mapper.ConfigurationProvider),
                    userParams.PageNumber, userParams.PageSize);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await context.Users
                .Include(p => p.Photos)
                .ToListAsync();

        }

        public async Task<bool> SaveAllAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            context.Entry(user).State = EntityState.Modified;

        }
    }
}
