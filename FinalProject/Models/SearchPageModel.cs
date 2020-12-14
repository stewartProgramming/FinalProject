using System.Collections.Generic;

namespace FinalProject.Models
{
    public class SearchPageModel
    {
        public IEnumerable<Highlight> AnotherHighlight { get; set; }
        public string SearchFor { get; set; }
    }
}
