namespace BudgetApp.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewPaymentPlanChargeTableAndLinkage : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LedgerEntries",
                c => new
                    {
                        LedgerEntryID = c.Int(nullable: false, identity: true),
                        Time = c.DateTime(nullable: false),
                        Description = c.String(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Category = c.String(),
                        EntryType = c.String(),
                        LedgerName = c.String(),
                    })
                .PrimaryKey(t => t.LedgerEntryID);
            
            CreateTable(
                "dbo.CreditEntries",
                c => new
                    {
                        CreditEntryId = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        PayDate = c.DateTime(),
                        Description = c.String(),
                        Card = c.String(),
                        PurchaseTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountPaid = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountRemaining = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Party = c.String(),
                    })
                .PrimaryKey(t => t.CreditEntryId);
            
            CreateTable(
                "dbo.PaymentPlanEntries",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        PaymentDate = c.DateTime(nullable: false),
                        PaymentTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Card = c.String(),
                        ResponsibleParty = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.PaymentPlanCharges",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        PaymentPlanChargeId = c.Int(nullable: false),
                        Description = c.String(),
                        PurchaseAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Comment = c.String(),
                        PaymentPlanEntry_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.PaymentPlanEntries", t => t.PaymentPlanEntry_id)
                .Index(t => t.PaymentPlanEntry_id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.PaymentPlanCharges", new[] { "PaymentPlanEntry_id" });
            DropForeignKey("dbo.PaymentPlanCharges", "PaymentPlanEntry_id", "dbo.PaymentPlanEntries");
            DropTable("dbo.PaymentPlanCharges");
            DropTable("dbo.PaymentPlanEntries");
            DropTable("dbo.CreditEntries");
            DropTable("dbo.LedgerEntries");
        }
    }
}
