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
using System.Net;
using Microsoft.Win32;
using System.Configuration;
using System.Net.Mail;
using System.Management;
using System.Reflection;

namespace Shubhascanner
{
    /// <summary>
    /// Interaction logic for Registartion.xaml
    /// </summary>
    public partial class Registartion : Window
    {
        public Registartion()
        {
            InitializeComponent();
        }



        //ignor java script error 
        void wb_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            string script = "document.body.style.overflow ='hidden'";
            System.Windows.Controls.WebBrowser wb = (System.Windows.Controls.WebBrowser)sender;
            wb.InvokeScript("execScript", new Object[] { script, "JavaScript" });
        }









        public void HideScriptErrors(WebBrowser wb, bool Hide)
        {

            FieldInfo fiComWebBrowser = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
            if
             (fiComWebBrowser == null)
                return;

            object
             objComWebBrowser = fiComWebBrowser.GetValue(wb);

            if
             (objComWebBrowser == null)
                return;

            objComWebBrowser.GetType().InvokeMember(
            "Silent",
            BindingFlags.SetProperty, null, objComWebBrowser, new object[] { Hide });

        }

        private void lead_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {

            HideScriptErrors(lead, true);

            if (lead.Source.ToString() == "http://shubhalabha.in/eng/crm/index.php?entryPoint=WebToLeadCapture")
            {

                RegistryKey regKey = Registry.CurrentUser;
                regKey = regKey.CreateSubKey(@"amis\");
                
                regKey.SetValue("sd", DateTime.Today.Day + "-" + DateTime.Today.Month + "-" + DateTime.Today.Year);
                regKey.SetValue("Login", "done");

               
                regKey.SetValue("crm", "done");
                this.Hide();
                Shubhascanner.Mainapplication  s = new Mainapplication();
                s.ShowDialog();

                return;
            }

        }

 
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            RegistryKey regKey = Registry.CurrentUser;
            regKey = regKey.CreateSubKey(@"amis\");
            var registrationdone = regKey.GetValue("crm");

            if (registrationdone == null || registrationdone=="")
            {

                Uri a3 = new System.Uri("http://shubhalabha.in/eng/dummy.html");
            try
            {

                lead.Source = a3;
            }
            catch
            {

            }
            }
            else if (registrationdone.ToString()=="done")
            {



            }
        }
    }
}
