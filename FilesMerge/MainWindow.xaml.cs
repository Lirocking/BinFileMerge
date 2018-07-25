using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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

namespace FilesMerge
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


        private ObservableCollection<SourceFile> files = new ObservableCollection<SourceFile>();
        private string DirectoryPath = String.Empty;
       
        private void AddFileButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            if (String.IsNullOrEmpty(DirectoryPath))
                openFileDialog.InitialDirectory = "c:\\";
            else
                openFileDialog.InitialDirectory = DirectoryPath;

            openFileDialog.Filter = "All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var fileInfo = new FileInfo(openFileDialog.FileName);
                DirectoryPath = fileInfo.DirectoryName;
                var file = new SourceFile(System.IO.Path.GetFileNameWithoutExtension(openFileDialog.FileName),
                    fileInfo.Length, 0, fileInfo.ToString(), fileInfo.DirectoryName);
                files.Add(file);
                Debug.WriteLine(fileInfo.Length);


            }
        }

        private void MergeButton_Click(object sender, RoutedEventArgs e)
        {
            if (files.Count < 2)
            {
                MessageBox.Show("文件数量不够!");
                return;
            }
            var file1 = files[0];
            var file2 = files[1];
            if (file1.Path == file2.Path)
            {
                MessageBox.Show("不能同时对同一个文件进行操作");
                return;
            }
            byte[] data1 = new byte[1024];
            byte[] data2 = new byte[1024];
            try
            {
                using (var stream1 = new FileStream(file1.Path, FileMode.Open))
                {
                    var stream2 = new FileStream(file2.Path, FileMode.Open);

                    stream1.Seek(0L, SeekOrigin.Begin);
                    stream2.Seek(0L, SeekOrigin.Begin);

                    var binaryReader1 = new BinaryReader(stream1);
                    var binaryReader2 = new BinaryReader(stream2);
                    var streamMerge = new FileStream(DirectoryPath + "\\merge.bin", FileMode.Create);
                    var binaryMerge = new BinaryWriter(streamMerge);
                    while (binaryReader1.BaseStream.Position < binaryReader1.BaseStream.Length)
                    {
                        binaryMerge.Write(binaryReader1.ReadByte());
                    }

                    if (file2.Offset > file1.Size)
                    { 
                        long num = file2.Offset - file1.Size;
                        long i = 0;
                        var ff = StringToToHexByte("FF");
                        while (i++ < num)
                        {
                            binaryMerge.Write(ff);
                        }
                    }
                    while (binaryReader2.BaseStream.Position < binaryReader2.BaseStream.Length)
                    {
                        binaryMerge.Write(binaryReader2.ReadByte());
                    }

                    binaryMerge.Close();
                    binaryReader1.Close();
                    binaryReader2.Close();
                    streamMerge.Close();
                    stream1.Close();
                    stream2.Close();
                }
            } catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                MessageBox.Show("合并过程出现错误");
                return;
            }
            MessageBox.Show("合并成功，文件名为 merge.bin");
            System.Diagnostics.Process.Start(DirectoryPath);

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FileGrid.ItemsSource = files;
        }
        private static byte[] StringToToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        class SourceFile : INotifyPropertyChanged
        {
            public SourceFile(string name, long size, int offset, string path, string directory)
            {
                this.Name = name;
                this.Size = size;
                this.Offset = offset;
                this.Path = path;
                this.Directory = directory;
            }
            public string Name { get; set; }
            public long Size { get; set; }
            public int Offset { get; set; }
            public string Path { get; set; }
            public string Directory { get; set; }

            public event PropertyChangedEventHandler PropertyChanged;
            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void OpenFolder_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(DirectoryPath))
                return;
            System.Diagnostics.Process.Start(DirectoryPath);
        }
    }
}
