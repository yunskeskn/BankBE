using BankBE.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using BankBE.Data;
using System.Net.Http.Headers;
using System.Text;
using BankBE.Utility;
using BankBE.Helper;
using System.Security.Cryptography;

namespace BankBE.Controllers
{
    public class DoTransactionController : ApiController
    {
        // GET: api/DoTransaction
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/DoTransaction/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/DoTransaction
        public async System.Threading.Tasks.Task<JObject> Post([FromBody]DoTransactionRequest doTransactionRequest)
        {
            Utilities util = new Utilities();
            SalePersistence sp = new SalePersistence();
            PosnetRequest posnetRequest = sp.selectTransactionByGuid(Convert.ToInt64(doTransactionRequest.guid));
            //merchant,terminal,amount dbden çekip doldu.
            //ApiType,ApiVersion ,IsMailOrder,PaymentInstrumentType,currencycode obje oluşurken doldu.
            posnetRequest.ApiType = "JSON";
            posnetRequest.ApiVersion = "1.0.0.0";
            posnetRequest.IsMailOrder = "N";
            posnetRequest.PaymentInstrumentType = "CARD";
            posnetRequest.CurrencyCode = "TL";
            posnetRequest.OrderId = "MPOS_" + doTransactionRequest.guid.PadLeft(19, '0');//üret
            posnetRequest.CardInformationData = new CardInformationData();
            posnetRequest.CardInformationData.CardNo = doTransactionRequest.card_no;
            posnetRequest.CardInformationData.Cvc2 = doTransactionRequest.cvc2;
            posnetRequest.CardInformationData.ExpireDate = doTransactionRequest.expire_date;
            posnetRequest.InstallmentCount = doTransactionRequest.installment_num;

            if(doTransactionRequest.installment_num>0)
            {
                posnetRequest.InstallmentType = "Y";
            }
            else
            {
                posnetRequest.InstallmentType = "N";
            }
            string encryptionKey = "10,10,10,10,10,10,10,10";
            posnetRequest.MACParams = "MerchantNo:TerminalNo:CardNo:Cvc2:ExpireDate";
            string macCheck = "6706598320670019854506344147404266000200410,10,10,10,10,10,10,10";//util.generateMac(posnetRequest) + encryptionKey.Trim();
            string k = util.generateMac(posnetRequest) + encryptionKey.Trim();
            if (macCheck == k)
            {
                string a = ";";
            }
            var sha = new SHA256CryptoServiceProvider();
            var hashedMacCheck = Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(macCheck)));
            posnetRequest.MAC = hashedMacCheck;
            sha.Clear();
            //posnetRequest.MAC = util.generateMac(posnetRequest);
            //TripleDESCustom tripleDES = new TripleDESCustom();
            //string macString = util.generateMac(posnetRequest);
            //var EncHashMacString = tripleDES.Encrypt(encryptionKey, macString, CipherMode.CBC);
            //posnetRequest.MAC = EncHashMacString;


            ////abdullah abinin servisini çağır posnetRequest ile,responsu ile de gelsin
            using (HttpClient client = new HttpClient())
            {
                string serviceUrl = "https://posnetict.yapikredi.com.tr/MerchantService/MerchantJSONAPI.svc/Sale";
                string serviceUrlMerchant = "http://192.168.43.14:58070/api/CompleteTransaction";
                client.DefaultRequestHeaders.Clear();
                var username = "user";
                var password = "pass";
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}")));

                JObject payLoad = new JObject(
                       new JProperty("ApiType", posnetRequest.ApiType),
                       new JProperty("ApiVersion", posnetRequest.ApiVersion),
                       new JProperty("MAC", posnetRequest.MAC),
                       new JProperty("MACParams", posnetRequest.MACParams),
                       new JProperty("MerchantNo", posnetRequest.MerchantNo),
                       new JProperty("TerminalNo", posnetRequest.TerminalNo),
                       new JProperty("CardInformationData",
                           new JObject(
                               new JProperty("CardHolderName", "MEHMET EMIN TOPRAK"),
                               new JProperty("CardNo", posnetRequest.CardInformationData.CardNo),
                               new JProperty("Cvc2", posnetRequest.CardInformationData.Cvc2),
                               new JProperty("ExpireDate", posnetRequest.CardInformationData.ExpireDate)
                             )
                       ),
                       new JProperty("IsMailOrder", posnetRequest.IsMailOrder),
                       new JProperty("IsTDSecureMerchant", null),
                       new JProperty("PaymentInstrumentType", posnetRequest.PaymentInstrumentType),
                       new JProperty("Amount", posnetRequest.Amount),
                       new JProperty("CurrencyCode", posnetRequest.CurrencyCode),
                       new JProperty("OrderId", posnetRequest.OrderId),
                       new JProperty("InstallmentCount", posnetRequest.InstallmentCount.ToString()),
                       new JProperty("InstallmentType", posnetRequest.InstallmentType),
                       new JProperty("PointAmount", null)
                 );
                
                var httpContent = new StringContent(payLoad.ToString(), Encoding.UTF8, "application/json");

                using (HttpResponseMessage response = await client.PostAsync(serviceUrl, httpContent))
                {
                    try
                    {
                        response.EnsureSuccessStatusCode();
                        // Handle success
                    }
                    catch (HttpRequestException e)
                    {
                        // Handle failure
                    }

                    int timeflag = 0;
                    JObject jsonResponse= new JObject();
                    string errorCode = "";
                    string errorDesc = ""; 
                    string status = "P";
                    string merchant_transaction_guid = "";


                    string responseBody = await response.Content.ReadAsStringAsync();
                    if (responseBody != "")
                    {
                        //gelen response daki bank_transaction_guid i where guid i mrcguid olanla dbde update et,tokendatayıda update et şekerim
                        //"{\"ServiceResponseData\":{\"ResponseCode\":\"E216\",\"ResponseDescription\":\"Mac Doğrulama hatalı\"},\"AuthCode\":null,\"ReferenceCode\":null,\"PointDataList\":null,\"InstallmentData\":null,\"MessageData\":null}"
                        JObject json = JObject.Parse(responseBody);
                        errorCode = json["ServiceResponseData"]["ResponseCode"].ToString().PadLeft(7, '0');
                        errorDesc = json["ServiceResponseData"]["ResponseDescription"].ToString();
                    }

                   

                    if (errorCode == "0000000")
                        status = "C"; //başarılı
                    else
                        status = "E"; //hatalı


                    //bankanın işlem statüsünü günceller
                    sp.updateTransactionStatus(status, Convert.ToInt64(doTransactionRequest.guid));

                    //merchant_ın guidsi elimde zaten
                    merchant_transaction_guid = sp.selectMerchantGuidByGuid(Convert.ToInt64(doTransactionRequest.guid));


                    //MerchantBE servisini çağır
                    JObject jsonMerchant = new JObject(
                        new JProperty("status", status),
                        new JProperty("merchant_guid", merchant_transaction_guid));
                    httpContent = new StringContent(jsonMerchant.ToString(), Encoding.UTF8, "application/json");
                    using (HttpResponseMessage responseMerchant = await client.PostAsync(serviceUrlMerchant, httpContent))
                    {
                        try
                        {
                            responseMerchant.EnsureSuccessStatusCode();
                            // Handle success
                        }
                        catch (HttpRequestException e1)
                        {
                            // Handle failure
                        }
                        string responseBodyMerchant = await response.Content.ReadAsStringAsync();
                    }



                        // "{\"ServiceResponseData\":{\"ResponseCode\":\"E216\",\"ResponseDescription\":\"Mac Doğrulama hatalı\"},\"AuthCode\":null,\"ReferenceCode\":null,\"PointDataList\":null,\"InstallmentData\":null,\"MessageData\":null}"
                        jsonResponse = new JObject(
                        new JProperty("error_code", errorCode),
                        new JProperty("error_desc", errorDesc),
                        new JProperty("token_data", ""),
                        new JProperty("bank_transaction_guid", "")
                    );

                    // JObject jsonResponse = new JObject(
                    //    new JProperty("error_code", "0000001")
                    //);

                    return jsonResponse;
                }
            }
        }

        // PUT: api/DoTransaction/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/DoTransaction/5
        public void Delete(int id)
        {
        }
    }
}
