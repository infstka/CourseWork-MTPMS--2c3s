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
    /// Логика взаимодействия для EditGroup.xaml
    /// </summary>
    public partial class EditGroup : Window
    {
        public Band Band;
        public bool IsImageLoaded = false;
        public EditGroup(Band band)
        {
            InitializeComponent();
            Band = band;
            Name.Text = Band.Name;
            Ganre.Text = Band.Ganre;
            Rate.Text = Band.Rate.ToString();
            Image.Text = Band.Image;
            try
            {
                ShowImage.Source = new BitmapImage(new Uri(Band.Image));
                IsImageLoaded = true;
            }
            catch
            {

            }
        }

        private void ConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Name.Text.Length < 3)
            {
                MessageBox.Show("Короткое название группы");
                return;
            }
            if (Name.Text.Length > 100)
            {
                MessageBox.Show("Длинное название группы");
                return;
            }
            if (Name.Text.Length < 2)
            {
                MessageBox.Show("Короткое название жанра");
                return;
            }
            if (Name.Text.Length > 100)
            {
                MessageBox.Show("Длинное название жанра");
                return;
            }
            try
            {
                if (int.Parse(Rate.Text) < 1)
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
            if (!IsImageLoaded)
            {
                MessageBox.Show("Загрузите изображение");
                return;
            }
            try
            {
                
                using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand($"UPDATE band SET name='{Name.Text}', ganre='{Ganre.Text}', rate={int.Parse(Rate.Text)}, image='{Image.Text}' WHERE id={Band.ID}", connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                MainWindow.MyForm.FieldForTemplate.Children.Clear();
                MainWindow.MyForm.FieldForTemplate.Children.Add(new Groups());
                MessageBox.Show("Группа была изменена");
                this.Close();
            }
            catch
            {
                MessageBox.Show("Невозможно изменить группу, попробуйте позже");
            }
        }

        private void Image_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                ShowImage.Source = new BitmapImage(new Uri(Image.Text));
                IsImageLoaded = true;
            }
            catch
            {

            }
        }
    }
}
