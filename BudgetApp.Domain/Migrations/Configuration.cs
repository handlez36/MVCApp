namespace BudgetApp.Domain.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using BudgetApp.Domain.Concrete;
    using BudgetApp.Domain.Entities;
    using System.Collections.Generic;

    internal sealed class Configuration : DbMigrationsConfiguration<BudgetApp.Domain.DAL.LedgerDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BudgetApp.Domain.DAL.LedgerDBContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            var PaymentPlans = new List<PaymentPlanEntry>
            {
                new PaymentPlanEntry { PaymentDate=DateTime.Parse("2013-03-01"), Card="American Express", PaymentTotal=40.0M },
            };
            PaymentPlans.ForEach(p => context.PaymentPlanEntries.Add(p));
            context.SaveChanges();
        }
    }
}
