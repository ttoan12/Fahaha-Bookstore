namespace WebService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _new : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SachTop5",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        name_author = c.String(),
                        price = c.Double(nullable: false),
                        price_discount = c.Double(nullable: false),
                        discount = c.Double(nullable: false),
                        img = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Watcheds",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        id_account = c.Int(),
                        id_book = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Accounts", t => t.id_account)
                .ForeignKey("dbo.Books", t => t.id_book, cascadeDelete: true)
                .Index(t => t.id_account)
                .Index(t => t.id_book);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Watcheds", "id_book", "dbo.Books");
            DropForeignKey("dbo.Watcheds", "id_account", "dbo.Accounts");
            DropIndex("dbo.Watcheds", new[] { "id_book" });
            DropIndex("dbo.Watcheds", new[] { "id_account" });
            DropTable("dbo.Watcheds");
            DropTable("dbo.SachTop5");
        }
    }
}
