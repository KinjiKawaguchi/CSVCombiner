using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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

            LoadCountry();

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
        }

        private static async void LoadCountry()
        {
            string CountryCode_Path = "";
            string CountryName_Path = "";
            int Number_Country = 0;
            try
            {
                using (StreamReader sr = new(CountryCode_Path, Encoding.GetEncoding("Shift_JIS")))
                {
                    for (int i = 0; i < Number_Country; i++)
                    {
#pragma warning disable CS8601 // Null 参照代入の可能性があります。
                        Global.CountryCode[i] = sr.ReadLine();
                    }
                }

                using (StreamReader sr = new(CountryName_Path, Encoding.GetEncoding("Shift_JIS")))
                {
                    for (int i = 0; i < Number_Country; i++)
                    {
                        Global.CountryName[i] = sr.ReadLine();
#pragma warning restore CS8601 // Null 参照代入の可能性があります。
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("国関連の情報読込みに失敗しました" +
                    "\nアプリケーションを終了します。");
                await Task.Delay(2000);
                Application.Current.Shutdown();
            }

        }

        private void EnableDragDrop(Control control)
        {
            //ドラッグ＆ドロップを受け付けられるようにする
            control.AllowDrop = true;

            //ドラッグが開始された時のイベント処理（マウスカーソルをドラッグ中のアイコンに変更）
            control.PreviewDragOver += (s, e) =>
            {
                //ファイルがドラッグされたとき、カーソルをドラッグ中のアイコンに変更し、そうでない場合は何もしない。
                e.Effects = (e.Data.GetDataPresent(DataFormats.FileDrop)) ? DragDropEffects.Copy : e.Effects = DragDropEffects.None;
                e.Handled = true;
            };

            //ドラッグ＆ドロップが完了した時の処理（ファイル名を取得し、ファイルの中身をTextプロパティに代入）
            control.PreviewDrop += (s, e) =>
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop)) // ドロップされたものがファイルかどうか確認する。
                {
                    string[] path = ((string[])e.Data.GetData(DataFormats.FileDrop));
                    if (File_Check(path[0]))
                    {
                        if (Global.File1_Path == path[0] || Global.File2_Path == path[0])
                        {
                            MessageBox.Show("同じファイルが選択されています。");
                            return;
                        }
                        if (control.Name == "Frame_DropFile1")
                        {
                            Global.File1_Path = path[0];
                            Global.File1_Exist = true;

                        }
                        else
                        {
                            Global.File2_Path = path[0];
                            Global.File2_Exist = true;
                        }
                        if (Global.File1_Exist && Global.File2_Exist)
                        {
                            Button_Execute.Visibility = Visibility.Visible;
                        }
                    }
                }
            };
        }

        private void Button_Execute_Click(object sender, RoutedEventArgs e)
        {

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
                MessageBox.Show("指令されたファイルはCSVでは内容です。" +
                    "\nファイルの拡張子を確認してください。");
                return false;
            }
        }
    }
}
