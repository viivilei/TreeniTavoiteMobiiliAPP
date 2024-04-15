using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeniTavoiteMobiiliAPP.Models
{
    internal class Goal
    {

        public int GoalId { get; set; }
        public int? UserId { get; set; }
        public string? GoalName { get; set; }
        public string? Notes { get; set; }
        public bool Reached { get; set; }

        public virtual User? User { get; set; }
    }
}
