using System;
using System.ComponentModel.DataAnnotations;

namespace AccountingPC
{
    public class Authorization
    {
        public String Login { get; set; }
        public String Pass { get; set; }
        [Compare("Pass")]
        public String ConfirmPass { get; set; }
    }
}
