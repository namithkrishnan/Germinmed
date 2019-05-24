using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using Germinmed.Models;


namespace Germinmed.DAL
{
    public class GerminmedContext : DbContext
    {

        public GerminmedContext() : base("GerminmedContext")
        {
           
        }

        public DbSet<Products> Product { get; set; }
        public DbSet<ProductImage> ProductImage { get; set; }
        public DbSet<Brands> Brand{ get; set; }
        public DbSet<Clients> Client { get; set; }
        public DbSet<Events> Event { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Promotions> Promotion { get; set; }
        public DbSet<Users> User { get; set; }
         public DbSet<Order> Orders { get; set; }
        public DbSet<Banner> Banners { get; set; }

              public DbSet<InnerBanner> InnerBanners { get; set; }

        public DbSet<Branches> Branch { get; set; }
        public DbSet<NewsLetter> NewsLetters { get; set; }

        public DbSet<MailSettings> MailSetting { get; set; }
      //  this.Database.SqlQuery<Products>("storedProcedureName",params);

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
           
        }

      
    }
}