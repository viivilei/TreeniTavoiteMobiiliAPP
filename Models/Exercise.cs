using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeniTavoiteMobiiliAPP.Models
{
    internal class Exercise
    {
        public int ExerciseId { get; set; }
        public int UserId { get; set; }
        public int GoalId { get; set; }
        public string? ExName { get; set; }
        public DateTime? Date { get; set; }
        public string? Notes { get; set; }

        public virtual Goal Goal { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
