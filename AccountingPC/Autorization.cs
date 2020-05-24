using System;
using System.ComponentModel.DataAnnotations;

namespace AccountingPC
{
    internal class Authorization
    {
        public String Login { get; set; }
        public String Pass { get; set; }
        [Compare("Pass")]
        public String ConfirmPass { get; set; }
    }
}
