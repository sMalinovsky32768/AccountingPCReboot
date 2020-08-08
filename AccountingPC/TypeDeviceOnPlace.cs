using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Windows;

namespace AccountingPC
{
    internal class TypeDeviceOnPlace : INotifyPropertyChanged
    {
        public delegate void TypeDeviceChangedEventHandler();

        private DeviceOnPlace device;
        private DataRowView row;

        private DataTable table = new DataTable();
        private TypeDeviceName typeDevice;

        public TypeDeviceOnPlace(Place place)
        {
            TypeDeviceChanged += TypeDeviceOnPlace_TypeDeviceChanged;
            Place = place;
        }

        public TypeDeviceOnPlace(TypeDeviceName typeDevice, DataRowView row, DeviceOnPlace device, int placeID,
            Place place)
        {
            TypeDeviceChanged += TypeDeviceOnPlace_TypeDeviceChanged;
            TypeDevice = typeDevice;
            this.row = row;
            this.device = device;
            Place = place;
            PlaceID = placeID;
        }

        public static string ConnectionString { get; } =
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public int PlaceID { get; set; }

        public TypeDeviceName TypeDevice
        {
            get => typeDevice;
            set
            {
                typeDevice = value;
                TypeDeviceChanged?.Invoke();
                OnPropertyChanged();
            }
        }

        public DeviceOnPlace Device
        {
            get => device;
            set
            {
                device = value;
                OnPropertyChanged();
            }
        }

        public DataRowView Row
        {
            get => row;
            set
            {
                var i = row != null;
                if (i && !Place.TypeDeviceRemovedCollection.Contains(this))
                    Place.TypeDeviceRemovedCollection.Add(
                        new TypeDeviceOnPlace(TypeDevice, Row, Device, PlaceID, Place));
                row = value;
                OnPropertyChanged();
            }
        }

        internal bool IsRemoved { get; set; } = false;

        internal DataTable Table
        {
            get => table;
            set
            {
                table = value;
                OnPropertyChanged();
            }
        }

        internal Place Place { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public event TypeDeviceChangedEventHandler TypeDeviceChanged;

        private void TypeDeviceOnPlace_TypeDeviceChanged()
        {
            try
            {
                switch (TypeDevice.Type)
                {
                    case AccountingPC.TypeDevice.InteractiveWhiteboard:
                        Table = new DataTable();
                        new SqlDataAdapter("Select ID, FullName, TableName from dbo.GetAllBoardWithFullName()",
                            ConnectionString).Fill(Table);
                        break;
                    case AccountingPC.TypeDevice.Monitor:
                        Table = new DataTable();
                        new SqlDataAdapter("Select ID, FullName, TableName from dbo.GetAllMonitorWithFullName()",
                            ConnectionString).Fill(Table);
                        break;
                    case AccountingPC.TypeDevice.NetworkSwitch:
                        Table = new DataTable();
                        new SqlDataAdapter("Select ID, FullName, TableName from dbo.GetAllNetworkSwitchWithFullName()",
                            ConnectionString).Fill(Table);
                        break;
                    case AccountingPC.TypeDevice.Notebook:
                        Table = new DataTable();
                        new SqlDataAdapter("Select ID, FullName, TableName from dbo.GetAllNotebookWithFullName()",
                            ConnectionString).Fill(Table);
                        break;
                    case AccountingPC.TypeDevice.OtherEquipment:
                        Table = new DataTable();
                        new SqlDataAdapter("Select ID, FullName, TableName from dbo.GetAllOtherEquipmentWithFullName()",
                            ConnectionString).Fill(Table);
                        break;
                    case AccountingPC.TypeDevice.PC:
                        Table = new DataTable();
                        new SqlDataAdapter("Select ID, FullName, TableName from dbo.GetAllPCWithFullName()",
                            ConnectionString).Fill(Table);
                        break;
                    case AccountingPC.TypeDevice.PrinterScanner:
                        Table = new DataTable();
                        new SqlDataAdapter("Select ID, FullName, TableName from dbo.GetAllPrinterScannerWithFullName()",
                            ConnectionString).Fill(Table);
                        break;
                    case AccountingPC.TypeDevice.Projector:
                        Table = new DataTable();
                        new SqlDataAdapter("Select ID, FullName, TableName from dbo.GetAllProjectorWithFullName()",
                            ConnectionString).Fill(Table);
                        break;
                    case AccountingPC.TypeDevice.ProjectorScreen:
                        Table = new DataTable();
                        new SqlDataAdapter("Select ID, FullName, TableName from dbo.GetAllProjectorScreenWithFullName()",
                            ConnectionString).Fill(Table);
                        break;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, e.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}