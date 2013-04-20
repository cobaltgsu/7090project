using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace DataMiningTeam.WindowsForms
{
    public partial class DMForm : Form
    {
        //Properties/Variables ************************************************
        

        //Constructors ********************************************************
        public DMForm()
        {
            InitializeComponent();
            cmbSource.Items.Add("'|' Delimited File");
            cmbSource.Items.Add("Comma Delimited File");
            cmbSource.Items.Add("Tab Delimited File");
        }//DMForm

        //Methods *************************************************************
        //Events **************************************************************
        private void btnExecute_Click(object sender, EventArgs e)
        {
        }//btnExecute_Click

        private void btnClear_Click(object sender, EventArgs e)
        {
            rtbResults.Text = "";
            txtMinConfidence.Text = "";
            txtMinSupport.Text = "";
        }//button1_Click
    }//class
}//namespace
