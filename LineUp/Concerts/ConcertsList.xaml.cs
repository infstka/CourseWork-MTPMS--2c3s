using MySql.Data.MySqlClient;
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

namespace LineUp.Concerts
{
    /// <summary>
    /// Логика взаимодействия для ConcertsList.xaml
    /// </summary>
    public partial class ConcertsList : UserControl
    {
        public List<Concert> concerts = new List<Concert>();
        public ConcertsList()
        {
            InitializeComponent();
            LoadConcerts();
            if(MainWindow.user.ID==0)
            {
                CreateConcert.Visibility = Visibility.Visible;
            }
        }
        public void LoadConcerts()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand($"SELECT * FROM concerts", connection);
                    MySqlDataReader reader = (MySqlDataReader)command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int id = int.Parse(reader.GetValue(0).ToString());
                            string name = reader.GetValue(1).ToString();
                            string image = reader.GetValue(2).ToString();
                            Concert concert = new Concert(id, name, image);
                            concerts.Add(concert);
                        }
                    }
                    connection.Close();
                }
                Concerts.ItemsSource = null;
                Concerts.ItemsSource = concerts;
            }
            catch
            {
                MessageBox.Show("Невозможно загрузить концерты");
                return;
            }
            
            
        }
        
        private void CreateConcert_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.MyForm.FieldForTemplate.Children.Clear();
            MainWindow.MyForm.FieldForTemplate.Children.Add(new CreateConcert());
        }

        private void BandList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var a = Concerts.SelectedItem as Concert;
            MainWindow.MyForm.FieldForTemplate.Children.Clear();
            MainWindow.MyForm.FieldForTemplate.Children.Add(new CurrentConcert(a));
        }

    }
    
}
