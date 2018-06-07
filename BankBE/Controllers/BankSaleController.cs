using BankBE.Data;
using BankBE.Request;
using BankBE.Response;
using BankBE.Utility;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BankBE.Controllers
{
    public class BankSaleController : ApiController
    {
        // GET: api/BankSale
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/BankSale/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/BankSale
        public HttpResponseMessage Post([FromBody]SaleRequest saleRequest)
        {
            //json formatında data gelcek
            //formatlıcak
            //dbye token oluşturup kayıt atcak            

            SaleResponse saleResponse = new SaleResponse();
            
            SalePersistence sp = new SalePersistence();
            long bankguid = 0;
            bankguid = sp.insertTransaction(saleRequest);

            //token algorihm
            
            string token_data = new Utilities().generateToken(saleRequest, bankguid.ToString());
            sp.updateTransactionTokenByGuid(token_data, bankguid);
            saleResponse.token_data = token_data;

            JObject payLoad = new JObject(
                new JProperty("error_code", "0000000"),
                new JProperty("error_desc", "Basarili"),
                new JProperty("token_data", saleResponse.token_data),
                new JProperty("bank_transaction_guid", bankguid)
            );

            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(payLoad.ToString()) };
        }

        // PUT: api/BankSale/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/BankSale/5
        public void Delete(int id)
        {
        }
    }
}
