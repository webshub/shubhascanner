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
using System.Management;

namespace Shubhascanner
{
    /// <summary>
    /// Interaction logic for Mainapplication.xaml
    /// </summary>
    public partial class Mainapplication : Window
    {
        public Mainapplication()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ManagementObject dsk1 = new ManagementObject(@"win32_logicaldisk.deviceid=""c:""");
            dsk1.Get();
           regid.Content   =regid.Content+ dsk1["VolumeSerialNumber"].ToString();
        }
    }
}
