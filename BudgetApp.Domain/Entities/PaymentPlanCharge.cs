using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BudgetApp.Domain.Entities
{
    public class PaymentPlanCharge
    {
        public string Description { get; set; }
        public decimal PurchaseAmount { get; set; }
        public string Comment { get; set; }
    }
}
