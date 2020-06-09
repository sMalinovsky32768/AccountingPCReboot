using System;
using System.Collections.Generic;
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
        private static readonly List<TypeDeviceName> collection = new List<TypeDeviceName>()
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
        public static List<TypeDeviceName> Collection => collection;

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
        public TypeDeviceOnPlace()
        {
            TypeDeviceChanged += TypeDeviceOnPlace_TypeDeviceChanged;
        }

        private void TypeDeviceOnPlace_TypeDeviceChanged()
        {
            switch (TypeDevice.Type)
            {
                case AccountingPC.TypeDevice.InteractiveWhiteboard:
                    Table = new DataTable();
                    new SqlDataAdapter("Select ID, [Наименование] from dbo.GetAllBoard()", ConnectionString).Fill(Table);
                    break;
                case AccountingPC.TypeDevice.Monitor:
                    Table = new DataTable();
                    new SqlDataAdapter("Select ID, [Наименование] from dbo.GetAllMonitor()", ConnectionString).Fill(Table);
                    break;
                case AccountingPC.TypeDevice.NetworkSwitch:
                    Table = new DataTable();
                    new SqlDataAdapter("Select ID, [Наименование] from dbo.GetAllNetworkSwitch()", ConnectionString).Fill(Table);
                    break;
                case AccountingPC.TypeDevice.Notebook:
                    Table = new DataTable();
                    new SqlDataAdapter("Select ID, [Наименование] from dbo.GetAllNotebook()", ConnectionString).Fill(Table);
                    break;
                case AccountingPC.TypeDevice.OtherEquipment:
                    Table = new DataTable();
                    new SqlDataAdapter("Select ID, [Наименование] from dbo.GetAllOtherEquipment()", ConnectionString).Fill(Table);
                    break;
                case AccountingPC.TypeDevice.PC:
                    Table = new DataTable();
                    new SqlDataAdapter("Select ID, [Наименование] from dbo.GetAllPC()", ConnectionString).Fill(Table);
                    break;
                case AccountingPC.TypeDevice.PrinterScanner:
                    Table = new DataTable();
                    new SqlDataAdapter("Select ID, [Наименование] from dbo.GetAllPrinterScanner()", ConnectionString).Fill(Table);
                    break;
                case AccountingPC.TypeDevice.Projector:
                    Table = new DataTable();
                    new SqlDataAdapter("Select ID, [Наименование] from dbo.GetAllProjector()", ConnectionString).Fill(Table);
                    break;
                case AccountingPC.TypeDevice.ProjectorScreen:
                    Table = new DataTable();
                    new SqlDataAdapter("Select ID, [Наименование] from dbo.GetAllScreen()", ConnectionString).Fill(Table);
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
        private AccountingCommand addTypeDevice;
        public AccountingCommand AddTypeDevice => addTypeDevice ??
            (addTypeDevice = new AccountingCommand(obj =>
            {
                TypeDeviceCollection.Add(new TypeDeviceOnPlace());
            },
            (obj) =>
            {
                return true;
            }
            ));

        private AccountingCommand delTypeDevice;
        private string name;
        private List<TypeDeviceOnPlace> typeDeviceCollection = new List<TypeDeviceOnPlace>();

        public AccountingCommand DelTypeDevice => delTypeDevice ??
            (delTypeDevice = new AccountingCommand(obj =>
            {
                ((TypeDeviceOnPlace)obj).IsRemoved = true;
            },
            (obj) =>
            {
                return true;
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
        public List<TypeDeviceOnPlace> TypeDeviceCollection 
        {
            get => typeDeviceCollection;
            set
            {
                typeDeviceCollection = value;
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
