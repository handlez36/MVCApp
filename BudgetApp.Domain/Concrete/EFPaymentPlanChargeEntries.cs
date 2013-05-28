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
    public class EFPaymentPlanChargeEntries : IPaymentPlanChargeEntry
    {
        private LedgerDBContext context;

        public EFPaymentPlanChargeEntries(LedgerDBContext context)
        {
            this.context = context;
        }

        public IQueryable<PaymentPlanCharge> PaymentPlanCharges
        {
            get { return context.PaymentPlanCharges; }
        }

        public void Add(PaymentPlanCharge entry)
        {
            context.PaymentPlanCharges.Add(entry);
            //context.SaveChanges();
        }

        public void Modify(PaymentPlanCharge entry)
        {
            context.Entry(entry).State = System.Data.EntityState.Modified;
            //context.SaveChanges();
        }

        public void Delete(PaymentPlanCharge entry)
        {
            context.PaymentPlanCharges.Remove(entry);
            //context.SaveChanges();
        }
    }
}
