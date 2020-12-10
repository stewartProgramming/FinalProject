using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FinalProject.Services
{
    public class HighlightService
    {
        public List<List<Highlight>> GetHighlights()
        {
            List<Highlight> highlights = FootballDAL.GetHighlights();
            List<List<Highlight>> list = SplitList(highlights).ToList();

            return list;
        }

        public List<List<Highlight>> GetFavoriteHighlights(List<Teams> teams)
        {
            List<Highlight> highlights = new List<Highlight>();
            foreach (var team in teams)
            {
                highlights.AddRange(FootballDAL.GetHighlights().Where(x => x.side1.name == team.TeamName || x.side2.name == team.TeamName));
            }
            
            List<List<Highlight>> list = SplitList(highlights).ToList();

            return list;
        }

        //break Highlight list into lists of 10
        public List<List<Highlight>> SplitList(List<Highlight> highlights)

        {
            var list = new List<List<Highlight>>();

            for (int i = 0; i < highlights.Count; i += 10)
            {
                list.Add(highlights.GetRange(i, Math.Min(10, highlights.Count - i)));
            }
            return list;
        }
    }
}
