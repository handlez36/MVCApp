using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using BudgetApp.Domain.Entities;
using BudgetApp.Domain.DAL;
using BudgetApp.Domain.Abstract;

namespace BudgetApp.Domain.Concrete
{
    public class EFPartyEntries : IResponsiblePartyEntries
    {
        LedgerDBContext context;

        public EFPartyEntries(LedgerDBContext context)
        {
            this.context = context;
        }

        public IQueryable<PartyEntry> Parties
        {
            get { return context.PartyEntries; }
            set { }
        }

        public void Add(PartyEntry Entry)
        {
            context.Entry(Entry).State = System.Data.EntityState.Added;
        }

        public void Edit(PartyEntry Entry)
        {
            context.Entry(Entry).State = System.Data.EntityState.Modified;
        }

        public void Delete(PartyEntry Entry)
        {
            context.Entry(Entry).State = System.Data.EntityState.Deleted;
        }
    }
}
