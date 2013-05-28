using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Concrete;

namespace BudgetApp.Domain.DAL
{
    public class LedgerInitializer : DropCreateDatabaseIfModelChanges<LedgerDBContext>
    {
        protected override void Seed(LedgerDBContext context)
        {
            // Add a default set of Credit Entries if the table is dropped and re-created
            List<CreditEntry> defaultEntries = new List<CreditEntry>
            {
                new CreditEntry{ CreditEntryId=1, Date=DateTime.Now, Card="AmEx", PurchaseTotal=100.00m, AmountPaid=0.00m, AmountRemaining=100.0m,
                    ResponsibleParty="Brandon", Description="Test", PayDate=null },
                new CreditEntry{ CreditEntryId=2, Date=DateTime.Now, Card="AmEx", PurchaseTotal=50.00m, AmountPaid=0.00m, AmountRemaining=50.0m,
                    ResponsibleParty="Brandon", Description="Test 2", PayDate=null },
                new CreditEntry{ CreditEntryId=3, Date=DateTime.Now, Card="Suntrust", PurchaseTotal=300.00m, AmountPaid=0.00m, AmountRemaining=300.00m,
                    ResponsibleParty="Jeanine", Description="Test 3", PayDate=null },

            };

            defaultEntries.ForEach(m => context.CreditEntries.Add(m));
            context.SaveChanges();
        }
    }
}
