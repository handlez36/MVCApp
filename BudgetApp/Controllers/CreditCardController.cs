using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BudgetApp.Domain.Abstract;
using BudgetApp.Domain.Concrete;
using BudgetApp.Domain.Entities;

namespace BudgetApp.WebUI.Controllers
{
    public class CreditCardController : Controller
    {
        public ICardEntries CardList;
        public IResponsiblePartyEntries PartyList;
        public ICreditEntries CreditEntryList;

        public CreditCardController(ICardEntries Cards, IResponsiblePartyEntries Parties, ICreditEntries CreditEntries)
        {
            this.CardList = Cards;
            this.PartyList = Parties;
            this.CreditEntryList = CreditEntries;
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
                 select new SelectListItem
                 {
                     Text = party,
                     Value = party
                 }).ToArray();

            return View("CreditView", CreditEntryList.CreditEntries);
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
            return value;
        }

        public ActionResult Delete(CreditEntry Entry)
        {
            EFCreditEntries dB = new EFCreditEntries();

            dB.Delete(Entry);
            return RedirectToAction("List");
        }

    }
}
