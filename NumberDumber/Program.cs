using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace NumberDumber
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false); 
            if(!System.IO.File.Exists(Application.StartupPath+"\\DB.txt") )
            {
                try
                {
                    System.IO.File.Create(Application.StartupPath + "\\DB.txt");
                }
                catch { }
            }
            try
            {
                Application.Run(new FormMain());
            }
            catch(Exception x) { MessageBox.Show(x.Message,"Error Please report it "); }
          

        }
        public static bool IsUrl(this string s)
        {
            try
            {
                Uri u = new Uri(s);
                return true;
            }
            catch { return false; }
        }
        private static bool checkDatabase()
        {
            return (System.IO.File.Exists(settings.GetDbPath()));
        }
        public static string TrimInAll(this string s , char c = ' ')
        {
            string x = "";
            foreach (char char_ in s)
                if (char_ != c)
                    x += char_.ToString();
            return x;
        }
        public static string IntOnly(this string s)
        {
            string res = "";
            foreach (char c in s)
                if (c >= '0' && c <= '9')
                    res += c.ToString();
            return res;
        }
        public static bool IsNumber(this string s)
        {
            try
            {
                int i = int.Parse(s);
                return true;
            }
            catch { return false; }
        }
        internal static string ReturnLocalFile()
        {
           return Application.StartupPath + "\\db.txt";
        }
    }
    public class Cookie2
    {
        public string cokName = "";
        public string cokValue = "";
        public Cookie2(string n, string v)
        {
            this.cokName = n;
            this.cokValue = v;
        }
        public Cookie2()
        {

        }
        public bool IsValid()
        {
            return this.cokName.Trim() != "";
        }
        public bool IsEmpty()
        {
            return this.cokValue.Trim() == "";
        }

    }
    
}
