using AutoMapper;
using InternetBanking.Core.Application.ViewModels.Beneficiary;
using InternetBanking.Core.Application.ViewModels.Pay;
using InternetBanking.Core.Application.ViewModels.Product;
using InternetBanking.Core.Application.ViewModels.Transaction;
using InternetBanking.Core.Application.ViewModels.User;
using InternetBanking.Core.Domain.Entities;
using StockApp.Core.Application.Dtos.Account;

namespace InternetBanking.Core.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<RegisterRequest, SaveUserViewModel>()
                .ForMember(dest => dest.HasError, opt => opt.Ignore())
                .ForMember(dest => dest.Error, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<UserViewModel, SaveUserViewModel>()
                .ForMember(d => d.CurrentPassword, o => o.Ignore())
                .ForMember(d => d.Password, o => o.Ignore())
                .ForMember(d => d.ConfirmPassword, o => o.Ignore())
                .ForMember(d => d.Type, o => o.Ignore())
                .ForMember(d => d.Amount, o => o.Ignore())
                .ForMember(d => d.HasError, o => o.Ignore())
                .ForMember(d => d.Error, o => o.Ignore())
                .ReverseMap()
                .ForMember(d => d.IsActive, o => o.Ignore())
                .ForMember(d => d.Products, o => o.Ignore())
                .ForMember(d => d.Pays, o => o.Ignore())
                .ForMember(d => d.Transactions, o => o.Ignore())
                .ForMember(d => d.Beneficiaries, o => o.Ignore());

            CreateMap<AuthenticationRequest, LoginViewModel>()
                .ForMember(dest => dest.Error, opt => opt.Ignore())
                .ForMember(dest => dest.HasError, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<Product, ProductViewModel>()
                .ReverseMap()
                .ForMember(d => d.Created, o => o.Ignore())
                .ForMember(d => d.CreatedBy, o => o.Ignore())
                .ForMember(d => d.Modified, o => o.Ignore())
                .ForMember(d => d.ModifiedBy, o => o.Ignore());

            CreateMap<Product, SaveProductViewModel>()
                .ReverseMap()
                .ForMember(d => d.Created, o => o.Ignore())
                .ForMember(d => d.CreatedBy, o => o.Ignore())
                .ForMember(d => d.Modified, o => o.Ignore())
                .ForMember(d => d.ModifiedBy, o => o.Ignore())
                .ForMember(d => d.TransactionsIn, o => o.Ignore())
                .ForMember(d => d.TransactionsOut, o => o.Ignore())
                .ForMember(d => d.Beneficiaries, o => o.Ignore());

            CreateMap<Beneficiary, BeneficiaryViewModel>()
                .ReverseMap()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.Created, o => o.Ignore())
                .ForMember(d => d.CreatedBy, o => o.Ignore())
                .ForMember(d => d.Modified, o => o.Ignore())
                .ForMember(d => d.ModifiedBy, o => o.Ignore());

            CreateMap<Beneficiary, SaveBeneficiaryViewModel>()
                .ReverseMap()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.Created, o => o.Ignore())
                .ForMember(d => d.CreatedBy, o => o.Ignore())
                .ForMember(d => d.Modified, o => o.Ignore())
                .ForMember(d => d.ModifiedBy, o => o.Ignore())
                .ForMember(d => d.Account, o => o.Ignore());

            CreateMap<Transaction, TransactionViewModel>()
                .ReverseMap()
                .ForMember(d => d.CreatedBy, o => o.Ignore())
                .ForMember(d => d.Modified, o => o.Ignore())
                .ForMember(d => d.ModifiedBy, o => o.Ignore())
                .ForMember(d => d.ClientId, o => o.Ignore())
                .ForMember(d => d.AccountToId, o => o.Ignore())
                .ForMember(d => d.AccountFromId, o => o.Ignore());
        }
    }
}