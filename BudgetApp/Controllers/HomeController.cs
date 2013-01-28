using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BudgetApp.Domain.Abstract;
using BudgetApp.Domain.Concrete;
using BudgetApp.WebUI.Models;

namespace BudgetApp.WebUI.Controllers
{
    public class HomeController : Controller
    {
        public ILedgerEntries Entries;
        private int EntriesPerPage = 3;
        private int NumberOfLinks = 0;

        public HomeController(ILedgerEntries budgetEntries)
        {
            Entries = budgetEntries;
        }

        // page - indicates which results page to display
        public ViewResult List(int page = 1)
        {
            IQueryable<LedgerEntry> list = Entries.GetEntries;

            int skipCount = (page - 1) * EntriesPerPage;            // Number of entries to skip depending on current page
            int leftOver = list.Count() - skipCount;                // Number of remaining entries
            NumberOfLinks = list.Count() / EntriesPerPage + (list.Count() % EntriesPerPage == 0 ? 0 : 1);

            LedgerListModel ListModel = new LedgerListModel
            {
                NumberOfEntries = (leftOver > EntriesPerPage ? EntriesPerPage : leftOver),
                EntriesPerPage = this.EntriesPerPage,
                NumberOfLinks = this.NumberOfLinks
            };
            
            ViewData["listmodel"] = list
                .OrderByDescending(e => e.Time)
                .Skip( skipCount )
                .Take(EntriesPerPage);

            return View(ListModel);
        }

        public ViewResult Edit(int entryID)
        {
            LedgerEntry entry = new LedgerEntry();
            if (entryID == -1)
            {
                ViewBag.Add = true;
            }
            else
            {
                IQueryable<LedgerEntry> list = Entries.GetEntries;

                entry = list.Select(e => e)
                .Where(e => e.LedgerEntryID == entryID)
                .First();
            }
            
            return View( entryID < 1 ?
                new LedgerEntry() :
                entry);
        }

        [HttpPost]
        public ActionResult Edit(LedgerEntry entry, bool add = false)
        {
            EFLedgerEntries ledgerEntries = new EFLedgerEntries();

            if (ModelState.IsValid)
            {
                if (!add)
                    ledgerEntries.Edit(entry);
                else
                    ledgerEntries.Add(entry);

                return RedirectToAction("List");
            }
            else
            {
                ViewBag.Add = add;
                return View();
            }
            
            
        }

        public ActionResult Delete(int entryID)
        {
            EFLedgerEntries ledgerEntries = new EFLedgerEntries();

            IQueryable<LedgerEntry> list = Entries.GetEntries;

            LedgerEntry entry = list.Select(e => e)
                .Where(e => e.LedgerEntryID == entryID)
                .First();

            ledgerEntries.Delete(entry);

            return RedirectToAction("List");
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
