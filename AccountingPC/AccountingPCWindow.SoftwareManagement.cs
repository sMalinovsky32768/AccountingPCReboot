using System;
using System.Data;
using System.Data.SqlClient;

namespace AccountingPC
{
    public partial class AccountingPCWindow
    {
        private void ChangeSoftwareView()
        {
            switch (softwareCategoryList.SelectedIndex)
            {
                case 0:
                    softwareView.ItemsSource = softwareDataSet.Tables[0].DefaultView;
                    TypeSoft = TypeSoft.LicenseSoftware;
                    break;
                case 1:
                    softwareView.ItemsSource = osDataSet.Tables[0].DefaultView;
                    TypeSoft = TypeSoft.OS;
                    break;
            }
        }

        private void UpdateSoftwareData()
        {
            switch (TypeSoft)
            {
                case TypeSoft.LicenseSoftware:
                    softwareDataAdapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllSoftware()", ConnectionString);
                    softwareDataSet = new DataSet();
                    softwareDataAdapter.Fill(softwareDataSet);
                    break;
                case TypeSoft.OS:
                    osDataAdapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllOS()", ConnectionString);
                    osDataSet = new DataSet();
                    osDataAdapter.Fill(osDataSet);
                    break;
            }
        }

        private void UpdateAllSoftwareData()
        {
            softwareDataAdapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllSoftware()", ConnectionString);
            softwareDataSet = new DataSet();
            softwareDataAdapter.Fill(softwareDataSet);

            osDataAdapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllOS()", ConnectionString);
            osDataSet = new DataSet();
            osDataAdapter.Fill(osDataSet);
        }

        private void AddSoftware()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = null;
                command = new SqlCommand($"AddLicenseSoftware", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@Name", softwareName.Text));
                command.Parameters.Add(new SqlParameter("@Price", Convert.ToSingle(softwareCost.Text)));
                command.Parameters.Add(new SqlParameter("@Count", Convert.ToInt32(softwareCount.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceID", softwareInvoice.Text));
                command.Parameters.Add(new SqlParameter("@Type", ((DataRowView)typeLicense.SelectedItem).Row[0]));
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
                command.Parameters.Add(new SqlParameter("@Name", softwareName.Text));
                command.Parameters.Add(new SqlParameter("@Price", Convert.ToSingle(softwareCost.Text)));
                command.Parameters.Add(new SqlParameter("@Count", Convert.ToInt32(softwareCount.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceID", softwareInvoice.Text));
                command?.ExecuteNonQuery();
            }
        }

        private void UpdateSoftware()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = null;
                command = new SqlCommand($"UpdateLicenseSoftware", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@Name", softwareName.Text));
                command.Parameters.Add(new SqlParameter("@Price", Convert.ToSingle(softwareCost.Text)));
                command.Parameters.Add(new SqlParameter("@Count", Convert.ToInt32(softwareCount.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceID", softwareInvoice.Text));
                command.Parameters.Add(new SqlParameter("@Type", ((DataRowView)typeLicense.SelectedItem).Row[0]));
                command?.ExecuteNonQuery();
            }
        }

        private void UpdateOS()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = null;
                command = new SqlCommand($"UpdateOS", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@Name", softwareName.Text));
                command.Parameters.Add(new SqlParameter("@Price", Convert.ToSingle(softwareCost.Text)));
                command.Parameters.Add(new SqlParameter("@Count", Convert.ToInt32(softwareCount.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceID", softwareInvoice.Text));
                command?.ExecuteNonQuery();
            }
        }

        private void GetLicenseSoftware(Software software, int id)
        {
            String commandString;
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = $"SELECT * FROM dbo.GetLicenseSoftwareByID({id})";
                reader = new SqlCommand(commandString, connection).ExecuteReader();
                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        software = new LicenseSoftware()
                        {
                            Name = reader["Name"].ToString(),
                            Cost = Convert.ToSingle(reader["Cost"]),
                            Count = Convert.ToInt32(reader["Count"]),
                            InvoiceNumber = reader["InvoiceNumber"].ToString(),
                            Type = Convert.ToInt32(reader["Type"]),
                        };

                        softwareName.Text = software.Name;
                        softwareCost.Text = software.Cost.ToString();
                        softwareCount.Text = software.Count.ToString();
                        softwareInvoice.Text = software.InvoiceNumber;
                        foreach (object obj in typeLicense.ItemsSource)
                        {
                            DataRowView row;
                            row = (obj as DataRowView);
                            if (Convert.ToUInt32(row.Row[0]) == (software as LicenseSoftware).Type)
                            {
                                typeLicense.SelectedItem = row;
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void GetOS(Software software, int id)
        {
            String commandString;
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = $"SELECT * FROM dbo.GetOSByID({id})";
                reader = new SqlCommand(commandString, connection).ExecuteReader();
                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        software = new OS()
                        {
                            Name = reader["Name"].ToString(),
                            Cost = Convert.ToSingle(reader["Cost"]),
                            Count = Convert.ToInt32(reader["Count"]),
                            InvoiceNumber = reader["InvoiceNumber"].ToString(),
                        };

                        softwareName.Text = software.Name;
                        softwareCost.Text = software.Cost.ToString();
                        softwareCount.Text = software.Count.ToString();
                        softwareInvoice.Text = software.InvoiceNumber;
                    }
                }
            }
        }
    }
}
