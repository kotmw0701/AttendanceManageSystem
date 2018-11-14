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

namespace AttendanceManageSystem2 {
    /// <summary>
    /// Register.xaml の相互作用ロジック
    /// </summary>
    public partial class Register : Window {
        public Register() {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e) {
            if (this.DataContext != null)
                ((MainModel)DataContext).Password = ((PasswordBox)sender).SecurePassword;
        }
    }
}
