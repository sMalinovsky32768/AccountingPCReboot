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
                    SetViewToLicenseSoftware();
                    break;
                case 1:
                    SetViewToOS();
                    break;
            }
        }

        internal void SetViewToLicenseSoftware()
        {
            //softwareView.ItemsSource = SoftwareDataSet.Tables[0].DefaultView;
            softwareView.ItemsSource = DefaultDataSet.Tables["Software"].DefaultView;

            TypeSoft = TypeSoft.LicenseSoftware;
        }

        internal void SetViewToOS()
        {
            //softwareView.ItemsSource = OsDataSet.Tables[0].DefaultView;
            softwareView.ItemsSource = DefaultDataSet.Tables["OS"].DefaultView;

            TypeSoft = TypeSoft.OS;
        }

        internal void UpdateSoftwareData()
        {
            switch (TypeSoft)
            {
                case TypeSoft.LicenseSoftware:
                    UpdateLicenseSoftwareData();
                    break;
                case TypeSoft.OS:
                    UpdateOSData();
                    break;
            }
        }

        private void UpdateAllSoftwareData()
        {
            UpdateLicenseSoftwareData();
            UpdateOSData();
        }

        private void UpdateLicenseSoftwareData()
        {
            //SoftwareDataSet = new DataSet();
            //new SqlDataAdapter("SELECT * FROM dbo.GetAllSoftware()", ConnectionString).Fill(SoftwareDataSet);
            DefaultDataSet.Tables["Software"].Clear();
            new SqlDataAdapter("SELECT * FROM dbo.GetAllSoftware()", ConnectionString).Fill(DefaultDataSet, "Software");
        }

        private void UpdateOSData()
        {
            //OsDataSet = new DataSet();
            //new SqlDataAdapter("SELECT * FROM dbo.GetAllOS()", ConnectionString).Fill(OsDataSet);
            DefaultDataSet.Tables["OS"].Clear();
            new SqlDataAdapter("SELECT * FROM dbo.GetAllOS()", ConnectionString).Fill(DefaultDataSet, "OS");
        }
    }
}
