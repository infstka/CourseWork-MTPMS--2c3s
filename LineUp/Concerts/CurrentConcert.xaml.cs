using LineUp.Groups;
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
using System.Windows.Threading;

namespace LineUp.Concerts
{
    /// <summary>
    /// Логика взаимодействия для CurrentConcert.xaml
    /// </summary>
    public partial class CurrentConcert : UserControl
    {
        public Concert Concert;
        public List<Band> bands = new List<Band>();
        public List<Song> songs = new List<Song>();

        public string DateToStart;
        public CurrentConcert(Concert concert)
        {
            InitializeComponent();
            Concert = concert;
            ConcertName.Text = Concert.Name;
            LoadBands();
            LoadSongs();
            GetTimer();
            if(MainWindow.user.ID==0)
            {
                DelConcert.Visibility = Visibility.Visible;
            }
        }

        public void GetTimer()
        {
            

            try
            {
                using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand($"SELECT time FROM concertstime WHERE concertid={Concert.ID}", connection);
                    MySqlDataReader reader = (MySqlDataReader)command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string date = reader.GetValue(0).ToString();
                            DateToStart = date;
                        }
                    }
                    
                }
                DispatcherTimer dt = new DispatcherTimer();
                dt.Interval = TimeSpan.FromSeconds(1);
                dt.Tick += dtTicker;
                dt.Start();
            }
            catch
            {
                MessageBox.Show("Невозможно загрузить дату, попробуйте позже");
                return;
            }
        }

        private void dtTicker(object Sender, EventArgs e)
        {
            try
            {
                DateTime CurrentDate = DateTime.Parse(DateToStart);
                TimeSpan time = CurrentDate - DateTime.Now;
                var days = time.Days;
                var seconds = time.Seconds;
                var minutes = time.Minutes;
                var hours = time.Hours;
                LeftDays.Content = days.ToString();
                LeftHours.Content = hours.ToString();
                LeftMinutes.Content = minutes.ToString();
                LeftSeconds.Content = seconds.ToString();
            }
            catch
            {
                
                return;
            }
        }

        public void LoadSongs()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand($"SELECT songs.id,name,bandid FROM songs INNER JOIN concertssongs ON songs.id = concertssongs.songid AND concertid={Concert.ID} ", connection);
                    MySqlDataReader reader = (MySqlDataReader)command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int id = int.Parse(reader.GetValue(0).ToString());
                            string name = reader.GetValue(1).ToString();
                            int bandid = int.Parse(reader.GetValue(2).ToString());

                            Song song = new Song(id, name, bandid);
                            songs.Add(song);

                        }
                    }
                    connection.Close();
                }
                Songs.ItemsSource = null;
                Songs.ItemsSource = songs;
            }
            catch
            {
                MessageBox.Show("Невозможно загрузить песни, попробуйте позже");
            }
        }

        public void LoadBands()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand($"SELECT band.id,name,ganre,rate,image FROM band INNER JOIN concertsbands ON band.id = concertsbands.bandid AND concertid={Concert.ID}", connection);
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

        private void DelConcert_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using(MySqlConnection connection = new MySqlConnection(Connection.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand($"DELETE FROM concertstime WHERE concertid={Concert.ID}",connection);
                    command.ExecuteNonQuery();
                    command = new MySqlCommand($"DELETE FROM concertssongs WHERE concertid={Concert.ID}", connection);
                    command.ExecuteNonQuery();
                    command = new MySqlCommand($"DELETE FROM concertsbands WHERE concertid={Concert.ID}", connection);
                    command.ExecuteNonQuery();
                    command = new MySqlCommand($"DELETE FROM concerts WHERE id={Concert.ID}", connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Концерт удален");
                    MainWindow.MyForm.FieldForTemplate.Children.Clear();
                    MainWindow.MyForm.FieldForTemplate.Children.Add(new ConcertsList());
                }
            }
            catch
            {
                MessageBox.Show("Невозможно удалить концерт");
                return;
            }
        }
    }
}
