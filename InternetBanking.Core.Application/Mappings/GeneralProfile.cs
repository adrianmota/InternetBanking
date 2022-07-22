using AutoMapper;
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

            CreateMap<AuthenticationRequest, LoginViewModel>()
                .ForMember(dest => dest.Error, opt => opt.Ignore())
                .ForMember(dest => dest.HasError, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<Product, ProductViewModel>()
                .ReverseMap()
                .ForMember(d => d.Created, o => o.Ignore())
                .ForMember(d => d.CreatedBy, o => o.Ignore())
                .ForMember(d => d.Modified, o => o.Ignore())
                .ForMember(d => d.ModifiedBy, o => o.Ignore())
                .ForMember(d => d.ClientId, o => o.Ignore())
                .ForMember(d => d.TransactionsIn, o => o.Ignore())
                .ForMember(d => d.TransactionsOut, o => o.Ignore())
                .ForMember(d => d.Beneficiaries, o => o.Ignore());

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