using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BudgetApp.Domain.Concrete
{
    public class LedgerEntry
    {
        public enum ItemType { CREDIT, EXPENSE };

        [HiddenInput(DisplayValue=false)]
        public int LedgerEntryID { get; set; }

        [DataType(DataType.Date)]
        public DateTime Time { get; set; }
        
        [Required(ErrorMessage="Please add a description")]    
        public String Description { get; set; }

        [DataType(DataType.Currency)]
        public Decimal Price { get; set; }
        public String Category { get; set; }
        public String EntryType { get; set; }
        public String LedgerName { get; set; }

    }
}
