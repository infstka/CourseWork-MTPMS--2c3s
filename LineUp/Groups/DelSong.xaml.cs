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
using System.Windows.Shapes;

namespace LineUp.Groups
{
    /// <summary>
    /// Логика взаимодействия для DelSong.xaml
    /// </summary>
    public partial class DelSong : Window
    {
        public int BandID;
        public DelSong(int bandid)
        {
            InitializeComponent();
            BandID = bandid;
            LoadSongs();
        }
        public void LoadSongs()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand($"SELECT id,name,bandid FROM songs WHERE bandid={BandID}", connection);
                    MySqlDataReader reader = (MySqlDataReader)command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int id = int.Parse(reader.GetValue(0).ToString());
                            string name = reader.GetValue(1).ToString(); 
                            int bandid = int.Parse(reader.GetValue(2).ToString());
                            Songs.Items.Add(name);
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
        private void ConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string songfordelete = Songs.SelectedItem.ToString();
                if (songfordelete != "")
                {
                    using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
                    {
                        connection.Open();
                        MySqlCommand command = new MySqlCommand($"DELETE FROM songs WHERE name='{songfordelete}' AND bandid={BandID}", connection);
                        command.ExecuteNonQuery();
                        connection.Close();
                        MessageBox.Show("Песня удалена");
                        this.Close();
                        MainWindow.MyForm.FieldForTemplate.Children.Clear();
                        MainWindow.MyForm.FieldForTemplate.Children.Add(new Groups());
                    }
                }
            }
            catch
            {
                MessageBox.Show("Невозможно удалить песню");
            }
        }
    }
}
