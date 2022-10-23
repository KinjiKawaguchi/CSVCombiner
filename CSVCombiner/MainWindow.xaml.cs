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
        public static class Global////Global変数定義
        {
            public static string File1_Path = "";
            public static string File2_Path = "";
            public static bool File1_Exist = false;
            public static bool File2_Exist = false;
        }

        public MainWindow()
        {
            InitializeComponent();

            EnableDragDrop(Frame_DropFile1);
            EnableDragDrop(Frame_DropFile2);
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
                    string[] paths = ((string[])e.Data.GetData(DataFormats.FileDrop));
                    if (control.Name == "Frame_DropFile1")
                    {
                        Global.File1_Path = paths[0];
                        Global.File1_Exist = true;
                    }
                    else
                    {
                        Global.File2_Path = paths[0];
                        Global.File2_Exist = true;
                    }
                    if(Global.File1_Exist && Global.File2_Exist)
                    {
                        Button_execute.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        Button_execute.Visibility = Visibility.Hidden;
                    }
                }
            };
        }

        private void Button_execute_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
