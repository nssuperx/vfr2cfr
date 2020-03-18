using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace vfr2cfr
{
    public partial class FormMain : Form
    {
        private string[] inputFilePaths;
        private static double videoDuration = 0.0;
        private static double videoOuttime = 0.0;
        private static int progressBarValue = 0;
        private readonly int[] fpsArray = {60,30};
        private int selectedFps = 0;
        private bool isEncoding = false;
        private int ffmpegPid;

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            selectedFps = 0;
            fpsButton.Text = fpsArray[selectedFps].ToString() + " fps";
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

        private void FpsButton_Click(object sender, EventArgs e)
        {
            selectedFps = (selectedFps + 1) % fpsArray.Length;
            fpsButton.Text = fpsArray[selectedFps].ToString() + " fps";
        }

        private async void OutButtonClickMethod()
        {
            textBox.Clear();
            int encodeingVideoNum = 0;
            isEncoding = true;
            foreach (string inputFilePath in inputFilePaths)
            {
                //プログレスバーの値の初期化
                videoDuration = 0.0;
                videoOuttime = 0.0;
                progressBarValue = 0;
                //StatusStripに文字列を出す。
                encodeingVideoNum++;
                toolStripStatusLabel1.Text = "(" + encodeingVideoNum.ToString() + " / " + inputFilePaths.Length.ToString() + ")" + "変換中["+ fpsArray[selectedFps].ToString() + "fps]:" + Path.GetFileName(inputFilePaths[encodeingVideoNum - 1]);
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
            isEncoding = false;
        }

        private void ConvertFile(string inputFilePath, string outputFilePath)
        {
            //参考ページ:https://dobon.net/vb/dotnet/process/standardoutput.html
            //Processオブジェクトを作成
            Process p = new Process();
            //ComSpec(cmd.exe)のパスを取得して、FileNameプロパティに指定
            p.StartInfo.FileName = Environment.GetEnvironmentVariable("ComSpec");
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
            p.StartInfo.Arguments = @"/c ffmpeg -i " + "\"" + inputFilePath + "\"" + " -progress - -r " + fpsArray[selectedFps].ToString() + " -vsync cfr -af aresample=async=1 -vcodec utvideo -acodec pcm_s16le -colorspace bt709 -pix_fmt yuv422p " + "\"" + outputFilePath + "\"";
            p.Start();
            //強制killするためにpidをもっとく
            ffmpegPid = p.Id;
            p.BeginOutputReadLine();
            p.WaitForExit();
            p.CancelOutputRead();
            p.Close();
        }

        static void p_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if(e.Data == null)
            {
                return;
            }
            //Console.WriteLine(e.Data);

            if (e.Data.Contains("duration="))
            {
                videoDuration = double.Parse(e.Data.Replace("duration=", "")) * Math.Pow(10, 6);
                //Console.WriteLine(videoDuration);
                return;
            }

            if (e.Data.Contains("out_time_us="))
            {
                videoOuttime = double.Parse(e.Data.Replace("out_time_us=", ""));
                //Console.WriteLine(videoOuttime);
                progressBarValue = (int)Math.Min(Math.Max(0, (videoOuttime / videoDuration * 100)), 100);
            }
            if (e.Data.Contains("progress=end"))
            {
                //Console.WriteLine(e.Data);
                progressBarValue = 100;
                return;
            }
        }

        private void UpdateProgressBar()
        {
            if ((int)videoDuration == 0)
            {
                return;
            }
            progressBar1.Value = progressBarValue;
            //Console.WriteLine(progressBar1.Value);
            return;
        }
        
        private void Timer1_Tick(object sender, EventArgs e)
        {
            UpdateProgressBar();
            //Console.WriteLine("ffmpegPid=" + ffmpegPid.ToString());
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isEncoding)
            {
                if (MessageBox.Show("変換の途中です!! 終了してもいいですか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    Process kp = Process.GetProcessById(ffmpegPid);
                    KillProcessTree(kp);
                }
            }
        }

        private void KillProcessTree(Process process)
        {
            //参考ページ:https://qiita.com/yohhoy/items/b6e32e17c9d568f927d8
            string taskkill = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "taskkill.exe");
            using (var procKiller = new Process())
            {
                procKiller.StartInfo.FileName = taskkill;
                procKiller.StartInfo.Arguments = string.Format("/PID {0} /T /F", process.Id);
                procKiller.StartInfo.CreateNoWindow = true;
                procKiller.StartInfo.UseShellExecute = false;
                procKiller.Start();
                procKiller.WaitForExit();
            }
        }
    }
}
