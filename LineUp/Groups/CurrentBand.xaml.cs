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

namespace LineUp.Groups
{
    /// <summary>
    /// Логика взаимодействия для CurrentBand.xaml
    /// </summary>
    public partial class CurrentBand : UserControl
    {
        public Band Band;
        public List<Member> members = new List<Member>();
        public List<Song> songs = new List<Song>();
        public CurrentBand(Band band)
        {
            InitializeComponent();
            if(MainWindow.user.ID==0)
            {
                AddMember.Visibility = Visibility.Visible;
                AddSong.Visibility = Visibility.Visible;
                DelBand.Visibility = Visibility.Visible;
                DelMember.Visibility = Visibility.Visible;
                DelSong.Visibility = Visibility.Visible;
                EditGroup.Visibility = Visibility.Visible;
            }

            Band = band;
            BandName.Text = Band.Name;
            BandGanre.Text = Band.Ganre;
            BandRate.Text = Band.Rate.ToString();
            try
            {
                BandImage.Source = new BitmapImage(new Uri(Band.Image));
            }
            catch
            {

            }
            LoadMembers();
            LoadSongs();
        }
        public void LoadMembers()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand($"SELECT id,name,img,bandid FROM members WHERE bandid={Band.ID}", connection);
                    MySqlDataReader reader = (MySqlDataReader)command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int id = int.Parse(reader.GetValue(0).ToString());
                            string name = reader.GetValue(1).ToString();
                            string img = reader.GetValue(2).ToString();
                            int bandid = int.Parse(reader.GetValue(3).ToString());

                            Member member = new Member(id, name, img, bandid);
                            members.Add(member);
                            

                        }
                    }
                    connection.Close();
                }
                Members.ItemsSource = null;
                Members.ItemsSource = members;
            }
            catch
            {
                MessageBox.Show("Невозможно загрузить участников, попробуйте позже");
            }
        }
        public void LoadSongs()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand($"SELECT id,name,bandid FROM songs WHERE bandid={Band.ID}", connection);
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

        private void AddMember_Click(object sender, RoutedEventArgs e)
        {
            AddMember addMemeber = new AddMember(Band.ID);
            addMemeber.Show();
        }

        private void AddSong_Click(object sender, RoutedEventArgs e)
        {
            AddSong addSong = new AddSong(Band.ID);
            addSong.Show();
        }

        private void DelBand_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using(MySqlConnection connection = new MySqlConnection(Connection.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand($"DELETE FROM members WHERE bandid={Band.ID}", connection);
                    command.ExecuteNonQuery();
                    command = new MySqlCommand($"DELETE FROM songs WHERE bandid={Band.ID}", connection);
                    command.ExecuteNonQuery();
                    command = new MySqlCommand($"DELETE FROM band WHERE id={Band.ID}",connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Группа удалена");
                    MainWindow.MyForm.FieldForTemplate.Children.Clear();
                    MainWindow.MyForm.FieldForTemplate.Children.Add(new Groups());
                }
            }
            catch
            {
                MessageBox.Show("Невозможно удалить группу, попробуйте позже");
                return;
            }
        }

        private void DelMember_Click(object sender, RoutedEventArgs e)
        {
            DelMember delMember = new DelMember(Band.ID);
            delMember.Show();
        }

        private void DelSong_Click(object sender, RoutedEventArgs e)
        {
            DelSong delSong = new DelSong(Band.ID);
            delSong.Show();
        }

        private void EditGroup_Click(object sender, RoutedEventArgs e)
        {
            EditGroup editGroup = new EditGroup(Band);
            editGroup.Show();
        }
    }
}
