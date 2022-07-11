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
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Pay> Pays { get; set; }

        protected override void OnModelCreating(ModelBuilder m)
        {
            #region tables
            m.Entity<User>().ToTable("Users");
            m.Entity<Product>().ToTable("Products");
            m.Entity<Transaction>().ToTable("Transactions");
            m.Entity<Pay>().ToTable("Pays");
            #endregion

            #region keys
            m.Entity<User>().HasKey(t => t.Id);
            m.Entity<Product>().HasKey(t => t.Id);
            m.Entity<Transaction>().HasKey(t => t.Id);
            m.Entity<Pay>().HasKey(t => t.Id);
            #endregion

            #region relations
            //User-Products
            m.Entity<User>()
                .HasMany<Product>(user => user.Products)
                .WithOne(prod => prod.Client)
                .HasForeignKey(prod => prod.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            //User-Pays (The Pays made by each user)
            m.Entity<User>()
                .HasMany<Pay>(user => user.Pays)
                .WithOne(pay => pay.Client)
                .HasForeignKey(pay => pay.ClientId)
                .OnDelete(DeleteBehavior.NoAction);

            //User-Transactions (The Transactions made by each User)
            m.Entity<User>()
                .HasMany<Transaction>(user => user.Transactions)
                .WithOne(tran => tran.Client)
                .HasForeignKey(tran => tran.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            //Product-Pays(Out), this one for the Pays that came out from the Product/Account
            m.Entity<Product>()
                .HasMany<Pay>(prod => prod.PaysOut)
                .WithOne(pay => pay.AccountFrom)
                .HasForeignKey(pay => pay.AccountFromId)
                .OnDelete(DeleteBehavior.NoAction);

            //Product-Pays(In), this one for the Pays that enter the Product/Account
            m.Entity<Product>()
                .HasMany<Pay>(prod => prod.PaysIn)
                .WithOne(pay => pay.AccountTo)
                .HasForeignKey(pay => pay.AccountToId)
                .OnDelete(DeleteBehavior.NoAction);

            //Product-Transactions(Out), this one for the Transactions that came out from the Product/Account
            m.Entity<Product>()
                .HasMany<Transaction>(prod => prod.TransactionsOut)
                .WithOne(tran => tran.AccountFrom)
                .HasForeignKey(tran => tran.AccountFromId)
                .OnDelete(DeleteBehavior.NoAction);

            //Product-Transactions(In), this one for the Transactions that enter the Product/Account
            m.Entity<Product>()
                .HasMany<Transaction>(prod => prod.TransactionsIn)
                .WithOne(tran => tran.AccountTo)
                .HasForeignKey(tran => tran.AccountToId)
                .OnDelete(DeleteBehavior.NoAction);

            //Relation M-M between Clients and Accounts for Beneficiaries
            m.Entity<User>()
                .HasMany(u => u.Beneficiaries)
                .WithMany(b => b.Beneficiaries)
                .UsingEntity<Beneficiary>(
                    j => j
                        .HasOne(ub => ub.Account)
                        .WithMany(b => b.ClientBeneficiaries)
                        .HasForeignKey(pc => pc.AccountId),
                    j => j
                        .HasOne(cb => cb.Client)
                        .WithMany(c => c.ClientBeneficiaries)
                        .HasForeignKey(cb => cb.ClientId),
                    j =>
                    {
                        j.ToTable("Beneficiaries");
                        j.HasKey(t => new { t.ClientId, t.AccountId });
                    });
            #endregion

            #region props config

            #region users
            m.Entity<User>().Property(t => t.Name).IsRequired();
            m.Entity<User>().Property(t => t.LastName).IsRequired();
            m.Entity<User>().Property(t => t.DNI).IsRequired();
            m.Entity<User>().Property(t => t.Email).IsRequired();
            m.Entity<User>().Property(t => t.Username).IsRequired();
            m.Entity<User>().Property(t => t.Password).IsRequired();
            m.Entity<User>().Property(t => t.Type).IsRequired();
            m.Entity<User>().Property(t => t.Active).IsRequired();
            #endregion

            #region products
            m.Entity<Product>().Property(t => t.Type).IsRequired();
            m.Entity<Product>().Property(t => t.Amount).IsRequired();
            m.Entity<Product>().Property(t => t.ClientId).IsRequired();
            #endregion

            #region pays
            m.Entity<Pay>().Property(t => t.Type).IsRequired();
            m.Entity<Pay>().Property(t => t.Amount).IsRequired();
            m.Entity<Pay>().Property(t => t.ClientId).IsRequired();
            m.Entity<Pay>().Property(t => t.AccountFromId).IsRequired();
            m.Entity<Pay>().Property(t => t.AccountToId).IsRequired();
            #endregion

            #region transactions
            m.Entity<Transaction>().Property(t => t.Type).IsRequired();
            m.Entity<Transaction>().Property(t => t.Amount).IsRequired();
            m.Entity<Transaction>().Property(t => t.ClientId).IsRequired();
            m.Entity<Transaction>().Property(t => t.AccountFromId).IsRequired();
            m.Entity<Transaction>().Property(t => t.AccountToId).IsRequired();
            #endregion

            #endregion
        }
    }
}
