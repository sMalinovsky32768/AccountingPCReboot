namespace AccountingPC
{
    internal class InstalledSoftware
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int CountInstalled { get; set; }
        public string CountInstalledString
        {
            get
            {
                if (CountInstalled > 0)
                {
                    return $"Установлено на {CountInstalled} устройствах";
                }
                else
                {
                    return "Ни разу не установлено";
                }
            }
        }
    }
}
