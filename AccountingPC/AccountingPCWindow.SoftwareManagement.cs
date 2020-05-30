using System;
using System.Windows;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Controls;

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
            softwareView.ItemsSource = softwareDataSet.Tables[0].DefaultView;
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
            softwareView.ItemsSource = osDataSet.Tables[0].DefaultView;

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
            softwareDataAdapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllSoftware()", ConnectionString);
            softwareDataSet = new DataSet();
            softwareDataAdapter.Fill(softwareDataSet);
        }

        private void UpdateOSData()
        {
            osDataAdapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllOS()", ConnectionString);
            osDataSet = new DataSet();
            osDataAdapter.Fill(osDataSet);
        }
    }
}
