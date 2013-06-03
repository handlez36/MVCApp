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
        private const string PURCHASEDATE = "entry-date";
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
            Dictionary<int, string> testDictionary = new Dictionary<int, string>();
            testDictionary.Add(1, "brandon");

            ViewBag.Cards =
                (from card in CardList.CreditCards.ToArray()
                 select new SelectListItem
                 {
                     Text = card.Card,
                     Value = card.Card
                 }).ToArray();

            ViewBag.Parties =
                (from party in PartyList.Parties.ToArray()
                 orderby party.PartyName
                 select new SelectListItem
                 {
                     Text = party.PartyName,
                     Value = party.PartyName
                 }).ToList();

            ViewBag.PaymentPlans = AggregatePaymentPlans();

            return View("CreditView", CreditEntryList.CreditEntries.Include(c=>c.Card).Include(c=>c.ResponsibleParty));
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
                                                .Include(x => x.Charges)
                                                .Include(p => p.ResponsibleParty);

            // Cycle through PaymentPlanEntries to populate PaymentPlanViewModel
            foreach (PaymentPlanEntry entry in payments)
            {
                IDictionary<string, decimal> AmtPerParty = new Dictionary<string, decimal>();

                IEnumerable<PartyEntry> parties = entry.Charges
                    .Select(c => c.CreditEntry.ResponsibleParty);

                if (PaymentPlans.plans.ContainsKey(entry.PaymentDate))
                {
                    var Amt = PaymentPlans.plans[entry.PaymentDate];

                    //Calculate PaymentPlanTotal from linked Charges
                    var total = entry.Charges.Sum(t => t.CreditEntry.AmountPaid);
                    Amt[entry.ResponsibleParty.PartyName] = total;                                    //entry.PaymentTotal;
                }
                else
                {
                    foreach (string party in parties.Select(p=>p.PartyName))
                    {
                        var total = entry.Charges
                                         .Where(c => c.CreditEntry.ResponsibleParty.PartyName == party)
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
            //string card = unitOfWork.CardRepo.CreditCards.Where(c => c.Card == Request.Params.Get("Card"));
            string card = Request.Params.Get("Card");
            CardEntry cardEntry = unitOfWork.CardRepo.CreditCards
                                    .Where(c => c.Card == card)
                                    .FirstOrDefault();
            Entry.Card = cardEntry;

            string party = Request.Params.Get("ResponsibleParty");
            PartyEntry partyEntry = unitOfWork.PartyRepo.Parties
                                    .Where(p => p.PartyName == party)
                                    .FirstOrDefault();
            Entry.ResponsibleParty = partyEntry;

            ModelState.SetModelValue("Card", new ValueProviderResult(cardEntry, card, System.Globalization.CultureInfo.InvariantCulture));


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

        public ActionResult Delete(CreditEntry Entry)
        {

            unitOfWork.CreditRepo.Delete(Entry);
            unitOfWork.Save();

            return RedirectToAction("List");
        }
        
        /**
         * Update a chosen Credit Entry field
         * 
         * id       id of the credit entry to be modified
         * value    updated value
         */
        public string UpdateField(string id, string value)
        {
         
            int EntryId = Convert.ToInt32(id.Substring(id.LastIndexOf('-')+1));
            id = id.Substring(0, id.LastIndexOf('-'));

            CreditEntry nakedEntry = (CreditEntry)unitOfWork.CreditRepo.CreditEntries
                                    .Where(c => c.CreditEntryId == EntryId)
                                    .Include( c => c.Card)
                                    .Include( c => c.ResponsibleParty)
                                    .First();

            CreditEntryWrapper wrappedEntry = new CreditEntryWrapper(nakedEntry, unitOfWork, CardList.CreditCards, PartyList.Parties);
            wrappedEntry.UpdateField(id, value);

            return value;
        }

    }
}
