using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
