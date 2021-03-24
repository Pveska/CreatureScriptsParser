using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CreatureScriptsParser
{
    public partial class MainForm : Form
    {
        private Creator creator;
        private Parsers parsers;
        public MainForm()
        {
            InitializeComponent();
        }

        private void toolStripButton_ImportSniff_Click(object sender, EventArgs e)
        {
            OpenFileDialog();
        }

        private void OpenFileDialog()
        {
            openFileDialog.Title = "Open File";
            openFileDialog.Filter = "Parsed Sniff or Data File (*.txt;*.dat)|*.txt;*.dat";
            openFileDialog.FilterIndex = 1;
            openFileDialog.ShowReadOnly = false;
            openFileDialog.Multiselect = false;
            openFileDialog.CheckFileExists = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                parsers = new Parsers(this);
                Dbc.Dbc.Load();

                if (openFileDialog.FileName.Contains("txt"))
                {
                    creator = new Creator(this, parsers.GetPacketsFromSniffFile(openFileDialog.FileName, checkBox_CreateDataFile.Checked));
                }
                else if (openFileDialog.FileName.Contains("dat"))
                {
                    creator = new Creator(this, parsers.GetPacketsFromDataFile(openFileDialog.FileName));
                }

                toolStripStatusLabel_FileStatus.Text = openFileDialog.FileName + " is selected for input.";
            }
        }

        private void toolStripButton_Search_Click(object sender, EventArgs e)
        {
            creator.CreateScriptsForCreatureWithGuid(toolStripTextBox_CreatureGuid.Text);
        }
    }
}
