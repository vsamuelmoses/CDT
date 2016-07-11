using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Braintree;

namespace PaymentProcessor
{
    public class PaymentGatewayProvider
    {
         
        BraintreeGateway gateway = new BraintreeGateway
        {
            Environment = Braintree.Environment.SANDBOX,
            MerchantId = ConfigurationManager.AppSettings["MerchantId"],
            PublicKey = ConfigurationManager.AppSettings["PublicKey"],
            PrivateKey = ConfigurationManager.AppSettings["PrivateKey"]
        };

        public bool CardPayment(string cardNumber, string cvv, string expirationMonth, string expirationYear, decimal paymentAmount, ref string message)
        {
        
            TransactionRequest request = new TransactionRequest
            {
                Amount = paymentAmount, //Transaction Amount
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = cardNumber,
                    CVV = cvv,
                    ExpirationMonth = expirationMonth,
                    ExpirationYear = expirationYear
                },
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            message = result.Message;
            if (result.IsSuccess())
            {
                Transaction transaction = result.Target;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool PaypalPayment(string paymentMethodNsonce, decimal paymentAmount)
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = paymentAmount,
                PaymentMethodNonce = paymentMethodNsonce,
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);

            if (result.IsSuccess())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string GeneratePaypalToken()
        {
            return gateway.ClientToken.generate();
        }
    }
}
