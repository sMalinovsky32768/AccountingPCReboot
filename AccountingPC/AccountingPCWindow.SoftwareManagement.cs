using System;
using System.Data;
using System.Data.SqlClient;

namespace AccountingPC
{
    public partial class AccountingPCWindow
    {
        internal void ChangeSoftwareView()
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

        internal void UpdateSoftwareData()
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
    }
}
