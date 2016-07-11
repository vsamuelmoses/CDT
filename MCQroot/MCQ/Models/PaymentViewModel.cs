using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MCQ.Models
{
    public class PaymentViewModel
    {
        [Required]
        [DisplayName("Card Number")]
        [MaxLength(16, ErrorMessage = "Incorrect Length"), MinLength(15, ErrorMessage = "Incorrect Length")]
        public string CardNumber { get; set; }

        [Required]
        [Range(minimum: 16, maximum: 50, ErrorMessage = "Incorrect year.")]
        public int ExpiryYear { get; set; }

        [Required]
        [Range(minimum: 1, maximum: 12, ErrorMessage = "Incorrect month.")]
        public int ExpiryMonth { get; set; }

        [Required]
        [MaxLength(4, ErrorMessage = "Incorrect Length"), MinLength(3, ErrorMessage = "Incorrect Length")]
        public string Cvv { get; set; }

        
        [Range(minimum: 1, maximum: 1, ErrorMessage = "You should agree on terms.")]
        public bool AgreeWithTerms { get; set; }
    }
}