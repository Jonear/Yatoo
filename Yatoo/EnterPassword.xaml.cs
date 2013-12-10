using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Yatoo
{
    /// <summary>
    /// EnterPassword.xaml 的交互逻辑
    /// </summary>
    public partial class EnterPassword : Window
    {
        public bool Is_GetPassword = false;
        public EnterPassword()
        {
            InitializeComponent();

            password.Focus();
        }
        private void GetPassword(object sender, RoutedEventArgs e)
        {
            Is_GetPassword = true;
            Close();
        }
    }
}
