//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;

//namespace AccountingPC
//{
//    public partial class ChangeWindow
//    {
//        public static Dictionary<TypeDevice, ChangeRelation> Collection { get; private set; }

//    }
//    internal class ChangeRelation
//    {
//        public string AddCommand { get; set; }
//        public string UpdateCommand { get; set; }
//        public List<ChangeColumnRelation> Columns { get; set; }
//    }

//    internal class ChangeColumnRelation
//    {
//        public delegate bool ConditionDelegate(UIElement element);
//        public UIElement Box
//        {
//            get => box;
//            set
//            {
//                box = value;
//                //addedValue = 
//            }
//        }
//        public object Value
//        {
//            get => addedValue;
//            set
//            {
//                addedValue = value;
//            }
//        }
//        public DependencyProperty Property { get; set; }
//        public Type TargetType { get; set; }
//        public string ParameterName { get; set; }
//        public ConditionDelegate CheckCondition;
//        private object addedValue;
//        private UIElement box;

//        public bool IsUpdateOnly { get; set; } = false;
//    }

//    internal class ChangeRelationCollection
//    {
//        private ChangeWindow CurrentWindow { get; set; }
//        private readonly Dictionary<TypeDevice, ChangeRelation> collection = new Dictionary<TypeDevice, ChangeRelation>()
//        {
//            {
//                TypeDevice.PC,
//                new ChangeRelation()
//                {
//                    AddCommand = "AddPC",
//                    UpdateCommand = "UpdatePC",
//                    Columns = new List<ChangeColumnRelation>()
//                    {
//                        new ChangeColumnRelation()
//                        {
//                            ParameterName="@ID",
//                            IsUpdateOnly = true,
//                            Value = CurrentWindow.DeviceID,
//                        },
//                        new ChangeColumnRelation()
//                        {
//                            ParameterName="@Name",
//                            Box=CurrentWindow.name,
//                            Property=TextBox.TextProperty,
//                        },
//                    }
//                }
//            }
//        }

//            public Dictionary<TypeDevice, ChangeRelation> Collection => collection;
//    }
//}
