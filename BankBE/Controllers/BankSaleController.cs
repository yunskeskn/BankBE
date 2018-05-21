using BankBE.Data;
using BankBE.Request;
using BankBE.Response;
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

            //token algorihm
            string token_data = "token is here";

            SalePersistence sp = new SalePersistence();
            long guid = 0;
            guid = sp.insertTransaction(saleRequest, token_data);

            saleResponse.token_data = token_data;

            JObject payLoad = new JObject(
                new JProperty("error_code", saleResponse.error_code),
                new JProperty("error_desc", saleResponse.error_desc),
                new JProperty("token_data", saleResponse.token_data)
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
