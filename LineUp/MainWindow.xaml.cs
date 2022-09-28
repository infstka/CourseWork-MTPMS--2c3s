using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LineUp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Auth.User user;
        public static MainWindow MyForm;
        public MainWindow()
        {
            InitializeComponent();
            MyForm = this;
            Auth.RegLog regLog = new Auth.RegLog();
            regLog.Show();
            this.Visibility = Visibility.Hidden;
            FieldForTemplate.Children.Clear();
            FieldForTemplate.Children.Add(new Main());
        }

        private void Groups_Click(object sender, RoutedEventArgs e)
        {
            FieldForTemplate.Children.Clear();
            FieldForTemplate.Children.Add(new Groups.Groups());
        }

        private void Main_Click(object sender, RoutedEventArgs e)
        {
            FieldForTemplate.Children.Clear();
            FieldForTemplate.Children.Add(new Main());
        }

        private void Concerts_Click(object sender, RoutedEventArgs e)
        {
            FieldForTemplate.Children.Clear();
            FieldForTemplate.Children.Add(new Concerts.ConcertsList());
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            Settings s1 = new Settings();
            s1.Show();
        }
    }
}
