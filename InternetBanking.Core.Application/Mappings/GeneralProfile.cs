using AutoMapper;
using InternetBanking.Core.Application.ViewModels.Pay;
using InternetBanking.Core.Application.ViewModels.Product;
using InternetBanking.Core.Application.ViewModels.Transaction;
using InternetBanking.Core.Application.ViewModels.User;
using InternetBanking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<User, UserViewModel>()
            .ReverseMap()
                .ForMember(d => d.Created, o => o.Ignore())
                .ForMember(d => d.CreatedBy, o => o.Ignore())
                .ForMember(d => d.Modified, o => o.Ignore())
                .ForMember(d => d.ModifiedBy, o => o.Ignore())
                .ForMember(d => d.ClientBeneficiaries, o => o.Ignore());

            CreateMap<Product, ProductViewModel>()
            .ReverseMap()
                .ForMember(d => d.Created, o => o.Ignore())
                .ForMember(d => d.CreatedBy, o => o.Ignore())
                .ForMember(d => d.Modified, o => o.Ignore())
                .ForMember(d => d.ModifiedBy, o => o.Ignore())
                .ForMember(d => d.ClientId, o => o.Ignore())
                .ForMember(d => d.PaysIn, o => o.Ignore())
                .ForMember(d => d.PaysOut, o => o.Ignore())
                .ForMember(d => d.TransactionsIn, o => o.Ignore())
                .ForMember(d => d.TransactionsOut, o => o.Ignore())
                .ForMember(d => d.Beneficiaries, o => o.Ignore())
                .ForMember(d => d.ClientBeneficiaries, o => o.Ignore());

            CreateMap<Pay, PayViewModel>()
            .ReverseMap()
                .ForMember(d => d.CreatedBy, o => o.Ignore())
                .ForMember(d => d.Modified, o => o.Ignore())
                .ForMember(d => d.ModifiedBy, o => o.Ignore())
                .ForMember(d => d.ClientId, o => o.Ignore())
                .ForMember(d => d.AccountToId, o => o.Ignore())
                .ForMember(d => d.AccountFromId, o => o.Ignore());

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
