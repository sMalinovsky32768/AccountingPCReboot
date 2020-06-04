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
            softwareView.ItemsSource = SoftwareDataSet.Tables[0].DefaultView;
            //while (softwareView.Columns.Count == 0) continue;

            //if (softwareView.Columns.Count > 0)
            //{
            //    softwareView.Columns[softwareDataSet.Tables[0].DefaultView.Table.Columns.IndexOf("ID")].Visibility = Visibility.Collapsed;
            //    ((DataGridTextColumn)softwareView.Columns[softwareDataSet.Tables[0].DefaultView.Table.
            //        Columns.IndexOf("Дата приобретения")]).Binding.StringFormat = "dd.MM.yyyy";
            //}

            TypeSoft = TypeSoft.LicenseSoftware;
        }

        internal void SetViewToOS()
        {
            softwareView.ItemsSource = OsDataSet.Tables[0].DefaultView;

            //softwareView.Columns[osDataSet.Tables[0].DefaultView.Table.Columns.IndexOf("ID")].Visibility = Visibility.Collapsed;
            //((DataGridTextColumn)softwareView.Columns[osDataSet.Tables[0].DefaultView.Table.
            //    Columns.IndexOf("Дата приобретения")]).Binding.StringFormat = "dd.MM.yyyy";

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
            SoftwareDataSet = new DataSet();
            new SqlDataAdapter("SELECT * FROM dbo.GetAllSoftware()", ConnectionString).Fill(SoftwareDataSet);
        }

        private void UpdateOSData()
        {
            OsDataSet = new DataSet();
            new SqlDataAdapter("SELECT * FROM dbo.GetAllOS()", ConnectionString).Fill(OsDataSet);
        }
    }
}
