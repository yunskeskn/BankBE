using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BankBE.Request
{
    public class GetTransactionDetailRequest
    {
        [DataMember(Name = "qr_string")]
        public string qr_string { get; set; }
    }
}