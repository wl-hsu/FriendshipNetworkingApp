using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _datacontext;

        public UserRepository(DataContext datacontext)
        {
            _datacontext = datacontext;
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _datacontext.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _datacontext.Users.SingleOrDefaultAsync(x => x.UserName == username.ToLower());
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _datacontext.Users.ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _datacontext.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            _datacontext.Entry(user).State = EntityState.Modified;
        }
    }
}