namespace Germinmed.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Banner",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DisplayOrder = c.Int(nullable: false),
                        Title = c.String(maxLength: 50),
                        ImageUrl = c.String(nullable: false),
                        Description = c.String(),
                        CreatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Branches",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        PoBox = c.String(),
                        Phone = c.String(),
                        Fax = c.String(),
                        Email = c.String(),
                        Location = c.String(),
                        ImageUrl = c.String(),
                        CreatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Brands",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        ImageUrl = c.String(),
                        Url = c.String(),
                        ShowInBrandPage = c.Boolean(nullable: false),
                        Description = c.String(),
                        CreatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParentId = c.Int(),
                        Title = c.String(nullable: false),
                        ImageUrl = c.String(),
                        Description = c.String(),
                        CreatedDate = c.DateTime(),
                        Title1 = c.String(),
                        Level = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DisplayOrder = c.Int(nullable: false),
                        Title = c.String(),
                        ImageUrl = c.String(),
                        Url = c.String(),
                        Description = c.String(),
                        CreatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Title = c.String(),
                        ImageUrl = c.String(),
                        Date = c.DateTime(),
                        CreatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.InnerBanner",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DisplayOrder = c.Int(nullable: false),
                        Title = c.String(maxLength: 50),
                        PageName = c.String(nullable: false, maxLength: 100),
                        ImageUrl = c.String(nullable: false),
                        Description = c.String(),
                        CreatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MailSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 320),
                        Password = c.String(nullable: false, maxLength: 50),
                        Server = c.String(nullable: false, maxLength: 320),
                        Port = c.Int(nullable: false),
                        IsSSLEnabled = c.Boolean(nullable: false),
                        Contact = c.String(nullable: false, maxLength: 320),
                        Cc = c.String(maxLength: 320),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NewsLetter",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Subject = c.String(nullable: false),
                        Message = c.String(),
                        Recipients = c.String(),
                        ImageUrl = c.String(),
                        CreatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Order",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Qty = c.Int(),
                        CreatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductName = c.String(nullable: false, maxLength: 50),
                        CategoryId = c.Int(nullable: false),
                        Description = c.String(),
                        BrandId = c.Int(),
                        ItemCode = c.String(),
                        WebUrl = c.String(maxLength: 200),
                        VideoUrl = c.String(maxLength: 200),
                        Price = c.Decimal(precision: 18, scale: 2, storeType: "numeric"),
                        PromotionCode = c.Int(),
                        IsFeatured = c.Boolean(nullable: false),
                        IsOffer = c.Boolean(nullable: false),
                        OfferPercentage = c.String(),
                        ShowInHomePage = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 50),
                        InnerBannerImage = c.String(),
                        ImagePath = c.String(),
                        Catalogue = c.String(),
                        TechnicalSpecifications = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProductImage",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        ImageUrl = c.String(),
                        DisplayOrder = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Promotions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        Percentage = c.Int(),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        CreatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserTypeId = c.Int(nullable: false),
                        UserName = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        IsEmailVerified = c.Boolean(nullable: false),
                        ActivationCode = c.String(),
                        ResetPasswordCode = c.String(),
                        FirstName = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Phone = c.String(),
                        ClinicName = c.String(),
                        IsActive = c.Boolean(),
                        IsAdmin = c.Boolean(),
                        CreatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
            DropTable("dbo.Promotions");
            DropTable("dbo.ProductImage");
            DropTable("dbo.Products");
            DropTable("dbo.Order");
            DropTable("dbo.NewsLetter");
            DropTable("dbo.MailSettings");
            DropTable("dbo.InnerBanner");
            DropTable("dbo.Events");
            DropTable("dbo.Clients");
            DropTable("dbo.Category");
            DropTable("dbo.Brands");
            DropTable("dbo.Branches");
            DropTable("dbo.Banner");
        }
    }
}
