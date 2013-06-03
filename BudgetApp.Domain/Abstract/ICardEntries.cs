using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BudgetApp.Domain.Entities;

namespace BudgetApp.Domain.Abstract
{
    public interface ICardEntries
    {
        IQueryable<CardEntry> CreditCards { get; set; }
    }
}
