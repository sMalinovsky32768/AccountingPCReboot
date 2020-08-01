using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Windows;

namespace AccountingPC
{
    public partial class AccountingPCWindow
    {
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