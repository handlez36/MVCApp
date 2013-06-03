using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace BudgetApp.Domain.Entities
{
    public class PartyEntry
    {
        [Key]
        public int id { get; set; }
        public string PartyName { get; set; }
    }
}
