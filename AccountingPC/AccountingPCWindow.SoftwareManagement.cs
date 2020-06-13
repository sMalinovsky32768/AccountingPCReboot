using System.Data.SqlClient;
using System.Runtime.CompilerServices;

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