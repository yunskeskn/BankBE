using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BankBE.Request
{
    public class DoTransactionRequest
    {

        [DataMember(Name = "guid")]
        public string guid { get; set; }

        [DataMember(Name = "card_no")]
        public string card_no { get; set; }

        [DataMember(Name = "cvc2")]
        public string cvc2 { get; set; }

        [DataMember(Name = "expire_date")]
        public string expire_date { get; set; }

        [DataMember(Name = "installment_num")]
        public int installment_num { get; set; }

    }
}
