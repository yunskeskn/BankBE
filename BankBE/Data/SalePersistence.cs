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
            myConnectionString = "server=127.0.0.1;uid=root;pwd=3418;database=bank";
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
            string sqlStringInsert = "Insert into bank_transaction (merchant_no, terminal_no, amount, transaction_status, merchant_transaction_guid)values('" + saleRequest.merchant_no + "','" + saleRequest.terminal_no + "'," + saleRequest.amount + ", 'P', "+ saleRequest.merchant_transaction_guid +")";
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sqlStringInsert, conn);
            cmd.ExecuteNonQuery();
            long guidTokenless = cmd.LastInsertedId;
            return guidTokenless;
        }

        public void updateTransactionTokenByGuid(string token_data, long guid)
        {
            string sqlStringInsert = "update bank_transaction set token_data = '"+ token_data +"' where guid = "+ guid;
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sqlStringInsert, conn);
            cmd.ExecuteNonQuery();
        }

        public String selectTokenDataByGuid(Int64 guid)
        {
            MySqlDataReader rdr = null;
            string sqlStringInsert = "select token_data from bank.bank_transaction where guid = " + guid;
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sqlStringInsert, conn);
            rdr = cmd.ExecuteReader();
            if (rdr.Read())
                return rdr.GetString(0);

            else
                return "Guid doesn't match!!!";
        }

    }
}
