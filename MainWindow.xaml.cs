using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FFtool
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public string inputFileName;
        public string outputFileName;

        private void InputFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.Filter = "Sql 文件(*.sql)|*.sql";
            //过滤文件类型
            //openFileDialog.Filter = "文档(*.txt)|*.txt|所有文件(*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                //FileName 完整路径：相对路径+文件名+后缀
                //文件内容读取
                inputFileName = "\"" + openFileDialog.FileName + "\"";
            }
        }

        private void OutputFileButton1_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "mp4 |.mp4| avi |.avi| ts |.ts| mkv |.mkv| mov |.mov| wmv |.wmv| flv |.flv| mpeg |.mpeg| vob |.vob| rm |.rm| rmvb | *.rmvb";
            if (saveFileDialog.ShowDialog() == true)
            {
                //文件内容读取
                outputFileName = " \"" + saveFileDialog.FileName + "\"";
            }
        }

        public static void RunFFmpeg(string arguments)
        {
            string ffmpegPath = "ffmpeg"; // 如果ffmpeg可执行文件不在系统PATH中，需要提供完整路径

            // 启动FFmpeg进程
            Process ffmpegProcess = new Process();
            ffmpegProcess.StartInfo.FileName = ffmpegPath;
            ffmpegProcess.StartInfo.Arguments = arguments;
            ffmpegProcess.StartInfo.RedirectStandardOutput = true;
            ffmpegProcess.StartInfo.RedirectStandardError = true;
            ffmpegProcess.StartInfo.UseShellExecute = false;

            // 订阅OutputDataReceived和ErrorDataReceived事件以实时捕获FFmpeg输出
            ffmpegProcess.OutputDataReceived += (sender, e) => HandleOutputData(e.Data);
            ffmpegProcess.ErrorDataReceived += (sender, e) => HandleOutputData(e.Data);

            ffmpegProcess.Start();
            ffmpegProcess.BeginOutputReadLine();
            ffmpegProcess.BeginErrorReadLine();

            ffmpegProcess.WaitForExit();

            Console.WriteLine("FFmpeg process exited with code " + ffmpegProcess.ExitCode);
        }

        private static void HandleOutputData(string data)
        {
            if (data != null)
            {
                // 使用正则表达式从FFmpeg输出中提取进度信息
                Match match = Regex.Match(data, @"time=(\d\d:\d\d:\d\d\.\d\d)");
                if (match.Success)
                {
                    string timeString = match.Groups[1].Value;
                    TimeSpan currentTime = TimeSpan.Parse(timeString);
                    Console.WriteLine("Current time: " + currentTime);
                    // 在这里可以根据需要处理进度信息，比如计算百分比并显示在UI上
                }
            }
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            if (inputFileName == null || outputFileName == null)
            {
                MessageBox.Show("未选择文件");
                return;
            }
            //string cmd = "ffmpeg ";
            string cmd = "";
            //命令拼接
            cmd += "-i " + inputFileName;
            //视频
            if (v_fps.Text == "默认" && v_resolution.Text == "默认" && v_bitrate.Text == "默认")
            {
                cmd += " -vcodec copy";
            }
            else
            {
                if (v_codec.Text != "copy")
                {
                    cmd += " -vcodec " + v_codec.Text;
                }

            }



            if (v_fps.Text != "默认")
            {
                cmd += " -r " + v_fps.Text;
            }
            if (v_resolution.Text != "默认")
            {
                cmd += " -s " + v_resolution.Text;
            }
            if (v_bitrate.Text != "默认")
            {
                cmd += " -b:v " + v_bitrate.Text;
            }
            //音频
            

            if (a_disable.IsChecked == false)
            {
                if (a_bitrate.Text == "默认" && a_channels.Text == "默认" && a_sampleRate.Text == "默认" && a_volume.Text == "默认")
                {
                    cmd += " -acodec copy";
                }
                else
                {
                    if (v_codec.Text != "copy")
                    {
                        cmd += " -acodec " + a_codec.Text;
                    }

                }

                if (a_channels.Text != "默认")
                {
                    cmd += " -ac " + a_channels.Text;
                }
                if (a_bitrate.Text != "默认")
                {
                    cmd += " -b:a " + a_bitrate.Text;
                }
                if (a_sampleRate.Text != "默认")
                {
                    cmd += " -ar " + a_sampleRate.Text;
                }
                if (a_volume.Text != "默认")
                {
                    cmd += " -af \"volume=" + a_volume.Text + "\"";
                }
            }
            else
            {
                cmd += " -an ";
            }

            //裁剪
            if (StartTimeCheck.IsChecked == true)
            {
                cmd += " -ss " + StartTimeText.Text;
            }
            if (EndTimeCheck.IsChecked == true)
            {
                cmd += " -to " + EndTimeText.Text;
            }

            cmd += outputFileName;
            OutputCmdText.Text = cmd;

            appStatus.Content = "处理中...";
            AppProgressBar.Value = 50;
            RunFFmpeg(cmd);

            MainTab.SelectedIndex = 3;
            appStatus.Content = "已完成";
            AppProgressBar.Value = 100;
        }
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void StartTimeCheck_Checked(object sender, RoutedEventArgs e)
        {
            StartTimeText.IsEnabled = true;
        }

        private void StartTimeCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            StartTimeText.IsEnabled = false;
        }
        private void EndTimeCheck_Checked(object sender, RoutedEventArgs e)
        {
            EndTimeText.IsEnabled = true;
        }

        private void EndTimeCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            EndTimeText.IsEnabled = false;
        }

        private void a_disable_Checked(object sender, RoutedEventArgs e)
        {
            //a_codec.IsEnabled = false;
            a_bitrate.IsEnabled = false;
            a_channels.IsEnabled = false;
            a_codec.IsEnabled = false;
            a_volume.IsEnabled = false;
            a_sampleRate.IsEnabled = false;
        }

        private void a_disable_Unchecked(object sender, RoutedEventArgs e)
        {
            //a_codec.IsEnabled = true;
            a_bitrate.IsEnabled = true;
            a_channels.IsEnabled = true;
            a_codec.IsEnabled = true;
            a_volume.IsEnabled = true;
            a_sampleRate.IsEnabled = true;
        }
    }
}
