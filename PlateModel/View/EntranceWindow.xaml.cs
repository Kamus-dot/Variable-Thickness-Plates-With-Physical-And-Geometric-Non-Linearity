using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PlateModel
{
    /// <summary>
    /// Логика взаимодействия для EntranceWindow.xaml
    /// </summary>
    public partial class EntranceWindow : Window
    {
        public EntranceWindow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void btnAddCar_Click(object sender, RoutedEventArgs e)
        {
            var dblAnim = new DoubleAnimation
            {
                From = 1,
                To = 0
            };
            btnAddCar.BeginAnimation(OpacityProperty, dblAnim);
        }
    }
}
