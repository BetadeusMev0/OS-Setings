﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Enumeration;
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
using System.Net.NetworkInformation;
using System.Threading;

namespace OS_Settings
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitKeys();


        }
        RegistryKey trashKey = null;
        RegistryKey animationKey = null;
        RegistryKey tickKey = null;
        RegistryKey searchKey = null;
        RegistryKey numlockKey = null;
        RegistryKey defragKey = null;
        RegistryKey notifyKey = null;
        RegistryKey invisibleKey = null;
        RegistryKey commandKey = null;
        RegistryKey copyTo = null;
        RegistryKey fontKey = null;


        // new RegistryKey("HKEY_CURRENT_USER \\ Software \\ Microsoft \\ Windows \\ CurrentVersion \\ Explorer")
        private void InitKeys() 
        {
            trashKey = Registry.CurrentUser.OpenSubKey("SOFTWARE").OpenSubKey("Microsoft").OpenSubKey("Windows").OpenSubKey("CurrentVersion").OpenSubKey("Explorer").OpenSubKey("CLSID").OpenSubKey("{645FF040-5081-101B-9F08-00AA002F954E}" , true);
            trashName.Text = trashKey.GetValue("").ToString();
            animationKey = Registry.CurrentUser.OpenSubKey("Control Panel\\Desktop\\WindowMetrics", true);
            tickKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Advanced", true);
            searchKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Search" , true);
            numlockKey = Registry.CurrentUser.OpenSubKey("Control Panel\\Keyboard", true);
            defragKey = Registry.ClassesRoot.OpenSubKey("Drive\\shell", true);
            notifyKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Policies\\Microsoft\\Windows\\Explorer", true);
            invisibleKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize", true);
            //commandKey = Registry.ClassesRoot.OpenSubKey("Directory\\shell\\cmd");
            copyTo = Registry.ClassesRoot.OpenSubKey("AllFilesystemObjects\\shellex\\ContextMenuHandlers", true);
            fontKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Fonts", true);

            if (fontKey != null) { }

            if (copyTo.OpenSubKey("CopyTo") == null)
            {
                copyTo.CreateSubKey("CopyTo");
                copyTo.OpenSubKey("CopyTo").SetValue("", "{C2FBB630-2971-11D1-A18C-00C04FD75D13}");
            }


            if (copyTo.OpenSubKey("CopyTo").GetValue("") != "") copyToBox.IsChecked = true;

            if (invisibleKey.GetValue("AppsUseLightTheme") == "1") darkBox.IsChecked = false;
            else darkBox.IsChecked = true;


            if (invisibleKey.GetValue("EnableTransparency") == "1") invisibleBox.IsChecked = true;
            else invisibleBox.IsChecked = false;




            if (notifyKey.GetValue("DisableNotificationCenter") == null) notifyKey.SetValue("DisableNotificationCenter", 0, RegistryValueKind.DWord);


            if (notifyKey.GetValue("DisableNotificationCenter").ToString() == "0") notifyBox.IsChecked = true;
            if (notifyKey.GetValue("DisableNotificationCenter").ToString() == "1") notifyBox.IsChecked = false;


            if (defragKey.OpenSubKey("runas") == null) defragKey.CreateSubKey("runas");
            //if (defragKey.OpenSubKey("runas") != null) defragKey.OpenSubKey("runas").SetValue("", "Дефрагментация"); wip

            if ((string)numlockKey.GetValue("InitialKeyboardIndicators") == "2") numLockBox.IsChecked = true;
            else numLockBox.IsChecked = false;

            var tkValue = tickKey.GetValue("ShowSecondsInSystemClock");
            if (tkValue == null) tickKey.SetValue("ShowSecondsInSystemClock", 1, RegistryValueKind.DWord);
            tkValue = tickKey.GetValue("ShowSecondsInSystemClock");
            if((int)tkValue == 1) secBox.IsChecked = true;

            if (animationKey.GetValue("MinAnimate").ToString() == "1") onAnimation.IsChecked = true;
            else offAnimation.IsChecked = true;

            if ((int)searchKey.GetValue("SearchboxTaskbarMode") == 1 || (int)searchKey.GetValue("SearchboxTaskbarMode") == 2) searchBox.IsChecked = true; 


            

        }







        private void trashName_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }


        private bool onAnimationBool = false;
        private bool offAnimationBool = false;

        private void onAnimation_Checked(object sender, RoutedEventArgs e)
        {
            onAnimationBool = true;
            offAnimationBool = false;
        }

        private void offAnimation_Checked(object sender, RoutedEventArgs e)
        {
            onAnimationBool = false;
            offAnimationBool = true;
        }

        private void onAdapter_Checked(object sender, RoutedEventArgs e)
        {
            Process aq = new Process();
            aq.StartInfo.UseShellExecute = false;
            aq.StartInfo.FileName = "cmd.exe";
            aq.StartInfo.Arguments = "/C " + "netsh interface set interface name=Ethernet  admin=ENABLED";
            aq.StartInfo.CreateNoWindow = true;
            aq.Start();
        }

        private void offAdapter_Checked(object sender, RoutedEventArgs e)
        {
            Process aq = new Process();
            aq.StartInfo.UseShellExecute = false;
            aq.StartInfo.FileName = "cmd.exe";
            aq.StartInfo.Arguments = "/C " + "netsh interface set interface name=npk.local  admin=DISABLED";
            aq.StartInfo.CreateNoWindow = true;
            aq.Start();
        }

        private void InternetAdapter(string status) 
        {
            ProcessStartInfo internet = new ProcessStartInfo()
            {
                FileName = "cmd.exe",
                Arguments = "/C ipconfig /" + status,
                WindowStyle = ProcessWindowStyle.Hidden
            };
            Process.Start(internet);
        }

        public static void RestartExplorer()
        {
            Process.Start("taskkill", "/f /im explorer.exe");
            Thread.Sleep(1000);
            Process.Start("explorer.exe");
        }

        private void fontChange(string fontName) 
        {
            foreach (string value in fontKey.GetValueNames()) 
            {
                if (value.Contains("Segoe UI") && value != "Segue UI Emoji") { fontKey.SetValue(value, ""); }
            }
            var tmp = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\FontSubstitutes", true);

            if (tmp.GetValue("Segoe UI") == null) tmp.SetValue("Segoe UI", "Times New Roman");

            if (fontName != null) tmp.SetValue("Segoe Ui", fontName);

        }




        private void Button_Click(object sender, RoutedEventArgs e)
        {
            trashKey.SetValue("", trashName.Text);
            
            if (onAnimationBool) animationKey.SetValue("MinAnimate", "1");
            if (offAnimationBool) animationKey.SetValue("MinAnimate", "0");
            if ((bool)secBox.IsChecked) tickKey.SetValue("ShowSecondsInSystemClock", 1, RegistryValueKind.DWord);
            if ((bool)secBox.IsChecked == false) tickKey.SetValue("ShowSecondsInSystemClock", 0, RegistryValueKind.DWord);
            if ((bool)searchBox.IsChecked) searchKey.SetValue("SearchboxTaskbarMode", 2, RegistryValueKind.DWord);
            if (!((bool)searchBox.IsChecked)) searchKey.SetValue("SearchboxTaskbarMode", 0, RegistryValueKind.DWord);
            if (((bool)numLockBox.IsChecked)) numlockKey.SetValue("InitialKeyboardIndicators", "2");
            if (!((bool)numLockBox.IsChecked)) numlockKey.SetValue("InitialKeyboardIndicators", "0");
            if ((bool)notifyBox.IsChecked) notifyKey.SetValue("DisableNotificationCenter", 0, RegistryValueKind.DWord);
            if (!((bool)notifyBox.IsChecked)) notifyKey.SetValue("DisableNotificationCenter", 1, RegistryValueKind.DWord);
            if ((bool)invisibleBox.IsChecked) invisibleKey.SetValue("EnableTransparency", "1", RegistryValueKind.DWord);
            else invisibleKey.SetValue("EnableTransparency", 0, RegistryValueKind.DWord);
            if ((bool)darkBox.IsChecked) invisibleKey.SetValue("AppsUseLightTheme", 0, RegistryValueKind.DWord);
            else invisibleKey.SetValue("AppsUseLightTheme", 1, RegistryValueKind.DWord);
            if ((bool)offAdapter.IsChecked) InternetAdapter("release");
            if ((bool)onAdapter.IsChecked) InternetAdapter("renew");

            if ((bool)fontBox.IsChecked) fontChange("MS Serif"); 
            else fontChange("Calibri");


            //if ((bool)copyToBox.IsChecked) copyTo.OpenSubKey("CopyTo").SetValue("", "{C2FBB630-2971-11D1-A18C-00C04FD75D13}"); //НЕ РАБОТАЕТ 
            //else copyTo.OpenSubKey("CopyTo").SetValue("", "");
            RestartExplorer();
            
            new dialog().Show();

        }
    }
}