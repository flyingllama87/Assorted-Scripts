// UI & Main Form Stuff in this file here

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using CookComputing.XmlRpc;
using System.Collections;

namespace SSAConfluenceHRPageCycle
{
    public partial class UI_Main : Form
    {

        // GLOBAL VARIABLE INIT
        private Iconfluence confluenceProxy;
        public string token;


        public UI_Main()
        {
            InitializeComponent();
            txtStatus.Text += "Hi! \r\n\r\nPlease configure the settings to the right, click login and then use the process page buttons.  You will be prompted with a list of users/pages that will be processed for confirmation.  If you continue the script will take about 5 minutes to run and you will be notified when the job is complete.  You only have one shot so please measure twice and cut once.   Rarely modified values appear greyed out but can be changed.  Hover over the text boxes for tips.  Look at the README file for more." + System.Environment.NewLine;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        //*****************MAIN/ENTRY POINT****************

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new UI_Main());
        }


        //*****REFRESH STATUS TEXTBOX WHEN IT'S EDITED*****

        private void txtStatus_TextChanged(object sender, EventArgs e)
        {
            txtStatus.SelectionStart = txtStatus.Text.Length;
            txtStatus.ScrollToCaret();
            txtStatus.Refresh();
            //txtStatus.SelectionStart = txtStatus.Text.Length;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            //XmlRpcStruct TestStruct = new XmlRpcStruct();
            //TestStruct = confluenceProxy.getPage(token, txtSpaceKey.Text, "teafefawef ef Archive");
            
            Application.Exit();
        }

        private void chkProcessObjectives_CheckedChanged(object sender, EventArgs e)
        {
            chkCreateObjectives.Enabled = !chkCreateObjectives.Enabled; 
        }

        private void chkCreateObjectives_CheckedChanged(object sender, EventArgs e)
        {
           chkProcessObjectives.Enabled = !chkProcessObjectives.Enabled; 
        }

        private void txtExclude_TextChanged(object sender, EventArgs e)
        {

        }
    }


}