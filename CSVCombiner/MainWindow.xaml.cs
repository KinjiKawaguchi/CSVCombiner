using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Input;

namespace CSVCombiner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            GetCountryData();

            EnableDragDrop(DatePicker_DropFile1);
            EnableDragDrop(DatePicker_DropFile2);
        }

        public static class Global////Global変数定義
        {
            public static string File1_Path;
            public static string File2_Path;
            public static bool File1_Exist = false;
            public static bool File2_Exist = false;

            public static string[] CountryCode;
            public static string[] CountryName;
            
            public static List<string[]> CountryData = new List<string[]>();
        }

        public static void GetCountryData()
        {
            string path = "./data\\CountryData.csv";
            using (StreamReader readCsvObject = new StreamReader(path, Encoding.GetEncoding("utf-8")))
            {
                while (!readCsvObject.EndOfStream)
                {
                    var readCsvLine = readCsvObject.ReadLine();
                    Global.CountryData.Add(readCsvLine.Split(','));
                }
            }
            return;
        }

        private void EnableDragDrop(Control control)
        {
            control.AllowDrop = true;

            control.PreviewDragOver += (s, e) =>
            {
                e.Effects = (e.Data.GetDataPresent(DataFormats.FileDrop)) ? DragDropEffects.Copy : e.Effects = DragDropEffects.None;
                e.Handled = true;
            };

            control.PreviewDrop += (s, e) =>
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop)) 
                {
                    string[] paths = ((string[])e.Data.GetData(DataFormats.FileDrop));
                    string path = paths[0];

                    if (File_Check(path))
                    {
                        if (control.Name == "DatePicker_DropFile1")
                        {
                            Global.File1_Path = path;
                            Global.File1_Exist = true;
                            Frame_DropFile1.Content = Global.File1_Path;

                        }
                        else
                        {
                            Global.File2_Path = path;
                            Global.File2_Exist = true;
                            Frame_DropFile2.Content = Global.File2_Path;
                        }
                        if (Global.File1_Exist && Global.File2_Exist)
                        {
                            Button_Comb.Visibility = Visibility.Visible;
                        }
                    }
                }
            };
        }

        private bool File_Check(string path)
        {
            if (path == Global.File1_Path || path == Global.File2_Path)
            {
                MessageBox.Show("同じファイルが指定されています。");
                return false;
            }
            if (path[^4..] == ".csv")
            {
                return true;
            }
            else
            {
                MessageBox.Show("指令されたファイルはCSVではありません。" +
                    "\nファイルの拡張子を確認してください。");
                return false;
            }
        }
        /*
        private void Button_Ins(object sender, RoutedEventArgs e)
        {
            if(CountryName.status)
            {
                List<string[]> CountryCode = new List<string[]>();
                using(StreamReader readCsvObject = new StreamReader(path, Encoding.GetEncoding("shift-jis")));
                {
                    while(!readCsvObject.EndOfStream)
                    {
                        var readCsvColum = readCsvObject.ReadColum();
                        ReadCsvColum.Add(ReadCsvColum.Spilit("\n"));
                    }
                }
                for(int i = 0; i < ReadCsvColum.length;i++)
                {
                    return
                }
            }
            else
            {

            }
        }
        private void Button_Comb(object sender, RoutedEventArgs e)
        {
            string[] path;
            path[0] = Global.File1_Path;
            path[1] = Global.File2_Path;
            
            using(StreamReader readCsvObject = new StreamReader(path,Encoding.GetEncoding("shift-jis")))
            {
                while(!readCsvObject.EndOfStream)
                {
                    var 
                }
            }
            
        }
        */
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            bool yes_parse = false;
            {
                // 既存のテキストボックス文字列に、
                // 今新規に一文字追加された時、その文字列が
                // 数値として意味があるかどうかをチェック
                {
                    var tmp = Num_Colum.Text + e.Text;
                    yes_parse = Single.TryParse(tmp, out _);
                }
            }
            // 更新したい場合は false, 更新したくない場合は true
            // を返すべし。（混乱しやすいので注意！）
            e.Handled = !yes_parse;
        }
        private void Button_Ins_CN_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}