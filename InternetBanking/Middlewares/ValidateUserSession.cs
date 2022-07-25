﻿using Microsoft.AspNetCore.Http;
using StockApp.Core.Application.Dtos.Account;
using StockApp.Core.Application.Helpers;

namespace WebApp.InternetBanking.Middlewares
{
    public class ValidateUserSession
    {
        private readonly IHttpContextAccessor _httpContext;

        public ValidateUserSession(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public bool HasUser()
        {
            var user = _httpContext.HttpContext.Session.Get<AuthenticationResponse>("user");

            if (user != null)
            {
                return true;
            }

            return false;
        }
    }
}