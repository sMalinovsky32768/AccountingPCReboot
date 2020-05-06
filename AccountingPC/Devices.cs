using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingPC
{
    public class DeviceLocation
    {
        public UInt32 ID { get; set; }
        public String Name { get; set; }
    }
    public abstract class Device
    {
        public UInt32 ID { get; set; }
        [Required(ErrorMessage = "Поле является обязательным")]
        public UInt32 InventoryNumber { get; set; }
        [Required(ErrorMessage = "Поле является обязательным")]
        [StringLength(200, ErrorMessage = "Максимальная длина - 200")]
        public String Name { get; set; }
        public UInt32 Cost { get; set; }
        [StringLength(50, ErrorMessage = "Максимальная длина - 50")]
        public String Invoice { get; set; }
        public DeviceLocation Location { get; set; }
    }

    public class PC : Device
    {
        [StringLength(30, ErrorMessage = "Максимальная длина - 30")]
        public String Motherboard { get; set; }
        [StringLength(20, ErrorMessage = "Максимальная длина - 20")]
        public String CPU { get; set; }
        public int Cores { get; set; }
        public int Frequency { get; set; }
        public int MaxFrequency { get; set; }
        [StringLength(20, ErrorMessage = "Максимальная длина - 20")]
        public String VCard { get; set; }
        public UInt32 VideoRAM { get; set; }
        public UInt32 RAM { get; set; }
        public UInt32 FrequencyRAM { get; set; }
        public UInt32 HDD { get; set; }
        public String OS { get; set; }
        public List<String> VideoConnectors { get; set; }
        public int VideoConnectorsValue { get; set; }

        public PC() { }
    }

    public class Notebook : Device
    {
        [StringLength(20, ErrorMessage = "Максимальная длина - 20")]
        public String CPU { get; set; }
        public int Cores { get; set; }
        public int Frequency { get; set; }
        public int MaxFrequency { get; set; }
        [StringLength(30, ErrorMessage = "Максимальная длина - 30")]
        public String VCard { get; set; }
        public UInt32 VideoRAM { get; set; }
        public UInt32 RAM { get; set; }
        public UInt32 FrequencyRAM { get; set; }
        public UInt32 HDD { get; set; }
        public String OS { get; set; }
        public float Diagonal { get; set; }
        public List<String> VideoConnectors { get; set; }
        public int VideoConnectorsValue { get; set; }
        public int ResolutionID { get; set; }
        public int FrequencyID { get; set; }
        public int MatrixTechnologyID { get; set; }
        public int TypeID { get; set; }

        public Notebook() { }
    }

    public class InteractiveWhiteboard : Device
    {
        public Single Diagonal { get; set; }

        public InteractiveWhiteboard() { }
    }

    public class Projector : Device
    {
        public Single Diagonal { get; set; }
        public List<String> VideoConnectors { get; set; }
        public int VideoConnectorsValue { get; set; }
        public int ResolutionID { get; set; }
        public int ProjectorTechnologyID { get; set; }

        public Projector() { }
    }

    public class ProjectorScreen : Device
    {
        public Single Diagonal { get; set; }
        public bool IsElectronicDrive { get; set; }
        public int AspectRatioID { get; set; }
        public int ScreenInstalledID { get; set; }

        public ProjectorScreen() { }
    }

    public class PrinterScanner : Device
    {
        public Single Diagonal { get; set; }
        public int TypeID { get; set; }
        public int PaperSizeID { get; set; }

        public PrinterScanner() { }
    }

    public class Monitor : Device
    {
        public Single Diagonal { get; set; }
        public List<String> VideoConnectors { get; set; }
        public int VideoConnectorsValue { get; set; }
        public int ResolutionID { get; set; }
        public int FrequencyID { get; set; }
        public int MatrixTechnologyID { get; set; }

        public Monitor() { }
    }

    public class NetworkSwitch : Device
    {
        public UInt32 Ports { get; set; }
        public int WiFiFrequencyID { get; set; }

        public NetworkSwitch() { }
    }

    public class OtherEquipment : Device
    {
        public OtherEquipment() { }
    }
}
