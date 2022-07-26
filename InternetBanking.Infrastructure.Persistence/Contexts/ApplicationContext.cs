using InternetBanking.Core.Domain.Common;
using InternetBanking.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using StockApp.Core.Application.Dtos.Account;
using StockApp.Core.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InternetBanking.Infrastructure.Persistence.Contexts
{
    public class ApplicationContext:DbContext
    {
        private readonly IHttpContextAccessor _httpContext;

        public ApplicationContext(DbContextOptions<ApplicationContext> options, IHttpContextAccessor httpContext) : base(options)
        { 
            _httpContext = httpContext;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableBaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Created = DateTime.Now;
                        entry.Entity.CreatedBy = _httpContext.HttpContext.Session.Get<AuthenticationResponse>("user") == null ? "DefaultUser" : _httpContext.HttpContext.Session.Get<AuthenticationResponse>("user").Username;
                        break;

                    case EntityState.Modified:
                        entry.Entity.Modified = DateTime.Now;
                        entry.Entity.ModifiedBy = _httpContext.HttpContext.Session.Get<AuthenticationResponse>("user") == null ? "DefaultUser" : _httpContext.HttpContext.Session.Get<AuthenticationResponse>("user").Username;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        //public DbSet<Pay> Pays { get; set; }
        public DbSet<Beneficiary> Beneficiaries { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            #region tables
            builder.Entity<Product>().ToTable("Products");
            builder.Entity<Transaction>().ToTable("Transactions");
            //builder.Entity<Pay>().ToTable("Pays");
            builder.Entity<Beneficiary>().ToTable("Beneficiaries");
            #endregion

            #region keys
            builder.Entity<Product>().HasKey(t => t.Id);
            builder.Entity<Transaction>().HasKey(t => t.Id);
            //builder.Entity<Pay>().HasKey(t => t.Id);
            builder.Entity<Beneficiary>().HasKey(t => new { t.ClientId, t.AccountId });
            #endregion

            #region relations
            //User-Products
            //Weak relationship
            //ForeignKey(Product.ClientId)

            //User-Pays (The Pays made by each user)
            //Weak relationship
            //ForeignKey(Pay.ClientId)

            //User-Transactions (The Transactions made by each User)
            //Weak relationship
            //ForeignKey(Transaction.ClientId)

            //Product-Transactions(Out), this one for the Transactions that came out from the Product/Account
            //Weak relationship
            //ForeignKey(Transaction.AccountFromId)

            //Product-Transactions(In), this one for the Transactions that enter the Product/Account
            //Weak relationship
            //ForeignKey(Transaction.AccountToId)

            //Relation M-M between Clients and Accounts for Beneficiaries
            //Weak relationship
            //Only relationship between Accounts-Beneficiaries
            builder.Entity<Product>()
                .HasMany<Beneficiary>(prod => prod.Beneficiaries)
                .WithOne(ben => ben.Account)
                .HasForeignKey(ben => ben.AccountId)
                .OnDelete(DeleteBehavior.NoAction);
            #endregion

            #region props config

            //#region users
            //builder.Entity<User>().Property(t => t.Name).IsRequired();
            //builder.Entity<User>().Property(t => t.LastName).IsRequired();
            //builder.Entity<User>().Property(t => t.DNI).IsRequired();
            //builder.Entity<User>().Property(t => t.Email).IsRequired();
            //builder.Entity<User>().Property(t => t.Username).IsRequired();
            //builder.Entity<User>().Property(t => t.Password).IsRequired();
            //builder.Entity<User>().Property(t => t.Type).IsRequired();
            //builder.Entity<User>().Property(t => t.Active).IsRequired();
            //#endregion

            #region products
            builder.Entity<Product>().Property(t => t.Type).IsRequired();
            builder.Entity<Product>().Property(t => t.Amount).IsRequired();
            builder.Entity<Product>().Property(t => t.ClientId).IsRequired();
            #endregion

            //#region pays
            //builder.Entity<Pay>().Property(t => t.Type).IsRequired();
            //builder.Entity<Pay>().Property(t => t.Amount).IsRequired();
            //builder.Entity<Pay>().Property(t => t.ClientId).IsRequired();
            //builder.Entity<Pay>().Property(t => t.AccountFromId).IsRequired();
            //builder.Entity<Pay>().Property(t => t.AccountToId).IsRequired();
            //#endregion

            #region transactions
            builder.Entity<Transaction>().Property(t => t.Type).IsRequired();
            builder.Entity<Transaction>().Property(t => t.Amount).IsRequired();
            builder.Entity<Transaction>().Property(t => t.ClientId).IsRequired();
            builder.Entity<Transaction>().Property(t => t.AccountFromId).IsRequired();
            builder.Entity<Transaction>().Property(t => t.AccountToId).IsRequired();
            #endregion

            #endregion
        }
    }
}