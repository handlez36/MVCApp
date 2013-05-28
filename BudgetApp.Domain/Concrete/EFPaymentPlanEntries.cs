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
    public class EFPaymentPlanEntries : IPaymentPlanEntries
    {
        private LedgerDBContext context;

        public EFPaymentPlanEntries(LedgerDBContext context)
        {
            this.context = context;
        }

        public IQueryable<PaymentPlanEntry> PaymentPlanEntries
        {
            get { return context.PaymentPlanEntries; }
        }

        public void Add(PaymentPlanEntry entry)
        {
            context.PaymentPlanEntries.Add(entry);
        }

        public void Modify(PaymentPlanEntry entry)
        {
            context.Entry(entry).State = System.Data.EntityState.Modified;

            // Clean up, if necessary
            if (entry.Charges.Count < 1)
                Delete(entry);
        }

        public void Delete(PaymentPlanEntry entry)
        {
            context.PaymentPlanEntries.Remove(entry);
        }
    }
}
