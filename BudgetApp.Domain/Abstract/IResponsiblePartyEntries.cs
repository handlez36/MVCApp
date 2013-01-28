using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BudgetApp.Domain.Abstract
{
    public interface IResponsiblePartyEntries
    {
        IQueryable<String> Parties { get; set; }
    }
}
