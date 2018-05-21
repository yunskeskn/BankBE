using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BankBE.Request;
using MySql.Data;

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

        public long insertTransaction(SaleRequest saleRequest,string token_data)
        {
            string sqlString = "Insert into bank_transaction (merchant_no, terminal_no, amount, transaction_status,token_data)values('" + saleRequest.merchant_no + "','" + saleRequest.terminal_no + "'," + saleRequest.amount + ", 'P', '"+token_data+"')";
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sqlString, conn);
            cmd.ExecuteNonQuery();
            long guid = cmd.LastInsertedId;
            return guid;
        }
    }
}
