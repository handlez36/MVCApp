using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BudgetApp.Domain.Concrete;

namespace BudgetApp.Domain.DAL
{
    public class UnitofWork : IDisposable
    {
        private LedgerDBContext context = new LedgerDBContext();
        private EFCreditEntries creditRepo;
        private EFPaymentPlanEntries paymentPlanRepo;
        private EFPaymentPlanChargeEntries paymentPlanChargeRepo;
        private EFLedgerEntries LedgerRepo;
        private EFCardEntries cardRepo;
        private EFPartyEntries partyRepo;

        public EFCreditEntries CreditRepo
        {
            get
            {
                if (this.creditRepo == null)
                    this.creditRepo = new EFCreditEntries(context);

                return creditRepo;
            }
        }

        public EFPaymentPlanEntries PaymentPlanRepo
        {
            get
            {
                if (this.paymentPlanRepo == null)
                    this.paymentPlanRepo = new EFPaymentPlanEntries(context);

                return paymentPlanRepo;
            }
        }

        public EFCardEntries CardRepo
        {
            get
            {
                if (this.cardRepo == null)
                    this.cardRepo = new EFCardEntries(context);

                return cardRepo;
            }
        }

        public EFPartyEntries PartyRepo
        {
            get
            {
                if (this.partyRepo == null)
                    this.partyRepo = new EFPartyEntries(context);

                return partyRepo;
            }
        }

        public EFPaymentPlanChargeEntries PaymentPlanChargeRepo
        {
            get
            {
                if (this.paymentPlanChargeRepo == null)
                    this.paymentPlanChargeRepo = new EFPaymentPlanChargeEntries(context);

                return paymentPlanChargeRepo;
            }
        }

        public void Save()
        {
            var count = context.CardEntries.Count();
            context.SaveChanges();
            count = context.CardEntries.Count();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                context.Dispose();
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
