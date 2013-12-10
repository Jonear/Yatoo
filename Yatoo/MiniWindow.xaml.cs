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
using System.IO;
using System.Threading;
using System.Windows.Threading;

namespace Yatoo
{
    /// <summary>
    /// MiniWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MiniWindow : Window
    {
        private System.Windows.Forms.NotifyIcon notifyIcon = null;
        private Boolean ISClockWindow = false;
        private MainWindow parent = null;
        public UserWindow userwindow = null;
        private int clientHeight;
        private int clientWidth;
        private Boolean IsWindowMove = false;
        private ImageBrush MyImage;
        private ImageBrush MessageImage;
        public MiniWindow(MainWindow parent)
        {
            InitializeComponent();

            //获得父窗口对象
            this.parent = parent;

            //设置头像
            InitialImage();

            //屏幕大小
            clientHeight=System .Windows .Forms .Screen .PrimaryScreen .Bounds .Height;
            clientWidth = System.Windows.Forms.Screen.PrimaryScreen .Bounds .Width;

            //至于最前
            Topmost = true;

            //创建用户列表类对象
            userwindow = new UserWindow(parent);
        }

        #region 头像
        public void InitialImage()
        {
            MyImage = new ImageBrush(parent.Logo.Source);
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("Images/messages.jpg", UriKind.Relative);
            bitmap.EndInit();
            MessageImage = new ImageBrush(bitmap);
            BorderBackground.Background = MyImage;
        }
        public void FlashImage()
        {
            //闪动头像线程
            Thread FlashIconThread = new Thread(new ThreadStart(FlashImageThread));
            FlashIconThread.Start();
        }
        private void FlashImageThread()
        {
            while (parent.GetMsgList.Count != 0)
            {
                BorderBackground.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() => { BorderBackground.Background = MessageImage; }));

                Thread.Sleep(800);

                BorderBackground.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() => { BorderBackground.Background = MyImage; }));

                Thread.Sleep(800);
            }
        }
        #endregion

        #region 系统事件
        private void window_move(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && !ISClockWindow)
            {
                if (userwindow.IsVisible == true)
                {
                    userwindow.Hide();
                }   
                IsWindowMove = true;
                this.DragMove();
            }
        }

        private void mouseleave(object sender, MouseEventArgs e)
        {
            this.Opacity = 0.8;
        }

        private void mouseenter(object sender, MouseEventArgs e)
        {
            this.Opacity = 1;
        }
        private void mouseleftup(object sender, MouseButtonEventArgs e)
        {
            if (IsWindowMove == false)
            {
                if (parent.GetMsgList.Count != 0)
                {
                    TalkWindow talkwindow = new TalkWindow(parent.GetMsgList[0].user, parent.MyFont, parent);
                    for (int i = parent.GetMsgList.Count-1; i >= 0 ; i--)
                    {
                        if (parent.GetMsgList[i].user.ip == parent.GetMsgList[0].user.ip)
                        {
                            if (parent.GetMsgList[i].ID == MessageID.UserSendFile)
                            {
                                talkwindow.AddRecvFile(parent.GetMsgList[i].file, parent.GetMsgList[i].filesize);
                            }
                            else
                            {
                                talkwindow.ShowMessage(parent.GetMsgList[i].content, parent.GetMsgList[i].font);
                            }
                            parent.GetMsgList.RemoveAt(i);
                        }
                    }
                    parent.talklist.Add(talkwindow);
                    talkwindow.Show();
                }
                else if (userwindow.IsVisible == false)
                {
                    if ((this.Left + 70 + 600) <= clientWidth)
                    {
                        userwindow.Left = this.Left + 70;
                    }
                    else
                    {
                        userwindow.Left = this.Left - 580;
                    }
                    if ((this.Top + 400) <= clientHeight)
                    {
                        userwindow.Top = this.Top;
                    }
                    else
                    {
                        userwindow.Top = this.Top - 340;
                    }
                    userwindow.Show();
                    userwindow.Activate();
                }
                else
                {
                    userwindow.Hide();
                }
            }
            else
            {
                IsWindowMove = false;
            }
        }
        #endregion

        #region 右击菜单
        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            parent.notifyIcon.Dispose(); 
            parent.UserCheckOut();
            parent.SaveMyConfig();
            Environment.Exit(0);
        }

        private void ClockWindow(object sender, RoutedEventArgs e)
        {
            if (ISClockWindow)
            {
                ISClockWindow = false;
                MenuItem item = (MenuItem)sender;
                item.Header = "锁定窗口";
            }
            else
            {
                ISClockWindow = true;
                MenuItem item = (MenuItem)sender;
                item.Header = "解锁窗口";
            }
        }

        private void ReloadWindow(object sender, RoutedEventArgs e)
        {
            this.Hide();

            parent.Show();

            while (parent.Height < 600)
            {
                parent.Height += 30;
            }
        }

        private void TopWindow(object sender, RoutedEventArgs e)
        {
            if (Topmost)
            {
                Topmost = false;
                MenuItem item = (MenuItem)sender;
                item.Header = "至于最前";
            }
            else
            {
                Topmost = true;
                MenuItem item = (MenuItem)sender;
                item.Header = "取消最前";
            }
        }
        #endregion

        #region 屏蔽Alt+F4
        private bool AltDown = false;
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            if (e.SystemKey == Key.LeftAlt || e.SystemKey == Key.RightAlt)
            {
                AltDown = true;
            }
            else if (e.SystemKey == Key.F4 && AltDown)
            {
                e.Handled = true;
            }
        }

        private void Window_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.SystemKey == Key.LeftAlt || e.SystemKey == Key.RightAlt)
            {
                AltDown = false;
            }
        }
        #endregion

    }
}
