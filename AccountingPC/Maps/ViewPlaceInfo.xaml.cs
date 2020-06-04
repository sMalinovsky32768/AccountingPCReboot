using System.Windows;

namespace AccountingPC.Maps
{
    /// <summary>
    /// Логика взаимодействия для ViewPlaceInfo.xaml
    /// </summary>
    public partial class ViewPlaceInfo : Window
    {
        private readonly Place place;
        public ViewPlaceInfo(Place place)
        {
            InitializeComponent();
            this.place = place;
        }
    }
}
