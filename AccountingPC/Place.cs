using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingPC
{
    internal class DeviceOnPlace
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    internal class TypeDeviceOnPlace
    {
        public int PlaceID { get; set; }
        public TypeDevice TypeDevice { get; set; }
        public DeviceOnPlace Device { get; set; }
        internal bool IsRemoved { get; set; } = false;
    }

    internal class Place
    {
        private AccountingCommand addTypeDevice;
        internal AccountingCommand AddTypeDevice => addTypeDevice ??
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
        internal AccountingCommand DelTypeDevice => delTypeDevice ??
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
        public string Name { get; set; }
        public List<TypeDeviceOnPlace> TypeDeviceCollection { get; set; } = new List<TypeDeviceOnPlace>();
    }
}
