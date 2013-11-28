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
using Microsoft.Win32;
using System.Text.RegularExpressions;
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
            //show register no to user 
            ManagementObject dsk1 = new ManagementObject(@"win32_logicaldisk.deviceid=""c:""");
            dsk1.Get();
           regid.Content   =regid.Content+ dsk1["VolumeSerialNumber"].ToString();

            //add values in drop down list 
           bbprice.Items.Add("Open");
           bbprice.Items.Add("High");
           bbprice.Items.Add("Low");
           bbprice.Items.Add("Close");

           rocprice.Items.Add("Open");
           rocprice.Items.Add("High");
           rocprice.Items.Add("Low");
           rocprice.Items.Add("Close");




           try
           {

               //take user save value 
               RegistryKey regKey = Registry.CurrentUser;
               regKey = regKey.CreateSubKey(@"amis\");
               var bbprice1 = regKey.GetValue("bbprice");
               var bbperiod1 = regKey.GetValue("bbperiod");
               var bbwidth1 = regKey.GetValue("bbwidth");
               var atr1 = regKey.GetValue("atr");
               var rocprice1 = regKey.GetValue("rocprice");
               var rocperiod1 = regKey.GetValue("rocperiod");
               var rsi1 = regKey.GetValue("rsi");
               var saracceleration1 = regKey.GetValue("saracceleration");
               var sarmaxacceleration1 = regKey.GetValue("sarmaxacceleration");
               var macdfastavg1 = regKey.GetValue("macdfastavg");
               var macdslowavg1 = regKey.GetValue("macdslowavg");
               var macdsignal1 = regKey.GetValue("macdsignal");

               if (bbprice1 == null || bbprice1 == "")
               {
                   bbprice.SelectedIndex = 0;
               }
               else
               {
                   bbprice.SelectedItem = bbprice1.ToString();
               }


               if (bbperiod1 == null || bbperiod1 == "")
               {
                   //default value 
                   bbperiod.Text = "15";
               }
               else
               {
                   bbperiod.Text = bbperiod1.ToString();

               }
               if (bbwidth1 == null || bbwidth1 == "")
               {
                   bbwidth.Text = "2";
               }
               else
               {

                   bbwidth.Text = bbwidth1.ToString();

               }

               if (atr1 == null || atr1 == "")
               {
                   atr.Text = "15";
               }
               else
               {
                   atr.Text = atr1.ToString();
               }
               if (rocprice1 == null || rocprice1 == "")
               {
                   rocprice.SelectedIndex = 0;
               }
               else
               {
                   rocprice.SelectedItem = rocprice1.ToString();
               }
               if (rocperiod1 == null || rocperiod1=="")
               {
                   rocperiod.Text = "15";
               }
               else
               {
                   rocperiod.Text = rocperiod1.ToString();

               }
               if (rsi1 == null || rsi1=="")
               {
                   rsi.Text = "15";
               }
               else
               {
                   rsi.Text = rsi1.ToString();
               }

               if (saracceleration1 == null || saracceleration1=="")
               {
                   saracceleration.Text = "0.02";
               }
               else
               {
                   saracceleration.Text = saracceleration1.ToString();
               }

               if (sarmaxacceleration1 == null || sarmaxacceleration1=="")
               {
                   sarmaxacceleration.Text = "0.2";
               }
               else
               {
                   sarmaxacceleration.Text = sarmaxacceleration1.ToString();
               }

               if (macdfastavg1 == null || macdfastavg1=="")
               {
                   macdfastavg.Text = "12";
               }
               else
               {
                   macdfastavg.Text = macdfastavg1.ToString();
               }
               if (macdslowavg1 == null || macdslowavg1=="")
               {
                   macdslowavg.Text = "26";
               }
               else
               {
                   macdslowavg.Text = macdslowavg1.ToString();
               }
               if (macdsignal1 == null || macdsignal1=="")
               {
                   macdsignal.Text = "9";
               }
               else
               {
                   macdsignal.Text = macdsignal1.ToString();
               }
             



               
           }
           catch
           {
           }

        }



        //save values into registry entry 
        private void save_Click(object sender, RoutedEventArgs e)
        {
            RegistryKey regKey = Registry.CurrentUser;
            regKey = regKey.CreateSubKey(@"amis\");
            regKey.SetValue("bbprice",bbprice.SelectedItem.ToString());
            regKey.SetValue("bbperiod", bbperiod.Text );
            regKey.SetValue("bbwidth", bbwidth.Text);
            regKey.SetValue("atr", atr.Text);
            regKey.SetValue("rocprice", rocprice.SelectedItem.ToString());
            regKey.SetValue("rocperiod", rocperiod.Text);
            regKey.SetValue("rsi", rsi.Text);
            regKey.SetValue("saracceleration", saracceleration.Text);
            regKey.SetValue("sarmaxacceleration", sarmaxacceleration.Text);
            regKey.SetValue("macdfastavg", macdfastavg.Text);
            regKey.SetValue("macdslowavg", macdslowavg.Text);
            regKey.SetValue("macdsignal", macdsignal.Text);



            System.Windows.MessageBox.Show("Vlaue Saved ", "Success Message", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information );









        }

        //Reset values to default 
        private void reset_Click(object sender, RoutedEventArgs e)
        {
            bbprice.SelectedIndex = 0;
            bbperiod.Text = "15";
            bbwidth.Text = "2";
            atr.Text = "15";
            rocperiod.Text = "15";
            rocprice.SelectedIndex = 0;
            rsi.Text = "15";
            saracceleration.Text = "0.02";
            sarmaxacceleration.Text = "0.2";
            macdslowavg.Text = "26";
            macdsignal.Text = "9";
            macdfastavg.Text = "12";
           
                
           
                
           
          
        }

        private int  enternoonly(string Value)
        {

            if (!System.Text.RegularExpressions.Regex.IsMatch(Value, @"[0-9]+(\.[0-9][0-9]?)?"))
            {
                System.Windows.MessageBox.Show("Enter numbar only ", "Warning Message", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);

            }
            return 0;
        }

        private void bbperiod_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void enternoonly(object sender, TextCompositionEventArgs e)
        {

            Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));

          
        }

       
       
      
    }
}
