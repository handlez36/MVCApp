using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BudgetApp.Domain.Entities;

namespace BudgetApp.Domain.Abstract
{
    public interface IPaymentPlanEntries
    {
        IQueryable<PaymentPlanEntry> PaymentPlanEntries { get; }

        void Add(PaymentPlanEntry entry);
        void Modify(PaymentPlanEntry entry);
        void Delete(PaymentPlanEntry entry);
    }
}
