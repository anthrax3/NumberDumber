using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace NumberDumber
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void Check_Click(object sender, EventArgs e)
        {
            gpxInfo.Visible = false;
            try
            {
                try
                {
                    this.PICBXnumber.Image = global::NumberDumber.Properties.Resources.defpic;
                }
                catch { }

                /*//////*/
                lblNumberError.Visible = false;
                /*//////*/
                DumbedNumber d = new DumbedNumber();
                d.number = txbxNumber.Text.IntOnly();
                d.CountryAbb = DumbedNumber.Abb(cmbx_Check_countryAbb.SelectedIndex);
                d.TrueCallerInfo();
                /*//////*/
                rtxbxInfo_Of_number.Text = d.ToString(Environment.NewLine + Environment.NewLine + "-");
               if(d.ErrorType==NumberError.SessionError)
                rtxbxInfo_Of_number.Text += "\r\nPlease restart the session \r\n add cookie \r\n or \r\n log out and log in again ";
                
                gpxInfo.Text = d.name + "`s Info";
                gpxInfo.Visible = true;
                PICBXnumber.Image = DefaultImage();
                if (d.isValid() && onList(d)==false)
                    this.Dumbeds.Add(d);
                /*//////*/
                string ip = Application.StartupPath + "\\imgs\\" + d.CountryAbb.Trim() + "_" + d.number.Trim() + ".jpg";
                if (d.Has_Image())
                {
                    try
                    {
                        if (File.Exists(ip))
                            this.PICBXnumber.Image = Image.FromFile(ip);
                        else if (chkbx_name_loadOnline.Checked)
                        {
                            this.PICBXnumber.Load(d.ImageLink);
                            this.PICBXnumber.Image.Save(ip);
                        }

                    }
                    catch
                    {

                    }

                }
                /*//////*/
                dataBasemanager.AddNew(d);

                lblNumberError.Visible = d.HasError;
                lnkLblPlzSignIn.Visible = (d.ErrorType == NumberError.SessionError);
        
                gpxInfo.Visible=true;
            }
            catch {   }
        }

        private Image DefaultImage()
        {
            try
            {
                string blankPath = @"C:\Users\YasserGersy\Desktop\blank.bmp";//Application.StartupPath+"\\blank.bmp");
                Bitmap myBitmap = new Bitmap(blankPath);
                Graphics g = Graphics.FromImage(myBitmap);

                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                g.DrawString("My\nText",
                             new Font("Tahoma", 20),
                             Brushes.White,
                             new PointF(0, 0));

                StringFormat strFormat = new StringFormat();
                strFormat.Alignment = StringAlignment.Center;
                strFormat.LineAlignment = StringAlignment.Center;

                g.DrawString("My\nText",
                             new Font("Tahoma", 20), Brushes.White,
                             new RectangleF(0, 0, 500, 500),
                             strFormat);
                string curent_user_image = Application.StartupPath + "\\cu.bmp";
                return myBitmap;
                myBitmap.Save(curent_user_image);
                return Image.FromFile(curent_user_image);
            }catch{return null;}
        }

        private void txbxNumber_TextChanged(object sender, EventArgs e)
        {
            settings.setLastSearchedNumber(txbxNumber.Text);
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
        List<DumbedNumber> SearchResult = new List<DumbedNumber>();
        private void Search_Clicked(object sender, EventArgs e)
        {
            txbxLocalName.Text = txbxLocalName.Text.Trim();
            this.FirstSearched = true;
            this.SearchResult.Clear();
            lstbxLocal.Items.Clear();
            this.dataGridView1.Rows.Clear();


            if (this.ListReady() == false)
                return;

            int ok = 0;
            this.SearchList.Clear();
            foreach (DumbedNumber d in this.Dumbeds)
            {
                

                if (d.name.Contains(txbxLocalName.Text) == false )
                    continue;

                if (checkBoxHasImage.Checked && d.Has_Image() == false)
                    continue;

                if (chkbx_2_validName.Checked && d.name.Trim() == "")
                    continue;
                ok++;
                this.SearchList.Add(d);
                this.lstbxLocal.Items.Add(d.name);
                this.dataGridView1.Rows.Add(d.name,d.number,d.RealCountry());
            }
            lstbxLocal.Visible = true;
            btnViewLocal.Visible = btnViewLocal.Enabled = true;
            if(lstbxLocal.Items.Count==1)
            {
                lstbxLocal.SelectedIndex= 0;
                btnViewLocal.PerformClick();
            }

                gpxResult.Text = ((this.Dumbeds.Count== ok))?"All Contacts":"Contacts";
                gpxResult.Text += "= "+lstbxLocal.Items.Count.ToString();

        }

        private bool ListReady()
        {
            if (this.Dumbeds == null)
                return false;
            if (this.Dumbeds.Count < 1)
                return false;
            return true;
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            ReloadSettings();
        }

        private void ReloadSettings()
        {
            textBoxUserAgent.Text = settings.GetUserAgent();
            textBoxDBPath.Text = settings.GetDbPath();
            textBoxCookie.Text = settings.GetCookie();
            SetLastDumbed(settings.getlastDumbed());
            txbx_Dumber_prefix.Text = settings.GetLastPrefix();
            NumFrom.Value = settings.GetLastRangeFrom();
            NumTo.Value = settings.GetLastRangeTo();
            numUD_length.Value = settings.GetLastMobileNumberLength();
            txbxLocalName.Text = settings.GetLastSearchedName();
            txbxNumber.Text = settings.GetLastSearchedNumber();
            cmbx_Check_countryAbb.SelectedIndex = settings.GetLastSelectedCountryIndex();
            chkbx_name_loadOnline.Checked =checkBoxAutoLoadImages.Checked= settings.AutoLoadImageFromWeb();
            int x = dataBasemanager.GetContactsCount();
            if (x > -1)
                lblContactsCount.Text = "Contacts=" + x.ToString();
        }

        private void SetLastDumbed(string p)
        {
            lblLastDumbed.Text = "Last Dumbed "+p;
            lblLastDumbed.Visible=!(p.ToLower().Trim() == "");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            settings.SetCookie(textBoxCookie.Text);
            settings.SetUserAgent(textBoxUserAgent.Text);
            settings.Save();            
            ReloadSettings();
        }

        private void textBoxUserAgent_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxCookie_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxDBPath_TextChanged(object sender, EventArgs e)
        {
            settings.SetDBpath(textBoxDBPath.Text);
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            LoadDmbedBefore();
            this.MinimumSize = this.MaximumSize = this.Size;
            cmbx_Check_countryAbb.SelectedIndex=60;
            settings.setSessionExpired(false);
            ReloadSettings();
            try
            {
                this.tabControl1.SelectedIndex = settings.LastTabIndex();
            }
            catch { }

            Search_Clicked(null, null);
        }

        public List<DumbedNumber> Dumbeds = new List<DumbedNumber>();
        private void LoadDmbedBefore()
        {
            string[] lines = File.ReadAllLines(Program.ReturnLocalFile());
            this.Dumbeds = new List<DumbedNumber>();
            if (lines == null)
                return;
            foreach (string line in lines)
            {

                DumbedNumber d = DumbedNumber.Build(line);
                if (d.isValid() == false)
                    continue;
                this.Dumbeds.Add(d);
                this.SearchResult.Add(d);
            }
        }

        private void btnBrwsDb_Click(object sender, EventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog();
            if (o.ShowDialog() == DialogResult.OK)
                textBoxDBPath.Text = o.FileName;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 2)
                ReloadSettings();
            settings.LastTabIndex(tabControl1.SelectedIndex);
        }

        private void btnDumb_Click(object sender, EventArgs e)
        {
            int searched = 0;
            int hasImage = 0;
            int skiped = 0;
            rtxbxDumber.Text += " Starting ..... ";
            string prefix = txbx_Dumber_prefix.Text;
            int  Num_length = int.Parse(numUD_length.Value.ToString());
            
            int start = int.Parse(NumFrom.Value.ToString());
            int end = int.Parse(NumTo.Value.ToString());
            for(int  i=start;i<=end;i++)
            {
                string num = prefix + LeftZaeros((i), Num_length-prefix.Length );
                if (!settings.GetsessionExpired())
                {

                    DumbedNumber d = new DumbedNumber();
                    d.number = num;
                    if (this.onList(d) && this.ckbx_3_skipDumbedBefore.Checked)
                    {
                        skiped++;
                        continue;
                    }
                       d.TrueCallerInfo();

                    
                    if (dataBasemanager.NumberExisted(d.number) == false && d.Tostore())
                        dataBasemanager.AddNew(d);

                    if (d.Has_Image())
                        hasImage++;
                    searched++;
                    rtxbxDumber.Text += (Environment.NewLine + num + ": Dumbed at" + DateTime.Now.ToShortTimeString());

                    if (d.HasError)
                    {
                        rtxbxDumber.Text += Environment.NewLine + "     Error " + d.ErrorType.ToString();
                        return;
                    }
                    NumFrom.Value++;
                }
            }

            lbl_Dumber_Result_HasImage.Text = "With Images="+hasImage.ToString();
            lbl_Dumber_Result_Searched.Text = "Searched="+searched.ToString();
            lblSkipped.Text = "Skipped=" + skiped;
        }

        private bool onList(DumbedNumber d)
        {
            if (d == null)
                return false;

            if (this.Dumbeds == null)
                return false;
            if (this.Dumbeds.Count < 1)
                return false;
            foreach (DumbedNumber dn in this.Dumbeds)
                if (d.number == dn.number)
                    return true;

            return false;
        }

        private bool DumbNumber(string num)
        {
            try
            {

                DumbedNumber d = new DumbedNumber();
                d.number = num;
                d.TrueCallerInfo();

                if (dataBasemanager.NumberExisted(d.number) == false && d.Tostore())
                    dataBasemanager.AddNew(d);
             
                return !d.HasError;// false;
            }
            catch { return false; }
        }

        private string LeftZaeros(decimal num, int length)
        {  /// 85  , 4
            string z = "";
            for (int j = 0; j < (length - num.ToString().Length); j++)
                z += "0";
            return (z + num.ToString());
        }

        private string LeftZaeros(decimal num, decimal length)
        {  /// 85  , 4
            string z = "";
            for (int j = 0; j < (length - num.ToString().Length); j++)
                z += "0";
            return (z + num.ToString());
        }

        private void NumFrom_ValueChanged(object sender, EventArgs e)
        {
            settings.setLastRangeFrom(NumFrom.Value);
            this.NumTo.Minimum = this.NumFrom.Value;
            CreateExample();

        }





        private void btnView_Click(object sender, EventArgs e)
        {
            ViewSearchedLocal();
            
        }

        private void ViewSearchedLocal()
        {

            int SelectedIndex___ = dataGridView1.CurrentCell.RowIndex;
           
            try
            {
                this.picbxLocal.Image = global::NumberDumber.Properties.Resources.defpic;
            }
            catch { }
            if (SelectedIndex___ < 0)
                return;
            if (this.lstbxLocal.SelectedIndex >= this.Dumbeds.Count)
                return;

            DumbedNumber d = this.SearchList[SelectedIndex___];
            chkbx_name_loadOnline.Visible = d.Has_Image();
            this.rtbxLocal.Text = d.ToString(Environment.NewLine + Environment.NewLine + "-");
            string img_path = Application.StartupPath + "\\imgs\\" + d.CountryAbb.Trim() + "_" + d.number.Trim() + ".jpg";
            try
            {
                chkbx_name_loadOnline.Visible = false;
                if (File.Exists(img_path))
                    this.picbxLocal.Image = Image.FromFile(img_path);
                else if (chkbx_name_loadOnline.Checked)
                {
                    this.picbxLocal.Load(d.ImageLink);
                    this.picbxLocal.Image.Save(img_path);


                }
                else chkbx_name_loadOnline.Visible = true;


            }
            catch
            {

            }
            gpx_Local_view.Visible = true;

        }
         


         
        private bool User_Image_existed(string p)
        {
            return System.IO.File.Exists(Application.StartupPath + "\\imgs\\" + p + ".jpg");
        }

        private Image ImageFromLocal(string p)
        {
            try
            {
                return Image.FromFile(Application.StartupPath + "\\imgs\\" + p + ".jpg");
            }
            catch
            {
                return null;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try { System.Diagnostics.Process.Start(Application.StartupPath + "\\db.txt"); }
            catch { }
        
        }

        private void lnklblLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new LoginFrm().ShowDialog();
            this.ReloadSettings();
            lnkLblPlzSignIn.Visible = false;
        }

        private void txbx_Dumber_prefix_TextChanged(object sender, EventArgs e)
        {
            if(TimerIsWorking==false)
                timer1.Start();

            CreateExample();

        }

        private void CreateExample()
        {
            string from = txbx_Dumber_prefix.Text;
            from = from + LeftZaeros(this.NumFrom.Value,numUD_length.Value-txbx_Dumber_prefix.TextLength);


            string ex = txbx_Dumber_prefix.Text;
            ex = ex + LeftZaeros(this.NumTo.Value, numUD_length.Value - txbx_Dumber_prefix.TextLength);


            this.rtxbxDumber.Text = "Starting Number:"+from+Environment.NewLine+"Ending  number:"+ex+Environment.NewLine+"              ............"+Environment.NewLine;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.TimerIsWorking = true;
            this.TimerTicks++;
            if(this.TimerTicks==2)
            {
                this.timer1.Stop();
                this.TimerTicks = 0;
                string x = "";
                bool InsertedAplus = false;
                foreach (char c in txbx_Dumber_prefix.Text)
                {
                    if (c == '+') 
                        InsertedAplus = true;
                           
                    if ((c >= '0' && c <= '9') || (c == '+' && !InsertedAplus))
                        x += c;
                    settings.SetLastPrefix( txbx_Dumber_prefix.Text);
                }
                txbx_Dumber_prefix.Text = x;
            }

            this.TimerIsWorking = false;
        }


        public int TimerTicks =0;

        public bool TimerIsWorking = false;

        private void NumTo_ValueChanged(object sender, EventArgs e)
        {
            settings.setLastRangeTo(NumTo.Value);
            CreateExample();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            settings.SetLastSearchedName(txbxLocalName.Text);
        }

        private void cmbx_Check_countryAbb_SelectedIndexChanged(object sender, EventArgs e)
        {
            settings.SetLastSelectedCountryIndex(cmbx_Check_countryAbb.SelectedIndex);
        }

        private void lstbxLocal_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnViewLocal.PerformClick();
        }

        private void s(object sender, EventArgs e)
        {

        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBoxUserAgent.Text = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:38.0) Gecko/20100101";
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            textBoxCookie.Text = settings.GetCookie();
        }

        private void numUD_length_ValueChanged(object sender, EventArgs e)
        {
            CreateExample();

        }

        private void lnkLblPlzSignIn_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tabControl1.SelectedIndex = 3;
            //lnklblLogin_LinkClicked(null, null);
        }

        private void rtxbxDumber_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBoxHasImage_CheckedChanged(object sender, EventArgs e)
        {
            if (this.FirstSearched)
                Search_Clicked(null, null);


        }

        public bool FirstSearched { get; set; }

        private void chkbx_name_loadOnline_CheckedChanged(object sender, EventArgs e)
        {
            btnViewLocal.PerformClick();
            settings.AutoLoadImageFromWeb(chkbx_name_loadOnline.Checked);
        }

        private void checkBoxAutoLoadImages_CheckedChanged(object sender, EventArgs e)
        {
            settings.AutoLoadImageFromWeb(checkBoxAutoLoadImages.Checked);

        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
          this.about =  new AboutFrm();//.Show();
          this.about.Show();
        }

        public AboutFrm about { get; set; }

        private void linkLabelHowTocookie_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Log in truecaller "+Environment.NewLine+" Use Tamper Data Firefox Extension to get cookie"+Environment.NewLine+" then paste it ","How to get Cookie");
        }

        private void btn_GTD_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog s = new SaveFileDialog();
                s.FileName = "tamper_data-11.0.1-fx.xpi";
                if (s.ShowDialog() == DialogResult.OK)
                {
                    if (!s.FileName.EndsWith(".xpi"))
                        s.FileName += ".xpi";

                    List<byte> l = new List<byte>();
                    foreach (int b in TamperData.TDByts_11)
                        l.Add(byte.Parse(b.ToString()));
                    File.WriteAllBytes(s.FileName, l.ToArray());
                    MessageBox.Show("Done ");

                }
            }
            catch { }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            try
            {
                SaveFileDialog s = new SaveFileDialog();
                s.FileName = "tamper_data-10.1.1-fx.xpi";
                if (s.ShowDialog() == DialogResult.OK)
                {
                    if (!s.FileName.EndsWith(".xpi"))
                        s.FileName += ".xpi";

                    List<byte> l = new List<byte>();
                    foreach (int b in TamperData.TDByts_10)
                        l.Add(byte.Parse(b.ToString()));
                    File.WriteAllBytes(s.FileName, l.ToArray());
                    MessageBox.Show("Done ");

                }
            }
            catch { }
        }

        private void chkbx_2_validName_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            ViewSearchedLocal();
        }

        public List<DumbedNumber> SearchList = new List<DumbedNumber>();

        private void rtxbxInfo_Of_number_TextChanged(object sender, EventArgs e)
        {
             
        }

        private void lnklblCokieMngr_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CookieFrm n = new CookieFrm(textBoxCookie.Text);
            if(n.ShowDialog()==DialogResult.OK)
                settings.SetCookie(n.CookieString);

            textBoxCookie.Text = settings.GetCookie();
        }
    }
}
