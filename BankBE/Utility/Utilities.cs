using BankBE.Helper;
using BankBE.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankBE.Utility
{
    public class Utilities
    {
        public string generateToken(SaleRequest saleRequest, String guid)
        {
            //TODO encryption fonksiyonlarıyla çok daha güvenilir halde taşınması lazım.
            return saleRequest.merchant_no + ":" + saleRequest.terminal_no + ":" + saleRequest.amount + ":" + guid;
        }

        
        public string generateMac(PosnetRequest posnetRequest)
        {
            //MacParams: "MerchantNo:TerminalNo:CardNo:Cvc2:ExpireDate"  - EncryptionKey
            String[] splittedParameters = posnetRequest.MACParams.Split(':');
            String macCheck = "";

            foreach (var keyName in splittedParameters)
            {               
                if (keyName == "Cvc2")
                {
                    macCheck += "***";
                }
                else if (keyName == "ExpireDate")
                {
                    macCheck += "****";
                }
                else
                {
                    macCheck += getRequestDataValue(keyName, posnetRequest);
                }
            }

            //byte[] hashMac = TripleDESCustom.sha256Hashing(macCheck);
            //var hashMacStr = Convert.ToBase64String(hashMac);

            return macCheck;
        }

        private string getRequestDataValue(string keyName, PosnetRequest posnetRequest)
        {

            switch (keyName)
            {
                case "MerchantNo":
                    return posnetRequest.MerchantNo;
                case "TerminalNo":
                    return posnetRequest.TerminalNo;
                case "CardNo":
                    return posnetRequest.CardInformationData.CardNo;
                case "Cvc2":
                    return posnetRequest.CardInformationData.Cvc2;
                case "ExpireDate":
                    return posnetRequest.CardInformationData.CardNo;
                case "Amount":
                    return posnetRequest.Amount;
            }
            return null;
        }
    }
}