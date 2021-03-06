﻿using System.ComponentModel.DataAnnotations;

namespace AccountingPC
{
    internal class Software
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Поле является обязательным")]
        [StringLength(200, ErrorMessage = "Максимальная длина - 200")]
        public string Name { get; set; }

        public int Count { get; set; }
        public decimal Cost { get; set; }

        [StringLength(50, ErrorMessage = "Максимальная длина - 50")]
        public string InvoiceNumber { get; set; }
    }

    internal class LicenseSoftware : Software
    {
        public int Type { get; set; }
    }

    internal class OS : Software
    {
    }
}