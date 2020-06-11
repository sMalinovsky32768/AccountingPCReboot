using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AccountingPC
{
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
}
