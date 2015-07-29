using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace NumberDumber
{
    class dataBasemanager
    {
        
        internal static bool NumberExisted(string s )
        {


            try
            {
                return File.ReadAllText(settings.GetDbPath()).Contains(s);
            }
            catch { return false; }
             
        }

        internal static void AddNew(DumbedNumber dn)
        {
           
            try
            {

                if (dataBasemanager.NumberExisted(dn.number) || dn==null)
                    return;
            }
            catch { }
            try
            {
                string d = "";
                if (File.Exists(settings.GetDbPath()))
                    d += File.ReadAllText(settings.GetDbPath());

                d += (Environment.NewLine + dn.ToString("     "));
                File.WriteAllText(settings.GetDbPath(), d);
            }
            catch { }
                
        }

        internal static int GetContactsCount()
        {

            try
            {
                return File.ReadAllLines(settings.GetDbPath()).Length;
            }
            catch { return 0; }
        }
    }
}
