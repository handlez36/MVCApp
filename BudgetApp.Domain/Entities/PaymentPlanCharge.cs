using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace BudgetApp.Domain.Entities
{
    public class PaymentPlanCharge
    {
        [Key]
        public int id { get; set; }

        public string Description { get; set; }
        public decimal PurchaseAmount { get; set; }
        public string Comment { get; set; }

        public virtual PaymentPlanEntry PaymentPlanEntry { get; set; }      // Each PaymentPlanCharge is mapped to one PaymentPlanEntry

        [Required]
        public virtual CreditEntry CreditEntry { get; set; }                // Each PaymentPlanCharge is mapped to one CreditEntry
    }
}
