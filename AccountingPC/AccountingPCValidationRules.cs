using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows.Controls;

namespace AccountingPC
{
    internal class InventoryNumberValidationRule : ValidationRule
    {
        //public bool IsAvailable { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (!Check(value))
            {
                return new ValidationResult(false, $"Оборудование с таким инвентарным номеров уже существует");
            }
            return new ValidationResult(true, null);
        }

        public bool Check(object value)
        {
            string s = (string)value;
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            int inv;
            if (s != "" && int.TryParse(s, out inv))
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand($"SELECT dbo.IsAvailableInventoryNumber({inv})", connection);
                    object obj = command.ExecuteScalar();
                    return Convert.ToBoolean(obj);
                }
            }
            return true;
        }
    }
}
