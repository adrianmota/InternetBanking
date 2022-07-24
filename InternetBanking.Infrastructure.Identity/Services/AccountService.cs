using AutoMapper;
using InternetBanking.Core.Application.Enums;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.User;
using InternetBanking.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using StockApp.Core.Application.Dtos.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Infrastructure.Identity.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;

        public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request)
        {
            AuthenticationResponse response = new();

            var user = await _userManager.FindByNameAsync(request.UserName);

            if (user == null)
            {
                response.HasError = true;
                response.Error = $"No Accounts registered with username {request.UserName}";
                return response;
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = $"Invalid credentials for {request.UserName}";
                return response;
            }

            if (!user.EmailConfirmed)
            {
                response.HasError = true;
                response.Error = $"Account no activated to {request.UserName}";
                return response;
            }

            response.Id = user.Id;
            response.Email = user.Email;
            response.Username = user.UserName;

            var roles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

            response.Roles = roles.ToList();
            response.IsVerified = user.EmailConfirmed;

            return response;
        }

        public async Task<string> SetUserStatusAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return "No accounts registered with this user";
            }

            var active = user.EmailConfirmed;
            user.EmailConfirmed = !active;
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return $"An error occurred while activating {user.Email}.";
            }

            return $"Account activated for {user.Email}.";
        }

        public async Task<RegisterResponse> RegisterAdminUserAsync(RegisterRequest request)
        {
            RegisterResponse response = new();
            response.HasError = false;

            var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);

            if (userWithSameUserName != null)
            {
                response.HasError = true;
                response.Error = $"Username {request.UserName} is already taken";
                return response;
            }

            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);

            if (userWithSameEmail != null)
            {
                response.HasError = true;
                response.Error = $"Email {request.Email} is already registered";
                return response;
            }

            var user = new ApplicationUser
            {
                Email = request.Email,
                UserName = request.UserName,
                FirstName = request.FirstName,
                LastName = request.LastName,
                DNI = request.DNI
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Roles.Admin.ToString());
            }
            else
            {
                response.HasError = true;
                response.Error = $"An error occurred trying to register the user";
                return response;
            }

            return response;
        }

        public async Task<RegisterResponse> RegisterClientUserAsync(RegisterRequest request)
        {
            RegisterResponse response = new();
            response.HasError = false;

            var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);

            if (userWithSameUserName != null)
            {
                response.HasError = true;
                response.Error = $"Username {request.UserName} is already taken";
                return response;
            }

            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);

            if (userWithSameEmail != null)
            {
                response.HasError = true;
                response.Error = $"Email {request.Email} is already registered";
                return response;
            }

            var user = new ApplicationUser
            {
                Email = request.Email,
                UserName = request.UserName,
                FirstName = request.FirstName,
                LastName = request.LastName,
                DNI = request.DNI
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Roles.Client.ToString());
            }
            else
            {
                response.HasError = true;
                response.Error = $"An error occurred trying to register the user";
                for(int i = 0; i < result.Errors.Count(); i++)
                {
                    response.Error += $"\n{result.Errors.ElementAt(i).Description}";
                }
                return response;
            }

            return new RegisterResponse()
            {
                UserId = await _userManager.GetUserIdAsync(user)
            };
        }

        public async Task<RegisterResponse> UpdateUserAsync(RegisterRequest request)
        {
            RegisterResponse response = new();
            response.HasError = false;

            ApplicationUser user = await _userManager.FindByIdAsync(request.Id);

            if (user == null)
            {
                response.HasError = true;
                response.Error = "User not found";
                return response;
            }

            if (request.Email != user.Email)
            {
                ApplicationUser userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);

                if (userWithSameEmail != null)
                {
                    response.HasError = true;
                    response.Error = "An user exists with the same email";
                    return response;
                }
            }

            if (request.UserName != user.UserName)
            {
                ApplicationUser userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);

                if (userWithSameUserName != null)
                {
                    response.HasError = true;
                    response.Error = "An user exists with the same username";
                    return response;
                }
            }

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.UserName = request.UserName;
            user.Email = request.Email;
            user.DNI = request.DNI;

            var result = await _userManager.UpdateAsync(user);
            var passResult = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.Password);

            if (!result.Succeeded || !passResult.Succeeded)
            {
                response.HasError = true;
                response.Error = "An error occurred during the update of the user";
                return response;
            }

            return response;
        }

        public async Task<SaveUserViewModel> GetByIdSaveViewModel(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var roles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            var role = roles[0];

            SaveUserViewModel saveViewModel = new SaveUserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DNI = user.DNI,
                Email = user.Email,
                UserName = user.UserName,
                Role = role
            };

            return saveViewModel;
        }

        public async Task<List<UserViewModel>> GetAllUsers()
        {
            List<ApplicationUser> users = _userManager.Users.ToList();
            List<UserViewModel> viewModelList = users.Select(user => new UserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DNI = user.DNI,
                Email = user.Email,
                UserName = user.UserName,
                IsActive = user.EmailConfirmed,
            }).ToList();

            int counter = 0;

            foreach (ApplicationUser user in users)
            {
                var roles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
                string role = roles[0];
                viewModelList[counter].Role = role;
                counter++;
            }

            return viewModelList;
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}