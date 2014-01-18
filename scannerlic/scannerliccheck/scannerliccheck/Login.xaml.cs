
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Web;
using System.IO;
using System.Linq;

using System.Net;
using Microsoft.Win32;
using System.Configuration;
using System.Net.Mail;
using System.Management;
using System.Reflection;

namespace scannerliccheck
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

      
        public void liccheck()
        {

            RegistryKey regKey = Registry.CurrentUser;
            regKey = regKey.CreateSubKey(@"amis\");

            var registerdate = regKey.GetValue("sd");
            var paidornot = regKey.GetValue("sp");

            ///////////////////////////////
            //expiration

            try
            {







                string[] datefromreg = registerdate.ToString().Split('-');



                DateTime reg = new DateTime(Convert.ToInt32(datefromreg[2]), Convert.ToInt32(datefromreg[1]), Convert.ToInt32(datefromreg[0]));




                reg = reg.AddDays(9);
                //its checking trail period expired or not 
                if (reg < DateTime.Today.Date)
                {
                    // Uri a = new System.Uri("http://besttester.com/lic/lic.txt");
                    Uri a = new System.Uri("http://shubhalabha.in/scannerlic.txt");


                    // webBrowser1.Source = a;
                    string credentials = "liccheck:lic123!@#";
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(a);
                    request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials)));
                    request.PreAuthenticate = true;
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    StreamReader reader = new StreamReader(response.GetResponseStream());

                    ////////////////////////////////////////////

                    //checking file which is store  on server 
                    string[] serverdata = reader.ReadToEnd().Split(',');
                    string[] serverdata1 = null;
                    int flagforuserpresentonserver = 0;
                    for (int i = 0; i < serverdata.Count(); i++)
                    {
                        serverdata1 = serverdata[i].Split(' ');
                        string[] datefromserver = serverdata1[1].ToString().Split('-');
                        // DateTime dateonserver=Convert.ToDateTime(serverdata1[1]);
                        DateTime dateonserver = new DateTime(Convert.ToInt32(datefromserver[2]), Convert.ToInt32(datefromserver[1]), Convert.ToInt32(datefromserver[0]));

                        ManagementObject dsk1 = new ManagementObject(@"win32_logicaldisk.deviceid=""c:""");
                        dsk1.Get();
                        string id1 = dsk1["VolumeSerialNumber"].ToString();
                        if (id1 == serverdata1[0])
                        {
                            flagforuserpresentonserver = 1;
                            if (dateonserver < DateTime.Today.Date)
                            {
                                System.Windows.MessageBox.Show("Your Trial version is expired, please contact sales@shubhalabha.in'", "Warning Message", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);



                                regKey.SetValue("valid", "notworking");

                            }
                        }
                    }

                    if (flagforuserpresentonserver == 0)
                    {
                        System.Windows.MessageBox.Show("Your Trial version is expired, please contact sales@shubhalabha.in'", "Warning Message", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);


                        regKey.SetValue("valid", "notworking");
                        Environment.Exit(0);
                        return;
                        // closeallprocess();
                    }
                    else
                    {


                        regKey.SetValue("valid", "working");
                        Environment.Exit(0);
                    }
                    ///////////////////////////////////////////


                }
                else
                {
                    System.Windows.MessageBox.Show("Your trial period will expire on " + reg.ToShortDateString(), "Warning Message", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                    
                  
                    regKey.SetValue("valid", "working");
                    Environment.Exit(0);
                }


            }
            catch (Exception ex)
            {

                System.Windows.MessageBox.Show(ex.Message, "Error Message", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);

            }


















        }
       
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            liccheck();
        }
    }
}
