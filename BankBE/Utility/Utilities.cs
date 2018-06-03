using BankBE.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankBE.Utility
{
    public class Utilities
    {
        public string generateToken(SaleRequest saleRequest)
        {
            //TODO encryption fonksiyonlarıyla çok daha güvenilir halde taşınması lazım.
            return saleRequest.merchant_no + saleRequest.terminal_no + saleRequest.amount + saleRequest.merchant_transaction_guid;
        }



    }
}