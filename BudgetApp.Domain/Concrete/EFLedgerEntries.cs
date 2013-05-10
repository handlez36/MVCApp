using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BudgetApp.Domain.Abstract;
using BudgetApp.Domain.Concrete;
using BudgetApp.Domain.DAL;

namespace BudgetApp.Domain.Concrete
{
    public class EFLedgerEntries : ILedgerEntries
    {
        LedgerDBContext dBContext = new LedgerDBContext();

        public IQueryable<LedgerEntry> GetEntries
        {
            get { return dBContext.LedgerEntries; }
            set { }
        }

        public void Add(LedgerEntry entry)
        {
            entry.Time = DateTime.Now;
            dBContext.Entry(entry).State = System.Data.EntityState.Added;
            dBContext.SaveChanges();
        }

        public void Edit(LedgerEntry entry)
        {
            dBContext.Entry(entry).State = System.Data.EntityState.Modified;
            dBContext.SaveChanges();
        }

        public void Delete(LedgerEntry entry)
        {
            dBContext.Entry(entry).State = System.Data.EntityState.Deleted;
            dBContext.SaveChanges();
        }
    }
}
