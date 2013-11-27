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
using System.IO;
using System.Diagnostics;

namespace Shubhascanner
{
    /// <summary>
    /// Interaction logic for GNUGPL.xaml
    /// </summary>
    public partial class GNUGPL : UserControl
    {
        public GNUGPL()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            System.IO.StreamReader objReader = new StreamReader("C:\\myshubhalabha\\Notice.txt");

            if (File.Exists("C:\\myshubhalabha\\Notice.txt"))
            {
                richTextBox1.AppendText(objReader.ReadToEnd());
            }
            else richTextBox1.AppendText("ERROR: File not found!");
            objReader.Close();
        }
        protected void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
