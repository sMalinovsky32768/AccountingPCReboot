using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows.Controls;

namespace AccountingPC
{
    public class ComboBoxValidationRule : ValidationRule
    {
        public UInt32 MaxLength { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            /*BindingExpression bindingExpression = BindingOperations.GetBindingExpression(value, ComboBox.TextProperty);
            String bind = (String)bindingExpression?.DataItem;
            Attribute[] attrs = Attribute.GetCustomAttributes(typeof(PC).GetProperty(bind));
            foreach (Attribute attr in attrs)
            {
                if (attr.GetType() == typeof(System.ComponentModel.DataAnnotations.StringLengthAttribute))
                {
                    if (value is ComboBox)
                    {
                        Int32 len = ((System.ComponentModel.DataAnnotations.StringLengthAttribute)attr).MaximumLength;
                        if (((string)((ComboBox)value).SelectedItem).Length > len)
                        {
                            return new ValidationResult(false, $"Максимальная длина - {len}");
                        }
                    }
                }
            }*/
            if (((String)value).Length > MaxLength)
            {
                return new ValidationResult(false, $"Максимальная длина - {MaxLength}");
            }
            return new ValidationResult(true, null);
        }
    }

    public class InventoryNumberValidationRule : ValidationRule
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

        public Boolean Check(object value)
        {
            String s = (String)value;
            String connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            int inv;
            if (s != "" && Int32.TryParse(s, out inv))
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
