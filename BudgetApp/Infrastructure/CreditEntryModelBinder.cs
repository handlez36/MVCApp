using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BudgetApp.Domain.Entities;

namespace BudgetApp.WebUI.Infrastructure
{
    public class CreditEntryModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            CardEntry card = new CardEntry { id = 10, Card = "Temp" };
            PartyEntry party = new PartyEntry { id = 10, PartyName = "Temp" };

            // Obtain the model object that we plan to bind to
            CreditEntry entry = (CreditEntry)bindingContext.Model
                ?? new CreditEntry
                {
                    CreditEntryId = 1,
                    Date = DateTime.Now,
                    Card = card,
                    PurchaseTotal = 0.00m,
                    AmountPaid = 0.00m,
                    AmountRemaining = 0.0m,
                    ResponsibleParty = party,
                    Description = "Test",
                    PayDate = null
                };

            bool hasPrefix = bindingContext.ValueProvider.ContainsPrefix(bindingContext.ModelName);
            string searchPrefix = (hasPrefix) ? bindingContext.ModelName + "." : "";

            // Populate object model fields
            //entry.CreditEntryId = int.Parse(GetValue(bindingContext, searchPrefix, "CreditEntryId"));
            entry.Date = DateTime.Parse(GetValue(bindingContext, searchPrefix, "Date"));
            entry.PurchaseTotal = decimal.Parse(GetValue(bindingContext, searchPrefix, "PurchaseTotal"));
            entry.AmountPaid = decimal.Parse(GetValue(bindingContext, searchPrefix, "AmountPaid"));
            entry.AmountRemaining = 0m;
            entry.Description = GetValue(bindingContext, searchPrefix, "Description");
            entry.PayDate = null;
            entry.Card = new CardEntry { id = 10, Card = "Temp" };
            entry.ResponsibleParty = new PartyEntry { id = 10, PartyName = "Temp" };

            return entry;

        }

        private string GetValue(ModelBindingContext context, string prefix, string key)
        {
            ValueProviderResult vpr = context.ValueProvider.GetValue(prefix + key);
            return vpr == null ? null : vpr.AttemptedValue;
        }
    }
}