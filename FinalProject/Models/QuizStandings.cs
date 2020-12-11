using System;
using System.Collections.Generic;

namespace FinalProject.Models
{
    public partial class QuizStandings
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int QuizAttempts { get; set; }
        public int CorrectAnswers { get; set; }

        public virtual AspNetUsers User { get; set; }
    }
}
