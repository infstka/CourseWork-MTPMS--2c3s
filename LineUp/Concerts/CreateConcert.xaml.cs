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
    /// Логика взаимодействия для CreateConcert.xaml
    /// </summary>
    public partial class CreateConcert : UserControl
    {
        public bool IsImageLoad = false;
        public int CurrentConcertID;
        public int CurrentConcerstSongID;
        public int CurrentConcertBandID;

        public List<Groups.Band> bands = new List<Groups.Band>();
        public List<Groups.Song> songs = new List<Groups.Song>();
        
        public CreateConcert()
        {
            InitializeComponent();
            LoadBands();
        }
        public void GetConcertID()
        {
            try
            {

                using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
                {

                    connection.Open();
                    MySqlCommand command = new MySqlCommand("SELECT id FROM concerts ORDER BY id", connection);
                    MySqlDataReader reader = (MySqlDataReader)command.ExecuteReader();
                    if (reader.HasRows) // если есть данные
                    {
                        while (reader.Read()) // построчно считываем данные
                        {
                            int id = int.Parse(reader.GetValue(0).ToString());
                            CurrentConcertID = id;
                        }
                    }
                    connection.Close();
                }

            }
            catch
            {
                MessageBox.Show("Невозможно подключиться к базе данных, попробуйте позже!");
                return;
            }
        }
        public void GetConcertSongID()
        {
            try
            {

                using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
                {

                    connection.Open();
                    MySqlCommand command = new MySqlCommand("SELECT id FROM concertssongs ORDER BY id", connection);
                    MySqlDataReader reader = (MySqlDataReader)command.ExecuteReader();
                    if (reader.HasRows) // если есть данные
                    {
                        while (reader.Read()) // построчно считываем данные
                        {
                            int id = int.Parse(reader.GetValue(0).ToString());
                            CurrentConcerstSongID = id;
                        }
                    }
                    connection.Close();
                }

            }
            catch
            {
                MessageBox.Show("Невозможно подключиться к базе данных, попробуйте позже!");
                return;
            }
        }
        public void GetConcertBandID()
        {
            try
            {

                using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
                {

                    connection.Open();
                    MySqlCommand command = new MySqlCommand("SELECT id FROM concertsbands ORDER BY id", connection);
                    MySqlDataReader reader = (MySqlDataReader)command.ExecuteReader();
                    if (reader.HasRows) // если есть данные
                    {
                        while (reader.Read()) // построчно считываем данные
                        {
                            int id = int.Parse(reader.GetValue(0).ToString());
                            CurrentConcertBandID = id;
                        }
                    }
                    connection.Close();
                }

            }
            catch
            {
                MessageBox.Show("Невозможно подключиться к базе данных, попробуйте позже!");
                return;
            }
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

                            Group1.Items.Add(name);
                            Group2.Items.Add(name);
                            Group3.Items.Add(name);

                            Groups.Band band = new Groups.Band(id, name, ganre, rate, image);
                            bands.Add(band);
                        }
                    }
                    connection.Close();

                    connection.Open();
                    command = new MySqlCommand($"SELECT id,name,bandid FROM songs", connection);
                    reader = (MySqlDataReader)command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int id = int.Parse(reader.GetValue(0).ToString());
                            string name = reader.GetValue(1).ToString();
                            int bandid = int.Parse(reader.GetValue(2).ToString());

                            Groups.Song song = new Groups.Song(id, name, bandid);
                            songs.Add(song);
                        }
                    }
                    connection.Close();
                }
               
            }
            catch
            {
                MessageBox.Show("Невозможно загрузить группы, попробуйте позже!");
            }
        }
        

        private void ConcertImage_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                ConcertImageShow.Source = new BitmapImage(new Uri(ConcertImage.Text));
                IsImageLoad = true;
            }
            catch
            {

            }
        }

        private void CreateConcertBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ConcertName.Text.Length < 3)
            {
                MessageBox.Show("Короткое имя концерта");
                return;
            }
            if(!IsImageLoad)
            {
                MessageBox.Show("Загрузите изображение");
                return;
            }
            if(Group1.SelectedItem == null && Group2.SelectedItem==null && Group3.SelectedItem==null)
            {
                MessageBox.Show("Выберите хотя бы 1 группу");
                return;
            }
            if(ChoosedDate.SelectedDate.Value < DateTime.Now)
            {
                MessageBox.Show("Выберите другую дату");
                return;
            }
            try
            {
                if(int.Parse(Hours.Text)<0 || int.Parse(Hours.Text)>23)
                {
                    MessageBox.Show("Введите часы от 0 до 23");
                    return;
                }
                if (int.Parse(Minutes.Text) < 0 || int.Parse(Minutes.Text) > 59)
                {
                    MessageBox.Show("Введите минуты от 0 до 59");
                    return;
                }
            }
            catch
            {
                MessageBox.Show("Часы и минуты должны быть числами!");
                return;
            }

            try
            {
                using(MySqlConnection connection = new MySqlConnection(Connection.connectionString))
                {
                    GetConcertID();
                    connection.Open();
                    MySqlCommand command = new MySqlCommand($"INSERT INTO concerts(id,name,image) VALUES ({++CurrentConcertID},'{ConcertName.Text}','{ConcertImage.Text}')", connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    InsertBands();
                    InsertSongs();
                    InsertDate();
                    MessageBox.Show("Концерт создан");
                }
            }
            catch
            {
                MessageBox.Show("Невозможно добавить концерт, попробуйте позже");
            }
        }

        public void InsertDate()
        {
            try
            {
                string NeededString;
                string day = ChoosedDate.SelectedDate.Value.Day.ToString();
                string month = ChoosedDate.SelectedDate.Value.Month.ToString();
                string year = ChoosedDate.SelectedDate.Value.Year.ToString();

                NeededString = day + "." + month + "." + year + " " + Hours.Text + ":" + Minutes.Text + ":" + "00";
                using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand($"INSERT INTO concertstime (id,time,concertid) VALUES ({CurrentConcertID},'{NeededString}',{CurrentConcertID})", connection);
                    command.ExecuteNonQuery();
                    connection.Close();

                }

            }
            catch
            {
                MessageBox.Show("Невозможно добавить дату");
                return;
            }
        }

        public void InsertBands()
        {
            int whichband=1;
            try
            {
                if(Group1.SelectedItem !=null)
                {
                    foreach(Groups.Band band in bands)
                    {
                        if(band.Name == Group1.SelectedItem.ToString())
                        {
                            whichband = band.ID;
                        }
                    }
                    using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
                    {
                        GetConcertBandID();
                        connection.Open();
                        MySqlCommand command = new MySqlCommand($"INSERT INTO concertsbands (id,concertid,bandid) VALUES ({++CurrentConcertBandID},{CurrentConcertID},{whichband})", connection);
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                

                if (Group2.SelectedItem != null)
                {
                    foreach (Groups.Band band in bands)
                    {
                        if (band.Name == Group2.SelectedItem.ToString())
                        {
                            whichband = band.ID;
                        }
                    }
                    using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
                    {
                        GetConcertBandID();
                        connection.Open();
                        MySqlCommand command = new MySqlCommand($"INSERT INTO concertsbands (id,concertid,bandid) VALUES ({++CurrentConcertBandID},{CurrentConcertID},{whichband})", connection);
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                

                if (Group3.SelectedItem != null)
                {
                    foreach (Groups.Band band in bands)
                    {
                        if (band.Name == Group3.SelectedItem.ToString())
                        {
                            whichband = band.ID;
                        }
                    }
                    using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
                    {
                        GetConcertBandID();
                        connection.Open();
                        MySqlCommand command = new MySqlCommand($"INSERT INTO concertsbands (id,concertid,bandid) VALUES ({++CurrentConcertBandID},{CurrentConcertID},{whichband})", connection);
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                
            }
            catch
            {
                MessageBox.Show("Невозможно добавить концерт, попробуйте позже");
            }
        }
        public void InsertSongs()
        {
            try
            {
                int whichsong = 1;
                if (Song11.SelectedItem != null)
                {
                    foreach (Groups.Song song in songs)
                    {
                        if (song.Name == Song11.SelectedItem.ToString())
                        {
                            whichsong = song.ID;
                        }
                    }
                    using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
                    {
                        GetConcertSongID();
                        connection.Open();
                        MySqlCommand command = new MySqlCommand($"INSERT INTO concertssongs (id,concertid,songid) VALUES ({++CurrentConcerstSongID},{CurrentConcertID},{whichsong})", connection);
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                if (Song12.SelectedItem != null)
                {
                    foreach (Groups.Song song in songs)
                    {
                        if (song.Name == Song12.SelectedItem.ToString())
                        {
                            whichsong = song.ID;
                        }
                    }
                    using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
                    {
                        GetConcertSongID();
                        connection.Open();
                        MySqlCommand command = new MySqlCommand($"INSERT INTO concertssongs (id,concertid,songid) VALUES ({++CurrentConcerstSongID},{CurrentConcertID},{whichsong})", connection);
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                if (Song13.SelectedItem != null)
                {
                    foreach (Groups.Song song in songs)
                    {
                        if (song.Name == Song13.SelectedItem.ToString())
                        {
                            whichsong = song.ID;
                        }
                    }
                    using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
                    {
                        GetConcertSongID();
                        connection.Open();
                        MySqlCommand command = new MySqlCommand($"INSERT INTO concertssongs (id,concertid,songid) VALUES ({++CurrentConcerstSongID},{CurrentConcertID},{whichsong})", connection);
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                if (Song14.SelectedItem != null)
                {
                    foreach (Groups.Song song in songs)
                    {
                        if (song.Name == Song14.SelectedItem.ToString())
                        {
                            whichsong = song.ID;
                        }
                    }
                    using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
                    {
                        GetConcertSongID();
                        connection.Open();
                        MySqlCommand command = new MySqlCommand($"INSERT INTO concertssongs (id,concertid,songid) VALUES ({++CurrentConcerstSongID},{CurrentConcertID},{whichsong})", connection);
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                if (Song21.SelectedItem != null)
                {
                    foreach (Groups.Song song in songs)
                    {
                        if (song.Name == Song21.SelectedItem.ToString())
                        {
                            whichsong = song.ID;
                        }
                    }
                    using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
                    {
                        GetConcertSongID();
                        connection.Open();
                        MySqlCommand command = new MySqlCommand($"INSERT INTO concertssongs (id,concertid,songid) VALUES ({++CurrentConcerstSongID},{CurrentConcertID},{whichsong})", connection);
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                if (Song22.SelectedItem != null)
                {
                    foreach (Groups.Song song in songs)
                    {
                        if (song.Name == Song22.SelectedItem.ToString())
                        {
                            whichsong = song.ID;
                        }
                    }
                    using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
                    {
                        GetConcertSongID();
                        connection.Open();
                        MySqlCommand command = new MySqlCommand($"INSERT INTO concertssongs (id,concertid,songid) VALUES ({++CurrentConcerstSongID},{CurrentConcertID},{whichsong})", connection);
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                if (Song23.SelectedItem != null)
                {
                    foreach (Groups.Song song in songs)
                    {
                        if (song.Name == Song23.SelectedItem.ToString())
                        {
                            whichsong = song.ID;
                        }
                    }
                    using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
                    {
                        GetConcertSongID();
                        connection.Open();
                        MySqlCommand command = new MySqlCommand($"INSERT INTO concertssongs (id,concertid,songid) VALUES ({++CurrentConcerstSongID},{CurrentConcertID},{whichsong})", connection);
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                if (Song24.SelectedItem != null)
                {
                    foreach (Groups.Song song in songs)
                    {
                        if (song.Name == Song24.SelectedItem.ToString())
                        {
                            whichsong = song.ID;
                        }
                    }
                    using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
                    {
                        GetConcertSongID();
                        connection.Open();
                        MySqlCommand command = new MySqlCommand($"INSERT INTO concertssongs (id,concertid,songid) VALUES ({++CurrentConcerstSongID},{CurrentConcertID},{whichsong})", connection);
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                if (Song31.SelectedItem != null)
                {
                    foreach (Groups.Song song in songs)
                    {
                        if (song.Name == Song31.SelectedItem.ToString())
                        {
                            whichsong = song.ID;
                        }
                    }
                    using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
                    {
                        GetConcertSongID();
                        connection.Open();
                        MySqlCommand command = new MySqlCommand($"INSERT INTO concertssongs (id,concertid,songid) VALUES ({++CurrentConcerstSongID},{CurrentConcertID},{whichsong})", connection);
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                if (Song32.SelectedItem != null)
                {
                    foreach (Groups.Song song in songs)
                    {
                        if (song.Name == Song32.SelectedItem.ToString())
                        {
                            whichsong = song.ID;
                        }
                    }
                    using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
                    {
                        GetConcertSongID();
                        connection.Open();
                        MySqlCommand command = new MySqlCommand($"INSERT INTO concertssongs (id,concertid,songid) VALUES ({++CurrentConcerstSongID},{CurrentConcertID},{whichsong})", connection);
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                if (Song33.SelectedItem != null)
                {
                    foreach (Groups.Song song in songs)
                    {
                        if (song.Name == Song33.SelectedItem.ToString())
                        {
                            whichsong = song.ID;
                        }
                    }
                    using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
                    {
                        GetConcertSongID();
                        connection.Open();
                        MySqlCommand command = new MySqlCommand($"INSERT INTO concertssongs (id,concertid,songid) VALUES ({++CurrentConcerstSongID},{CurrentConcertID},{whichsong})", connection);
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                if (Song34.SelectedItem != null)
                {
                    foreach (Groups.Song song in songs)
                    {
                        if (song.Name == Song34.SelectedItem.ToString())
                        {
                            whichsong = song.ID;
                        }
                    }
                    using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
                    {
                        GetConcertSongID();
                        connection.Open();
                        MySqlCommand command = new MySqlCommand($"INSERT INTO concertssongs (id,concertid,songid) VALUES ({++CurrentConcerstSongID},{CurrentConcertID},{whichsong})", connection);
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
            catch
            {
                MessageBox.Show("Невозможно добавить песни");
                return;
            }
            
        }


        private void Group1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(Group1.SelectedItem!=null)
            {
                Song11.Items.Clear();
                Song12.Items.Clear();
                Song13.Items.Clear();
                Song14.Items.Clear();
                string Bname = Group1.SelectedItem.ToString();
                try
                {
                    using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
                    {
                        connection.Open();
                        MySqlCommand command = new MySqlCommand($"SELECT songs.id,songs.name,bandid FROM songs INNER JOIN band ON songs.bandid=band.id WHERE band.name='{Bname}'  ", connection);
                        MySqlDataReader reader = (MySqlDataReader)command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int id = int.Parse(reader.GetValue(0).ToString());
                                string name = reader.GetValue(1).ToString();
                               
                                int bandid = int.Parse(reader.GetValue(2).ToString());
                                

                                Song11.Items.Add(name);
                                Song12.Items.Add(name);
                                Song13.Items.Add(name);
                                Song14.Items.Add(name);
                            }
                        }
                        connection.Close();
                    }

                }
                catch
                {
                    MessageBox.Show("Невозможно загрузить песни, попробуйте позже");
                }
            }
        }

        private void Group2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Group2.SelectedItem != null)
            {
                Song21.Items.Clear();
                Song22.Items.Clear();
                Song23.Items.Clear();
                Song24.Items.Clear();
                string Bname = Group2.SelectedItem.ToString();
                try
                {
                    using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
                    {
                        connection.Open();
                        MySqlCommand command = new MySqlCommand($"SELECT songs.id,songs.name,bandid FROM songs INNER JOIN band ON songs.bandid=band.id WHERE band.name='{Bname}'  ", connection);
                        MySqlDataReader reader = (MySqlDataReader)command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int id = int.Parse(reader.GetValue(0).ToString());
                                string name = reader.GetValue(1).ToString();

                                int bandid = int.Parse(reader.GetValue(2).ToString());


                                Song21.Items.Add(name);
                                Song22.Items.Add(name);
                                Song23.Items.Add(name);
                                Song24.Items.Add(name);
                            }
                        }
                        connection.Close();
                    }

                }
                catch
                {
                    MessageBox.Show("Невозможно загрузить песни, попробуйте позже");
                }
            }
        }

        private void Group3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Group3.SelectedItem != null)
            {
                Song31.Items.Clear();
                Song32.Items.Clear();
                Song33.Items.Clear();
                Song34.Items.Clear();
                string Bname = Group3.SelectedItem.ToString();
                try
                {
                    using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
                    {
                        connection.Open();
                        MySqlCommand command = new MySqlCommand($"SELECT songs.id,songs.name,bandid FROM songs INNER JOIN band ON songs.bandid=band.id WHERE band.name='{Bname}'  ", connection);
                        MySqlDataReader reader = (MySqlDataReader)command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int id = int.Parse(reader.GetValue(0).ToString());
                                string name = reader.GetValue(1).ToString();

                                int bandid = int.Parse(reader.GetValue(2).ToString());


                                Song31.Items.Add(name);
                                Song32.Items.Add(name);
                                Song33.Items.Add(name);
                                Song34.Items.Add(name);
                            }
                        }
                        connection.Close();
                    }

                }
                catch
                {
                    MessageBox.Show("Невозможно загрузить песни, попробуйте позже");
                }
            }
        }
    }
}
