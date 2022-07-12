using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Domain.Entities;
using InternetBanking.Infrastructure.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Infrastructure.Persistence.Repositories
{
    public class BeneficiaryRepository : GenericRepository<Beneficiary>, IBeneficiaryRepository
    {
        private readonly ApplicationContext _dbContext;

        public BeneficiaryRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
