using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingPC
{
    public class Software
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Поле является обязательным")]
        [StringLength(200, ErrorMessage = "Максимальная длина - 200")]
        public String Name { get; set; }
        public int Count { get; set; }
        public float Cost { get; set; }
        [StringLength(50, ErrorMessage = "Максимальная длина - 50")]
        public string InvoiceNumber { get; set; }
    }
    
    public class LicenseSoftware : Software
    {
        public int Type { get; set; }
    }

    public class OS : Software
    {

    }
}
