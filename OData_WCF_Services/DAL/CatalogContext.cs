using OData_WCF_Services.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace OData_WCF_Services.DAL
{
    public class CatalogContext : DbContext
    {
        public CatalogContext() : base("CatalogContext")
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<CatalogContext>(new CatalogInitializer());
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }

    public class CatalogInitializer : System.Data.Entity.CreateDatabaseIfNotExists<CatalogContext>
    {
        protected override void Seed(CatalogContext context)
        {
            var categories = new List<Category>
            {
            new Category{ID=1,Name="Categoria 1",Code="cat1"},
            new Category{ID=2,Name="Categoria 2",Code="cat2"},
            new Category{ID=3,Name="Categoria 3",Code="cat3"},
            };
            categories.ForEach(s => context.Categories.Add(s));
            context.SaveChanges();

            var products = new List<Product>
            {
            new Product{ID=1,Name="Prodotto 1",Code="prd1"},
            new Product{ID=2,Name="Prodotto 2",Code="prd2"},
            new Product{ID=3,Name="Prodotto 3",Code="prd3"},
            new Product{ID=4,Name="Prodotto 4",Code="prd4"},
            new Product{ID=5,Name="Prodotto 5",Code="prd5"},
            };
            products.ForEach(s => context.Products.Add(s));
            context.SaveChanges();

        }
    }

}