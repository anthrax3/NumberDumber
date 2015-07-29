using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;


namespace NumberDumber
{
    public partial class LoginFrm : Form
    {
        [DllImport("wininet.dll", SetLastError = true)]
        public static extern bool InternetGetCookieEx(
            string url,
            string cookieName,
            StringBuilder cookieData,
            ref int size,
            Int32 dwFlags,
            IntPtr lpReserved);

        private const Int32 InternetCookieHttponly = 0x2000;

        /// <summary>
        /// Gets the URI cookie container.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        public static CookieContainer GetUriCookieContainer(Uri uri)
        {
            CookieContainer cookies = null;
            // Determine the size of the cookie
            int datasize = 8192 * 16;
            StringBuilder cookieData = new StringBuilder(datasize);
            if (!InternetGetCookieEx(uri.ToString(), null, cookieData, ref datasize, InternetCookieHttponly, IntPtr.Zero))
            {
                if (datasize < 0)
                    return null;
                // Allocate stringbuilder large enough to hold the cookie
                cookieData = new StringBuilder(datasize);
                if (!InternetGetCookieEx(
                    uri.ToString(),
                    null, cookieData,
                    ref datasize,
                    InternetCookieHttponly,
                    IntPtr.Zero))
                    return null;
            }
            if (cookieData.Length > 0)
            {
                cookies = new CookieContainer();
                cookies.SetCookies(uri, cookieData.ToString().Replace(';', ','));
            }
            return cookies;
        }

        public LoginFrm(bool logout=false)
        {
            InitializeComponent();
            if (webBrowser1.Document != null && logout)
                webBrowser1.Document.Cookie = "";
            webBrowser1.Navigate("http://truecaller.com/sign-out");
            if (webBrowser1.Document != null && logout )
                webBrowser1.Document.Cookie = "";
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (formloaded == false)
                return;
            btnOk.Visible = true;
            CheckNewCookie();
        }

        private void CheckNewCookie()
        {
            settings.SetCookie(this.webBrowser1.Document.Cookie);
            if (this.webBrowser1.DocumentText.Contains(">Sign Out</a"))
            {
                string dc = webBrowser1.Document.Cookie;
                try
                {
                    CookieContainer c = LoginFrm.GetUriCookieContainer(new Uri("http://truecaller.com"));
                    foreach (Cookie cok in c.GetCookies(new Uri("http://truecaller.com")))
                        dc += cok.Name + "=" + cok.Value + ";";
                    dc = dc.Substring(0, dc.Length - 1);

                }
                catch { }// c.GetCookies()
                settings.SetCookie(dc);
                this.FoundNewCookie = true;
                this.Close();
            }

        }

        public bool formloaded { get; set; }

        private void LoginFrm_Load(object sender, EventArgs e)
        {
            formloaded = true;
        }

        public bool FoundNewCookie { get; set; }

        private void btnOk_Click(object sender, EventArgs e)
        {
            CheckNewCookie();
            this.Close();
        }
    }
}
