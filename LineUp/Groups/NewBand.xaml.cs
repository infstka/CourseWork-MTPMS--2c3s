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
    /// Логика взаимодействия для NewBand.xaml
    /// </summary>
    public partial class NewBand : UserControl
    {
        public bool IsImageLoaded = false;
        public int CurrentID;
        public NewBand()
        {
            InitializeComponent();
        }
        public void GetID()
        {
            try
            {

                using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
                {

                    connection.Open();
                    MySqlCommand command = new MySqlCommand("SELECT id FROM band ORDER BY id", connection);
                    MySqlDataReader reader = (MySqlDataReader)command.ExecuteReader();
                    if (reader.HasRows) // если есть данные
                    {
                        while (reader.Read()) // построчно считываем данные
                        {
                            int id = int.Parse(reader.GetValue(0).ToString());
                            CurrentID = id;
                        }
                    }
                    connection.Close();
                }

            }
            catch
            {
                MessageBox.Show("Невозможно подключиться к базе данных, попробуйте позже");
                return;
            }
        }
        private void ConfirtmBtn_Click(object sender, RoutedEventArgs e)
        {
            if(NewName.Text.Length<3)
            {
                MessageBox.Show("Короткое название группы");
                return;
            }
            if(NewName.Text.Length>100)
            {
                MessageBox.Show("Длинное название группы");
                return;
            }
            if(NewGanre.Text.Length<2)
            {
                MessageBox.Show("Короткое название жанра");
                return;
            }
            if(NewGanre.Text.Length>100)
            {
                MessageBox.Show("Длинное название жанра");
                return;
            }
            try
            {
                if (int.Parse(NewRate.Text) < 1)
                {
                    MessageBox.Show("Добавьте рейтинг от 1 до 10");
                    return;
                }
            }
            catch
            {
                MessageBox.Show("Рейтинг должен быть числом");
                return;
            }
            if(!IsImageLoaded)
            {
                MessageBox.Show("Загрузите изображение");
                return;
            }

            try
            {
                GetID();
                using(MySqlConnection connection = new MySqlConnection(Connection.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand($"INSERT INTO band(id,name,ganre,rate,image) VALUES({++CurrentID},'{NewName.Text}','{NewGanre.Text}',{int.Parse(NewRate.Text)},'{NewImage.Text}')", connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                MainWindow.MyForm.FieldForTemplate.Children.Clear();
                MainWindow.MyForm.FieldForTemplate.Children.Add(new Groups());
            }
            catch
            {
                MessageBox.Show("Группа с таким названием уже существует");
            }
            
        }

        private void NewImage_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                ShowImage.Source = new BitmapImage(new Uri(NewImage.Text));
                IsImageLoaded = true;
            }
            catch
            {

            }
        }
    }
}
