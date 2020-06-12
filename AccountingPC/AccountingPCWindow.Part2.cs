using AccountingPC.Properties;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Windows;

namespace AccountingPC
{
    public partial class AccountingPCWindow
    {
        public static string GetVideoConnectors(int value)
        {
            List<string> arr = GetListVideoConnectors(value);
            string res = string.Empty;
            for (int i = 0; i < arr.Count; i++)
            {
                res += $"{arr[i]}";
                if (i < arr.Count - 1)
                {
                    res += ", ";
                }
            }
            return res;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static List<string> GetListVideoConnectors(int value)
        {
            List<string> arr = new List<string>
            {
                Capacity = 20
            };
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlDataReader reader = new SqlCommand("Select * from dbo.GetAllVideoConnector() Order by value desc", connection).ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int connectorValue = Convert.ToInt32(reader["Value"]);
                        if (value >= connectorValue)
                        {
                            value -= connectorValue;
                            arr.Add(reader["Name"].ToString());
                        }
                    }
                }
            }
            return arr;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Dictionary<int, byte[]> GetImages()
        {
            Dictionary<int, byte[]> temp = new Dictionary<int, byte[]>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlDataReader reader = new SqlCommand("SELECT * FROM dbo.GetAllImages()", connection).ExecuteReader();

                while (reader.Read())
                {
                    temp.Add(reader.GetInt32(0), (byte[])reader.GetValue(1));
                }
            }

            return temp;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void LoadFromSettings()
        {
            Height = AccountingPCWindowSettings.Default.Height > MinHeight ? AccountingPCWindowSettings.Default.Height : MinHeight;
            Width = AccountingPCWindowSettings.Default.Width > MinWidth ? AccountingPCWindowSettings.Default.Width : MinWidth;
            LastHeight = AccountingPCWindowSettings.Default.lastHeignt;
            LastWidth = AccountingPCWindowSettings.Default.lastWidth;
            WindowState = AccountingPCWindowSettings.Default.WindowState != string.Empty ?
                (WindowState)Enum.Parse(typeof(WindowState), AccountingPCWindowSettings.Default.WindowState) : WindowState.Normal;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void ChangeWindowState()
        {
            if (WindowState == WindowState.Maximized)
            {
                ((System.Windows.Shapes.Path)buttonMaximized.Template.FindName("Maximize", buttonMaximized)).Visibility = Visibility.Collapsed;
                ((System.Windows.Shapes.Path)buttonMaximized.Template.FindName("Restore", buttonMaximized)).Visibility = Visibility.Visible;
            }
            else if (WindowState == WindowState.Normal)
            {
                ((System.Windows.Shapes.Path)buttonMaximized.Template.FindName("Maximize", buttonMaximized)).Visibility = Visibility.Visible;
                ((System.Windows.Shapes.Path)buttonMaximized.Template.FindName("Restore", buttonMaximized)).Visibility = Visibility.Collapsed;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SelectViewEquipment()
        {
            NowView = View.Equipment;
            equipmentGrid.Visibility = Visibility.Visible;
            softwareGrid.Visibility = Visibility.Collapsed;
            locationManagementGrid.Visibility = Visibility.Collapsed;
            invoiceManagementGrid.Visibility = Visibility.Collapsed;
            equipmentCategoryList.SelectedIndex = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SelectViewSoftware()
        {
            NowView = View.Software;
            equipmentGrid.Visibility = Visibility.Collapsed;
            softwareGrid.Visibility = Visibility.Visible;
            locationManagementGrid.Visibility = Visibility.Collapsed;
            invoiceManagementGrid.Visibility = Visibility.Collapsed;
            softwareCategoryList.SelectedIndex = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SelectViewInvoice()
        {
            NowView = View.Invoice;
            equipmentGrid.Visibility = Visibility.Collapsed;
            softwareGrid.Visibility = Visibility.Collapsed;
            locationManagementGrid.Visibility = Visibility.Collapsed;
            invoiceManagementGrid.Visibility = Visibility.Visible;
            LoadInvoiceList();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SelectViewLocation()
        {
            NowView = View.Location;
            equipmentGrid.Visibility = Visibility.Collapsed;
            softwareGrid.Visibility = Visibility.Collapsed;
            locationManagementGrid.Visibility = Visibility.Visible;
            invoiceManagementGrid.Visibility = Visibility.Collapsed;
            UpdateAudienceList();
        }
    }
}
