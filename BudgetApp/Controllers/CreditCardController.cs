using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using BudgetApp.Domain.Abstract;
using BudgetApp.Domain.Concrete;
using BudgetApp.Domain.Entities;
using BudgetApp.WebUI.ViewModels;

namespace BudgetApp.WebUI.Controllers
{
    public class CreditCardController : Controller
    {
        public ICardEntries CardList;
        public IResponsiblePartyEntries PartyList;
        public ICreditEntries CreditEntryList;
        public IPaymentPlanEntries PaymentPlanEntries;

        public CreditCardController(ICardEntries Cards, IResponsiblePartyEntries Parties, ICreditEntries CreditEntries, IPaymentPlanEntries PaymentPlanEntries)
        {
            this.CardList = Cards;
            this.PartyList = Parties;
            this.CreditEntryList = CreditEntries;
            this.PaymentPlanEntries = PaymentPlanEntries;
        }

        public ActionResult List()
        {
            ViewBag.Cards =
                (from card in CardList.CreditCards.ToArray()
                 select new SelectListItem
                 {
                     Text = card,
                     Value = card
                 }).ToArray();

            ViewBag.Parties =
                (from party in PartyList.Parties.ToArray()
                 orderby party
                 select new SelectListItem
                 {
                     Text = party,
                     Value = party
                 }).ToList();

            ViewBag.PaymentPlans = AggregatePaymentPlans(PartyList.Parties.ToList());

            return View("CreditView", CreditEntryList.CreditEntries);
        }

        public PaymentPlanViewModel AggregatePaymentPlans(List<string> AllParties)
        {
            // Create ViewModel object and set the Parties property
            PaymentPlanViewModel PaymentPlans = new PaymentPlanViewModel();
            PaymentPlans.Parties = AllParties;

            // Sort PaymentPlanEntries by ascending date
            // Also eagerly load the PaymentPlanEntries and PaymentPlanCharges
            IQueryable<PaymentPlanEntry> payments = PaymentPlanEntries.PaymentPlanEntries
                                                .OrderBy(p => p.PaymentDate)
                                                .Include(x => x.Charges);


            // Cycle through PaymentPlanEntries to populate PaymentPlanViewModel
            foreach (PaymentPlanEntry entry in payments)
            {
                IDictionary<string, decimal> AmtPerParty = new Dictionary<string, decimal>();

                if (PaymentPlans.plans.ContainsKey(entry.PaymentDate))
                {
                    var Amt = PaymentPlans.plans[entry.PaymentDate];
                    Amt[entry.ResponsibleParty] = entry.PaymentTotal;
                }
                else
                {
                    AmtPerParty[entry.ResponsibleParty] = entry.PaymentTotal;
                    PaymentPlans.plans.Add(entry.PaymentDate, AmtPerParty);
                }

            }

            //return PartialView(PaymentPlans);
            return PaymentPlans;
        }

        public ActionResult Edit(int? EntryID)
        {
            CreditEntry Entry;

            // If EntryID is null, we are adding
            if (EntryID == null)
            {
                Entry = new CreditEntry();
                ViewBag.Action = true;
            }
            else
            {
                Entry = (CreditEntry)CreditEntryList.CreditEntries
                    .Where(m => m.CreditEntryId == EntryID)
                    .First();
                ViewBag.Action = false;
            }

            ViewBag.Cards = CardList.CreditCards;

            return View(Entry);
        }

        [HttpPost]
        public ActionResult Edit(CreditEntry Entry, bool add)
        {
            EFCreditEntries dB = new EFCreditEntries();

            if (ModelState.IsValid)
            {
                Entry.AmountRemaining = Entry.PurchaseTotal - Entry.AmountPaid;

                if (add)
                    dB.Add(Entry);
                else
                    dB.Edit(Entry);

                return RedirectToAction("List");
            }
            else
            {
                return View();
            }
            
        }

        [HttpPost]
        public string UpdateField(string id, string value, int EntryId)
        {
            EFCreditEntries dB = new EFCreditEntries();

            CreditEntry Entry = (CreditEntry)CreditEntryList.CreditEntries
                                    .Where(c => c.CreditEntryId == EntryId)
                                    .First();
            switch (id)
            {
                case "paydate-spinner":
                    Entry.PayDate = DateTime.Parse(value);
                    break;
                case "entry-description":
                    Entry.Description = value;
                    break;
                case "entry-amount":
                    Entry.PurchaseTotal = Decimal.Parse(value);
                    break;
                case "amount-paid":
                    Entry.AmountPaid = Decimal.Parse(value);
                    break;
                default:
                    break;
            }

            if (Entry.PayDate.HasValue)
                UpdatePaymentPlans(Entry);

            dB.Edit(Entry);

            return value;
        }

        private void UpdatePaymentPlans(CreditEntry entry)
        {
            // Is this a new payment plan add, or an update to an existing payment plan
            bool newAdd = true;

            // Is there already a payment plan entry for this date and card?
            foreach (PaymentPlanEntry payment in PaymentPlanEntries.PaymentPlanEntries)
            {
                // Check for plan matching entry's date and card
                if (payment.PaymentDate.Equals(entry.PayDate.Value) &&
                    (payment.Card == entry.Card))
                {
                    newAdd = false;
                    payment.Charges.Add(new PaymentPlanCharge{ PurchaseAmount = entry.PurchaseTotal, Description = entry.Description });
                    break;
                } 
            }

            if (newAdd)
            {
                PaymentPlanEntries.PaymentPlanEntries.ToList().Add(new PaymentPlanEntry {
                    Card = entry.Card,
                    Charges = new List<PaymentPlanCharge> { 
                        new PaymentPlanCharge { PurchaseAmount = entry.PurchaseTotal, Description = entry.Description }
                    },
                    PaymentDate = entry.PayDate.Value,
                    PaymentTotal = entry.AmountPaid
                });
            }

        }

        public string ListPayments()
        {
            return "Response";
        }

        public ActionResult Delete(CreditEntry Entry)
        {
            EFCreditEntries dB = new EFCreditEntries();

            dB.Delete(Entry);
            return RedirectToAction("List");
        }

    }
}
