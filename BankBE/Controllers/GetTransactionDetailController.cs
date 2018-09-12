using BankBE.Data;
using BankBE.Request;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BankBE.Controllers
{
    public class GetTransactionDetailController : ApiController
    {
        // GET: api/GetTransactionDetail
        public JObject Get()
        {
            JObject payLoad = new JObject(
                new JProperty("error_code", "1"),
                new JProperty("error_desc", "2"),
                new JProperty("merchant_no", "3"),
                new JProperty("terminal_no", "4"),
                new JProperty("amount", "5")
            );
            return payLoad;
        }

        // GET: api/GetTransactionDetail/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/GetTransactionDetail
        public JObject Post([FromBody]GetTransactionDetailRequest getTransactionDetailRequest)
        {
            //TODO qr string geldi,çözümle
            //yeni nesne classı transaction diye,
            //o nesneyle dbye git,bekliyor statulu kaydı bul dicez
            //response olarak sonucunu dön
            String error_code = "";
            String error_desc = "";
            string jsonRequest = "";
            try
            {
                jsonRequest = getTransactionDetailRequest.qr_string;
            }
            catch (Exception)
            {
                error_code = "1234567";
                error_desc = "Hatali QR";
                throw;
            }
            String[] getTokenData = jsonRequest.Split('=');


            String[] splitted = getTokenData[1].Split(':');
            long guid = Convert.ToInt64(splitted[3]);



            SalePersistence sp = new SalePersistence();
            string db_token = sp.selectTokenDataByGuid(guid);
            if (db_token == getTokenData[1])
            {
                error_code = "0000000";
                error_desc = "Basarili";
            }
            else
            {
                error_code = "1234567";
                error_desc = "Hata!!!";
            }

            JObject payLoad = new JObject(
                new JProperty("error_code", error_code),
                new JProperty("error_desc", error_desc),
                new JProperty("merchant_no", splitted[0]),
                new JProperty("terminal_no", splitted[1]),
                new JProperty("amount", splitted[2]),
                new JProperty("guid", guid.ToString())
           );


            return payLoad;
        }

        // PUT: api/GetTransactionDetail/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/GetTransactionDetail/5
        public void Delete(int id)
        {
        }
    }
}