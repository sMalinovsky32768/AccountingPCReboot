using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AccountingPC
{
    internal class TypeDeviceName
    {
        public TypeDevice Type { get; set; }
        public string Name { get; set; }
    }

    internal static class TypeDeviceNames
    {
        public static List<TypeDeviceName> Collection { get; } = new List<TypeDeviceName>()
        {
            new TypeDeviceName()
            {
                Type = TypeDevice.InteractiveWhiteboard,
                Name = "Интерактивная доска",
            },
            new TypeDeviceName()
            {
                Type = TypeDevice.Monitor,
                Name = "Монитор",
            },
            new TypeDeviceName()
            {
                Type = TypeDevice.NetworkSwitch,
                Name = "Сетевое оборудование",
            },
            new TypeDeviceName()
            {
                Type = TypeDevice.Notebook,
                Name = "Ноутбук",
            },
            new TypeDeviceName()
            {
                Type = TypeDevice.OtherEquipment,
                Name = "Прочее",
            },
            new TypeDeviceName()
            {
                Type = TypeDevice.PC,
                Name = "Компьютер",
            },
            new TypeDeviceName()
            {
                Type = TypeDevice.PrinterScanner,
                Name = "Принтер/Сканер",
            },
            new TypeDeviceName()
            {
                Type = TypeDevice.Projector,
                Name = "Проектор",
            },
            new TypeDeviceName()
            {
                Type = TypeDevice.ProjectorScreen,
                Name = "Экран для проектора",
            },
        };

        public static TypeDeviceName GetTypeDeviceName(TypeDevice type)
        {
            for (int i = 0; i < Collection.Count; i++)
            {
                if (Collection[i].Type == type)
                {
                    return Collection[i];
                }
            }
            return null;
        }
    }

    internal class DeviceOnPlace : INotifyPropertyChanged
    {
        private string name;

        public int ID { get; set; }
        public string Name 
        { 
            get => name; 
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }

    internal class TypeDeviceOnPlace : INotifyPropertyChanged
    {
        public static string ConnectionString { get; private set; } = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public delegate void TypeDeviceChangedEventHandler();
        public event TypeDeviceChangedEventHandler TypeDeviceChanged;

        private TypeDeviceName typeDevice;
        private DataTable table = new DataTable();
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
                row = value;
                if (i && !Place.TypeDeviceRemovedCollection.Contains(this)) 
                {
                    Place.TypeDeviceRemovedCollection.Add(new TypeDeviceOnPlace(Place)
                    {
                        PlaceID = PlaceID,
                        TypeDevice = TypeDevice,
                        Device = Device,
                        Row = Row,
                    });
                }
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

        private void TypeDeviceOnPlace_TypeDeviceChanged()
        {
            switch (TypeDevice.Type)
            {
                case AccountingPC.TypeDevice.InteractiveWhiteboard:
                    Table = new DataTable();
                    new SqlDataAdapter("Select ID, FullName, TableName from dbo.GetAllBoardWithFullName()", ConnectionString).Fill(Table);
                    //new SqlDataAdapter("Select ID, FullName, TableName from dbo.GetAllBoardWithFullName() where PlaceID is null", ConnectionString).Fill(Table);
                    break;
                case AccountingPC.TypeDevice.Monitor:
                    Table = new DataTable();
                    new SqlDataAdapter("Select ID, FullName, TableName from dbo.GetAllMonitorWithFullName()", ConnectionString).Fill(Table);
                    //new SqlDataAdapter("Select ID, FullName, TableName from dbo.GetAllMonitorWithFullName() where PlaceID is null", ConnectionString).Fill(Table);
                    break;
                case AccountingPC.TypeDevice.NetworkSwitch:
                    Table = new DataTable();
                    new SqlDataAdapter("Select ID, FullName, TableName from dbo.GetAllNetworkSwitchWithFullName()", ConnectionString).Fill(Table);
                    //new SqlDataAdapter("Select ID, FullName, TableName from dbo.GetAllNetworkSwitchWithFullName() where PlaceID is null", ConnectionString).Fill(Table);
                    break;
                case AccountingPC.TypeDevice.Notebook:
                    Table = new DataTable();
                    new SqlDataAdapter("Select ID, FullName, TableName from dbo.GetAllNotebookWithFullName()", ConnectionString).Fill(Table);
                    //new SqlDataAdapter("Select ID, FullName, TableName from dbo.GetAllNotebookWithFullName() where PlaceID is null", ConnectionString).Fill(Table);
                    break;
                case AccountingPC.TypeDevice.OtherEquipment:
                    Table = new DataTable();
                    new SqlDataAdapter("Select ID, FullName, TableName from dbo.GetAllOtherEquipmentWithFullName()", ConnectionString).Fill(Table);
                    //new SqlDataAdapter("Select ID, FullName, TableName from dbo.GetAllOtherEquipmentWithFullName() where PlaceID is null", ConnectionString).Fill(Table);
                    break;
                case AccountingPC.TypeDevice.PC:
                    Table = new DataTable();
                    new SqlDataAdapter("Select ID, FullName, TableName from dbo.GetAllPCWithFullName()", ConnectionString).Fill(Table);
                    //new SqlDataAdapter("Select ID, FullName, TableName from dbo.GetAllPCWithFullName() where PlaceID is null", ConnectionString).Fill(Table);
                    break;
                case AccountingPC.TypeDevice.PrinterScanner:
                    Table = new DataTable();
                    new SqlDataAdapter("Select ID, FullName, TableName from dbo.GetAllPrinterScannerWithFullName()", ConnectionString).Fill(Table);
                    //new SqlDataAdapter("Select ID, FullName, TableName from dbo.GetAllPrinterScannerWithFullName() where PlaceID is null", ConnectionString).Fill(Table);
                    break;
                case AccountingPC.TypeDevice.Projector:
                    Table = new DataTable();
                    new SqlDataAdapter("Select ID, FullName, TableName from dbo.GetAllProjectorWithFullName()", ConnectionString).Fill(Table);
                    //new SqlDataAdapter("Select ID, FullName, TableName from dbo.GetAllProjector() where PlaceID is null", ConnectionString).Fill(Table);
                    break;
                case AccountingPC.TypeDevice.ProjectorScreen:
                    Table = new DataTable();
                    new SqlDataAdapter("Select ID, FullName, TableName from dbo.GetAllScreenWithFullName()", ConnectionString).Fill(Table);
                    //new SqlDataAdapter("Select ID, FullName, TableName from dbo.GetAllScreen() where PlaceID is null", ConnectionString).Fill(Table);
                    break;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }

    internal class Place : INotifyPropertyChanged
    {
        private string name;
        private ObservableCollection<TypeDeviceOnPlace> typeDeviceCollection = new ObservableCollection<TypeDeviceOnPlace>();
        private ObservableCollection<TypeDeviceOnPlace> typeDeviceRemovedCollection = new ObservableCollection<TypeDeviceOnPlace>();

        private AccountingCommand addTypeDevice;
        public AccountingCommand AddTypeDevice => addTypeDevice ??
            (addTypeDevice = new AccountingCommand(
                obj =>
                {
                    TypeDeviceCollection.Add(new TypeDeviceOnPlace(this));
                },
                (obj) =>
                {
                    return true;
                }
            ));

        private AccountingCommand delTypeDevice;
        public AccountingCommand DelTypeDevice => delTypeDevice ??
            (delTypeDevice = new AccountingCommand(
                obj =>
                {
                    TypeDeviceOnPlace temp = (TypeDeviceOnPlace)obj;
                    temp.IsRemoved = true;
                    TypeDeviceRemovedCollection.Add(temp);
                    TypeDeviceCollection.Remove(temp);
                    //((TypeDeviceOnPlace)obj).IsRemoved = true;
                },
                (obj) =>
                {
                    if (obj != null)
                        return true;
                    return false;
                }
            ));

        public int ID { get; set; }
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<TypeDeviceOnPlace> TypeDeviceCollection
        {
            get => typeDeviceCollection;
            set
            {
                typeDeviceCollection = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<TypeDeviceOnPlace> TypeDeviceRemovedCollection
        {
            get => typeDeviceRemovedCollection;
            set
            {
                typeDeviceRemovedCollection = value;
                OnPropertyChanged();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
