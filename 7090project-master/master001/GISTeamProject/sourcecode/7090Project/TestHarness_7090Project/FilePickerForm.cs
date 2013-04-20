using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace DataMiningTeam.WindowsForms
{
    public partial class frmFilePicker : Form
    {
        
        //Constructors ******************************************************
        public frmFilePicker()
        {
            InitializeComponent();
        }//frmFilePicker

        //Methods ***********************************************************
        //Events ************************************************************
        private void frmFilePicker_Load(object sender, EventArgs e)
        {
            lblExplaination.Text = "File must be in the format 'Transaction ID <Delimiter> Product <Delimiter> Product ....";
        }//frmFilePicker_Load

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            
        }//btnBrowse_Click

        private void btnOK_Click(object sender, EventArgs e)
        {
            
        }//btnOK_Click

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }//btnCancel_Click
    }//class
}//namespace
