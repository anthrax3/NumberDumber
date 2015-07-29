using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NumberDumber
{
    public partial class CookieFrm : Form
    {
        public CookieFrm(string x ="")
        {
            InitializeComponent();
            this.CookieString = x;
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void CookieFrm_Load(object sender, EventArgs e)
        {
              SerializeMRX();
             DisplayElements();

             this.MinimumSize = this.Size;
             this.MaximumSize = new Size(this.Size.Width, 5000);
          }

        private void SerializeMRX()
        {
            this.lst = new List<Cookie2>();
            if (CookieString.Trim() != "")
            {
                /// modes 
                /// cokname
                /// cokname=cokval
                /// cokname1=cokval1 ; cokname2=cokval2
                if (!CookieString.Contains('=') && !CookieString.Contains(';'))
                    lst.Add(new Cookie2(CookieString, ""));
                else if (CookieString.Contains('=') && !CookieString.Contains(';'))
                {
                    string[] candv = CookieString.Split(new char[] { '=' });
                    lst.Add(new Cookie2(candv[0], candv[1]));
                }
                else if (CookieString.Contains('=') && CookieString.Contains(';'))
                {
                    string[] coks = CookieString.Split(new char[] { ';' });
                    foreach (string pair in coks)
                    {
                        /// modes 
                        /// cokname
                        /// cokname=cokval
                        if (!pair.Contains('='))
                            lst.Add(new Cookie2(pair, ""));
                        else if (pair.Contains('='))
                        {
                            string[] c_and_v = pair.Split(new char[] { '=' });
                            if (c_and_v[0].Trim() != "")
                                lst.Add(new Cookie2(c_and_v[0], c_and_v[1]));
                        }


                    }
                }


            }
        }

        private void DisplayElements()
        {
            RemoveELements(); 

            this._ChildTabIndex = 1;

            this._foreach_counter = 0;

            foreach (Cookie2 coka in lst)
            {
                 

                TextBox t_name = new TextBox();
                t_name.Name = "Name_"+_foreach_counter;
                t_name.Text = coka.cokName;
                t_name.Location = new Point( 12, (24 + (26 * _foreach_counter)));
                t_name.TabIndex = _ChildTabIndex; _ChildTabIndex++;
                t_name.Size = new Size(150, 20);
                t_name.TextChanged += new EventHandler(this.TextBoxeses_TextChanged);
                this.panel1.Controls.Add(t_name);

                TextBox t_val = new TextBox();
                t_val.Name = "Value_" + _foreach_counter;
                t_val.Text = coka.cokValue;
                t_val.Location = new Point( 168, (24 + (26 * _foreach_counter)));
                t_val.TabIndex = _ChildTabIndex; _ChildTabIndex++;
                t_val.Size = new Size(250, 20);
                t_val.TextChanged += new EventHandler(this.TextBoxeses_TextChanged);
                this.panel1.Controls.Add(t_val);


                if(_foreach_counter %2==1)
                t_name.BackColor=t_val.BackColor=Color.FromKnownColor(KnownColor.ControlDarkDark);
                LinkLabel l_close = new LinkLabel();
                l_close.AutoSize = true;
                l_close.LinkColor = System.Drawing.Color.Red;
                l_close.Location = new System.Drawing.Point(420, (24 + (26 * _foreach_counter)));
                l_close.Name = "close_" + _foreach_counter;
                l_close.Size = new System.Drawing.Size(14, 13);
                l_close.TabStop = true;
                l_close.Text = "X";
                l_close.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.CloseLinklabel_Link_Clicked);
                l_close.TabIndex = _ChildTabIndex; _ChildTabIndex++;
                this.panel1.Controls.Add(l_close);
                 

                _foreach_counter++;

            } 

        
        }

        private void BtnOk_Clicked(object sender, EventArgs e)
        {
            this.CookieString = "";
            foreach (Cookie2 c in this.lst)
                CookieString += (c.cokName + "=" + c.cokValue + ";");
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            
        }

        public string CookieString = "";

        private void CloseLinklabel_Link_Clicked(object sender, LinkLabelLinkClickedEventArgs e)
        { 
            (sender as LinkLabel).Visible = false;
             
            string key = (sender as LinkLabel).Name;
            if (!key.Contains('_'))
                return;
            string[] sepd = key.Split(new char[] { '_' });
            if (!sepd[1].IsNumber())
                return;
            
            int index_ = int.Parse(sepd[1]);
            RemoveListElement(index_);
            DisplayElements(); 
            
        } 
        private void RemoveListElement(int index_)
        {
            if (index_ > this.lst.Count - 1)
                return;
            this.lst.RemoveAt(index_);  
            
        }

        private void TextBoxeses_TextChanged(object sender, EventArgs e)
        {
            string key = (sender as TextBox).Name;
            if (!key.Contains('_'))
                return;
            string[] sepd = key.Split(new char[] { '_' });
            if (!sepd[1].IsNumber())
                return;
            int index_ = int.Parse(sepd[1]);
            string text = (sender as TextBox).Text;

            if (index_ < this.lst.Count)
            {
                if (sepd[0].Trim().ToLower() == "value")
                    lst[index_].cokValue = text;
                else
                    if (sepd[0].Trim().ToLower() == "name")
                        lst[index_].cokName = text;
            }  
        }

        
        private void RemoveELements()
        { 
            this.panel1.Controls.Clear();
        }

        public List<Cookie2> lst = new List<Cookie2>();

        private void button3_Click(object sender, EventArgs e)
        {
            DisplayElements();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RemoveELements();
        }
 
        private void button2_Click_1(object sender, EventArgs e)
        {
            foreach (Control c in this.panel1.Controls)
                if (c is TextBox)
                    (c as TextBox).Text = c.Name;
        }

        public int _ChildTabIndex { get; set; }

        public int _foreach_counter { get; set; }

        private void btnNew_Click(object sender, EventArgs e)
        {
            Cookie2 cok = new Cookie2( );
            this.lst.Add(cok);
            DisplayElements();

        }
    }
}
