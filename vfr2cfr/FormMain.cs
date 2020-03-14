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
            if (dr == DialogResult.OK)
            {
                openFilesList.Items.Clear();
                outFilePaths = openFileDialog.FileNames;
                if(outFilePaths.Length > 0)
                {
                    outButton.Enabled = true;
                }
                foreach (string strFilePath in outFilePaths)
                {
                    openFilesList.Items.Add(Path.GetFileName(strFilePath));
                }
            }
        }

        private void OutButton_Click(object sender, EventArgs e)
        {
            
            //参考ページ:https://dobon.net/vb/dotnet/process/standardoutput.html
            //Processオブジェクトを作成
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            //ComSpec(cmd.exe)のパスを取得して、FileNameプロパティに指定
            p.StartInfo.FileName = System.Environment.GetEnvironmentVariable("ComSpec");
            p.StartInfo.CreateNoWindow = true;

            //出力を読み取れるようにする
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;

            //入力できるようにする
            p.StartInfo.RedirectStandardInput = true;

            foreach (string outFilePath in outFilePaths)
            {
                string f = outFilePath;
                f = Path.ChangeExtension(f, "avi");
                string[] outDirFiles = Directory.GetFiles(Path.GetDirectoryName(f));
                while (Array.IndexOf(outDirFiles, f) != -1)
                {
                    f = Path.Combine(Path.GetDirectoryName(f), Path.GetFileNameWithoutExtension(f) + "-out.avi");
                    outDirFiles = Directory.GetFiles(Path.GetDirectoryName(f));
                }

                //Console.WriteLine("Input file: " + Path.GetFileName(outFilePath));
                textBox.AppendText("Input file: " + Path.GetFileName(outFilePath) + Environment.NewLine);
                p.StartInfo.Arguments = @"/c ffmpeg -i " + "\"" + outFilePath + "\"" + " -r 60 -vsync cfr -af aresample=async=1 -vcodec utvideo -acodec pcm_s16le " + "\"" + f + "\"";
                //p.StartInfo.Arguments = @"/c ipconfig";
                p.Start();
                string results = p.StandardOutput.ReadToEnd();
                p.WaitForExit();
                p.Close();
                textBox.AppendText(results + Environment.NewLine);
                //Console.WriteLine("Done. output file: " + Path.GetFileName(f));
                //textBox.AppendText("Done. output file: " + Path.GetFileName(f) + Environment.NewLine);
            }
        }

        /*
        static void p_OutputDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            //出力された文字列を表示する
            Console.WriteLine(e.Data);
        }
        */

    }
}
