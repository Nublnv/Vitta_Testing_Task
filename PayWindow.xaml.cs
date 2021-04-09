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

namespace Vitta_Testing_Task
{
    /// <summary>
    /// Логика взаимодействия для PayWindow.xaml
    /// </summary>
    public partial class PayWindow : Window
    {
        public PayWindow(decimal toPayment, decimal amount)
        {
            InitializeComponent();
            textBoxValue.Text = sliderValue.Value.ToString();
            sliderValue.Maximum = Math.Max((double)toPayment, (double)amount);
            sliderValue.IsEnabled = true;
            labelMax.Content +=" " + amount.ToString();
        }

        private int dS_Count(string s)
        {
            string substr = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0].ToString();
            int count = (s.Length - s.Replace(substr, "").Length) / substr.Length;
            return count;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            textBoxValue.Text = sliderValue.Value.ToString();
        }

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Owner;
            mainWindow.toPayment = (decimal)double.Parse(textBoxValue.Text);
            this.Close();
        }

        private void textBoxValue_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !((Char.IsDigit(e.Text, 0) || ((e.Text == System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0].ToString()) && (dS_Count(((TextBox)sender).Text) < 1))));
        }

        
    }
}
