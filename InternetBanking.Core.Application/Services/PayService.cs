using AutoMapper;
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.Pay;
using InternetBanking.Core.Application.ViewModels.User;
using InternetBanking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Services
{
    public class PayService : IGenericService<Pay, PayViewModel>, IPayService
    {
        private readonly IPayRepository _payRepository;
        private readonly IMapper _mapper;

        public PayService(IPayRepository repo, IMapper mapper)
        {
            _payRepository = repo;
            _mapper = mapper;
        }
    }
}
