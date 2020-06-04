using System.ComponentModel.DataAnnotations;

namespace AccountingPC
{
    internal class Authorization
    {
        public string Login { get; set; }
        public string Pass { get; set; }
        [Compare("Pass")]
        public string ConfirmPass { get; set; }
    }
}
