using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class QuizResultViewModel
    {
        public string Answer { get; set; }
        public List<string> RandomAnswers { get; set; }
        public string CorrectAnswer { get; set; }
        public string Question { get; set; }
    }
}
