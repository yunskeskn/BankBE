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

        [DataMember(Name = "merchant_transaction_guid")]
        public long merchant_transaction_guid { get; set; }

        [DataMember(Name = "card_no")]
        public string card_no { get; set; }

        [DataMember(Name = "cvc2")]
        public string cvc2 { get; set; }

        [DataMember(Name = "expire_date")]
        public string expire_date { get; set; }

        [DataMember(Name = "encryption_key")]
        public string encryption_key { get; set; }

        [DataMember(Name = "mac")]
        public string mac { get; set; }

        [DataMember(Name = "mac_params")]
        public string mac_params { get; set; }


    }
}