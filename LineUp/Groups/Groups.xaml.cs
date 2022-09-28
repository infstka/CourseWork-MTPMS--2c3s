using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace LineUp.Groups
{
    /// <summary>
    /// Логика взаимодействия для Groups.xaml
    /// </summary>
    public partial class Groups : UserControl
    {
        public List<Band> bands = new List<Band>();
        public Groups()
        {
            InitializeComponent();
            if(MainWindow.user.ID==0)
            {
                CreateBand.Visibility = Visibility.Visible;
            }
            LoadBands();
        }
        public void LoadBands()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand($"SELECT id,name,ganre,rate,image FROM band", connection);
                    MySqlDataReader reader = (MySqlDataReader)command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int id = int.Parse(reader.GetValue(0).ToString());
                            string name = reader.GetValue(1).ToString();
                            string ganre = reader.GetValue(2).ToString();
                            int rate = int.Parse(reader.GetValue(3).ToString());
                            string image = reader.GetValue(4).ToString();

                            Band band = new Band(id, name, ganre, rate, image);
                            bands.Add(band);

                        }
                    }
                    connection.Close();
                }
                BandList.ItemsSource = null;
                BandList.ItemsSource = bands;
            }
            catch
            {
                MessageBox.Show("Невозможно загрузить группы, попробуйте позже");
            }
            
        }

        private void CreateBand_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.MyForm.FieldForTemplate.Children.Clear();
            MainWindow.MyForm.FieldForTemplate.Children.Add(new NewBand());
        }

        private void SearchField_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<Band> foundbands = new List<Band>();
            Regex regex = new Regex(SearchField.Text);
            foreach(Band band in bands)
            {
                if (regex.IsMatch(band.Name))
                {
                    foundbands.Add(band);
                }
            }
            BandList.ItemsSource = null;
            BandList.ItemsSource = foundbands;
        }

        private void BandList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentBand currentBand = new CurrentBand(BandList.SelectedItem as Band);
            MainWindow.MyForm.FieldForTemplate.Children.Clear();
            MainWindow.MyForm.FieldForTemplate.Children.Add(currentBand);
        }
    }
}
