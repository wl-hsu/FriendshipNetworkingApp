using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _datacontext;

        private readonly IMapper _mapper;

        public UserRepository(DataContext datacontext, IMapper mapper)
        {
            _datacontext = datacontext;
            _mapper = mapper;
        }

        public async Task<MemberDto> GetMemberAsync(string username)
        {
            return await _datacontext.Users
                .Where(x => x.UserName == username.ToLower())
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
        {
            var query = _datacontext.Users.AsQueryable();
            query = query.Where(x => x.UserName != userParams.CurrentUsername);
            query = query.Where(x => x.Gender == userParams.Gender);

            var minDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MaxAge - 1));
            var maxDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MinAge));
            query = query.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);

            query = userParams.OrderBy switch 
            {
                "created" => query.OrderByDescending(u => u.Created),
                _ => query.OrderByDescending(u => u.LastActive)
            };

            return await PagedList<MemberDto>.CreateAsync(query.AsNoTracking().ProjectTo<MemberDto>(_mapper.ConfigurationProvider),
                                             userParams.PageNumber,
                                              userParams.PageSize);
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _datacontext.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _datacontext.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.UserName == username.ToLower());
        }

        public async Task<string> GEtUserGender(string username)
        {
            return await _datacontext.Users.Where(x => x.UserName == username).Select(x => x.Gender).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _datacontext.Users
                .Include(p => p.Photos)
                .ToListAsync();
        }

        public void Update(AppUser user)
        {
            _datacontext.Entry(user).State = EntityState.Modified;
        }
    }
}