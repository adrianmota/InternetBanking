using InternetBanking.Core.Application.ViewModels.User;
using StockApp.Core.Application.Dtos.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Interfaces.Services
{
    public interface IAccountService
    {
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);
        Task<string> SetUserStatusAsync(string userId);
        Task<RegisterResponse> RegisterAdminUserAsync(RegisterRequest request);
        Task<RegisterResponse> RegisterClientUserAsync(RegisterRequest request);
        Task<RegisterResponse> UpdateUserAsync(RegisterRequest request);
        Task<List<UserViewModel>> GetAllUsers();
        Task<SaveUserViewModel> GetByIdSaveViewModel(string id);
        Task DeleteUserAsync(string id);
        Task SignOutAsync();
    }
}