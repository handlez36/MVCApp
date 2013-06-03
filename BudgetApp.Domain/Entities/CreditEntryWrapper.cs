using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BudgetApp.Domain.DAL;

namespace BudgetApp.Domain.Entities
{
    public class CreditEntryWrapper
    {
        private CreditEntry mEntry;
        private UnitofWork mUnitOfWork;

        private IEnumerable<CardEntry> mCards;
        private IEnumerable<PartyEntry> mParties;

        // Constant strings for CreditEntry field names
        private const string PURCHASEDATE = "entry-date";
        private const string SCHEDULEDDATE = "scheduledate";
        private const string DESCRIPTION = "entry-description";
        private const string PURCHASEAMOUNT = "entry-amount";
        private const string PAYAMOUNT = "amount-paid";
        private const string CARD = "card-spinner";
        private const string PARTY = "party-spinner";

        public CreditEntryWrapper(CreditEntry entry, UnitofWork unitOfWork, IEnumerable<CardEntry> cards, IEnumerable<PartyEntry> parties)
        {
            this.mEntry = entry;
            this.mUnitOfWork = unitOfWork;

            if (cards != null) mCards = cards;
            if (parties != null) mParties = parties;

        }

        public void Add() { }

        public void Edit() { }

        public void UpdateField(string fieldName, string value)
        {
            bool dateChange = false;

            switch (fieldName)
            {
                case PURCHASEDATE:
                    mEntry.Date = DateTime.Parse(value);
                    break;
                case SCHEDULEDDATE:
                    mEntry.PayDate = DateTime.Parse(value);
                    dateChange = true;
                    break;
                case DESCRIPTION:
                    mEntry.Description = value;
                    break;
                case PURCHASEAMOUNT:
                    mEntry.PurchaseTotal = Decimal.Parse(value);
                    break;
                case PAYAMOUNT:
                    mEntry.AmountPaid = Decimal.Parse(value);
                    mEntry.AmountRemaining = mEntry.PurchaseTotal - Decimal.Parse(value);
                    break;
                case CARD:
                    //mEntry.Card = value;
                    
                    var newCard = mUnitOfWork.CardRepo
                        .CreditCards
                        .Where(c => c.Card == value)
                        .FirstOrDefault();

                    mEntry.Card = newCard;
                    break;
                case PARTY:

                    var newParty = mUnitOfWork.PartyRepo
                        .Parties
                        .Where(p => p.PartyName == value)
                        .FirstOrDefault();

                    mEntry.ResponsibleParty = newParty;
                    break;
                default:
                    break;

            }

            if (dateChange)
            {
                if (mEntry.PayDate.HasValue)
                    UpdatePaymentPlans(mEntry);
                else
                    DeleteCharges(mEntry);
            }

            mUnitOfWork.CreditRepo.Edit(mEntry);
            mUnitOfWork.Save();

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
            charge = mUnitOfWork.PaymentPlanChargeRepo.PaymentPlanCharges
                .Where(c => c.CreditEntry.CreditEntryId == entry.CreditEntryId)
                .SingleOrDefault();

            if (charge != null)
            {
                oldPlan = charge.PaymentPlanEntry;
                newAdd = false;
            }
            else
            {
                charge = new PaymentPlanCharge
                    {
                        PurchaseAmount = entry.PurchaseTotal,
                        Description = entry.Description,
                        Comment = "Added" + DateTime.Now.ToShortDateString(),
                        CreditEntry = entry
                    };
            }

            // Does a paymentplan with the modified date already exist
            PaymentPlanEntry plan = mUnitOfWork.PaymentPlanRepo.PaymentPlanEntries
                .Where(p => p.PaymentDate == entry.PayDate.Value)
                .SingleOrDefault();

            if (plan != null)
            {
                plan.Charges.Add(charge);
                mUnitOfWork.PaymentPlanRepo.Modify(plan);
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
                mUnitOfWork.PaymentPlanRepo.Add(newEntry);
            }

            // If this is an existing charge, remove from old PaymentPlan
            if (!newAdd)
            {
                oldPlan.Charges.Remove(charge);
                mUnitOfWork.PaymentPlanRepo.Modify(oldPlan);
            }


        }

        private void DeleteCharges(CreditEntry entry)
        {
            // Find existing charge and the PaymentPlan it's attached to
            PaymentPlanCharge charge = mUnitOfWork.PaymentPlanChargeRepo.PaymentPlanCharges
                .Where(c => c.CreditEntry == entry)
                .First();

            PaymentPlanEntry oldPlan = charge.PaymentPlanEntry;

            // Remove charge from payment plan.  Then remove charge.
            oldPlan.Charges.Remove(charge);
            mUnitOfWork.PaymentPlanChargeRepo.Delete(charge);
        }
    }
}
