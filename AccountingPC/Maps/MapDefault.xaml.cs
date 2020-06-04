using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace AccountingPC.Maps
{
    /// <summary>
    /// Логика взаимодействия для MapDefault.xaml
    /// </summary>
    public partial class MapDefault : Page
    {
        public MapDefault()
        {
            InitializeComponent();
        }

        private void MouseDownHandler(object sender, MouseButtonEventArgs e)
        {
            new ViewPlaceInfo(new Place(((Path)e.OriginalSource).Name.Split('_'))).ShowDialog();
        }
    }
}
