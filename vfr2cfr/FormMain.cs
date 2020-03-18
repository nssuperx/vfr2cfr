using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace vfr2cfr
{
    public partial class FormMain : Form
    {
        private string[] inputFilePaths;
        private static double videoDuration = 0.0;
        private static double videoOuttime = 0.0;
        private static int progressBarValue = 0;
        public FormMain()
        {
            InitializeComponent();
        }

        private void OpenButton_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            videoDuration = 0.0;
            videoOuttime = 0.0;
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
            openButton.Enabled = false;
            OutButtonClickMethod();
            
        }

        private async void OutButtonClickMethod()
        {
            textBox.Clear();
            int encodeingVideoNum = 0;
            foreach (string inputFilePath in inputFilePaths)
            {
                //StatusStripに文字列を出す。
                encodeingVideoNum++;
                toolStripStatusLabel1.Text = "(" + encodeingVideoNum.ToString() + " / " + inputFilePaths.Length.ToString() + ")" + "変換中:" + Path.GetFileName(inputFilePaths[encodeingVideoNum - 1]);
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
                textBox.AppendText("変換元: " + Path.GetFileName(inputFilePath) + Environment.NewLine);
                //非同期処理
                //参考ページ:https://qiita.com/gonavi/items/2980b0791a4c14906cd1
                //プログレスバーの更新
                timer1.Enabled = true;
                //変換
                await Task.Run(() => ConvertFile(inputFilePath, outputFilePath));
                //テキストボックスに何か表示（出力ファイル名）
                textBox.AppendText("変換後: " + Path.GetFileName(outputFilePath) + Environment.NewLine);
                timer1.Enabled = false;
                //最後に一回
                UpdateProgressBar();
            }
            toolStripStatusLabel1.Text = "すべての変換が完了しました";
            openButton.Enabled = true;
            //timer1.Enabled = false;
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
            //OutputDataReceivedイベントハンドラを追加
            p.OutputDataReceived += p_OutputDataReceived;
            //入力できるようにする
            p.StartInfo.RedirectStandardInput = false;

            //ffprobeで情報取得
            p.StartInfo.Arguments = @"/c ffprobe " + "\"" + inputFilePath + "\"" + " -hide_banner -show_entries format=duration";
            p.Start();
            p.BeginOutputReadLine();
            p.WaitForExit();
            p.CancelOutputRead();
            p.Close();

            //ffmpegでエンコード
            p.StartInfo.Arguments = @"/c ffmpeg -i " + "\"" + inputFilePath + "\"" + " -progress - -r 30 -vsync cfr -af aresample=async=1 -vcodec utvideo -acodec pcm_s16le -colorspace bt709 -pix_fmt yuv422p " + "\"" + outputFilePath + "\"";
            p.Start();
            p.BeginOutputReadLine();
            p.WaitForExit();
            p.CancelOutputRead();
            p.Close();
        }

        static void p_OutputDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            if(e.Data == null)
            {
                return;
            }
            //Console.WriteLine(e.Data);

            if (e.Data.Contains("duration="))
            {
                //Console.WriteLine(e.Data);
                videoDuration = double.Parse(e.Data.Replace("duration=", "")) * Math.Pow(10, 6);
                Console.WriteLine(videoDuration);
                return;
            }

            if (e.Data.Contains("out_time_us="))
            {
                //Console.WriteLine(e.Data);
                videoOuttime = double.Parse(e.Data.Replace("out_time_us=", ""));
                Console.WriteLine(videoOuttime);
                //Console.WriteLine(videoOuttime / videoDuration);
            }
            if (e.Data.Contains("progress=end"))
            {
                Console.WriteLine(e.Data);
                videoOuttime = videoDuration;
            }
            progressBarValue = (int)Math.Min(Math.Max(0, (videoOuttime / videoDuration * 100)), 100);
        }

        private void UpdateProgressBar()
        {
            if ((int)videoDuration == 0)
            {
                return;
            }
            progressBar1.Value = progressBarValue;
            Console.WriteLine(progressBar1.Value);
            return;
        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateProgressBar();
        }
    }
}
