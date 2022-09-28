using LineUp.Command;
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
using System.Windows.Shapes;

namespace LineUp
{
    /// <summary>
    /// Логика взаимодействия для Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Singleton s1 = Singleton.GetInstance();
            Singleton s2 = Singleton.GetInstance();

            if (s1 == s2)
            {
                MessageBox.Show("Singleton is working!");
            }
            else
            {
                MessageBox.Show("Fail");
            }
        }


        private void Lang_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Ru.IsHighlighted)
            {
                var uri = new Uri(@"Dictionaries\Ru.xaml", UriKind.Relative);
                ResourceDictionary resourceDictionary = Application.LoadComponent(uri) as ResourceDictionary;
                Application.Current.Resources.Clear();
                Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
            }

            else if (By.IsHighlighted)
            {
                var uri = new Uri(@"Dictionaries\By.xaml", UriKind.Relative);
                ResourceDictionary resourceDictionary = Application.LoadComponent(uri) as ResourceDictionary;
                Application.Current.Resources.Clear();
                Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
            }

            else if (Eng.IsHighlighted)
            {
                var uri = new Uri(@"Dictionaries\Eng.xaml", UriKind.Relative);
                ResourceDictionary resourceDictionary = Application.LoadComponent(uri) as ResourceDictionary;
                Application.Current.Resources.Clear();
                Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
            }
        }

        private void Theme_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Light.IsHighlighted)
            {
                var uri = new Uri(@"Themes\Light.xaml", UriKind.Relative);
                ResourceDictionary resourceDictionary = Application.LoadComponent(uri) as ResourceDictionary;
                Application.Current.Resources.Clear();
                Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
            }
            else
            {
                var uri = new Uri(@"Themes\Dark.xaml", UriKind.Relative);
                ResourceDictionary resourceDictionary = Application.LoadComponent(uri) as ResourceDictionary;
                Application.Current.Resources.Clear();
                Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
