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
    /// Логика взаимодействия для AddMember.xaml
    /// </summary>
    public partial class AddMember : Window
    {
        public int CurrentID;
        public int BandID;
        public bool IsImageLoaded = false;
        public AddMember(int bandid)
        {
            InitializeComponent();
            BandID = bandid;
        }
        public void GetID()
        {
            try
            {

                using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
                {

                    connection.Open();
                    MySqlCommand command = new MySqlCommand("SELECT id FROM members ORDER BY id", connection);
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
        private void ConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            if(MemberName.Text.Length<2)
            {
                MessageBox.Show("Короткое имя");
                return;
            }
            if(MemberName.Text.Length>100)
            {
                MessageBox.Show("Длинное имя");
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
                using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand($"INSERT INTO members(id,name,img,bandid) VALUES({++CurrentID},'{MemberName.Text}','{MemberImage.Text}',{BandID})", connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    
                }
                this.Close();
                MainWindow.MyForm.FieldForTemplate.Children.Clear();
                MainWindow.MyForm.FieldForTemplate.Children.Add(new Groups());
                
            }
            catch
            {
                MessageBox.Show("Невозможно добавить нового участника или участник с таким именем уже существует");
                return;
            }
        }

        private void MemberImage_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                MemberImageShow.Source = new BitmapImage(new Uri(MemberImage.Text));
                IsImageLoaded = true;
            }
            catch
            {

            }
        }
    }
}
