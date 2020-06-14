using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Windows;

namespace AccountingPC
{
    public partial class AccountingPCWindow
    {
        private AccountingCommand addAudience;

        private AccountingCommand delAudience;

        internal AccountingCommand AddAudience => addAudience ??
                                                  (addAudience = new AccountingCommand(obj =>
                                                      {
                                                          var name = obj.ToString();
                                                          try
                                                          {
                                                              using (var connection = new SqlConnection(ConnectionString))
                                                              {
                                                                  connection.Open();
                                                                  var command =
                                                                      new SqlCommand(
                                                                          "Insert Into Audience (Name) Values (@Name)",
                                                                          connection);
                                                                  command.Parameters.AddWithValue("@Name", name);
                                                                  command.ExecuteNonQuery();
                                                              }
                                                          }
                                                          catch (Exception exception)
                                                          {
                                                              MessageBox.Show(exception.Message, exception.GetType().Name, MessageBoxButton.OK,
                                                                  MessageBoxImage.Error);
                                                          }

                                                          addAudienceGrid.Visibility = Visibility.Collapsed;
                                                          UpdateAudienceList();
                                                      },
                                                      obj =>
                                                      {
                                                          if (string.IsNullOrEmpty(obj.ToString())) return false;

                                                          return true;
                                                      }));

        internal AccountingCommand DelAudience => delAudience ??
                                                  (delAudience = new AccountingCommand(obj =>
                                                      {
                                                          var row = ((DataRowView) obj).Row;
                                                          try
                                                          {
                                                              using (var connection = new SqlConnection(ConnectionString))
                                                              {
                                                                  connection.Open();
                                                                  var command =
                                                                      new SqlCommand("Delete from Audience where ID=@ID",
                                                                          connection);
                                                                  command.Parameters.AddWithValue("@ID",
                                                                      Convert.ToInt32(row["ID"]));
                                                                  command.ExecuteNonQuery();
                                                              }
                                                          }
                                                          catch (Exception exception)
                                                          {
                                                              MessageBox.Show(exception.Message, exception.GetType().Name, MessageBoxButton.OK,
                                                                  MessageBoxImage.Error);
                                                          }
                                                      },
                                                      obj =>
                                                      {
                                                          if (obj == null) return false;

                                                          return true;
                                                      }));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UpdateAudienceList()
        {
            try
            {
                DefaultDataSet.Tables["Audience"].Clear();
                new SqlDataAdapter("SELECT * FROM dbo.[GetAllAudience]()",
                    ConnectionString).Fill(DefaultDataSet, "Audience");
                audienceList.ItemsSource = DefaultDataSet.Tables["Audience"].DefaultView;
                audienceList.DisplayMemberPath = "Name";
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, exception.GetType().Name, MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ChangeLocationView()
        {
            try
            {
                DefaultDataSet.Tables["AudiencePlace"].Clear();
                new SqlDataAdapter($"Select * From dbo.[GetAllLocationInAudience]({AudienceID})",
                    ConnectionString).Fill(DefaultDataSet, "AudiencePlace");
                audienceTableView.ItemsSource = DefaultDataSet.Tables["AudiencePlace"].DefaultView;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, exception.GetType().Name, MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}