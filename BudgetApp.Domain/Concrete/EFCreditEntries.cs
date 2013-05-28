using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BudgetApp.Domain.Abstract;
using BudgetApp.Domain.Concrete;
using BudgetApp.Domain.Entities;
using BudgetApp.Domain.DAL;

namespace BudgetApp.Domain.Concrete
{
    public class EFCreditEntries : ICreditEntries
    {
        LedgerDBContext DbContext;

        public EFCreditEntries(LedgerDBContext context)
        {
            this.DbContext = context;
        }

        public IQueryable<CreditEntry> CreditEntries
        {
            get { return DbContext.CreditEntries; }
            set { }
        }

        public void Add(CreditEntry Entry)
        {
            DbContext.Entry(Entry).State = System.Data.EntityState.Added;
            //DbContext.SaveChanges();
        }

        public void Edit(CreditEntry Entry)
        {
            DbContext.Entry(Entry).State = System.Data.EntityState.Modified;
            //DbContext.SaveChanges();
        }

        public void Delete(CreditEntry Entry)
        {
            DbContext.Entry(Entry).State = System.Data.EntityState.Deleted;
            //DbContext.SaveChanges();
        }
    }
}
