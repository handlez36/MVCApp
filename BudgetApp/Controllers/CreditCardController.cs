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
using BudgetApp.Domain.DAL;

namespace BudgetApp.WebUI.Controllers
{
    public class CreditCardController : Controller
    {
        public ICardEntries CardList;
        public IResponsiblePartyEntries PartyList;
        public ICreditEntries CreditEntryList;
        public IPaymentPlanEntries PaymentPlanEntries;
        public UnitofWork unitOfWork;

        // Constant strings for CreditEntry field names
        private const string SCHEDULEDDATE = "scheduledate";
        private const string DESCRIPTION = "entry-description";
        private const string PURCHASEAMOUNT = "entry-amount";
        private const string PAYAMOUNT = "amount-paid";
        private const string CARD = "card-spinner";

        public CreditCardController(ICardEntries Cards, IResponsiblePartyEntries Parties, ICreditEntries CreditEntries, IPaymentPlanEntries PaymentPlanEntries)
        {
            this.CardList = Cards;
            this.PartyList = Parties;
            this.CreditEntryList = CreditEntries;
            this.PaymentPlanEntries = PaymentPlanEntries;

            unitOfWork = new UnitofWork();
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

            ViewBag.PaymentPlans = AggregatePaymentPlans();

            return View("CreditView", CreditEntryList.CreditEntries);
        }

        /**
         * Action method for updating PaymentPlan Partial View
         * 
         */
        public ActionResult UpdatePaymentPlans()
        {
            PaymentPlanViewModel plans = AggregatePaymentPlans();
            return PartialView("AggregatePaymentPlans", plans);
        }

        /**
         * Action method for updating PaymentPlanViewModel 
         * 
         */
        public PaymentPlanViewModel AggregatePaymentPlans()
        {
            // Create ViewModel object and set the Parties property
            PaymentPlanViewModel PaymentPlans = new PaymentPlanViewModel();
            PaymentPlans.Parties = PartyList.Parties.ToList();

            // Sort PaymentPlanEntries by ascending date
            // Also eagerly load the PaymentPlanEntries and PaymentPlanCharges
            IQueryable<PaymentPlanEntry> payments = PaymentPlanEntries.PaymentPlanEntries
                                                .OrderBy(p => p.PaymentDate)
                                                .Include(x => x.Charges);

            // Cycle through PaymentPlanEntries to populate PaymentPlanViewModel
            foreach (PaymentPlanEntry entry in payments)
            {
                IDictionary<string, decimal> AmtPerParty = new Dictionary<string, decimal>();

                IEnumerable<string> parties = entry.Charges
                    .Select(c => c.CreditEntry.ResponsibleParty);

                if (PaymentPlans.plans.ContainsKey(entry.PaymentDate))
                {
                    var Amt = PaymentPlans.plans[entry.PaymentDate];

                    //Calculate PaymentPlanTotal from linked Charges
                    var total = entry.Charges.Sum(t => t.CreditEntry.AmountPaid);
                    Amt[entry.ResponsibleParty] = total;                                    //entry.PaymentTotal;
                }
                else
                {
                    foreach (string party in parties)
                    {
                        var total = entry.Charges
                                         .Where(c => c.CreditEntry.ResponsibleParty == party)
                                         .Sum(t => t.CreditEntry.AmountPaid);

                        AmtPerParty[party] = total;
                    }
                    PaymentPlans.plans.Add(entry.PaymentDate, AmtPerParty);
                }

            }

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
            //EFCreditEntries dB = new EFCreditEntries();

            if (ModelState.IsValid)
            {
                Entry.AmountRemaining = Entry.PurchaseTotal - Entry.AmountPaid;

                if (add)
                    unitOfWork.CreditRepo.Add(Entry);
                else
                    unitOfWork.CreditRepo.Edit(Entry);

                unitOfWork.Save();
                return RedirectToAction("List");
            }
            else
            {
                return View();
            }
            
        }

        public string UpdateField(string id, string value)
        {
            bool dateChange = false;
            
            /*CreditEntry Entry = (CreditEntry)unitOfWork.CreditRepo.CreditEntries
                                    .Where(c => c.CreditEntryId == EntryId)
                                    .First();*/

            int EntryId = Convert.ToInt32(id.Substring(id.LastIndexOf('-')+1));
            id = id.Substring(0, id.LastIndexOf('-'));

            CreditEntry Entry = (CreditEntry)unitOfWork.CreditRepo.CreditEntries
                                    .Where(c => c.CreditEntryId == EntryId)
                                    .First();

            switch (id)
            {
                case SCHEDULEDDATE:
                    Entry.PayDate = DateTime.Parse(value);
                    dateChange = true;
                    break;
                case DESCRIPTION:
                    Entry.Description = value;
                    break;
                case PURCHASEAMOUNT:
                    Entry.PurchaseTotal = Decimal.Parse(value);
                    break;
                case PAYAMOUNT:
                    Entry.AmountPaid = Decimal.Parse(value);
                    break;
                case CARD:
                    Entry.Card = value;
                    break;
                default:
                    break;
            }

            if (dateChange)
            {
                if (Entry.PayDate.HasValue)
                    UpdatePaymentPlans(Entry);
                else
                    DeleteCharges(Entry);
            } 


            /*if (Entry.PayDate.HasValue && dateChange)
                UpdatePaymentPlans(Entry);
            else
            {
                DeleteCharges(Entry);
            }*/

            unitOfWork.CreditRepo.Edit(Entry);
            unitOfWork.Save();

            return value;
        }

        private void UpdatePaymentPlans(CreditEntry entry)
        {
            // Is this a new payment plan charge we're adding?
            bool newAdd = true;
            PaymentPlanCharge charge;
            PaymentPlanEntry oldPlan = null;

            // Pull the charge for the DB.
            // If charge exists, pull the attached PaymentPlan
            // If charge does not exist, create a new charge
            charge = unitOfWork.PaymentPlanChargeRepo.PaymentPlanCharges
                .Where(c => c.CreditEntry.CreditEntryId == entry.CreditEntryId)
                .SingleOrDefault();

            if (charge != null) {
                oldPlan = charge.PaymentPlanEntry;
                newAdd = false;
            }
            else {
                charge = new PaymentPlanCharge
                    {
                        PurchaseAmount = entry.PurchaseTotal,
                        Description = entry.Description,
                        Comment = "Added" + DateTime.Now.ToShortDateString(),
                        CreditEntry = entry
                    };
            }

            // Does a paymentplan with the modified date already exist
            PaymentPlanEntry plan = unitOfWork.PaymentPlanRepo.PaymentPlanEntries
                .Where(p => p.PaymentDate == entry.PayDate.Value)
                .SingleOrDefault();

            if (plan != null)
            {
                plan.Charges.Add(charge);
                unitOfWork.PaymentPlanRepo.Modify(plan);
            }
            else
            {
                PaymentPlanEntry newEntry = new PaymentPlanEntry
                {
                    Card = entry.Card,
                    Charges = new List<PaymentPlanCharge> { charge },
                    PaymentDate = entry.PayDate.Value,
                    PaymentTotal = entry.AmountPaid,
                    ResponsibleParty = entry.ResponsibleParty

                };
                unitOfWork.PaymentPlanRepo.Add(newEntry);
            }

            // If this is an existing charge, remove from old PaymentPlan
            if (!newAdd)
            {
                oldPlan.Charges.Remove(charge);
                unitOfWork.PaymentPlanRepo.Modify(oldPlan);
            }
                

        }

        private void DeleteCharges(CreditEntry entry)
        {
            // Find existing charge and the PaymentPlan it's attached to
            PaymentPlanCharge charge = unitOfWork.PaymentPlanChargeRepo.PaymentPlanCharges
                .Where(c => c.CreditEntry == entry)
                .First();

            PaymentPlanEntry oldPlan = charge.PaymentPlanEntry;

            // Remove charge from payment plan.  Then remove charge.
            oldPlan.Charges.Remove(charge);
            unitOfWork.PaymentPlanChargeRepo.Delete(charge);
        }

        public ActionResult Delete(CreditEntry Entry)
        {
            //EFCreditEntries dB = new EFCreditEntries();

            //dB.Delete(Entry);
            unitOfWork.CreditRepo.Delete(Entry);
            unitOfWork.Save();

            return RedirectToAction("List");
        }

    }
}
