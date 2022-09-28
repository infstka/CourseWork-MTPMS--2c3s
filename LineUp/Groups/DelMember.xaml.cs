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
    /// Логика взаимодействия для DelMember.xaml
    /// </summary>
    public partial class DelMember : Window
    {
        public int BandID;
        public DelMember(int bandid)
        {
            InitializeComponent();
            BandID = bandid;
            LoadMembers();
        }

        public void LoadMembers()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand($"SELECT id,name,img,bandid FROM members WHERE bandid={BandID}", connection);
                    MySqlDataReader reader = (MySqlDataReader)command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int id = int.Parse(reader.GetValue(0).ToString());
                            string name = reader.GetValue(1).ToString();
                            string img = reader.GetValue(2).ToString();
                            int bandid = int.Parse(reader.GetValue(3).ToString());

                            
                            MembersList.Items.Add(name);


                        }
                    }
                    connection.Close();
                }
                
            }
            catch
            {
                MessageBox.Show("Невозможно загрузить участников, попробуйте позже");
            }
        }
        private void ConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string memberfordelete = MembersList.SelectedItem.ToString();
                if (memberfordelete != "")
                {
                    using(MySqlConnection connection = new MySqlConnection(Connection.connectionString))
                    {
                        connection.Open();
                        MySqlCommand command = new MySqlCommand($"DELETE FROM members WHERE name='{memberfordelete}' AND bandid={BandID}", connection);
                        command.ExecuteNonQuery();
                        connection.Close();
                        MessageBox.Show("Участник удален");
                        this.Close();
                        MainWindow.MyForm.FieldForTemplate.Children.Clear();
                        MainWindow.MyForm.FieldForTemplate.Children.Add(new Groups());
                    }
                }
            }
            catch
            {
                MessageBox.Show("Невозможно удалить участника");
            }
        }
    }
}
