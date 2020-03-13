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
            //Processオブジェクトを作成
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            //ComSpec(cmd.exe)のパスを取得して、FileNameプロパティに指定
            p.StartInfo.FileName = System.Environment.GetEnvironmentVariable("ComSpec");
            //出力を読み取れるようにする
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            //入力できるようにする
            p.StartInfo.RedirectStandardInput = true;
            //非同期で出力を読み取れるようにする
            p.StartInfo.RedirectStandardOutput = true;
            p.OutputDataReceived += p_OutputDataReceived;
            //起動
            p.Start();
            //非同期で出力の読み取りを開始
            p.BeginOutputReadLine();
            //入力のストリームを取得
            System.IO.StreamWriter sw = p.StandardInput;
            if (sw.BaseStream.CanWrite)
            {
                foreach (string outFilePath in outFilePaths)
                {
                    string f = outFilePath;
                    if (Path.GetExtension(f) == ".avi")
                    {
                        f = Path.Combine(Path.GetDirectoryName(f), Path.GetFileNameWithoutExtension(f) + "-out");
                    }
                    f = Path.ChangeExtension(f, "avi");
                    //Console.WriteLine(f);

                    //コマンドラインを指定（"/c"は実行後閉じるために必要）
                    //p.StartInfo.Arguments = @"dir c:\ /w";

                
                    //「dir c:\ /w」を実行する
                    sw.WriteLine(@"echo " + f);
                    //終了する
                    //sw.WriteLine("exit");

                    

                    //出力を読み取る
                    //string results = p.StandardOutput.ReadToEnd();

                    //プロセス終了まで待機する
                    //WaitForExitはReadToEndの後である必要がある
                    //(親プロセス、子プロセスでブロック防止のため)
                    //p.WaitForExit();


                    //出力された結果を表示
                    //Console.WriteLine(results);
                }
                
            }
            sw.Close();
            p.WaitForExit();
            p.Close();
            Console.ReadLine();
        }
        //OutputDataReceivedイベントハンドラ
        //行が出力されるたびに呼び出される
        static void p_OutputDataReceived(object sender,
            System.Diagnostics.DataReceivedEventArgs e)
        {
            //出力された文字列を表示する
            Console.WriteLine(e.Data);
        }
    }
}
