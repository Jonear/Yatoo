using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace Yatoo
{
    /// <summary>
    /// SettingWindow.xaml 的交互逻辑
    /// </summary>
	public class theme
	{
		public string name{get;set;}
		public string imagpath{get;set;}
	}
    public partial class SettingWindow : Window
    {
        private MainWindow  Parent = null;
        private List<theme> themelist = null;
        public SettingWindow(MainWindow parent,bool ShowTheme=false)
        {
            InitializeComponent();

            Parent = parent;

            InitialTheme();

            InitialMyConfig();

            InitialShared();

            if (ShowTheme)
            {
                Tabitem2.Focus();
            }
        }

        #region 主题设置
        private void InitialTheme()
        {
            themelist = new List<theme>{
                new theme(){name = "background",imagpath="Images/theme/background.jpg"},
                new theme(){name = "theme1",imagpath="Images/theme/theme1.jpg"},
                new theme(){name = "theme2",imagpath="Images/theme/theme2.jpg"},
                new theme(){name = "theme3",imagpath="Images/theme/theme3.jpg"},
                new theme(){name = "theme4",imagpath="Images/theme/theme4.jpg"},
                new theme(){name = "theme5",imagpath="Images/theme/theme5.jpg"},
                new theme(){name = "theme6",imagpath="Images/theme/theme6.jpg"},
                new theme(){name = "theme7",imagpath="Images/theme/theme7.jpg"},
            };

            List_Theme.ItemsSource = null;
            List_Theme.ItemsSource = themelist;

            WindowOpacity.Value = Parent.WinOpacity;
            ForeOpacity.Value = Parent.ForeOpacity;
        }
		private void AddImageClick(object sender, RoutedEventArgs e)
		{
            OpenFileDialog open = new OpenFileDialog();//定义打开文本框实体
            open.Title = "打开文件";//对话框标题
            open.Filter = "图片文件(*.jpg,*.gif,*.bmp)|*.jpg;*.gif;*.bmp";//文件扩展名
            if ((bool)open.ShowDialog().GetValueOrDefault())//打开
            {
                theme newtheme = new theme();
                newtheme.name = open.FileName;
                newtheme.imagpath = open.FileName;

                themelist.Add(newtheme);

                List_Theme.ItemsSource = null;
                List_Theme.ItemsSource = themelist;

                List_Theme.ScrollIntoView(newtheme);
                
            }
		}
        #endregion

        #region 系统菜单
        private void window_move(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if ((e.GetPosition(this).Y < 30))
                {
                    this.DragMove();
                }
            }
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Parent.settingwindow = null;
            this.Close();
        }
        #endregion

        #region 个人信息
        private void InitialMyConfig()
        {
            UserName.Text = Parent.Hoster.name;

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(Parent.Hoster.image, UriKind.Relative);
            bitmap.EndInit();
            UserImage.Source = bitmap;

            UserTalk.Text = Parent.Hoster.talk;
        }

        private void SelectImageClick(object sender, MouseButtonEventArgs e)
        {
            ListBox image = (ListBox)sender;

            int index = image.SelectedIndex;
            //MessageBox.Show(index.ToString());
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("Images/portrait/" + index.ToString() + ".jpg", UriKind.Relative);
            bitmap.EndInit();
            UserImage.Source = bitmap;
        }
        #endregion

        #region 共享文件夹
        //初始化共享文件夹信息
        private void InitialShared()
        {
            TextBox_SharedDir.Text = Parent.SharedDir;
            TextBox_DownedDir.Text = Parent.DownedDir;
            if (Parent.SharedWay == DownloadWay.NeedNothing)
            {
                RadioButton_Down.IsChecked = true;
            }
            else if (Parent.SharedWay == DownloadWay.NeedPassword)
            {
                RadioButton_DownPassword.IsChecked = true;
            }
            DownPassword.Text = Parent.DownPassword;
        }
        //修改共享文件夹
        private void ChangeSharedDirClick(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            fbd.ShowDialog();
            if (fbd.SelectedPath != string.Empty)
            {
                TextBox_SharedDir.Text = fbd.SelectedPath;
            }
        }
        //修改下载到默认路径
        private void ChangeDownedDirClick(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            fbd.ShowDialog();
            if (fbd.SelectedPath != string.Empty)
            {
                TextBox_DownedDir.Text = fbd.SelectedPath;
            }
        }
        //共享方式设置
        private void RadioButton_DownChecked(object sender, RoutedEventArgs e)
        {
            DownPassword.IsEnabled = false;
        }
        private void RadioButton_DownAllowChecked(object sender, RoutedEventArgs e)
        {
            DownPassword.IsEnabled = false;
        }
        private void RadioButton_DownPasswordChecked(object sender, RoutedEventArgs e)
        {
            DownPassword.IsEnabled = true;
        }
        #endregion

        #region 下面三个确定菜单
        private void Button_Look(object sender, RoutedEventArgs e)
        {
            SaveMyConfig();
            SaveMySkin();
            SaveMyShareDir();
        }
        private void Button_OK(object sender, RoutedEventArgs e)
        {
            SaveMyConfig();
            SaveMySkin();
            SaveMyShareDir();
            Parent.settingwindow = null;
            this.Close();
        }
        private void SaveMyConfig()
        {
            Parent.Hoster.name = UserName.Text;
            if (List_Image.SelectedIndex != -1)
            {
                Parent.Hoster.image = "Images/portrait/" + List_Image.SelectedIndex.ToString() + ".jpg";
            }
            Parent.Hoster.talk = UserTalk.Text;

            Parent.UpdateShowMyMsg();
        }
        private void SaveMySkin()
        {
            Parent.WinOpacity = WindowOpacity.Value;
            Parent.ForeOpacity = ForeOpacity.Value;
            if (List_Theme.SelectedIndex != -1)
            {
                Parent.BackgroundImage = themelist[List_Theme.SelectedIndex].imagpath;
            }

            Parent.UpdateShowMySkin();
        }
        private void SaveMyShareDir()
        {
            Parent.SharedDir = TextBox_SharedDir.Text;
            Parent.DownedDir = TextBox_DownedDir.Text;

            if (RadioButton_Down.IsChecked == true)
            {
                Parent.SharedWay = DownloadWay.NeedNothing;
            }
            else if (RadioButton_DownPassword.IsChecked == true)
            {
                Parent.SharedWay = DownloadWay.NeedPassword;
            }
            Parent.DownPassword = DownPassword.Text;
        }

        private void Button_Close(object sender, RoutedEventArgs e)
        {
            Parent.settingwindow = null;
            this.Close();
        }
        #endregion

    }
}
