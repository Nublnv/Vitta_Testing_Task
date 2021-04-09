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
using System.Data.SqlClient;
using Vitta_Testing_Task.Properties;

namespace Vitta_Testing_Task
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DBData _dBData = null;
        public DBData dBData { private get { return _dBData; } set { _dBData = value; } }
        private bool connectionFlag = false;
        private decimal _toPayment;
        public decimal toPayment { private get { return _toPayment; } set { _toPayment = value; } }
        private SqlConnection _sqlConnection = null;
        public MainWindow()
        {
            InitializeComponent();
            listView1.IsEnabled = false;
            listView1.Visibility = Visibility.Hidden;
            listView2.IsEnabled = false;
            listView2.Visibility = Visibility.Hidden;
        }

        private void menuItemConnect_Click(object sender, RoutedEventArgs e)
        {
            if (!connectionFlag)
            {
                while (!connectionFlag)
                {
                    dBData = null;
                    LogonWindow logonWindow = new LogonWindow();
                    logonWindow.Owner = this;
                    logonWindow.ShowDialog();
                    if (dBData != null)
                    {
                        _sqlConnection = new SqlConnection(dBData.ConnectionString);
                        try
                        {
                            _sqlConnection.Open();
                        }
                        catch (SqlException)
                        {
                            MessageBox.Show("Неверный логин или пароль");
                            continue;
                        }
                        break;
                    }
                    else return;
                }
                connectionFlag = true;
                menuItemConnect.Header = "Отключиться";
                listView1.IsEnabled = true;
                listView1.Visibility = Visibility.Visible;
                listView1.ItemsSource = DBData.DefaultView(_sqlConnection);
                label1.Content ="Остаток на счете: " + DBData.Amount(_sqlConnection).ToString();
            }
            else
            {
                _sqlConnection.Close();
                connectionFlag = false;
                listView1.IsEnabled = false;
                listView1.Visibility = Visibility.Hidden;
                listView2.IsEnabled = false;
                listView2.Visibility = Visibility.Hidden;
                buttonPay.IsEnabled = false;
                menuItemConnect.Header = "Подключиться";
            }

        }

        private void listView1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            buttonPay.IsEnabled = true;
        }

        private void buttonPay_Click(object sender, RoutedEventArgs e)
        {
            PayWindow payWindow = new PayWindow(((Order)listView1.SelectedItem).toPayment,DBData.Amount(_sqlConnection));
            payWindow.Owner = this;
            payWindow.ShowDialog();
            try
            {
                DBData.Pay(_sqlConnection, (Order)listView1.SelectedItem, _toPayment);
            }
            catch
            {
                MessageBox.Show("Транзакция отменена");
                buttonPay.IsEnabled = false;
                label1.Content = "Остаток на счете: " + DBData.Amount(_sqlConnection).ToString();
                return;
            }
            listView1.ItemsSource = DBData.DefaultView(_sqlConnection);
            buttonPay.IsEnabled = false;
            label1.Content = "Остаток на счете: " + DBData.Amount(_sqlConnection).ToString();
        }

        private void menuItemPayment_Click(object sender, RoutedEventArgs e)
        {
            buttonPay.IsEnabled = false;
            listView1.IsEnabled = false;
            listView1.Visibility = Visibility.Hidden;
            listView2.IsEnabled = true;
            listView2.Visibility = Visibility.Visible;
            listView2.ItemsSource = DBData.PaymentView(_sqlConnection);
            label1.Content = "Остаток на счете: " + DBData.Amount(_sqlConnection).ToString();
        }

        private void menuItemOrders_Click(object sender, RoutedEventArgs e)
        {
            buttonPay.IsEnabled = false;
            listView2.IsEnabled = false;
            listView2.Visibility = Visibility.Hidden;
            listView1.IsEnabled = true;
            listView1.Visibility = Visibility.Visible;
            listView1.ItemsSource = DBData.DefaultView(_sqlConnection);
            label1.Content = "Остаток на счете: " + DBData.Amount(_sqlConnection).ToString();
        }
    }
}
