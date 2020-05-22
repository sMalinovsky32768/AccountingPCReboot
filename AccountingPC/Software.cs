using System;

namespace AccountingPC
{
    public class Software
    {
        private int countInstalled;

        public int ID { get; set; }
        public String Name { get; set; }
        public int CountInstalled { get => countInstalled; set => countInstalled = value; }
        public String CountInstalledString
        {
            get
            {
                if (countInstalled > 0)
                    return $"Установлено на {countInstalled} устройствах";
                else
                    return "Ни разу не установлено";
            }
        }
    }
}
