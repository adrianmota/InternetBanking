using InternetBanking.Core.Application.ViewModels.User;
using InternetBanking.Core.Domain.Entities;
using StockApp.Core.Application.Dtos.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<AuthenticationResponse> LoginAsync(LoginViewModel login);
        Task LogOut();
    }
}