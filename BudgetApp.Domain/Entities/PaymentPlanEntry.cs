using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BudgetApp.Domain.Entities
{
    public class PaymentPlanEntry
    {
        public DateTime PaymentDate { get; set; }
        public decimal PaymentTotal { get; set; }
        public string Card { get; set; }
        public List<PaymentPlanCharge> Charges { get; set; }
    }
}
