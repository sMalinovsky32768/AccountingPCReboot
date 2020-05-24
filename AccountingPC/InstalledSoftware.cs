using System;

namespace AccountingPC
{
    internal class InstalledSoftware
    {
        public int ID { get; set; }
        public String Name { get; set; }
        public int CountInstalled { get; set; }
        public String CountInstalledString
        {
            get
            {
                if (CountInstalled > 0)
                    return $"Установлено на {CountInstalled} устройствах";
                else
                    return "Ни разу не установлено";
            }
        }
    }
}
