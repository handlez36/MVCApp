using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BudgetApp.Domain.Concrete;


namespace BudgetApp.Domain.Abstract
{
    public interface ILedgerEntries
    {
        IQueryable<LedgerEntry> GetEntries { get; set; }
    }
}
