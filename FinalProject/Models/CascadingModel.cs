using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class CascadingModel
    {
        public CascadingModel()
        {
            Leagues = new List<SelectListItem>();
            Teams = new List<SelectListItem>();
        }
        public List<SelectListItem> Leagues { get; set; }
        public List<SelectListItem> Teams { get; set; }
        public int LeagueID { get; set; }
        public int TeamID { get; set; }
    }
}
