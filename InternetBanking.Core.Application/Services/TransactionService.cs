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
    public class TransactionService : GenericService<SaveTransactionViewModel, TransactionViewModel, Transaction>, ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public TransactionService(ITransactionRepository repository, IMapper mapper) : base(repository, mapper)
        {
            _transactionRepository = repository;
            _mapper = mapper;
        }

        public async Task<List<TransactionViewModel>> GetTodayTransactions()
        {
            int today = DateTime.Now.Day;
            var transactions = await _transactionRepository.GetAllAsync();

            List<Transaction> todayTransactions = transactions.FindAll(t => t.Created.Day == today);
            List<TransactionViewModel> todayVM = _mapper.Map<List<TransactionViewModel>>(todayTransactions);

            return todayVM;
        }

    }
}