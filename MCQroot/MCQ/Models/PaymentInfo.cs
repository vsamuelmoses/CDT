using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCQ.Models
{
    public class PaymentInfo
    {
        public bool DoesHaveAccessNow { get; set; }

        public DateTime? LastPaymentDate { get; set; }

        public DateTime? ExpireDate { get; set; } 
    }
}