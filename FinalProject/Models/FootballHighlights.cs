using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class FootballHighlights
    {
        public string name { get; set; }
        public List<Highlight> Property1 { get; set; }
    }

    public class Highlight
    {
        public string title { get; set; }
        public string embed { get; set; }
        public string url { get; set; }
        public string thumbnail { get; set; }
        public DateTime date { get; set; }
        public Side1 side1 { get; set; }
        public Side2 side2 { get; set; }
        public Competition competition { get; set; }
        public Video[] videos { get; set; }
    }

    public class Side1
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Side2
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Competition
    {
        public string name { get; set; }
        public int id { get; set; }
        public string url { get; set; }
    }

    public class Video
    {
        public string title { get; set; }
        public string embed { get; set; }
        public List<VideoComments> VideoComments { get; set; }
    }

}
