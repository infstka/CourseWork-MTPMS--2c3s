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
    /// Логика взаимодействия для Concerts.xaml
    /// </summary>
    public partial class Concerts : UserControl
    {
        public Concerts()
        {
            InitializeComponent();
            DispatcherTimer dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromSeconds(1);
            dt.Tick += dtTicker;
            dt.Start();
        }
        public string testdate = "04.12.2026 18:30:15";
       
        public DateTime somedate = new DateTime(2026,12,4,18,30,15);
        
        private void dtTicker(object Sender, EventArgs e)
        {
            DateTime teee = DateTime.Parse(testdate);
            TimeSpan time = teee - DateTime.Now;
            var days = time.Days;
            var seconds = time.Seconds;
            var minutes = time.Minutes;
            var hours = time.Hours;
            TimerDays.Content = days.ToString();
            TimerHours.Content = hours.ToString();
            TimerMinutes.Content = minutes.ToString();
            TimerSeconds.Content = seconds.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(DateChoose.SelectedDate.ToString());
        }
    }
}
