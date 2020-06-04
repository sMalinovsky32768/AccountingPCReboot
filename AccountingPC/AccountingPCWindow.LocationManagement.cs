using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AccountingPC
{
    public partial class AccountingPCWindow
    {
        private AccountingCommand addAudience;
        internal AccountingCommand AddAudience
        {
            get
            {
                return addAudience ??
                    (addAudience = new AccountingCommand(obj =>
                    {
                        String name = obj.ToString();
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
                        if (string.IsNullOrEmpty(obj.ToString())) return false;
                        return true;
                    }));
            }
        }

        private AccountingCommand delAudience;
        internal AccountingCommand DelAudience
        {
            get
            {
                return delAudience ??
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
                        if (obj == null) return false;
                        return true;
                    }));
            }
        }

        private void UpdateAudienceList()
        {
            audienceDataAdapter = new SqlDataAdapter("SELECT * FROM dbo.[GetAllAudience]()", ConnectionString);
            audienceDataSet = new DataSet();
            audienceDataAdapter.Fill(audienceDataSet);
            audienceList.ItemsSource = audienceDataSet.Tables[0].DefaultView;
            audienceList.DisplayMemberPath = "Name";
        }

        private void UpdateLocationData()
        {
            audiencePlaceDataSet = new DataSet();
            new SqlDataAdapter($"Select distinct * From dbo.[GetAllLocationInAudience]({AudienceID})", ConnectionString).Fill(audiencePlaceDataSet);
        }

        private void ChangeLocationView()
        {
            UpdateLocationData();
            audienceTableView.ItemsSource = audiencePlaceDataSet.Tables[0].DefaultView;
            //invoiceItemsControl.ItemsSource = invoiceSoftwareAndEquipmentDataSet.Tables;

            //invoiceNumberManager.Text = ((DataRowView)invoiceList?.SelectedItem)?.Row?["Number"].ToString();
            //dateManager.SelectedDate = Convert.ToDateTime(((DataRowView)invoiceList?.SelectedItem)?.Row?["Date"]);
        }
    }
}
