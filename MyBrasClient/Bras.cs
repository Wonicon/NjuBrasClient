using System.IO;
using System.Net;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Xaml;
// Custom reference
using Newtonsoft.Json.Linq;

namespace MyBrasClient
{
    public class Bras : DependencyObject
    {
        #region Status　DP
        public string Status
        {
            get { return (string)GetValue(StatusProperty); }
            set { SetValue(StatusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Status.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StatusProperty =
            DependencyProperty.Register("Status", typeof(string), typeof(Bras), new PropertyMetadata("Checking..."));
        #endregion

        #region Info DP
        public string Info
        {
            get { return (string)GetValue(InfoProperty); }
            set { SetValue(InfoProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Info.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InfoProperty =
            DependencyProperty.Register("Info", typeof(string), typeof(Bras), new PropertyMetadata(""));
        #endregion

        #region LoggedOut DP
        public bool LoggedOut
        {
            get { return (bool)GetValue(LoggedOutProperty); }
            set { SetValue(LoggedOutProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LoggedOut.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LoggedOutProperty =
            DependencyProperty.Register("LoggedOut", typeof(bool), typeof(Bras), new PropertyMetadata(true));
        #endregion

        private readonly string baseURL = "http://p.nju.edu.cn/portal_io/";

        private async Task<JObject> JsonResponseHelper(string post)
        {
            Debug.WriteLine(baseURL + post);
            WebRequest request = WebRequest.Create(baseURL + post);
            WebResponse response = await request.GetResponseAsync();
            Stream responseStream = response.GetResponseStream();
            return JObject.Parse((new StreamReader(responseStream)).ReadToEnd());
        }

        private void CheckLogStatusHelper(JObject json)
        {
            Info = json["reply_msg"].ToString();
            string code = json["reply_code"].ToString();
            // 0: 通过getinfo查到的已登录
            // 1: 登录成功
            // 2: 通过getinfo查到的无登录
            // 3: 信息错误
            // 6: 已登录
            // 101: 登出
            switch (code)
            {
                case "0": LoggedOut = false; break;
                case "1": LoggedOut = false; break;
                case "2": LoggedOut = true; break;
                case "3": LoggedOut = true; break;
                case "6": LoggedOut = false; break;
                case "101": LoggedOut = true; break;
                default: LoggedOut = true; break;
            }

            if (LoggedOut) { Status = "Log in"; }
            else { Status = "Log out"; }
        }

        public async void Check()
        {
            var responseJSON = await JsonResponseHelper("getinfo");
            Debug.WriteLine(responseJSON);
            CheckLogStatusHelper(responseJSON);
        }

        public async void Login(string username, string password)
        {
            string post = string.Format("login?username={0}&password={1}", username, password);
            var json = await JsonResponseHelper(post);
            CheckLogStatusHelper(json);
        }

        public async void Logout()
        {
            var json = await JsonResponseHelper("logout");
            CheckLogStatusHelper(json);
        }

        public Bras()
        {
            Check();
        }
    }
}
