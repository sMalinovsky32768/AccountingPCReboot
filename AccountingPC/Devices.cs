﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AccountingPC
{
    internal class Device
    {
        public uint ID { get; set; }

        [Required(ErrorMessage = "Поле является обязательным")]
        public long InventoryNumber { get; set; }

        [Required(ErrorMessage = "Поле является обязательным")]
        [StringLength(200, ErrorMessage = "Максимальная длина - 200")]
        public string Name { get; set; }

        public decimal Cost { get; set; }

        [StringLength(50, ErrorMessage = "Максимальная длина - 50")]
        public string InvoiceNumber { get; set; }

        public uint PlaceID { get; set; }
    }

    internal class PC : Device
    {
        [StringLength(30, ErrorMessage = "Максимальная длина - 30")]
        public string Motherboard { get; set; }

        [StringLength(20, ErrorMessage = "Максимальная длина - 20")]
        public string CPU { get; set; }

        public uint Cores { get; set; }
        public uint Frequency { get; set; }
        public uint MaxFrequency { get; set; }

        [StringLength(20, ErrorMessage = "Максимальная длина - 20")]
        public string VCard { get; set; }

        public uint VideoRAM { get; set; }
        public uint RAM { get; set; }
        public uint FrequencyRAM { get; set; }
        public uint SSD { get; set; }
        public uint HDD { get; set; }
        public uint OSID { get; set; }
        public List<string> VideoConnectors { get; set; }
        public int VideoConnectorsValue { get; set; }
    }

    internal class Notebook : Device
    {
        [StringLength(20, ErrorMessage = "Максимальная длина - 20")]
        public string CPU { get; set; }

        public uint Cores { get; set; }
        public uint Frequency { get; set; }
        public uint MaxFrequency { get; set; }

        [StringLength(30, ErrorMessage = "Максимальная длина - 30")]
        public string VCard { get; set; }

        public uint VideoRAM { get; set; }
        public uint RAM { get; set; }
        public uint FrequencyRAM { get; set; }
        public uint SSD { get; set; }
        public uint HDD { get; set; }
        public uint OSID { get; set; }
        public decimal Diagonal { get; set; }
        public List<string> VideoConnectors { get; set; }
        public int VideoConnectorsValue { get; set; }
        public uint ResolutionID { get; set; }
        public uint FrequencyID { get; set; }
        public uint MatrixTechnologyID { get; set; }
        public uint TypeID { get; set; }
    }

    internal class InteractiveWhiteboard : Device
    {
        public decimal Diagonal { get; set; }
    }

    internal class Projector : Device
    {
        public decimal Diagonal { get; set; }
        public List<string> VideoConnectors { get; set; }
        public int VideoConnectorsValue { get; set; }
        public uint ResolutionID { get; set; }
        public uint ProjectorTechnologyID { get; set; }
    }

    internal class ProjectorScreen : Device
    {
        public decimal Diagonal { get; set; }
        public bool IsElectronicDrive { get; set; }
        public uint AspectRatioID { get; set; }
        public uint ScreenInstalledID { get; set; }
    }

    internal class PrinterScanner : Device
    {
        public uint TypeID { get; set; }
        public uint PaperSizeID { get; set; }
    }

    internal class Monitor : Device
    {
        public decimal Diagonal { get; set; }
        public List<string> VideoConnectors { get; set; }
        public int VideoConnectorsValue { get; set; }
        public uint ResolutionID { get; set; }
        public uint FrequencyID { get; set; }
        public uint MatrixTechnologyID { get; set; }
    }

    internal class NetworkSwitch : Device
    {
        public uint TypeID { get; set; }
        public uint Ports { get; set; }
        public uint WiFiFrequencyID { get; set; }
    }

    internal class OtherEquipment : Device
    {
    }
}