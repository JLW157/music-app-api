using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicAppApi.Core.interfaces.Repositories;
using MusicAppApi.Models.DbModels;
using MusicAppApi.Models.Responses;

namespace MusicAppApi.Core.interfaces.Services
{
    public interface IUserService
    {
        Task<User?> GetUserByEmail(string email);
        Task<CreateUserResponse> CreateUser(User user, string password);
        Task<IList<string>> GetUserRoles(User user);
        Task<bool> CheckPasswordAsync(User user, string password);
        Task<bool> ConfirmEmail(User user, string token);

        Task<bool> CheckUserExistsByEmail(string email);
        Task<bool> CheckUserExistsByUsername(string username);
        Task<bool> AddUserToRole(User user, string role);
        Task<string> GenerateConfirmEmailToken(User user);
        Task<User?> GetUserById(Guid Id);
        Task<List<User>?> GetUsersBySpecificCase(List<string> artistsIds);
    }
}
