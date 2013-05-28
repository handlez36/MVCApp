using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace BudgetApp.Domain.Entities
{
    public class PaymentPlanEntry
    {
        [Key]
        public int id { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal PaymentTotal { get; set; }
        public string Card { get; set; }
        public string ResponsibleParty { get; set; }
        
        //public string ForceDrop { get; set; }

        public virtual ICollection<PaymentPlanCharge> Charges { get; set; }
    }
}
