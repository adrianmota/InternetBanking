using AutoMapper;
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.Beneficiary;
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
    public class BeneficiaryService : GenericService<SaveBeneficiaryViewModel, BeneficiaryViewModel, Beneficiary>, IBeneficiaryService
    {
        private readonly IBeneficiaryRepository _beneficiaryRepository;
        private readonly IMapper _mapper;

        public BeneficiaryService(IBeneficiaryRepository repository, IMapper mapper) : base(repository, mapper)
        {
            _beneficiaryRepository = repository;
            _mapper = mapper;
        }

        public async Task<List<BeneficiaryViewModel>> GetBeneficiariesByUser(string userId)
        {
            List<Beneficiary> beneficiaries = await _beneficiaryRepository.GetAllAsync();
            List<Beneficiary> beneficiaryList = beneficiaries.FindAll(b => b.ClientId == userId);
            List<BeneficiaryViewModel> listVM = _mapper.Map<List<BeneficiaryViewModel>>(beneficiaryList);

            return listVM;
        }

        public async Task DeleteByUserAndAccount(string userId, int accountId)
        {
            var beneficiaries = await _beneficiaryRepository.GetAllAsync();
            var beneficiary = beneficiaries.FirstOrDefault(b => b.ClientId == userId && b.AccountId == accountId);

            await _beneficiaryRepository.DeleteAsync(beneficiary);
        }
    }
}