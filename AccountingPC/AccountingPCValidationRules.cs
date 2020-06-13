using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows.Controls;

namespace AccountingPC
{
    internal class InventoryNumberValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (!Check(value))
                return new ValidationResult(false, "Оборудование с таким инвентарным номеров уже существует");
            return new ValidationResult(true, null);
        }

        public bool Check(object value)
        {
            var s = (string) value;
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            if (s != "" && int.TryParse(s, out var inv))
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var command = new SqlCommand($"SELECT dbo.IsAvailableInventoryNumber({inv})", connection);
                    var obj = command.ExecuteScalar();
                    return Convert.ToBoolean(obj);
                }

            return true;
        }
    }
}