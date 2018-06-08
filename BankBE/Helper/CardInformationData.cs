using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BankBE.Helper
{
    public class CardInformationData
    {
        [DataMember(Name = "CardHolderName")]
        public string CardHolderName { get; set; }

        [DataMember(Name = "CardNo")]
        public string CardNo { get; set; }

        [DataMember(Name = "Cvc2")]
        public string Cvc2 { get; set; }

        [DataMember(Name = "ExpireDate")]
        public string ExpireDate { get; set; }

    }
}