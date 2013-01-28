using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BudgetApp.Domain.Abstract;
using BudgetApp.Domain.Concrete;
using BudgetApp.Domain.Entities;

namespace BudgetApp.Domain.Concrete
{
    public class EFCreditEntries : ICreditEntries
    {
        CreditDBContext DbContext = new CreditDBContext();

        public IQueryable<CreditEntry> CreditEntries
        {
            get { return DbContext.CreditEntries; }
            set { }
        }

        public void Add(CreditEntry Entry)
        {
            DbContext.Entry(Entry).State = System.Data.EntityState.Added;
            DbContext.SaveChanges();
        }

        public void Edit(CreditEntry Entry)
        {
            DbContext.Entry(Entry).State = System.Data.EntityState.Modified;
            DbContext.SaveChanges();
        }

        public void Delete(CreditEntry Entry)
        {
            DbContext.Entry(Entry).State = System.Data.EntityState.Deleted;
            DbContext.SaveChanges();
        }
    }
}
