using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using BudgetApp.Domain.Abstract;
using BudgetApp.Domain.Concrete;
using BudgetApp.Domain.Entities;

namespace BudgetApp.Domain.DAL
{
    public class LedgerDBContext : DbContext
    {
        public DbSet<LedgerEntry> LedgerEntries { get; set; }
        public DbSet<CreditEntry> CreditEntries { get; set; }
        public DbSet<PaymentPlanEntry> PaymentPlanEntries { get; set; }
        public DbSet<PaymentPlanCharge> PaymentPlanCharges { get; set; }
    }
}
