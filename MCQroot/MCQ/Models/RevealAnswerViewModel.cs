using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCQ.Models
{
    public class RevealAnswerViewModel
    {
        public int QuestionId { get; set; }

        public List<int> Answers { get; set; }
    }
}