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
            context.SaveChanges();
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
