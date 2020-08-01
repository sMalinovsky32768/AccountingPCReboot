using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace AccountingPC
{
    public class AudienceManagement
    {
        public AccountingPCWindow AccWindow { get; set; }

        public AudienceManagement(AccountingPCWindow accountingPCWindow)
        {
            AccWindow = accountingPCWindow;
        }

        public static string ConnectionString { get; } =
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        private AccountingCommand addAudience;

        private AccountingCommand delAudience;

        public AccountingCommand AddAudience => addAudience ??
            (addAudience = new AccountingCommand(
                obj =>
                {
                    var name = obj.ToString();
                    try
                    {
                        using (var connection = new SqlConnection(ConnectionString))
                        {
                            connection.Open();
                            var command = new SqlCommand("Insert Into Audience (Name) Values (@Name)", connection);
                            command.Parameters.AddWithValue("@Name", name);
                            command.ExecuteNonQuery();
                        }
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message, exception.GetType().Name, MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                    AccWindow.addAudienceGrid.Visibility = Visibility.Collapsed;
                    AccWindow.UpdateAudienceList();
                },
                obj =>
                {
                    if (string.IsNullOrEmpty(obj.ToString()))
                        return false;

                    return true;
                }));

        public AccountingCommand DelAudience => delAudience ??
            (delAudience = new AccountingCommand(
                obj =>
                {
                    var row = ((DataRowView)obj).Row;
                    try
                    {
                        using (var connection = new SqlConnection(ConnectionString))
                        {
                            connection.Open();
                            var command = new SqlCommand("Delete from Audience where ID=@ID", connection);
                            command.Parameters.AddWithValue("@ID", Convert.ToInt32(row["ID"]));
                            command.ExecuteNonQuery();
                        }
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message, exception.GetType().Name, MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                    AccWindow.UpdateAudienceList();
                },
                obj =>
                {
                    if (obj == null) return false;

                    return true;
                }));
    }
}
