using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BudgetApp.Domain.Entities
{
    public class PaymentPlanCharge
    {
        public int id { get; set; }

        public int PaymentPlanChargeId { get; set; }
        public string Description { get; set; }
        public decimal PurchaseAmount { get; set; }
        public string Comment { get; set; }

        public virtual PaymentPlanEntry PaymentPlanEntry { get; set; }      // Each PaymentPlanCharge is mapped to one instance
    }
}
