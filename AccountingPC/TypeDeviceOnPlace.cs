using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;

namespace AccountingPC
{
    internal class TypeDeviceOnPlace : INotifyPropertyChanged
    {
        public static string ConnectionString { get; private set; } = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public delegate void TypeDeviceChangedEventHandler();
        public event TypeDeviceChangedEventHandler TypeDeviceChanged;

        private DataTable table = new DataTable();
        private TypeDeviceName typeDevice;
        private DataRowView row;
        private DeviceOnPlace device;

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
                bool i = row != null;
                if (i && !Place.TypeDeviceRemovedCollection.Contains(this))
                {
                    Place.TypeDeviceRemovedCollection.Add(new TypeDeviceOnPlace(TypeDevice, Row, Device, PlaceID, Place));
                }
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

        public TypeDeviceOnPlace(Place place)
        {
            TypeDeviceChanged += TypeDeviceOnPlace_TypeDeviceChanged;
            Place = place;
        }

        public TypeDeviceOnPlace(TypeDeviceName typeDevice, DataRowView row, DeviceOnPlace device, int placeID, Place place)
        {
            TypeDeviceChanged += TypeDeviceOnPlace_TypeDeviceChanged;
            TypeDevice = typeDevice;
            this.row = row;
            this.device = device;
            Place = place;
            PlaceID = placeID;
        }

        private void TypeDeviceOnPlace_TypeDeviceChanged()
        {
            switch (TypeDevice.Type)
            {
                case AccountingPC.TypeDevice.InteractiveWhiteboard:
                    Table = new DataTable();
                    new SqlDataAdapter("Select ID, FullName, TableName from dbo.GetAllBoardWithFullName()", ConnectionString).Fill(Table);
                    break;
                case AccountingPC.TypeDevice.Monitor:
                    Table = new DataTable();
                    new SqlDataAdapter("Select ID, FullName, TableName from dbo.GetAllMonitorWithFullName()", ConnectionString).Fill(Table);
                    break;
                case AccountingPC.TypeDevice.NetworkSwitch:
                    Table = new DataTable();
                    new SqlDataAdapter("Select ID, FullName, TableName from dbo.GetAllNetworkSwitchWithFullName()", ConnectionString).Fill(Table);
                    break;
                case AccountingPC.TypeDevice.Notebook:
                    Table = new DataTable();
                    new SqlDataAdapter("Select ID, FullName, TableName from dbo.GetAllNotebookWithFullName()", ConnectionString).Fill(Table);
                    break;
                case AccountingPC.TypeDevice.OtherEquipment:
                    Table = new DataTable();
                    new SqlDataAdapter("Select ID, FullName, TableName from dbo.GetAllOtherEquipmentWithFullName()", ConnectionString).Fill(Table);
                    break;
                case AccountingPC.TypeDevice.PC:
                    Table = new DataTable();
                    new SqlDataAdapter("Select ID, FullName, TableName from dbo.GetAllPCWithFullName()", ConnectionString).Fill(Table);
                    break;
                case AccountingPC.TypeDevice.PrinterScanner:
                    Table = new DataTable();
                    new SqlDataAdapter("Select ID, FullName, TableName from dbo.GetAllPrinterScannerWithFullName()", ConnectionString).Fill(Table);
                    break;
                case AccountingPC.TypeDevice.Projector:
                    Table = new DataTable();
                    new SqlDataAdapter("Select ID, FullName, TableName from dbo.GetAllProjectorWithFullName()", ConnectionString).Fill(Table);
                    break;
                case AccountingPC.TypeDevice.ProjectorScreen:
                    Table = new DataTable();
                    new SqlDataAdapter("Select ID, FullName, TableName from dbo.GetAllScreenWithFullName()", ConnectionString).Fill(Table);
                    break;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
