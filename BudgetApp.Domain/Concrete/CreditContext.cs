using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Concrete;

namespace BudgetApp.Domain.Concrete
{
    public class CreditContext : DbContext
    {
        DbSet<CreditEntry> CreditEntries { get; set; }
        DbSet<PaymentPlanEntry> PaymentPlayEntries { get; set; }
    }
}
