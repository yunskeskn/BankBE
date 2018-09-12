using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BankBE.Request;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace BankBE.Data
{
    public class SalePersistence
    {
        private MySql.Data.MySqlClient.MySqlConnection conn;

        public SalePersistence()
        {
            string myConnectionString;
            myConnectionString = "server=198.71.227.92;Port=3306;uid=yunskeskn;pwd=Canik_6168;database=poc_be;SslMode=none";
            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = myConnectionString;
                conn.Open();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {

                throw;
            }
        }

        public long insertTransaction(SaleRequest saleRequest)
        {
            string sqlStringInsert = "Insert into BANK_TRANSACTION (merchant_no, terminal_no, amount, transaction_status, merchant_transaction_guid)values('" + saleRequest.merchant_no + "','" + saleRequest.terminal_no + "'," + saleRequest.amount + ", 'P', "+ saleRequest.merchant_transaction_guid +")";
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sqlStringInsert, conn);
            cmd.ExecuteNonQuery();
            long guidTokenless = cmd.LastInsertedId;
            return guidTokenless;
        }

        public void updateTransactionTokenByGuid(string token_data, long guid)
        {
            string sqlStringInsert = "update BANK_TRANSACTION set token_data = '" + token_data +"' where guid = "+ guid;
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sqlStringInsert, conn);
            cmd.ExecuteNonQuery();
        }

        public String selectTokenDataByGuid(Int64 guid)
        {
            MySqlDataReader rdr = null;
            string sqlStringInsert = "select token_data from BANK_TRANSACTION where guid = " + guid;
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sqlStringInsert, conn);
            rdr = cmd.ExecuteReader();
            if (rdr.Read())
                return rdr.GetString(0);

            else
                return "Guid doesn't match!!!";
        }

        public PosnetRequest selectTransactionByGuid(Int64 guid)
        {
            MySqlDataReader rdr = null;
            PosnetRequest posnetRequest = new PosnetRequest();
            string sqlStringInsert = "select MERCHANT_NO, TERMINAL_NO, AMOUNT from BANK_TRANSACTION where GUID = " + guid;
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sqlStringInsert, conn);
            rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                posnetRequest.MerchantNo = rdr.GetString(0).ToString();
                posnetRequest.TerminalNo = rdr.GetString(1).ToString();
                posnetRequest.Amount = (rdr.GetDecimal(2) * 100).ToString();
            }
            rdr.Close();
            return posnetRequest;
        }

        public void insertTransactionLog(string methodName, string responseJson, string requestJson, long guidOrginal, string url)
        {
            if (responseJson != "" && requestJson != "" && guidOrginal != 0 && url != "")
            {
                long sysDate = 0;
                long sysTime = 0;
                DateTime dt = new DateTime();
                dt = DateTime.Now;
                sysDate = Convert.ToInt64(dt.Year.ToString().PadLeft(4, '0') + dt.Month.ToString().PadLeft(2, '0') + dt.Day.ToString().PadLeft(2, '0'));
                sysTime = Convert.ToInt64(dt.Hour.ToString().PadLeft(2, '0') + dt.Minute.ToString().PadLeft(2, '0') + dt.Second.ToString().PadLeft(2, '0'));
                string sqlStringInsert = "Insert into BANK_TRANSACTION_LOG (METHOD_NAME, GUID_ORGINAL, RESPONSE_JSON, REQUEST_JSON, SYS_DATE, SYS_TIME)values( '" + methodName + "', " + guidOrginal + ", '" + responseJson + "', '" + requestJson + "', " + sysDate + ", " + sysTime + ", '" + url + "')";
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sqlStringInsert, conn);
                cmd.ExecuteNonQuery();
            }
        }

        public void updateTransactionStatus(string status, long guid)
        {
            string sqlStringInsert = "update BANK_TRANSACTION set transaction_status = '" + status + "' where guid = " + guid;
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sqlStringInsert, conn);
            cmd.ExecuteNonQuery();
        }

        public string selectMerchantGuidByGuid(Int64 guid)
        {
            MySqlDataReader rdr = null;
            string sqlStringInsert = "select MERCHANT_TRANSACTION_GUID from BANK_TRANSACTION where GUID = " + guid;
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sqlStringInsert, conn);
            rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                return rdr.GetDecimal(0).ToString();
            }
            rdr.Close();
            return "";
        }

    }
}
