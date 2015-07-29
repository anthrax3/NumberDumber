using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace NumberDumber
{
   public class DumbedNumber
    {
      public  string name = "unknown";
      public string number = "0";
      public string CountryAbb = "eg";
      public bool TrueCalled_Stored = false;
      public bool Checked_OntrueCaller = false;
      
      public bool Image_Loaded = false;
      private bool HasImage = false;
      public string ImageLink = "";
      public NumberError ErrorType;
        public DumbedNumber(string number="0")
        {
            this.number = "0";
            this.CreationTime = DateTime.Now.ToString();
        }

        public DumbedNumber(bool p)
        {
            this.name = this.number = this.ImageLink = this.CreationTime = "";
        }

        /// <summary>
        /// Check if athe number is on truecaller DB
        /// and return it 
        /// </summary>
        public void TrueCallerInfo()
        {
            try
            {
                this.ErrorType = NumberError.none;
                var request = (HttpWebRequest)WebRequest.Create("http://truecaller.com/" + CountryAbb + "/" + number);
                request.UserAgent = settings.GetUserAgent();
                request.Accept = settings.GetAccept();
                request.Headers.Add("Accept-Language", "en-US,en;q=0.5");
                request.Headers.Add("cookie", settings.GetCookie());
                request.AllowAutoRedirect = false;

                var respons = (HttpWebResponse)request.GetResponse();
                int resp_code = ((int)respons.StatusCode);

                string docmnt = new StreamReader(respons.GetResponseStream()).ReadToEnd();
                try { settings.setLastDumbed(this.number); } catch { }                
                WebHeaderCollection __respHEADERScollection = respons.Headers;
                for (int i = 0; i < __respHEADERScollection.Count; i++)
                {

                    String header__ = __respHEADERScollection.GetKey(i);
                    if (header__.ToLower() != "location")
                        continue;

                    String[] values = __respHEADERScollection.GetValues(header__);
                    if (values.Length > 0)
                    {
                        this.SetSessionExpired(true);
                        this.Checked_OntrueCaller = true;
                        this.ErrorType = NumberError.SessionError;
                        this.HasError = true;
                        return;
                    }
                }
                this.HasError = true;
                if(docmnt.Contains("You need to sign in to view the result"))
                {
                    this.ErrorType = NumberError.SessionError;
                     return;
                }
                if(docmnt.Contains("<p>The document has moved <a")||docmnt.Contains("Redirecting to <a href=\"http://www.truecaller.com\">http://www.truecaller.com</a>")||docmnt.Contains("You have excedeed your search limit. Please try again in 12 hours "))
                {
                    this.ErrorType = NumberError.SessionError;
                     return;
                }
                bool IfoundThename = false, IfoundTheimage=false,Finished=false;
                string[] sepd = docmnt.Split(new string[] { "<" }, StringSplitOptions.RemoveEmptyEntries);
                  if (docmnt.Contains("The truth is out there"))
                    {
                        this.ErrorType = NumberError.NotOnTrueCaller;
                        this.HasError = false;
                        return;

                    }
                  else if (docmnt.Contains(">You need to sign in to view the result"))
                  {
                      this.SetSessionExpired(true);
                      this.ErrorType = NumberError.SessionError;
                      this.Checked_OntrueCaller = false;
                      return;
                  }
                
                    
                  foreach (string s in sepd)
                  { // check if number has name
                      if (s.Contains("class=\"result__name") && !s.Contains("The truth is out there") && !IfoundThename)
                      {
                          Checked_OntrueCaller = true;
                          this.TrueCalled_Stored = true;
                          string[] rps = s.Split(new string[] { ">", "<" }, StringSplitOptions.RemoveEmptyEntries);
                          this.name = rps[1];
                          IfoundThename = this.TrueCalled_Stored=true;
                          this.ErrorType = NumberError.none;
                          this.HasError = false;
                      }

                      // check if number has image
                      else if (!IfoundTheimage && s.Contains("img width=\"128px\" height=\"128px\"") && s.Contains("truecaller.com/myview/"))
                      {
                          HasImage = true;
                          string[] sepdbyqout = s.Split(new char[] { '"' });
                          if (sepdbyqout.Length > 5)
                              this.ImageLink = sepdbyqout[5];
                          IfoundTheimage = true;
                      }
                      else if ((IfoundThename && IfoundTheimage) || Finished)
                          break;
                  }

            }
            catch(Exception x) 
            {
                TrueCalled_Stored = false;
                this.HasError = true;
                if (x.Message.Contains("The remote name could not be resolved"))
                    this.ErrorType = NumberError.InternetError;
                }
        }

        private void SetSessionExpired(bool p)
        {
            settings.setSessionExpired(p);
        }

        public string ToString(string space = "    ")
        {
            if (this.HasError)
            {
                return ErrorType.ToString();
            }
            else
            {
                if (space.Trim() == "")
                    space = " ® ";

            } return "Number:" + number + space + "Country:" + DumbedNumber.RealCountry(this.CountryAbb) + space + GetTrueCallerStored() + space + GetImageStatue() + space + "Time:" + this.CreationTime;
        }

       public string RealCountry()
        {
            return DumbedNumber.RealCountry(this.CountryAbb);
        }
        public static string RealCountry(string p)
        {
            try
            {
                for (int i = 0; i < DumbedNumber.abbs.Length; i++)
                    if (DumbedNumber.abbs[i] == p)
                        return DumbedNumber.Countries[i];
                return p;
            }
            catch { return p; }

        }

        private string GetImageStatue()
        {
            if (HasImage)
                return "Has image at :" + ImageLink;
            else
                return "Has no image";
        }

        

        private string GetTrueCallerStored()
        {
            if (this.TrueCalled_Stored || name.Trim()!="unknown")
                return "Name:"+name;
            return "Not  on truecaller";
            
        }

        internal bool Has_Image()
        {
            try{
            return this.HasImage && ImageLink.Length > 1;
        }
        catch { return false; }
        }

        internal bool Tostore()
        { 
            return this.ErrorType == NumberError.none || ErrorType == NumberError.NotOnTrueCaller;
        }

        internal static DumbedNumber Build(string line)
        {
            DumbedNumber d = new DumbedNumber(true);
            if (line.Contains("®") == false)
            {
                if (line.Contains(":"))
                {
                    string[] pcs = line.Split(new char[] { ':' });
                    d.number = pcs[1];

                }
                else return null;
            }
            else
            {
                string[] spdByPeace = line.Split(new char[] { '®' });
                
                string numbandval =spdByPeace[0];
                if(numbandval.Contains(":"))
                    d.number = numbandval.Split(new char[] { ':' })[1];



                string countru_and_val = spdByPeace[1];
                if (countru_and_val.Contains(":"))
                    d.CountryAbb = DumbedNumber.ValidAbb (countru_and_val.Split(new char[] { ':' })[1]);
                


                if(spdByPeace.Length<3)
                    return d;
                string  nameandVal = spdByPeace[2];
                if(nameandVal.Contains(":"))
                    d.name = nameandVal.Split(new char[] { ':' })[1];

                if (spdByPeace.Length < 4)
                    return d;
                
                string imagandlink = spdByPeace[3];
                if (imagandlink.Contains(":"))
                {
                    if (imagandlink.Contains("Has no ima") == false)
                    {  // has image at:http://ask.com
                        string [] tmp =imagandlink.Split(new char [] {':'});
                        for (int i = 1; i < tmp.Length; i++)
                            d.ImageLink += tmp[i] + ":";
                        d.ImageLink = d.ImageLink.Substring(0, d.ImageLink.Length - 1);

                    }
                    if (d.ImageLink.IsUrl())
                        d.HasImage = true;
                }


                if (spdByPeace.Length < 5)
                    return d;
                string timeandVal = spdByPeace[4];
                if (timeandVal.Contains(":"))
                    d.CreationTime = timeandVal.Split(new char[] { ':' })[1];


            }

            return d;
            
        }

        private static string ValidAbb(string p)
        {
            try
            {
                p = p.Trim();
                foreach (string s in DumbedNumber.abbs)
                    if (s == p)
                        return s;
                for (int i = 0; i < DumbedNumber.Countries.Length; i++)
                    if (DumbedNumber.Countries[i] == p)
                        return DumbedNumber.abbs[i];

                return p;
            }
            catch { return p; }
        }

        internal bool Matches(string p)
        {
            if (this == null)
                return false;
            string pl=p.ToLower();
            if (this.name.ToLower().Contains(pl))
                return true;
            if (this.number.ToLower().Contains(pl))
                return true;
            if (this.CountryAbb.ToLower().Contains(pl))
                return true;
            return false;
        }

       public static string[] abbs = new string[] { "af", "al", "dz", "as", "ad", "ao", "ai", "ag", "ar", "am", "aw", "au", "at", "az", "bs", "bh", "bd", "bb", "by", "be", "bz", "bj", "bm", "bt", "bo", "ba", "bw", "br", "vg", "bn", "bg", "bf", "mm", "bi", "kh", "cm", "ca", "cv", "ky", "cf", "td", "cl", "cn", "cx", "co", "km", "cg", "ck", "cr", "hr", "cu", "cw", "cy", "cz", "dk", "dj", "dm", "do", "tl", "ec", "eg", "sv", "gq", "er", "ee", "et", "fo", "fj", "fi", "fr", "gf", "pf", "ga", "gm", "ge", "de", "gh", "gi", "gr", "gl", "gd", "gp", "gu", "gt", "gn", "gw", "gy", "ht", "hn", "hk", "hu", "is", "in", "id", "ir", "iq", "ie", "il", "it", "ci", "jm", "jp", "jo", "kz", "ke", "ki", "kw", "kg", "la", "lv", "lb", "ls", "lr", "ly", "li", "lt", "lu", "mo", "mk", "mg", "mw", "my", "mv", "ml", "mt", "mh", "mq", "mr", "mu", "yt", "mx", "md", "mc", "mn", "me", "ms", "ma", "mz", "na", "nr", "np", "nl", "nc", "nz", "ni", "ne", "ng", "nu", "nf", "mp", "kp", "no", "om", "pk", "pw", "ps", "pa", "pg", "py", "pe", "ph", "pn", "pl", "pt", "pr", "qa", "re", "ro", "ru", "rw", "sh", "kn", "lc", "mf", "pm", "ws", "sm", "st", "sa", "sn", "rs", "sc", "sl", "sg", "sk", "si", "sb", "so", "za", "kr", "ss", "es", "lk", "sd", "sr", "sz", "se", "ch", "sy", "tw", "tj", "tz", "th", "tg", "tk", "to", "tt", "tn", "tr", "tm", "tc", "tv", "ug", "gb", "ua", "ae", "uy", "us", "uz", "vu", "ve", "vn", "vi", "wf", "ye", "zm", "zw" };
       public static string[] Countries = new string[] { "Afghanistan ", "Albania ", "Algeria ", "American Samoa ", "Andorra ", "Angola ", "Anguilla ", "Antigua and Barbuda ", "Argentina-Buenos Aires ", "Armenia ", "Aruba ", "Australia ", "Austria ", "Azerbaijan ", "Bahamas ", "Bahrain ", "Bangladesh ", "Barbados ", "Belarus ", "Belgium ", "Belize ", "Benin ", "Bermuda ", "Bhutan ", "Bolivia ", "Bosnia and Herzegovina ", "Botswana ", "Brazil ", "British Virgin Islands ", "Brunei ", "Bulgaria ", "Burkina Faso ", "Burma-Myanmar ", "Burundi ", "Cambodja ", "Cameroon ", "Canada ", "Cape Verde ", "Cayman Islands ", "Central African Republic ", "Chad ", "Chile ", "China ", "Christmas Island ", "Colombia ", "Comoros ", "Congo ", "Cook Islands ", "Costa Rica ", "Croatia ", "Cuba ", "Curaçao ", "Cyprus ", "Czech Republic ", "Denmark ", "Djibouti ", "Dominica ", "Dominican Republic ", "East Timor ", "Ecuador ", "Egypt ", "El Salvador ", "Equatorial Guinea ", "Eritrea ", "Estonia ", "Ethiopia ", "Faroe Islands ", "Fiji ", "Finland ", "France ", "French Guiana ", "French Polynesia ", "Gabon ", "Gambia ", "Georgia ", "Germany ", "Ghana ", "Gibraltar ", "Greece ", "Greenland ", "Grenada ", "Guadeloupe ", "Guam ", "Guatemala ", "Guinea ", "Guinea-Bissau ", "Guyana ", "Haiti ", "Honduras ", "Hong Kong ", "Hungary ", "Iceland ", "India ", "Indonesia ", "Iran ", "Iraq ", "Ireland ", "Israel ", "Italy ", "Ivory Coast ", "Jamaica ", "Japan ", "Jordan ", "Kazakhstan ", "Kenya ", "Kiribati ", "Kuwait ", "Kyrgyzstan ", "Laos ", "Latvia ", "Lebanon ", "Lesotho ", "Liberia ", "Libya ", "Liechtenstein ", "Lithuania ", "Luxembourg ", "Macau ", "Macedonia ", "Madagascar ", "Malawi ", "Malaysia ", "Maldives ", "Mali ", "Malta ", "Marshall Islands ", "Martinique ", "Mauritania ", "Mauritius ", "Mayotte ", "Mexico ", "Moldova ", "Monaco ", "Mongolia ", "Montenegro ", "Montserrat ", "Morocco ", "Mozambique ", "Namibia ", "Nauru ", "Nepal ", "Netherlands ", "New Caledonia ", "New Zealand ", "Nicaragua ", "Niger ", "Nigeria ", "Niue ", "Norfolk Island ", "Northern Mariana Islands ", "North Korea ", "Norway ", "Oman ", "Pakistan ", "Palau ", "Palestine ", "Panama ", "Papua New Guinea ", "Paraguay ", "Peru ", "Philippines ", "Pitcairn Islands ", "Poland ", "Portugal ", "Puerto Rico ", "Qatar ", "Réunion ", "Romania ", "Russia ", "Rwanda ", "Saint Helena ", "Saint Kitts and Nevis ", "Saint Lucia ", "Saint Martin ", "Saint Pierre and Miquelon ", "Samoa ", "San Marino ", "São Tomé and Príncipe ", "Saudi Arabia ", "Senegal ", "Serbia ", "Seychelles ", "Sierra Leone ", "Singapore ", "Slovakia ", "Slovenia ", "Solomon Islands ", "Somalia ", "South Africa ", "South Korea ", "South Sudan ", "Spain ", "Sri Lanka ", "Sudan ", "Suriname ", "Swaziland ", "Sweden ", "Switzerland ", "Syria ", "Taiwan ", "Tajikistan ", "Tanzania ", "Thailand ", "Togo ", "Tokelau ", "Tonga ", "Trinidad and Tobago ", "Tunisia ", "Turkey ", "Turkmenistan ", "Turks and Caicos Islands ", "Tuvalu ", "Uganda ", "United Kingdom ", "Ukraine ", "United Arab Emirates ", "Uruguay ", "United States ", "Uzbekistan ", "Vanuatu ", "Venezuela ", "Vietnam ", "Virgin Islands ", "Wallis and Futuna ", "Yemen ", "Zambia ", "Zimbabwe " };
       public static string[] Keys = new string[] { "(+93)", "(+355)", "(+213)", "(+1684)", "(+376)", "(+244)", "(+1264)", "(+1268)", "(+54)", "(+374)", "(+297)", "(+61)", "(+43)", "(+994)", "(+1242)", "(+973)", "(+880)", "(+1246)", "(+375)", "(+32)", "(+501)", "(+229)", "(+1441)", "(+975)", "(+591)", "(+387)", "(+267)", "(+55)", "(+1284)", "(+673)", "(+359)", "(+226)", "(+95)", "(+257)", "(+855)", "(+237)", "(+1)", "(+238)", "(+1345)", "(+236)", "(+235)", "(+56)", "(+86)", "(+6189)", "(+57)", "(+269)", "(+242)", "(+682)", "(+506)", "(+385)", "(+53)", "(+599)", "(+357)", "(+420)", "(+45)", "(+253)", "(+1767)", "(+1809)", "(+670)", "(+593)", "(+20)", "(+503)", "(+240)", "(+291)", "(+372)", "(+251)", "(+298)", "(+679)", "(+358)", "(+33)", "(+594)", "(+689)", "(+241)", "(+220)", "(+995)", "(+49)", "(+233)", "(+350)", "(+30)", "(+299)", "(+1473)", "(+590)", "(+1671)", "(+502)", "(+224)", "(+245)", "(+592)", "(+509)", "(+504)", "(+852)", "(+36)", "(+354)", "(+91)", "(+62)", "(+98)", "(+964)", "(+353)", "(+972)", "(+39)", "(+225)", "(+1876)", "(+81)", "(+962)", "(+7)", "(+254)", "(+686)", "(+965)", "(+996)", "(+856)", "(+371)", "(+961)", "(+266)", "(+231)", "(+218)", "(+423)", "(+370)", "(+352)", "(+853)", "(+389)", "(+261)", "(+265)", "(+60)", "(+960)", "(+223)", "(+356)", "(+692)", "(+596)", "(+222)", "(+230)", "(+262)", "(+52)", "(+373)", "(+377)", "(+976)", "(+382)", "(+1664)", "(+212)", "(+258)", "(+264)", "(+674)", "(+977)", "(+31)", "(+687)", "(+64)", "(+505)", "(+227)", "(+234)", "(+683)", "(+672)", "(+1670)", "(+850)", "(+47)", "(+968)", "(+92)", "(+680)", "(+970)", "(+507)", "(+675)", "(+595)", "(+51)", "(+63)", "(+870)", "(+48)", "(+351)", "(+1787)", "(+974)", "(+262)", "(+40)", "(+7)", "(+250)", "(+290)", "(+1869)", "(+1758)", "(+1599)", "(+508)", "(+685)", "(+378)", "(+239)", "(+966)", "(+221)", "(+381)", "(+248)", "(+232)", "(+65)", "(+421)", "(+386)", "(+677)", "(+252)", "(+27)", "(+82)", "(+211)", "(+34)", "(+94)", "(+249)", "(+597)", "(+268)", "(+46)", "(+41)", "(+963)", "(+886)", "(+992)", "(+255)", "(+66)", "(+228)", "(+690)", "(+676)", "(+1868)", "(+216)", "(+90)", "(+993)", "(+1649)", "(+688)", "(+256)", "(+44)", "(+380)", "(+971)", "(+598)", "(+1)", "(+998)", "(+678)", "(+58)", "(+84)", "(+1340)", "(+681)", "(+967)", "(+260)", "(+263)" };
       private bool p;
       internal static string Abb(int p)
        {
            try
            {
                return abbs[p];
            }
            catch { return ""; }
           
        }

        public bool HasError { get; set; }

        public string CreationTime { get; set; }

        internal bool isValid()
        {
            return this.number.Trim() != "";
        }
    }
   public enum NumberError { InternetError, WrongFormat, NotOnTrueCaller, none, SessionError }
}
