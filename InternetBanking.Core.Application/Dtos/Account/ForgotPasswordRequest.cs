﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Core.Application.Dtos.Account
{
    public class ForgotPasswordRequest
    {
        public string Email { get; set; }
    }
}