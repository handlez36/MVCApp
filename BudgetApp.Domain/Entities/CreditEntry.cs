using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BudgetApp.Domain.Entities
{
    public class CreditEntry
    {

        [HiddenInput(DisplayValue=false)]
        public int CreditEntryId { get; set; }

        [DataType(DataType.Date)]
        [Display(Name="The Date")]
        public DateTime Date { get; set; }

        [DataType(DataType.Date)]
        public DateTime? PayDate { get; set; }

        public String Description { get; set; }
        public String Card { get; set; }
        
        [DataType(DataType.Currency)]
        public decimal PurchaseTotal { get; set; }
        [DataType(DataType.Currency)]
        public decimal AmountPaid { get; set; }
        [DataType(DataType.Currency)]
        public decimal AmountRemaining { get; set; }

        public String ResponsibleParty { get; set; }

    }
}
