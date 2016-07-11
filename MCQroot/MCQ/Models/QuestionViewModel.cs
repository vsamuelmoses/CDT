using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MCQ.Domain;

namespace MCQ.Models
{
    public class QuestionViewModel
    {
        public Question Question { get; set; }

        public List<Answer> UserAnswers { get; set; }

        public bool? IsCorrect { get; set; }
    }
}