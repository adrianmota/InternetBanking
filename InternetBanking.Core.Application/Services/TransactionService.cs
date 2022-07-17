using AutoMapper;
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.Transaction;
using InternetBanking.Core.Application.ViewModels.User;
using InternetBanking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Services
{
    public class TransactionService : IGenericService<Transaction, TransactionViewModel>, ITransactionService
    {
        private readonly ITransactionRepository _tranRepository;
        private readonly IMapper _mapper;

        public TransactionService(ITransactionRepository repo, IMapper mapper)
        {
            _tranRepository = repo;
            _mapper = mapper;
        }
    }
}