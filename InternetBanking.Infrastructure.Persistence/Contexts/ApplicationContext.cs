using InternetBanking.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Infrastructure.Persistence.Contexts
{
    public class ApplicationContext:DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Pay> Pays { get; set; }
        public DbSet<Beneficiary> Beneficiaries { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            #region tables
            builder.Entity<Product>().ToTable("Products");
            builder.Entity<Transaction>().ToTable("Transactions");
            builder.Entity<Pay>().ToTable("Pays");
            builder.Entity<Beneficiary>().ToTable("Beneficiaries");
            #endregion

            #region keys
            builder.Entity<Product>().HasKey(t => t.Id);
            builder.Entity<Transaction>().HasKey(t => t.Id);
            builder.Entity<Pay>().HasKey(t => t.Id);
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

            //Product-Pays(Out), this one for the Pays that came out from the Product/Account
            builder.Entity<Product>()
                .HasMany<Pay>(prod => prod.PaysOut)
                .WithOne(pay => pay.AccountFrom)
                .HasForeignKey(pay => pay.AccountFromId)
                .OnDelete(DeleteBehavior.NoAction);

            //Product-Pays(In), this one for the Pays that enter the Product/Account
            builder.Entity<Product>()
                .HasMany<Pay>(prod => prod.PaysIn)
                .WithOne(pay => pay.AccountTo)
                .HasForeignKey(pay => pay.AccountToId)
                .OnDelete(DeleteBehavior.NoAction);

            //Product-Transactions(Out), this one for the Transactions that came out from the Product/Account
            builder.Entity<Product>()
                .HasMany<Transaction>(prod => prod.TransactionsOut)
                .WithOne(tran => tran.AccountFrom)
                .HasForeignKey(tran => tran.AccountFromId)
                .OnDelete(DeleteBehavior.NoAction);

            //Product-Transactions(In), this one for the Transactions that enter the Product/Account
            builder.Entity<Product>()
                .HasMany<Transaction>(prod => prod.TransactionsIn)
                .WithOne(tran => tran.AccountTo)
                .HasForeignKey(tran => tran.AccountToId)
                .OnDelete(DeleteBehavior.NoAction);

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

            #region pays
            builder.Entity<Pay>().Property(t => t.Type).IsRequired();
            builder.Entity<Pay>().Property(t => t.Amount).IsRequired();
            builder.Entity<Pay>().Property(t => t.ClientId).IsRequired();
            builder.Entity<Pay>().Property(t => t.AccountFromId).IsRequired();
            builder.Entity<Pay>().Property(t => t.AccountToId).IsRequired();
            #endregion

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