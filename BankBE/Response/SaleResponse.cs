using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BankBE.Response
{
    [DataContract]
    public class SaleResponse
    {
        [DataMember(Name = "error_code")]
        public string error_code { get; set; }

        [DataMember(Name = "error_desc")]
        public string error_desc { get; set; }

        [DataMember(Name = "token_data")]
        public string token_data { get; set; }

       
    }
}