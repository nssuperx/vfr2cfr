using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace vfr2cfr
{
    public partial class FormMain : Form
    {
        private string[] outFilePaths;
        public FormMain()
        {
            InitializeComponent();
        }

        private void TestButton_Click(object sender, EventArgs e)
        {
            DialogResult dr = openFileDialog.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                openFilesList.Items.Clear();
                outFilePaths = openFileDialog.FileNames;
                foreach (string strFilePath in outFilePaths)
                {
                    openFilesList.Items.Add(Path.GetFileName(strFilePath));
                }
            }
        }

        private void OutButton_Click(object sender, EventArgs e)
        {
            foreach (string outFilePath in outFilePaths)
            {
                Console.WriteLine(Path.ChangeExtension(outFilePath,"avi"));
            }
        }
    }
}
