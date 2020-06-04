namespace AccountingPC
{
    internal enum TypeChange : byte
    {
        Add,
        Change,
    }

    internal enum TypeSoft : byte
    {
        LicenseSoftware,
        OS,
    }

    internal enum View : byte
    {
        Equipment,
        Software,
        Location,
        Invoice,
    }

    internal enum TypeDevice : byte
    {
        PC,
        Notebook,
        Monitor,
        PrinterScanner,
        Projector,
        InteractiveWhiteboard,
        NetworkSwitch,
        OtherEquipment,
        ProjectorScreen,
    }
}
