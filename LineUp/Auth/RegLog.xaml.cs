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

namespace LineUp.Auth
{
    /// <summary>
    /// Логика взаимодействия для RegLog.xaml
    /// </summary>
    public partial class RegLog : Window
    {
        public bool islog = true;
        public login login = new login();
        public logup logup = new logup();
        public int CurrentID;
        public RegLog()
        {
            InitializeComponent();
            RegLogTemplate.Children.Clear();
            RegLogTemplate.Children.Add(login);
        }

        public void GetID()
        {
            try
            {
                
                using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
                {

                    connection.Open();
                    MySqlCommand command = new MySqlCommand("SELECT id FROM user ORDER BY id", connection);
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

        public bool CheckUser()
        {
            string NewLogin = logup.newloginfield.Text;
            string NewPassword = logup.newpasswordfield.Password;
            using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand($"SELECT * FROM user WHERE login = '{NewLogin}'", connection);
                MySqlDataReader readerForCheckLogin = (MySqlDataReader)command.ExecuteReader();
                if (readerForCheckLogin.HasRows)
                {
                    connection.Close();
                    return true;
                }
                connection.Close();
            }
            return false;
        }

        private void createoralready_Click(object sender, RoutedEventArgs e)
        {
            if(islog)
            {
                RegLogTemplate.Children.Clear();
                RegLogTemplate.Children.Add(logup);
                islog = false;
                createoralready.Content = "Уже есть аккаунт";
                logorreg.Content = "Создать";
            }
            else
            {
                RegLogTemplate.Children.Clear();
                RegLogTemplate.Children.Add(login);
                islog = true;
                createoralready.Content = "Создать аккаунт";
                logorreg.Content = "Войти";
            }
        }

        private void logorreg_Click(object sender, RoutedEventArgs e)
        {
            if(islog)
            {
                try
                {
                    string ULogin = login.loginfield.Text;
                    string Upassword = login.passwordfield.Password;
                    using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
                    {
                        User user = null;
                        
                        connection.Open();
                        MySqlCommand command = new MySqlCommand($"SELECT id,login,password FROM user WHERE login='{ULogin}' AND password='{Upassword}'", connection);
                        MySqlDataReader reader = (MySqlDataReader)command.ExecuteReader();
                        if (reader.HasRows) // если есть данные
                        {
                            while (reader.Read())
                            {
                                int id = int.Parse(reader.GetValue(0).ToString());
                                string login = reader.GetValue(1).ToString();
                                string password = reader.GetValue(2).ToString();
                                    
                                user = new User(id, login, password);
                                MainWindow.user = user;
                                MessageBox.Show("Добро пожаловать!");
                                MainWindow.MyForm.Visibility = Visibility.Visible;
                                this.Visibility = Visibility.Hidden;
                            }
                        }
                        connection.Close();
                    }
                }
                catch
                {
                    MessageBox.Show("cНевозможно войти, введите данные");
                    return;
                }
            }
            else
            {
                try
                {
                    string NewLogin = logup.newloginfield.Text;
                    string NewPassword = logup.newpasswordfield.Password;
                    if(NewLogin.Length<3)
                    {
                        MessageBox.Show("Слишком короткий логин!");
                        return;
                    }
                    if(NewPassword.Length<3)
                    {
                        MessageBox.Show("Слишком короткий пароль!");
                        return;
                    }
                    if(NewLogin.Length>49)
                    {
                        MessageBox.Show("Слишком длинный логин!");
                        return;
                    }
                    if(NewPassword.Length>49)
                    {
                        MessageBox.Show("Слишком длинный пароль!");
                        return;
                    }
                    if(CheckUser())
                    {
                        MessageBox.Show("Пользователь с таким именем существует!");
                        return;
                    }
                    using(MySqlConnection connection = new MySqlConnection(Connection.connectionString))
                    {
                        GetID();
                        connection.Open();
                        MySqlCommand command = new MySqlCommand($"INSERT INTO user (id,login,password) VALUES ({++CurrentID},'{NewLogin}','{NewPassword}')", connection);
                        command.ExecuteNonQuery();
                        connection.Close();
                        MessageBox.Show("Аккаунт создан!");
                        RegLogTemplate.Children.Clear();
                        RegLogTemplate.Children.Add(login);
                    }
                }
                catch
                {
                    MessageBox.Show("Невозможно добавить нового пользователя, попробуйте позже!");
                    return;
                }
            }
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            Settings s1 = new Settings();
            s1.Show();
        }
    }
}
