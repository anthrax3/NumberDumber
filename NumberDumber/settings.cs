using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NumberDumber
{
    class settings
    {
        public static string GetUserAgent()
        {
            return global::NumberDumber.Properties.Settings.Default.UserAgent;
        }

        internal static string GetAccept()
        {
            return global::NumberDumber.Properties.Settings.Default.Accept;

        }

        internal static string GetCookie()
        {
            return global::NumberDumber.Properties.Settings.Default.Cookie;

        }

        internal static string GetDbPath()
        {
            return global::NumberDumber.Properties.Settings.Default.DbPath;

        }

        internal static string ReturnConnectionString()
        {
            string x = string.Format("Data Source={0};Version=3;New=False;Compress=True;", GetDbPath());
            return ((GetDbPath().Trim().Length > 1) ? x : "Data Source=db;Version=3;New=True;Compress=True;");
        }

        internal static void Save()
        {
            global::NumberDumber.Properties.Settings.Default.Save();

        }

        internal static void SetDBpath(string p)
        {
            global::NumberDumber.Properties.Settings.Default.DbPath = p;

        }

        internal static void SetCookie(string p)
        {
            global::NumberDumber.Properties.Settings.Default.Cookie = p;
            global::NumberDumber.Properties.Settings.Default.Save();


        }

        internal static void SetUserAgent(string p)
        {
            global::NumberDumber.Properties.Settings.Default.UserAgent = p;
        }

        internal static void setLastDumbed(string p)
        {
            global::NumberDumber.Properties.Settings.Default.LastDumbed = p;
            global::NumberDumber.Properties.Settings.Default.Save();

        }

        internal static string getlastDumbed()
        {
            return global::NumberDumber.Properties.Settings.Default.LastDumbed;
        }

        internal static void setSessionExpired(bool p)
        {
            global::NumberDumber.Properties.Settings.Default.SessionExpired = p;
            global::NumberDumber.Properties.Settings.Default.Save();

        }

        internal static bool GetsessionExpired()
        {
            return global::NumberDumber.Properties.Settings.Default.SessionExpired;
        }

        internal static void SetLastPrefix(string p)
        {
            global::NumberDumber.Properties.Settings.Default.LastPrefix = p;
            global::NumberDumber.Properties.Settings.Default.Save();
        }

        internal static void setLastRangeFrom(decimal p)
        {
            try
            {
                global::NumberDumber.Properties.Settings.Default.LastRangeFrom = int.Parse(p.ToString());
                global::NumberDumber.Properties.Settings.Default.Save();
            }
            catch { }

        }

        internal static void setLastRangeTo(decimal p)
        {

            try
            {
                global::NumberDumber.Properties.Settings.Default.LastRangeTo = int.Parse(p.ToString());
                global::NumberDumber.Properties.Settings.Default.Save();
            }
            catch { }
        }

        internal static string GetLastPrefix()
        {
            return global::NumberDumber.Properties.Settings.Default.LastPrefix;
        }

        internal static decimal GetLastRangeTo()
        {
            return global::NumberDumber.Properties.Settings.Default.LastRangeTo;

        }

        internal static decimal GetLastRangeFrom()
        {
            return global::NumberDumber.Properties.Settings.Default.LastRangeFrom;

        }

        internal static decimal GetLastMobileNumberLength()
        {
            try
            {
                return decimal.Parse(global::NumberDumber.Properties.Settings.Default.LastLength.ToString());
            }
            catch { return 11; }
        }

        internal static void SetLastSearchedName(string p)
        {
            try
            {
                global::NumberDumber.Properties.Settings.Default.LastSearchedName = p;//s int.Parse(p.ToString());
                global::NumberDumber.Properties.Settings.Default.Save();
            }
            catch { }
        }

        internal static void setLastSearchedNumber(string p)
        {
            try
            {
                global::NumberDumber.Properties.Settings.Default.LastSearchedNumber = p;//s int.Parse(p.ToString());
                global::NumberDumber.Properties.Settings.Default.Save();
            }
            catch { }
        }

        internal static void SetLastSelectedCountryIndex(int p)
        {
            try
            {
                global::NumberDumber.Properties.Settings.Default.LastCountryIndex = p;//s int.Parse(p.ToString());
                global::NumberDumber.Properties.Settings.Default.Save();
            }
            catch { }
        }

        internal static string GetLastSearchedName()
        {
            return global::NumberDumber.Properties.Settings.Default.LastSearchedName;
        }
        internal static string GetLastSearchedNumber()
        {
            return global::NumberDumber.Properties.Settings.Default.LastSearchedNumber;
        }

        internal static int GetLastSelectedCountryIndex()
        {
            return global::NumberDumber.Properties.Settings.Default.LastCountryIndex;

        }

        internal static int LastTabIndex()
        {
            return global::NumberDumber.Properties.Settings.Default.LastTabIndex;

        }
        internal static void LastTabIndex(int p)
        {
             global::NumberDumber.Properties.Settings.Default.LastTabIndex=p;
             global::NumberDumber.Properties.Settings.Default.Save();// = p;


        }


        internal static bool AutoLoadImageFromWeb()
        {

            return global::NumberDumber.Properties.Settings.Default.LoadImageFromWeb;

        }
        internal static void AutoLoadImageFromWeb(bool p)
        {

             global::NumberDumber.Properties.Settings.Default.LoadImageFromWeb=p;
            global::NumberDumber.Properties.Settings.Default.Save();

        }
    }
}
