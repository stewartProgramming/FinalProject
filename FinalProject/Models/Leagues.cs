using System;
using System.Collections.Generic;

namespace FinalProject.Models
{
    public partial class Leagues
    {
        public Leagues()
        {
            Teams = new HashSet<Teams>();
        }

        public int Id { get; set; }
        public string LeagueName { get; set; }

        public virtual ICollection<Teams> Teams { get; set; }
    }
}
