using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace BankBE.Request
{
    [DataContract]
    public class SaleRequest
    {
        [DataMember(Name = "merchant_no")]
        public string merchant_no { get; set; }

        [DataMember(Name = "terminal_no")]
        public string terminal_no { get; set; }

        [DataMember(Name = "amount")]
        public double amount { get; set; }
    }
}