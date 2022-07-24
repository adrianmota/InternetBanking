using AutoMapper;
using InternetBanking.Core.Application.Enums;
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.Product;
using InternetBanking.Core.Application.ViewModels.User;
using InternetBanking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Services
{
    public class ProductService : GenericService<SaveProductViewModel, ProductViewModel, Product>, IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository repository, IMapper mapper) : base(repository, mapper)
        {
            _productRepository = repository;
            _mapper = mapper;
        }

        public override async Task<ProductViewModel> Add(SaveProductViewModel vm)
        {
            int minValue=0, maxValue=0;
            switch (vm.Type)
            {
                case (int)ProductType.MainSavingAccount:
                    minValue = 100000000;
                    maxValue = 199999999;
                    break;

                case (int)ProductType.SavingAccount:
                    minValue = 200000000;
                    maxValue = 399999999;
                    break;

                case (int)ProductType.CreditCard:
                    minValue = 400000000;
                    maxValue = 599999999;
                    break;

                case (int)ProductType.Loan:
                    minValue = 600000000;
                    maxValue = 799999999;
                    break;
            }

            Random random = new();
            bool idIsUsed;

            do
            {
                vm.Id = random.Next(minValue, maxValue);
                SaveProductViewModel saveProduct = await GetByIdSaveViewModel(vm.Id);
                idIsUsed = saveProduct != null;
            } while (idIsUsed);

            return await base.Add(vm);
        }

        public async Task AddAmountToMainAccount(string userId, double amount)
        {
            List<Product> products = await _productRepository.GetAllAsync();
            Product product = products.FirstOrDefault(p => p.ClientId == userId && p.Type == (int)ProductType.MainSavingAccount);
            product.Amount += amount;
            await _productRepository.UpdateAsync(product, product.Id);
        }

        public async Task<List<ProductViewModel>> GetProductsByUserId(string userId)
        {
            List<Product> products = await _productRepository.GetAllAsync();
            return _mapper.Map<List<ProductViewModel>>(products.FindAll(p => p.ClientId == userId));
        }
    }
}