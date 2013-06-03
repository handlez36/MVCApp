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
            // Create Card objects
            CardEntry card1 = new CardEntry { id=1, Card="AmEx" };
            CardEntry card2 = new CardEntry { id=2, Card="Suntrust" };
            CardEntry card3 = new CardEntry { id = 3, Card = "Kohl" };

            // Create Party objects
            PartyEntry party1 = new PartyEntry{ id = 1, PartyName = "Brandon" };
            PartyEntry party2 = new PartyEntry{ id = 2, PartyName = "Jeanine" };
            PartyEntry party3 = new PartyEntry{ id = 3, PartyName = "Joint" };
            PartyEntry party4 = new PartyEntry{ id = 4, PartyName = "Vacation" };
            PartyEntry party5 = new PartyEntry{ id = 5, PartyName = "Emergency" }; 
            PartyEntry party6 = new PartyEntry{ id = 6, PartyName = "Savings" };
            PartyEntry party7 = new PartyEntry{ id = 7, PartyName = "TBD" };

            // Add a default set of Credit Cards if the table is dropped and re-created
            var cards = new List<CardEntry> { card1, card2, card3 };
            cards.ForEach(c=>context.CardEntries.Add(c));
            context.SaveChanges();

            // Add a default set of Responsible Party names if the table is dropped and re-created
            var parties = new List<PartyEntry> { party1, party2, party3, party4, party5, party6, party7 };
            parties.ForEach(c => context.PartyEntries.Add(c));
            context.SaveChanges();

            // Add a default set of Credit Entries if the table is dropped and re-created
            List<CreditEntry> defaultEntries = new List<CreditEntry>
            {
                new CreditEntry{ CreditEntryId=1, Date=DateTime.Now, Card=card1, PurchaseTotal=100.00m, AmountPaid=0.00m, AmountRemaining=100.0m,
                    ResponsibleParty=party1, Description="Test", PayDate=null },
                new CreditEntry{ CreditEntryId=2, Date=DateTime.Now, Card=card1, PurchaseTotal=50.00m, AmountPaid=0.00m, AmountRemaining=50.0m,
                    ResponsibleParty=party1, Description="Test 2", PayDate=null },
                new CreditEntry{ CreditEntryId=3, Date=DateTime.Now, Card=card2, PurchaseTotal=300.00m, AmountPaid=0.00m, AmountRemaining=300.00m,
                    ResponsibleParty=party2, Description="Test 3", PayDate=null },

            };

            defaultEntries.ForEach(m => context.CreditEntries.Add(m));
            context.SaveChanges();
        }
    }
}
