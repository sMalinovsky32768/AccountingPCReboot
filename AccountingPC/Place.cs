﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AccountingPC
{
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
