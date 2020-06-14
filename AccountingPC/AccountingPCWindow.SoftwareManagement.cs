using System;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Windows;

namespace AccountingPC
{
    public partial class AccountingPCWindow
    {
        internal void ChangeSoftwareView()
        {
            try
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
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, exception.GetType().Name, MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void SetViewToLicenseSoftware()
        {
            softwareView.ItemsSource = DefaultDataSet.Tables["Software"].DefaultView;

            TypeSoft = TypeSoft.LicenseSoftware;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void SetViewToOS()
        {
            softwareView.ItemsSource = DefaultDataSet.Tables["OS"].DefaultView;

            TypeSoft = TypeSoft.OS;
        }

        internal void UpdateSoftwareData()
        {
            try
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
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, exception.GetType().Name, MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void UpdateAllSoftwareData()
        {
            try
            {
                UpdateLicenseSoftwareData();
                UpdateOSData();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, exception.GetType().Name, MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateLicenseSoftwareData()
        {
            DefaultDataSet.Tables["Software"].Clear();
            new SqlDataAdapter("SELECT * FROM dbo.GetAllSoftware()", ConnectionString).Fill(DefaultDataSet, "Software");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateOSData()
        {
            DefaultDataSet.Tables["OS"].Clear();
            new SqlDataAdapter("SELECT * FROM dbo.GetAllOS()", ConnectionString).Fill(DefaultDataSet, "OS");
        }
    }
}