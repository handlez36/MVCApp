using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetApp.WebUI.Models
{
    public class LedgerListModel
    {
        public int NumberOfEntries { get; set; }
        public int EntriesPerPage { get; set; }
        public int NumberOfLinks { get; set; }
    }
}