using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OS_Settings
{
    /// <summary>
    /// Логика взаимодействия для dialog.xaml
    /// </summary>
    public partial class dialog : Window
    {
        public dialog()
        {
            InitializeComponent();
        }

        private void noButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void yesButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("shutdown", "/r /t 0");
        }
    }
}
