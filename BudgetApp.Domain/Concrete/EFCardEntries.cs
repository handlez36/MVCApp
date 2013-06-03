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
    public class EFCardEntries : ICardEntries
    {
        LedgerDBContext context;

        public EFCardEntries(LedgerDBContext context)
        {
            this.context = context;
        }

        public IQueryable<CardEntry> CreditCards
        {
            get { return context.CardEntries; }
            set { }
        }

        public void Add(CardEntry Entry)
        {
            context.Entry(Entry).State = System.Data.EntityState.Added;
        }

        public void Edit(CardEntry Entry)
        {
            context.Entry(Entry).State = System.Data.EntityState.Modified;
        }

        public void Delete(CardEntry Entry)
        {
            context.Entry(Entry).State = System.Data.EntityState.Deleted;
        }
    }
}
