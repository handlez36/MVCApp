using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BudgetApp.Domain.Entities;

namespace BudgetApp.WebUI.ViewModels
{
    public class PaymentPlanViewModel
    {

        public PaymentPlanViewModel()
        {
            Parties = new List<PartyEntry>();
            plans = new Dictionary<DateTime, IDictionary<string, decimal>>();
        }

        public IEnumerable<PartyEntry> Parties { get; set; }
        public Dictionary<DateTime, IDictionary<string, decimal>> plans { get; set; }
    }
}