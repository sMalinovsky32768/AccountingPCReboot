using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace AccountingPC
{
    public partial class AccountingPCWindow
    {
        private AccountingCommand addAudience;
        internal AccountingCommand AddAudience => addAudience ??
                    (addAudience = new AccountingCommand(obj =>
                    {
                        string name = obj.ToString();
                        using (SqlConnection connection = new SqlConnection(ConnectionString))
                        {
                            SqlCommand command = new SqlCommand("Insert Into Audience (Name) Values (@Name)", connection);
                            command.Parameters.AddWithValue("@Name", name);
                            command.ExecuteNonQuery();
                        }
                        addAudienceGrid.Visibility = Visibility.Collapsed;
                        UpdateAudienceList();
                    },
                    (obj) =>
                    {
                        if (string.IsNullOrEmpty(obj.ToString()))
                        {
                            return false;
                        }

                        return true;
                    }));

        private AccountingCommand delAudience;
        internal AccountingCommand DelAudience => delAudience ??
                    (delAudience = new AccountingCommand(obj =>
                    {
                        DataRow row = ((DataRowView)obj).Row;
                        using (SqlConnection connection = new SqlConnection(ConnectionString))
                        {
                            SqlCommand command = new SqlCommand("Delete from Audience where ID=@ID", connection);
                            command.Parameters.AddWithValue("@ID", Convert.ToInt32(row["ID"]));
                            command.ExecuteNonQuery();
                        }
                    },
                    (obj) =>
                    {
                        if (obj == null)
                        {
                            return false;
                        }

                        return true;
                    }));

        private void UpdateAudienceList()
        {
            //AudienceDataSet = new DataSet();
            //new SqlDataAdapter("SELECT * FROM dbo.[GetAllAudience]()", ConnectionString).Fill(AudienceDataSet);
            //audienceList.ItemsSource = AudienceDataSet.Tables[0].DefaultView;
            DefaultDataSet.Tables["Audience"].Clear();
            new SqlDataAdapter("SELECT * FROM dbo.[GetAllAudience]()", 
                ConnectionString).Fill(DefaultDataSet, "Audience");
            audienceList.ItemsSource = DefaultDataSet.Tables["Audience"].DefaultView;
            audienceList.DisplayMemberPath = "Name";
        }

        private void ChangeLocationView()
        {
            //AudiencePlaceDataSet = new DataSet();
            //new SqlDataAdapter($"Select distinct * From dbo.[GetAllLocationInAudience]({AudienceID})", ConnectionString).Fill(AudiencePlaceDataSet);
            //audienceTableView.ItemsSource = AudiencePlaceDataSet.Tables[0].DefaultView;
            DefaultDataSet.Tables["AudiencePlace"].Clear();
            new SqlDataAdapter($"Select * From dbo.[GetAllLocationInAudience]({AudienceID})",
                ConnectionString).Fill(DefaultDataSet, "AudiencePlace");
            audienceTableView.ItemsSource = DefaultDataSet.Tables["AudiencePlace"].DefaultView;
        }
    }
}
