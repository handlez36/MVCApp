using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using BudgetApp.Domain.Abstract;
using BudgetApp.Domain.Concrete;
using BudgetApp.Domain.Entities;

namespace BudgetApp.Domain.Concrete
{
    public class CreditDBContext : DbContext
    {
        public DbSet<CreditEntry> CreditEntries { get; set; }
    }
}
