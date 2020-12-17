using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    public partial class QuizStandings
    {
        [Key]
        public string UserId { get; set; }
        public int QuizAttempts { get; set; }
        public int CorrectAnswers { get; set; }
        public double Accuracy { get; set; }

        public virtual AspNetUsers User { get; set; }
    }
}
