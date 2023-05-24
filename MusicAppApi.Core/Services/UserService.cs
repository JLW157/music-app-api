using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MusicAppApi.Core.Constants;
using MusicAppApi.Core.interfaces;
using MusicAppApi.Models.DbModels;
using MusicAppApi.Models.Responses;

namespace MusicAppApi.Core.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly MusicAppDbContext _context;

        public UserService(UserManager<User> userManager, MusicAppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<CreateUserResponse> CreateUser(User user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);

            return new CreateUserResponse() { Succeeded = result.Succeeded, User = user };
        }

        public async Task<IList<string>> GetUserRoles(User user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<bool> ConfirmEmail(User user, string token)
        {
            var res = await _userManager.ConfirmEmailAsync(user, token);

            return res.Succeeded;
        }

        public async Task<bool> CheckUserExistsByEmail(string email)
        {
            return await _context.Users.AnyAsync(x => x.Email == email);
        }

        public async Task<bool> CheckUserExistsByUsername(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName == username);
        }

        public async Task<bool> AddUserToRole(User user, string role)
        {
            return (await _userManager.AddToRoleAsync(user, role)).Succeeded;
        }

        public Task<string> GenerateConfirmEmailToken(User user)
        {
            return _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<User?> GetUserById(Guid Id)
        {
            return await _userManager.FindByIdAsync(Id.ToString());
        }

        // todo: rename this method
        public async Task<List<User>?> GetUsersBySpecificCase(List<string> artistsIds) =>
            await _context.Users.Where(a => artistsIds.Contains(a.Id.ToString())).ToListAsync();
    }
}
