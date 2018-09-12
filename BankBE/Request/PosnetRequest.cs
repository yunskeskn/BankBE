using BankBE.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BankBE.Request
{
    public class PosnetRequest
    {
        [DataMember(Name = "ApiType")]
        public string ApiType { get; set; }//= "JSON";

        [DataMember(Name = "ApiVersion")]
        public string ApiVersion { get; set; } //= "1.0.0.0";

        [DataMember(Name = "MAC")]
        public string MAC { get; set; }

        [DataMember(Name = "MACParams")]
        public string MACParams { get; set; }

        [DataMember(Name = "MerchantNo")]
        public string MerchantNo { get; set; }

        [DataMember(Name = "TerminalNo")]
        public string TerminalNo { get; set; }

        [DataMember(Name = "CardInformationData")]
        public CardInformationData CardInformationData { get; set; }

        [DataMember(Name = "IsMailOrder")]
        public string IsMailOrder { get; set; } //= "Y";

        [DataMember(Name = "PaymentInstrumentType")]
        public string PaymentInstrumentType { get; set; } //= "CARD";

        [DataMember(Name = "Amount")]
        public string Amount { get; set; }

        [DataMember(Name = "CurrencyCode")]
        public string CurrencyCode { get; set; } //= "TL";

        [DataMember(Name = "OrderId")] //"VZX_00000000000047285379"
        public string OrderId { get; set; }

        [DataMember(Name = "InstallmentCount")]
        public int InstallmentCount { get; set; }

        [DataMember(Name = "InstallmentType")]
        public string InstallmentType { get; set; }


    }
}