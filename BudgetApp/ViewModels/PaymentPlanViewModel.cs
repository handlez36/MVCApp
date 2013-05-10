using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetApp.WebUI.ViewModels
{
    public class PaymentPlanViewModel
    {

        public PaymentPlanViewModel()
        {
            Parties = new List<string>();
            plans = new Dictionary<DateTime, IDictionary<string, decimal>>();
        }

        public IEnumerable<string> Parties { get; set; }
        public Dictionary<DateTime, IDictionary<string, decimal>> plans { get; set; }
    }
}