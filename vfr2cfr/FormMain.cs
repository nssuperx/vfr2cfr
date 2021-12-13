using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace vfr2cfr
{
    public partial class FormMain : Form
    {
        // 32bit
        [DllImport("vfr2cfrLib.dll", EntryPoint = "TestString")]
        static extern int Test_32();

        // 64bit    
        [DllImport("vfr2cfrLib64.dll", EntryPoint = "TestString")]
        static extern int Test_64();

        static int Test()
        {
            return Environment.Is64BitProcess ? Test_32() : Test_64();
        }


        private string[] inputFilePaths;
        private static double videoDuration = 0.0;
        private static double videoOuttime = 0.0;
        private static int progressBarValue = 0;
        private static bool isError = false;
        private readonly int[] fpsArray = {60,30};
        private int selectedFps = 0;
        private bool isEncoding = false;
        private int ffmpegPid;

        public FormMain()
        {
            InitializeComponent();

            // test
            textBox1.AppendText(Test().ToString());
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            selectedFps = 0;
            fpsButton.Text = fpsArray[selectedFps].ToString() + " fps";
        }

        private void OpenButton_Click(object sender, EventArgs e)
        {
            progressBar.Value = 0;
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

                //StatusStripに文字列を出す
                encodeingVideoNum++;
                toolStripStatusLabel.Text = "(" + encodeingVideoNum.ToString() + " / " + inputFilePaths.Length.ToString() + ")" + "変換中["+ fpsArray[selectedFps].ToString() + "fps]:" + Path.GetFileName(inputFilePaths[encodeingVideoNum - 1]);
                
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
                textBox.AppendText("変換: " + Path.GetFileName(inputFilePath) + Environment.NewLine);
                
                //プログレスバーの更新
                timer.Enabled = true;
                //変換
                int result = await Task.Run(() => ConvertFile(inputFilePath, outputFilePath));
                timer.Enabled = false;
                //テキストボックスに何か表示（出力ファイル名）
                if (result == -1)
                {
                    textBox.AppendText("失敗: " + Path.GetFileName(inputFilePath) + Environment.NewLine);
                    continue;
                }
                textBox.AppendText("成功: " + Path.GetFileName(outputFilePath) + Environment.NewLine);
                
                //最後に一回　終わったときは100%に見せる
                UpdateProgressBar();
            }
            toolStripStatusLabel.Text = "すべての処理が完了しました";
            openButton.Enabled = true;
            isEncoding = false;
        }

        private int ConvertFile(string inputFilePath, string outputFilePath)
        {
            isError = false;

            //Processオブジェクトを作成
            Process ffmpegProcess = new Process();
            //cmd.exeのパス取得
            ffmpegProcess.StartInfo.FileName = Environment.GetEnvironmentVariable("ComSpec");
            ffmpegProcess.StartInfo.CreateNoWindow = true;

            //出力を読み取れるようにする
            ffmpegProcess.StartInfo.UseShellExecute = false;
            ffmpegProcess.StartInfo.RedirectStandardOutput = true;
            ffmpegProcess.StartInfo.RedirectStandardInput = false;
            ffmpegProcess.StartInfo.RedirectStandardError = true;

            ffmpegProcess.OutputDataReceived += processOutputDataReceived;
            ffmpegProcess.ErrorDataReceived += processErrorDataReceived;

            //ffprobeで情報取得
            ffmpegProcess.StartInfo.Arguments = @"/c ffprobe " + "\"" + inputFilePath + "\"" + " -hide_banner -show_entries format=duration";
            ffmpegProcess.Start();
            ffmpegProcess.BeginOutputReadLine();
            ffmpegProcess.BeginErrorReadLine();
            ffmpegProcess.WaitForExit();
            ffmpegProcess.CancelOutputRead();
            ffmpegProcess.CancelErrorRead();
            ffmpegProcess.Close();

            if (isError) return -1;

            //ffmpegでエンコード
            ffmpegProcess.StartInfo.Arguments = @"/c ffmpeg -i " + "\"" + inputFilePath + "\"" + " -progress - -r " + fpsArray[selectedFps].ToString() + " -vsync cfr -af aresample=async=1 -vcodec utvideo -acodec pcm_s16le -colorspace bt709 -pix_fmt yuv422p " + "\"" + outputFilePath + "\"";
            ffmpegProcess.Start();
            //強制killするためにpidをもっとく
            ffmpegPid = ffmpegProcess.Id;
            ffmpegProcess.BeginOutputReadLine();
            ffmpegProcess.BeginErrorReadLine();
            ffmpegProcess.WaitForExit();
            ffmpegProcess.CancelOutputRead();
            ffmpegProcess.CancelErrorRead();
            ffmpegProcess.Close();

            return 0;
        }

        static void processOutputDataReceived(object sender, DataReceivedEventArgs eventArgs)
        {
            Console.WriteLine(eventArgs.Data);
            if (eventArgs.Data == null)
            {
                return;
            }
            //Console.WriteLine(eventArgs.Data);

            if (eventArgs.Data.Contains("duration="))
            {
                videoDuration = double.Parse(eventArgs.Data.Replace("duration=", "")) * Math.Pow(10, 6);
                //Console.WriteLine(videoDuration);
                return;
            }

            if (eventArgs.Data.Contains("out_time_us="))
            {
                videoOuttime = double.Parse(eventArgs.Data.Replace("out_time_us=", ""));
                //Console.WriteLine(videoOuttime);
                progressBarValue = (int)Math.Min(Math.Max(0, (videoOuttime / videoDuration * 100)), 100);
            }
            if (eventArgs.Data.Contains("progress=end"))
            {
                //Console.WriteLine(eventArgs.Data);
                progressBarValue = 100;
                return;
            }
        }

        static void processErrorDataReceived(object sender,DataReceivedEventArgs eventArgs)
        {
            if (eventArgs.Data == null)
            {
                return;
            }
            //エラー出力された文字列を表示する
            //Console.WriteLine("ERR>{0}", eventArgs.Data);

            //これである程度は変換できないのをはじける
            if (eventArgs.Data.Contains("Invalid data found when processing input"))
            {
                isError = true;
            }

        }

        private void UpdateProgressBar()
        {
            if ((int)videoDuration == 0)
            {
                return;
            }
            progressBar.Value = progressBarValue;
            //Console.WriteLine(progressBar.Value);
            return;
        }
        
        private void Timer_Tick(object sender, EventArgs e)
        {
            UpdateProgressBar();
            //Console.WriteLine("ffmpegPid=" + ffmpegPid.ToString());
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isEncoding)
            {
                if (MessageBox.Show("変換の途中です！ 終了してもいいですか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
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
            string taskkill = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "taskkill.exe");
            using (var processKiller = new Process())
            {
                processKiller.StartInfo.FileName = taskkill;
                processKiller.StartInfo.Arguments = string.Format("/PID {0} /T /F", process.Id);
                processKiller.StartInfo.CreateNoWindow = true;
                processKiller.StartInfo.UseShellExecute = false;
                processKiller.Start();
                processKiller.WaitForExit();
            }
        }
    }
}
