using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace AccountingPC
{
    public partial class AccountingPCWindow
    {
        private String GetVideoConnectors(Int32 value)
        {
            List<String> arr = new List<String>();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlDataReader reader = new SqlCommand("Select * from dbo.GetAllVideoConnector() Order by value desc", connection).ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Int32 connectorValue = Convert.ToInt32(reader["Value"]);
                        if (value >= connectorValue)
                        {
                            value -= connectorValue;
                            arr.Add(reader["Name"].ToString());
                        }
                    }
                }
            }
            String res = String.Empty;
            for (int i = 0; i < arr.Count; i++)
            {
                res += $"{arr[i]}";
                if (i < arr.Count - 1)
                    res += ", ";
            }
            return res;
        }

        private List<String> GetListVideoConnectors(Int32 value)
        {
            List<String> arr = new List<String>();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlDataReader reader = new SqlCommand("Select * from dbo.GetAllVideoConnector() Order by value desc", connection).ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Int32 connectorValue = Convert.ToInt32(reader["Value"]);
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

        private Int32 GetValueVideoConnectors(ListBox list)
        {
            Int32 value = 0;
            foreach (var obj in list.SelectedItems)
            {
                foreach (DataRowView row in videoConnectorsDataSet.Tables[0].DefaultView)
                {
                    string s = (obj as ListBoxItem).Content.ToString();
                    if (row.Row[1].ToString() == s)
                        value += Convert.ToInt32(row.Row[2]);
                }
            }
            return value;
        }

        private void GridPlacement(UIElement element, int column, int row, int colSpan, int rowSpan = 1)
        {
            Grid.SetColumn(element, column);
            Grid.SetRow(element, row);
            Grid.SetColumnSpan(element, colSpan);
            Grid.SetRowSpan(element, rowSpan);
        }

        public CustomPopupPlacement[] ChangePopupPlacement(Size popupSize, Size targetSize, Point offset)
        {
            CustomPopupPlacement placement1 =
               new CustomPopupPlacement(new Point(0, 0), PopupPrimaryAxis.Vertical);

            CustomPopupPlacement[] ttplaces =
                    new CustomPopupPlacement[] { placement1 };
            return ttplaces;
        }

        private void ChangePopupPreClose()
        {
            if (changeEquipmentPopup.IsOpen)
            {
                IsPreOpenEquipmentPopup = true;
                changeEquipmentPopup.IsOpen = false;
            }
            if (changeSoftwarePopup.IsOpen)
            {
                IsPreOpenSoftwarePopup = true;
                changeSoftwarePopup.IsOpen = false;
            }
        }

        private void ChangePopupPostClose()
        {
            if (IsPreOpenEquipmentPopup)
            {
                changeEquipmentPopup.Height = Height - 200;
                changeEquipmentPopup.Width = Width - 400;
                changeEquipmentPopup.IsOpen = true;
                IsPreOpenEquipmentPopup = false;
            }
            if (IsPreOpenSoftwarePopup)
            {
                changeSoftwarePopup.Height = Height - 200;
                changeSoftwarePopup.Width = Width - 400;
                changeSoftwarePopup.IsOpen = true;
                IsPreOpenSoftwarePopup = false;
            }
        }

        /*private int saveImage(string filename)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "AddImage";
                command.Parameters.Add("@Image", SqlDbType.VarBinary);
                command.Parameters.Add("@ID", SqlDbType.Int).Direction = ParameterDirection.Output;

                byte[] imageData;
                using (FileStream fs = new FileStream(filename, FileMode.Open))
                {
                    imageData = new byte[fs.Length];
                    fs.Read(imageData, 0, imageData.Length);
                }
                command.Parameters["@Image"].Value = imageData;

                command.ExecuteNonQuery();
                return Convert.ToInt32(command.Parameters["@ID"].Value);
            }
        }*/

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

        private byte[] LoadImage(string path)
        {
            if (path != "")
            {
                byte[] data;
                try
                {
                    using (FileStream fs = new FileStream(path, FileMode.Open))
                    {
                        data = new byte[fs.Length];
                        fs.Read(data, 0, data.Length);
                    }
                    return data;
                }
                catch
                {
                    return null;
                }
            }
            else
            {
                int col;
                switch (TypeDevice)
                {
                    case TypeDevice.PC:
                    case TypeDevice.Notebook:
                    case TypeDevice.Monitor:
                    case TypeDevice.Projector:
                        col = equipmentView.Columns.Count - 2;
                        break;
                    default:
                        col = equipmentView.Columns.Count - 1;
                        break;
                }
                object obj = ((DataRowView)equipmentView.SelectedItems?[0]).Row[col];
                int id = Convert.ToInt32(obj.GetType() == typeof(DBNull) ? null : obj);
                if (id != 0)
                {
                    return images[id];
                }
                return null;
            }
        }

        private void DisabledRepeatInvN_Checked()
        {
            invNBinding = new Binding();
            invNBinding.Path = new PropertyPath("InventoryNumber");
            invNBinding.ValidationRules.Clear();
            invNBinding.ValidationRules.Add(new DataErrorValidationRule());
            invNBinding.ValidationRules.Add(new InventoryNumberValidationRule());
            inventoryNumber.SetBinding(TextBox.TextProperty, invNBinding);
        }

        private void DisabledRepeatInvN_Unchecked()
        {
            invNBinding = new Binding();
            invNBinding.Path = new PropertyPath("InventoryNumber");
            invNBinding.ValidationRules.Clear();
            invNBinding.ValidationRules.Add(new DataErrorValidationRule());
            inventoryNumber.SetBinding(TextBox.TextProperty, invNBinding);
        }

        private void LoadFromSettings()
        {
            Height = AccountingPCWindowSettings.Default.Height > MinHeight ? AccountingPCWindowSettings.Default.Height : MinHeight;
            Width = AccountingPCWindowSettings.Default.Width > MinWidth ? AccountingPCWindowSettings.Default.Width : MinWidth;
            lastHeight = AccountingPCWindowSettings.Default.lastHeignt;
            lastWidth = AccountingPCWindowSettings.Default.lastWidth;
            WindowState = AccountingPCWindowSettings.Default.WindowState != string.Empty ?
                (WindowState)Enum.Parse(typeof(WindowState), AccountingPCWindowSettings.Default.WindowState) : WindowState.Normal;
        }

        private void ChangeWindowState()
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

        private void SelectViewEquipment()
        {
            NowView = View.Equipment;
            equipmentGrid.Visibility = Visibility.Visible;
            softwareGrid.Visibility = Visibility.Collapsed;
            locationManagementGrid.Visibility = Visibility.Collapsed;
            equipmentCategoryList.SelectedIndex = 0;
        }

        private void SelectViewSoftware()
        {
            NowView = View.Software;
            equipmentGrid.Visibility = Visibility.Collapsed;
            softwareGrid.Visibility = Visibility.Visible;
            locationManagementGrid.Visibility = Visibility.Collapsed;
            softwareCategoryList.SelectedIndex = 0;
        }

        private void SelectViewLocation()
        {
            NowView = View.Location;
            equipmentGrid.Visibility = Visibility.Collapsed;
            softwareGrid.Visibility = Visibility.Collapsed;
            locationManagementGrid.Visibility = Visibility.Visible;
        }
    }
}
