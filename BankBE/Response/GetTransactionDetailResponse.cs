using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BankBE.Response
{
    public class GetTransactionDetailResponse
    {
        [DataMember(Name = "merchant_name")]
        public string merchant_name { get; set; }

        [DataMember(Name = "amount")]
        public double amount { get; set; }

        [DataMember(Name = "error_code")]
        public string error_code { get; set; }

        [DataMember(Name = "error_desc")]
        public string error_desc { get; set; }

    }
}