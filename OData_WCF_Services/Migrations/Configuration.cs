namespace OData_WCF_Services.Migrations
{
    using OData_WCF_Services.Model;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DAL.CatalogContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(OData_WCF_Services.DAL.CatalogContext context)
        {
            context.Categories.AddOrUpdate(
            new Category { ID = 1, Name = "Categoria 1", Code = "cat1" },
            new Category { ID = 2, Name = "Categoria 2", Code = "cat2" },
            new Category { ID = 3, Name = "Categoria 3", Code = "cat3" }
            );

            context.Products.AddOrUpdate(
            new Product { ID = 1, Name = "Prodotto 1", Code = "prd1" },
            new Product { ID = 2, Name = "Prodotto 2", Code = "prd2" },
            new Product { ID = 3, Name = "Prodotto 3", Code = "prd3" },
            new Product { ID = 4, Name = "Prodotto 4", Code = "prd4" },
            new Product { ID = 5, Name = "Prodotto 5", Code = "prd5" }
            );

        }
    }
}
