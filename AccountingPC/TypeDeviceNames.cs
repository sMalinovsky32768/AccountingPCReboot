using System.Collections.Generic;

namespace AccountingPC
{
    internal static class TypeDeviceNames
    {
        public static List<TypeDeviceName> Collection { get; } = new List<TypeDeviceName>
        {
            new TypeDeviceName
            {
                Type = TypeDevice.InteractiveWhiteboard,
                Name = "Интерактивное оборудование"
            },
            new TypeDeviceName
            {
                Type = TypeDevice.Monitor,
                Name = "Монитор"
            },
            new TypeDeviceName
            {
                Type = TypeDevice.NetworkSwitch,
                Name = "Сетевое оборудование"
            },
            new TypeDeviceName
            {
                Type = TypeDevice.Notebook,
                Name = "Портативный компьютер"
            },
            new TypeDeviceName
            {
                Type = TypeDevice.OtherEquipment,
                Name = "Прочее"
            },
            new TypeDeviceName
            {
                Type = TypeDevice.PC,
                Name = "Компьютер"
            },
            new TypeDeviceName
            {
                Type = TypeDevice.PrinterScanner,
                Name = "Принтер/Сканер"
            },
            new TypeDeviceName
            {
                Type = TypeDevice.Projector,
                Name = "Проектор"
            },
            new TypeDeviceName
            {
                Type = TypeDevice.ProjectorScreen,
                Name = "Экран для проектора"
            }
        };

        public static TypeDeviceName GetTypeDeviceName(TypeDevice type)
        {
            for (var i = 0; i < Collection.Count; i++)
                if (Collection[i].Type == type)
                    return Collection[i];
            return null;
        }
    }
}