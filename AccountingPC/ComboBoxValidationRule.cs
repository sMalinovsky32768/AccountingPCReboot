using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

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
        public bool isAvailable { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (!isAvailable)
            {
                return new ValidationResult(false, $"Оборудование с таким инвентарным номеров уже существует");
            }
            return new ValidationResult(true, null);
        }
    }
}
