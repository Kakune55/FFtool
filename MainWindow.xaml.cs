using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                inputFileName = openFileDialog.FileName;
            }
        }

        private void OutputFileButton1_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                //文件内容读取
                outputFileName = saveFileDialog.FileName;
            }
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            //string strCmdText;
            //strCmdText = "ping.exe baidu.com";
            //System.Diagnostics.Process.Start("CMD.exe", strCmdText);

            //System.Diagnostics.Process process = new System.Diagnostics.Process();

            //process.StartInfo.FileName = "ffmpeg.exe";   //IE浏览器，可以更换

            //string cmd = "-i '" + inputFileName + "' '" + outputFileName + "'";
            //Console.WriteLine(cmd);

            //process.StartInfo.Arguments = cmd;
            //process.Start();

            MainTab.SelectedIndex = 3;
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
    }
}
