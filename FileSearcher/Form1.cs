using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileSearcher
{
    public partial class frmMain : Form
    {
        private const string APP_NAME = "File Searcher and Sorter";
        public frmMain()
        {
            InitializeComponent();
            this.setDefaultValues();
        }

        private void setDefaultValues()
        {
            this.txtInput.Text = "D:\\repository\\dotnet\\Test Data\\input";
            this.txtOutput.Text = "D:\\repository\\dotnet\\Test Data\\output";
            this.txtSearch.Text = "MEDU6161670, 227873504";
            this.chkDelete.Checked = false;
            this.chkClear.Checked = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.txtInput.Text))
                MessageBox.Show("Input Directory is required!", APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (string.IsNullOrWhiteSpace(this.txtOutput.Text))
                MessageBox.Show("Output Directory is required!", APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (string.IsNullOrWhiteSpace(this.txtSearch.Text))
                MessageBox.Show("Search text is required!", APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                if (this.chkClear.Checked)
                    this.clearOutputFolder();

                this.lstProgress.Items.Clear();
                this.btnStart.Enabled = false;
                this.btnStop.Enabled = true;
                this.process();
            }
            this.btnStart.Enabled = true;
            this.btnStop.Enabled = false;
        }

        private void clearOutputFolder()
        {
            var directory = new DirectoryInfo(this.txtOutput.Text);
            foreach (var file in directory.GetFiles()) file.Delete();
            foreach (var subDirectory in directory.GetDirectories()) subDirectory.Delete(true);
        }

        private void process()
        {
            var searchStrings = this.txtSearch.Text.Split(",");

            foreach (string file in Directory.EnumerateFiles(this.txtInput.Text, ""))
            {
                this.lstProgress.Items.Add(file);
                this.lstProgress.SelectedIndex = this.lstProgress.Items.Count - 1;

                var fileContents = File.ReadAllText(file);

                foreach (var searchStr in searchStrings)
                {
                    if (fileContents.Contains(searchStr.Trim()))
                    {
                        var destination = this.txtOutput.Text + "\\" + Path.GetFileName(file);

                        int count = 1;
                        while (File.Exists(destination))
                        {
                            var tempFilename = string.Format("{0}_{1}", Path.GetFileNameWithoutExtension(file), count++);
                            destination = this.txtOutput.Text + "\\" + tempFilename + Path.GetExtension(file);
                        }

                        //Copy File to Output
                        File.Copy(file, destination);

                        if (this.chkDelete.Checked)
                            File.Delete(file);

                        break;
                    }
                }

                Application.DoEvents();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                this.txtInput.Text = dialog.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                this.txtOutput.Text = dialog.FileName;
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {

        }
    }
}
