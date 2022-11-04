using System.Net.Mail;
using System.Net.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
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
#pragma warning disable CA2211 // 非定数フィールドは表示されません
            public static string? first_file_path;
            public static string? second_file_path;
            public static bool first_file_exists_is = false;
            public static bool second_file_exists_is = false;
            public static List<string[]> CountryData = new();
#pragma warning restore CA2211 // 非定数フィールドは表示されません
        }

        public static class Constans
		{
			public const int COUNTRY_NUM = 238;
			public const int COUNTRY_NAME_COLUMN =  3;
			public const int COUNTRY_CODE_COLUMN = 1;
		}
        
        public static void GetCountryData()
        {
            string path = "./data\\CountryData.csv";
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance); // memo: Shift-JISを扱うためのおまじない
            using StreamReader readCsvObject = new(path, Encoding.GetEncoding("Shift-JIS"));
            while (!readCsvObject.EndOfStream)
            {
                var readCsvLine = readCsvObject.ReadLine();
                if (readCsvLine == null)
                {
                    MessageBox.Show("重大なエラーです。作成者に連絡してください。" +
                        "\n【github】https://www.github.com/KinjiKawaguchi");
                    return;
                }
                Global.CountryData.Add(readCsvLine.Split(','));
            }
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
                    //seting path = ((string)e.Data.GetData(DataFormats.FileDrop));←試しましょう
                    string[] paths = ((string[])e.Data.GetData(DataFormats.FileDrop));
                    string path = paths[0];

                    if (ConfirmFileRightness(path))
                    {
                        if (control.Name == "DatePicker_DropFile1")
                        {
                            Global.first_file_path = path;
                            Global.first_file_exists_is = true;
                            Frame_DropFile1.Content = path;

                        }
                        else
                        {
                            Global.second_file_path = path;
                            Global.second_file_exists_is = true;
                            Frame_DropFile2.Content = path;;
                        }
                        if (Global.first_file_exists_is && Global.second_file_exists_is)
                        {
                            Button_Comb.Visibility = Visibility.Visible;
                        }
                    }
                }
            };
        }
		
        private static bool ConfirmFileRightness(string path)////指定されたファイルが適切か
        {
            if (path == Global.first_file_path || path == Global.second_file_path)//
            {
                MessageBox.Show("同じファイルが指定されています。");
                
                return false;
            }
            if (!(path[^4..] == ".csv"))//拡張子がcsvじゃない場合は不正
            {
                MessageBox.Show("指令されたファイルはCSVではありません。" +
                    "\nファイルの拡張子を確認してください。");
                    
                return false;
            }

            return true;
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            bool yes_parse;
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
            if(Global.first_file_exists_is == false)
            {
                MessageBox.Show("上のファイルボックスにCSVファイルをドラッグアンドドロップしてください。");
                return;
            }

            Insert_Column("CountryName", int.Parse(Num_Colum.Text));
        }
        
        private void Button_Ins_CC_Click(object sender, RoutedEventArgs e)
        {
            if(Global.first_file_exists_is == false)
            {
                MessageBox.Show("上のファイルボックスにCSVファイルをドラッグアンドドロップしてください。");
                return;
            }

            Insert_Column("CountryCode", int.Parse(Num_Colum.Text));
        }
        
        private static void Insert_Column(String object_to_add, int specified_column)
        {
            if(specified_column < 1)
            {
                MessageBox.Show("指定列数は1以上を設定してください");
                
                return;
            }

            List<string> insert_list;
            insert_list = CreateInsertList(object_to_add, specified_column);
#pragma warning disable CS0642 // empty ステートメントが間違っている可能性があります
            using (FileStream fs = File.Create("./output.csv")) ;
#pragma warning restore CS0642 // empty ステートメントが間違っている可能性があります
            using (StreamWriter sw = new("./output.csv", false, Encoding.GetEncoding("Shift-JIS")))
            {
                foreach (var line in insert_list)
                {
                    sw.WriteLine(line);
                }
            }
            MessageBox.Show(object_to_add + "列を追加しました。");
        }
        
        private static List<string> CreateInsertList(String object_to_add,int specified_column){
            int adding_data = 0;
            int reference_data = 0;
            List<string> insert_list = new();

            if (object_to_add == "CountryName")
            {
                adding_data = Constans.COUNTRY_NAME_COLUMN;
                reference_data = Constans.COUNTRY_CODE_COLUMN;
            }
            else if(object_to_add == "CountryCode")
            {
                adding_data = Constans.COUNTRY_CODE_COLUMN;
                reference_data = Constans.COUNTRY_NAME_COLUMN;
            }
            
            List<string[]> Contents = new();
            int raw_count = 0;
            int max_column = 0;
            
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance); // memo: Shift-JISを扱うためのおまじない
            if(Global.first_file_path == null)
            {
                MessageBox.Show("重大なエラーです。作成者に連絡してください。" +
                            "\n【github】https://www.github.com/KinjiKawaguchi");
                Application.Current.Shutdown();
                return insert_list;
            }
            using (StreamReader readCsvObject = new(Global.first_file_path, Encoding.GetEncoding("Shift-JIS")))
            {
                while (!readCsvObject.EndOfStream)
                {
                    var readCsvLine = readCsvObject.ReadLine();
                    if (readCsvLine == null)
                    {
                        MessageBox.Show("重大なエラーです。作成者に連絡してください。" +
                                    "\n【github】https://www.github.com/KinjiKawaguchi");
                        Application.Current.Shutdown();
                        return insert_list;
                    }
                    int columns = CountChar(readCsvLine, ',');
                    
                    if (max_column < columns)
                    {
                        max_column = columns;
                    }
                    
                    readCsvLine += ",";
                    Contents.Add(readCsvLine.Split(','));
                    
                    raw_count++;
                }
            }
            
            for (int i = 0; i < raw_count; i++)
            {
                for (int j = 0; j < Constans.COUNTRY_NUM; j++)
                {
                    if (Contents[i][specified_column - 1] == Global.CountryData[j][reference_data])
                    {
                        String CountryName = Global.CountryData[j][adding_data];
                        Contents[i][max_column + 1] = CountryName;
                        
                        break;
                    }
                }
            }
            
            foreach (var line in Contents)
            {
                insert_list.Add(string.Join(",", line));
            }
            return insert_list;
        }
        
        private static int CountChar(string s, char c)
        {
            return s.Length - s.Replace(c.ToString(), "").Length;
        }
    }
}