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
        private string[] inputFilePaths;
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
                inputFilePaths = openFileDialog.FileNames;
                if(inputFilePaths.Length > 0)
                {
                    outButton.Enabled = true;
                }
                foreach (string strFilePath in inputFilePaths)
                {
                    openFilesList.Items.Add(Path.GetFileName(strFilePath));
                }
            }
        }

        private void OutButton_Click(object sender, EventArgs e)
        {
            outButton.Enabled = false;
            OutButtonClickMethod();
        }

        private async void OutButtonClickMethod()
        {
            foreach (string inputFilePath in inputFilePaths)
            {
                //出力ファイル名かぶらないように頑張る
                string f = inputFilePath;
                f = Path.ChangeExtension(f, "avi");
                string[] outDirFiles = Directory.GetFiles(Path.GetDirectoryName(f));
                while (Array.IndexOf(outDirFiles, f) != -1)
                {
                    f = Path.Combine(Path.GetDirectoryName(f), Path.GetFileNameWithoutExtension(f) + "-out.avi");
                    outDirFiles = Directory.GetFiles(Path.GetDirectoryName(f));
                }
                string outputFilePath = f;

                //テキストボックスに何か表示（入力ファイル名）
                textBox.AppendText("Input file: " + Path.GetFileName(inputFilePath) + Environment.NewLine);
                //非同期処理
                //参考ページ:https://qiita.com/gonavi/items/2980b0791a4c14906cd1
                //変換
                await Task.Run(() => ConvertFile(inputFilePath, outputFilePath));
                //テキストボックスに何か表示（出力ファイル名）
                textBox.AppendText("Done. Output file: " + Path.GetFileName(outputFilePath) + Environment.NewLine);
            }
        }

        private void ConvertFile(string inputFilePath, string outputFilePath)
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

            //Console.WriteLine("Input file: " + Path.GetFileName(inputFilePath));
            p.StartInfo.Arguments = @"/c ffmpeg -i " + "\"" + inputFilePath + "\"" + " -r 60 -vsync cfr -af aresample=async=1 -vcodec utvideo -acodec pcm_s16le " + "\"" + outputFilePath + "\"";
            p.Start();
            p.WaitForExit();
            p.Close();
            //Console.WriteLine("Done. Output file: " + Path.GetFileName(outputFilePath));
        }
    }
}
