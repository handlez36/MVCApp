using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BudgetApp.Domain.Entities;

namespace BudgetApp.Domain.Abstract
{
    public interface IPaymentPlanChargeEntry
    {
        IQueryable<PaymentPlanCharge> PaymentPlanCharges { get; }

        void Add(PaymentPlanCharge charge);
        void Modify(PaymentPlanCharge charge);
        void Delete(PaymentPlanCharge charge);
    }
}
