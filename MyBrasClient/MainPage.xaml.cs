using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
// Custom using
using System.Diagnostics;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MyBrasClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public Bras bras { get; set; } = new Bras();

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Login(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine($"Username: {username.Text}\nPassword: {password.Password}");
            Debug.WriteLine(bras.Status);
            if (bras.LoggedOut) { bras.Login(username.Text, password.Password); }
            else { bras.Logout(); }
        }
    }
}
