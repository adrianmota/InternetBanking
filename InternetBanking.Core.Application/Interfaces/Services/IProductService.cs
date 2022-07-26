using InternetBanking.Core.Application.ViewModels.Product;
using InternetBanking.Core.Application.ViewModels.User;
using InternetBanking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Interfaces.Services
{
    public interface IProductService : IGenericService<SaveProductViewModel, ProductViewModel, Product>
    {
        Task AddAmountToMainAccount(string userId, double amount);
        Task<List<ProductViewModel>> GetProductsByUserId(string userId);
        Task<bool> CheckAccountAmount(int accountId, double amount);
        Task AddAmountToProduct(int accountId, double amount);
        Task SubstractAmountToProduct(int accountId, double amount);
        Task<bool> CheckCreditCardLimit(int cardId, double amount);
    }
}