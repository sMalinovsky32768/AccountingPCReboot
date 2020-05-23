using System;
using System.Data;
using System.Data.SqlClient;

namespace AccountingPC
{
    public partial class AccountingPCWindow
    {
        private void AddSoftware()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = null;
                command = new SqlCommand($"AddLicenseSoftware", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@Name", null));
                command.Parameters.Add(new SqlParameter("@Price", null));
                command.Parameters.Add(new SqlParameter("@Count", null));
                command.Parameters.Add(new SqlParameter("@InvoiceID", null));
                command.Parameters.Add(new SqlParameter("@Type", null));
                command?.ExecuteNonQuery();
            }
        }

        private void AddOS()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = null;
                command = new SqlCommand($"AddOS", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@Name", null));
                command.Parameters.Add(new SqlParameter("@Price", null));
                command.Parameters.Add(new SqlParameter("@Count", null));
                command.Parameters.Add(new SqlParameter("@InvoiceID", null));
                command?.ExecuteNonQuery();
            }
        }

        private void UpdateSoftware()
        {

        }

        private void UpdateOS()
        {

        }

        private void DelSoftware()
        {

        }

        private void DelOS()
        {

        }
    }
}
