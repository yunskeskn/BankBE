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

        
        public string generateMacField(SaleRequest saleRequest, String mac_params)
        {
            //MacParams: "MerchantNo:TerminalNo:CardNo:Cvc2:ExpireDate"  - EncryptionKey
            String[] splittedParameters = mac_params.Split(':');
            String macCheck = "";

            foreach (var keyName in splittedParameters)
            {
                macCheck += getRequestDataValue(keyName, saleRequest);
            }
            macCheck += saleRequest.encryption_key;

            byte[] hashMac = TripleDESCustom.sha256Hashing(macCheck);
            var hashMacStr = Convert.ToBase64String(hashMac);

            //if (saleRequest.mac == hashMacStr)//Gelen data yolda bozulmamışsa...
            //{
            //    //if req.isSecure == 'Y'
            //    decryptedCardInfo = tripleDESCustom.DecryptCBC(db_enc_key, saleRequest.encryption_key);
            //}
            return hashMacStr;
        }

        private string getRequestDataValue(string keyName, SaleRequest saleRequest)
        {

            switch (keyName)
            {
                case "MerchantNo":
                    return saleRequest.merchant_no;
                case "TerminalNo":
                    return saleRequest.terminal_no;
                case "CardNo":
                    return saleRequest.card_no;
                case "Cvc2":
                    return saleRequest.cvc2;
                case "ExpireDate":
                    return saleRequest.expire_date;
                case "Amount":
                    return saleRequest.amount.ToString();
            }
            return null;
        }
    }
}