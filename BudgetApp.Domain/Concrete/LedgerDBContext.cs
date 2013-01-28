using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace BudgetApp.Domain.Concrete
{
    public class LedgerDBContext : System.Data.Entity.DbContext
    {
        public DbSet<LedgerEntry> LedgerEntries { get; set; }

    }
}
