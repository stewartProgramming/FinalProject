using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class QuestionMultipleChoice
    {
        public string Question { get; set; }
        public string CorrectAnswer { get; set; }
        public string FirstIncorrect { get; set; }
        public string SecondIncorrect { get; set; }
        public string ThirdIncorrect { get; set; }

    }
}
