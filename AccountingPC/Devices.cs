using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingPC
{
    public class PC
    {
        public uint ID { get; set; }
        public uint InventoryNumber { get; set; }
        public string Name { get; set; }
        public uint Cost { get; set; }
        public string Motherboard { get; set; }
        public string CPU { get; set; }
        public uint RAM { get; set; }
        public string OS { get; set; }
        public string Invoice { get; set; }
        public string Location { get; set; }

        public PC() { }
        public PC(uint inv, string name, uint cost, string mb, 
            string cpu, uint ram, string os, string invoice, string location)
        {
            InventoryNumber = inv;
            Name = name;
            Cost = cost;
            Motherboard = mb;
            CPU = cpu;
            RAM = ram;
            OS = os;
            Invoice = invoice;
            Location = location;
        }
    }
}
