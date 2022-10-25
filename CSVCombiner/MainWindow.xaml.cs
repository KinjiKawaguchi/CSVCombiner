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

            EnableDragDrop(DatePicker_DropFile1);
            EnableDragDrop(DatePicker_DropFile2);
        }

        public static class Global////Global変数定義
        {
            public static string File1_Path = "";
            public static string File2_Path = "";
            public static bool File1_Exist = false;
            public static bool File2_Exist = false;
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
                    if (CSV_Check(path[0]))
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
                    else
                    {
                        MessageBox.Show("選択されたファイルはCSV形式では内容です。" +
                            "\n拡張子を確認してください");
                    }
                }
            };
        }

        private void Button_Execute_Click(object sender, RoutedEventArgs e)
        {

        }

        private bool CSV_Check(String path)
        {
            if(path == Global.File1_Path || path == Global.File2_Path)
            {
                MessageBox.Show("同じファイルが指定されています。");
                return false;
            }
            if(path.Substring(path.Length -4) == ".csv")
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
