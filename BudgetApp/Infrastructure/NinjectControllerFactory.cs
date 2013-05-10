using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using Moq;
using BudgetApp.Domain.Concrete;
using BudgetApp.Domain.Abstract;
using BudgetApp.Domain.Entities;
using BudgetApp.Domain.DAL;

namespace BudgetApp.WebUI.Infrastructure
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private IKernel kernel;

        public NinjectControllerFactory()
        {
            kernel = new StandardKernel();
            AddBindings();
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return controllerType == null ?
                null :
                (IController)kernel.Get(controllerType);
        }

        private void AddBindings()
        {
            IEnumerable<LedgerEntry> TestEntries = new List<LedgerEntry> {
                new LedgerEntry{ Time = DateTime.Now, Category = "null", Description = "First Expense", EntryType = "CREDIT", LedgerName = "First Ledger" },
                new LedgerEntry{ Time = DateTime.Now, Category = "null", Description = "Second Expense", EntryType = "CREDIT", LedgerName = "First Ledger" },
                new LedgerEntry{ Time = DateTime.Now, Category = "null", Description = "Third Expense", EntryType = "CREDIT", LedgerName = "First Ledger" },
            };

            IEnumerable<String> CardEntries = new List<String> { "AmEx", "Suntrust", "Kohls" };
            IEnumerable<String> PartyEntries = new List<String> { "Brandon", "Jeanine", "Joint", "Vacation", "Emergency", "Savings", "TBD" };

            IEnumerable<CreditEntry> CreditEntries = new List<CreditEntry> {
                new CreditEntry{ Date = DateTime.Now, Description = "Test Entry 1", AmountPaid = 200.00M, AmountRemaining = 300.00M, PurchaseTotal = 500.00M, Card = "American Express" , ResponsibleParty = "Brandon" },
                new CreditEntry{ Date = DateTime.Now, Description = "Test Entry 2", AmountPaid = 0.00M, AmountRemaining = 150.00M, PurchaseTotal = 150.00M, Card = "American Express" , ResponsibleParty = "Brandon" },
                new CreditEntry{ Date = DateTime.Now, Description = "Test Entry 3", AmountPaid = 50.00M, AmountRemaining = 450.00M, PurchaseTotal = 500.00M, Card = "Suntrust" , ResponsibleParty = "Brandon" },
                new CreditEntry{ Date = DateTime.Now, Description = "Test Entry 4", AmountPaid = 0.00M, AmountRemaining = 37.00M, PurchaseTotal = 37.00M, Card = "American Express" , ResponsibleParty = "Jeanine" },
            };

            IEnumerable<PaymentPlanEntry> PaymentPlanEntries = new List<PaymentPlanEntry>();

            /**** Set up Mock interfaces ***/
            Mock<ILedgerEntries> MockEntries = new Mock<ILedgerEntries>();
            MockEntries.Setup(m => m.GetEntries).Returns(TestEntries.AsQueryable);          // Set up LedgerEntries binding using Moq

            Mock<ICardEntries> MockCardEntries = new Mock<ICardEntries>();
            MockCardEntries.Setup(m => m.CreditCards).Returns(CardEntries.AsQueryable);

            Mock<IResponsiblePartyEntries> MockPartyEntries = new Mock<IResponsiblePartyEntries>();
            MockPartyEntries.Setup(m => m.Parties).Returns(PartyEntries.AsQueryable);

            Mock<ICreditEntries> MockCreditEntries = new Mock<ICreditEntries>();
            MockCreditEntries.Setup(m => m.CreditEntries).Returns(CreditEntries.AsQueryable);

            //Mock<IPaymentPlanEntries> MockPaymentPlanEntries = new Mock<IPaymentPlanEntries>();
            //MockPaymentPlanEntries.Setup(m => m.PaymentPlanEntries).Returns(PaymentPlanEntries.AsQueryable);
            /*******************************/


            //kernel.Bind<ILedgerEntries>().ToConstant(MockEntries.Object);
            //kernel.Bind<ICreditEntries>().ToConstant(MockCreditEntries.Object);
            LedgerDBContext dBContext = new LedgerDBContext();

            // Create bindings
            kernel.Bind<ILedgerEntries>().To<EFLedgerEntries>();
            kernel.Bind<ICreditEntries>().To<EFCreditEntries>();
            kernel.Bind<IPaymentPlanEntries>().To<EFPaymentPlanEntries>();
            
            kernel.Bind<ICardEntries>().ToConstant(MockCardEntries.Object);
            kernel.Bind<IResponsiblePartyEntries>().ToConstant(MockPartyEntries.Object);
        }
    }
}