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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Management;
using Microsoft.Win32;

namespace Shubhascanner
{
    /// <summary>
    /// Interaction logic for Amibrokerpath.xaml
    /// </summary>
    public partial class Amibrokerpath : UserControl
    {
        public Amibrokerpath()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            var Open_Folder = new System.Windows.Forms.FolderBrowserDialog();
            if (Open_Folder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string Target_Folder_Path = Open_Folder.SelectedPath;


                Amiexepath.Text = Target_Folder_Path;


            }
            RegistryKey regKey = Registry.CurrentUser;
            regKey = regKey.CreateSubKey(@"amis\");
            regKey.SetValue("Amiexepath", Amiexepath.Text.ToString());
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                System.OperatingSystem osInfo2 = System.Environment.OSVersion;
                string result = string.Empty;
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem");
                foreach (ManagementObject os in searcher.Get())
                {
                    result = os["Caption"].ToString();
                    if (IntPtr.Size == 8)
                    {
                        Amiexepath.Text = "C:\\Program Files (x86)\\AmiBroker";


                    }
                    else
                    {
                        Amiexepath.Text = "C:\\Program Files\\AmiBroker";

                    }

                    break;
                }
               
            }
            catch
            {
            }

        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            RegistryKey regKey = Registry.CurrentUser;
            regKey = regKey.CreateSubKey(@"amis\");
            regKey.SetValue("Amiexepath", Amiexepath.Text.ToString());
        }
    }
}
