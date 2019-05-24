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
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

     



    }
}