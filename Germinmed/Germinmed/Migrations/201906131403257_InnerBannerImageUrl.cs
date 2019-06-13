namespace Germinmed.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InnerBannerImageUrl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Category", "InnerBannerImageUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Category", "InnerBannerImageUrl");
        }
    }
}
