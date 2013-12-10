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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.IO;
using System.Xml;
using System.Media;
using System.Diagnostics;

namespace Yatoo
{
    /// <summary>
    /// 信息类Message
    /// </summary>
    [DataContract(Namespace = "http://coderzh.cnblogs.com")]
    public class Message
    {
        [DataMember(Order = 0)]
        public MessageID ID { get; set; }
        [DataMember(Order = 1)]
        public User user { get; set; }
        [DataMember(Order = 2)]
        public string content { get; set; }
        [DataMember(Order = 4)]
        public Font font { get; set; }
        [DataMember(Order = 4)]
        public string file { get; set; }
        [DataMember(Order = 5)]
        public long filesize { get; set; }
        [DataMember(Order = 6)]
        public int fileid { get; set; }
        [DataMember(Order = 7)]
        public List<SharedFilesTo> sharedfiles { get; set; }
        [DataMember(Order = 8)]
        public string sharedpassword { get; set; }
    }

    /// <summary>
    /// 用户类User
    /// </summary>
    [DataContract(Namespace = "http://coderzh.cnblogs.com")]
	public class User
	{
        [DataMember(Order = 0)]
		public string image { get; set; }
        [DataMember(Order = 1)]
		public string name { get; set; }
        [DataMember(Order = 2)]
		public string talk { get; set; }
        [DataMember(Order = 3)]
        public string ip { get; set; }
        [DataMember(Order = 4)]
        public string HostName { get; set; }
	}
    /// <summary>
    /// 共享文件类SharedFiles (用于显示)
    /// </summary>
    [DataContract(Namespace = "http://coderzh.cnblogs.com")]
    public class SharedFiles
    {
        [DataMember(Order = 0)]
        public string ip { get; set; }
        [DataMember(Order = 1)]
        public string username { get; set; }
        [DataMember(Order = 2)]
        public ImageSource image { get; set; }
        [DataMember(Order = 3)]
        public string name { get; set; }
        [DataMember(Order = 4)]
        public string size { get; set; }
        [DataMember(Order = 5)]
        public string filepath { get; set; }
        [DataMember(Order = 6)]
        public DownloadWay SharedWay { get; set; }  
    }
    /// <summary>
    /// 共享文件类SharedFilesTo (用于发送)
    /// </summary>
    [DataContract(Namespace = "http://coderzh.cnblogs.com")]
    public class SharedFilesTo
    {
        [DataMember(Order = 0)]
        public string ip { get; set; }
        [DataMember(Order = 1)]
        public string username { get; set; }
        [DataMember(Order = 2)]
        public string name { get; set; }
        [DataMember(Order = 3)]
        public string filepath { get; set; }
        [DataMember(Order = 4)]
        public string size { get; set; }
        [DataMember(Order = 5)]
        public DownloadWay SharedWay { get; set; }  
    }
    /// <summary>
    /// 字体类Font
    /// </summary>
    [DataContract(Namespace = "http://coderzh.cnblogs.com")]
    public class Font
    {
        [DataMember(Order = 0)]
        public String Fontfamily { get; set; }
        [DataMember(Order = 1)]
        public int Fontsize { get; set; }
        [DataMember(Order = 2)]
        public bool Fontstyle { get; set; }
        [DataMember(Order = 3)]
        public bool Fontweight { get; set; }
        [DataMember(Order = 4)]
        public String Fontcolor { get; set; }
        [DataMember(Order = 5)]
        public String Underline { get; set; } 
    }
    public class SendFile
    {
        public Socket SendSocket { get; set; }
        public String SendPath { get; set; }
    }
    /// <summary>
    /// 信息类型枚举
    /// </summary>
    public enum MessageID : int { 
        UserCheckIn, 
        UserCheckOut, 
        UserSendMessage, 
        UserBeIn, 
        UserSendFile,
        UserRecvFile, 
        UserRefuseFile, 
        UserCancelFile,
        UserGetSharedFiles,
        UserSharedFiles,
        UserGetSharedFile,
        UserGetAllFiles,
        UserSharedALLFiles
    };
    /// <summary>
    /// 下载方式枚举
    /// </summary>
    public enum DownloadWay : int
    {
        NeedNothing,
        NeedPassword
    };
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int BORDER = 3;
        private const int AUTOHIDETIME = 50;
        private const int TOPPOINT = -597;          //上缩窗口的顶部坐标

        MiniWindow miniwindow = null;               //迷你框对象
        public SettingWindow settingwindow = null;

        private Location _Location = Location.None;
        private bool _IsHidded = false;
        private DispatcherTimer _AutoHideTimer = null;

		private const int WM_NCHITTEST = 0x0084;
        private readonly int agWidth = 12;          //拐角宽度
        private readonly int bThickness = 4;        //边框宽度
        private Point mousePoint = new Point();     //鼠标坐标

        public System.Windows.Forms.NotifyIcon notifyIcon = null;  //托盘对象
        private System.Drawing.Icon NormalIcon;                     //正常托盘图标
        private System.Drawing.Icon MessageIcon;                    //来消息的托盘图标
        public List<Message> GetMsgList = new List<Message>();      //未显示消息列表

        public List<User> userlist = new List<User>();     //用户列表
        public List<TalkWindow> talklist = new List<TalkWindow>(); 
        public User Hoster;                                //本地用户信息

        private const int PortSend = 8182;          //默认发送端口号
        private const int PortServ = 8384;          //默认接收端口号
        private UdpClient udpClient = null;         //发送UdpClient
        private UdpClient udpReceiver = null;       //接收UdpClient
        Thread receiveThread;                       //接收线程
        Thread updateThread;                        //更新用户线程
        bool receiveThreadIsToRun = true;           //是否开始接收
        string ReceiverIP;                          //接到的对方IP

        public delegate void NextPrimeDelegate();   //定义委托

        public Font MyFont;                         //我的字体

        public string BackgroundImage = "Images/theme/background.jpg";  //背景
        public double WinOpacity = 1;                                   //主窗体透明度
        public double ForeOpacity = 1;                                  //前景透明度

        public List<SharedFilesTo> MySharedFiles = new List<SharedFilesTo>();
        public string SharedDir;                                        //共享路径
        public string DownedDir;                                        //下载路径
        public DownloadWay SharedWay;                                   //共享方式
        public string DownPassword;                                     //下载密码
        public List<SharedFiles> AllFiles;
        private const int Port = 8283;

        Shell32.SHFILEINFO shfileinfo = new Shell32.SHFILEINFO();

        public MainWindow()
        {
            InitializeComponent();

            //各种初始化
            InitialALL();
            
            ShowSharedFile();

            //更新用户
            UpdateUser();
        }

        #region 初始化本地信息
        private void InitialALL()
        {
            //初始化托盘
            InitialTray();
            //初始化通讯UDP
            InitialSocket();
            //初始化本地用户信息
            InitialHoster();
            //初始化字体
            InitialFont();
            //初始化共享文件
            InitialShared();
            //初始化本地设置
            InitialMyConfig();
            //初始化主题设置
            UpdateShowMySkin();
            //将本地信息显示出来
            UpdateShowMyMsg();

            //至于最前
            //Topmost = true;

            //窗口缩进
            this._AutoHideTimer = new DispatcherTimer();
            this._AutoHideTimer.Interval = TimeSpan.FromMilliseconds(AUTOHIDETIME);
            this._AutoHideTimer.Tick += new EventHandler(AutoHideTimer_Tick);
            //防止最大化全屏
            FullScreenManager.RepairWpfWindowFullScreenBehavior(this);
            //隐藏任务栏里的图标
            this.ShowInTaskbar = false;
        }
        private void InitialMyConfig()
        {
            if (File.Exists("MyConfig.xml") == false)
            {
                if (Directory.Exists(System.Environment.CurrentDirectory + "\\YatooShare") == false)
                {
                    Directory.CreateDirectory(System.Environment.CurrentDirectory + "\\YatooShare");
                }
                return;
            }
            
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.Load("MyConfig.xml");

            XmlNode xn = xmlDoc.SelectSingleNode("Config");

            XmlNodeList xnl = xn.ChildNodes;

            foreach (XmlNode xnf in xnl)
            {
                XmlElement xe = (XmlElement)xnf;

                //MessageBox.Show(xe.Name + ":" + xe.InnerText);

                switch (xe.Name)
                {
                    case "name":
                        Hoster.name = xe.InnerText;
                        break;
                    case"image":
                        Hoster.image = xe.InnerText;
                        break;
                    case "talk":
                        Hoster.talk = xe.InnerText;
                        break;
                    case "Fontfamily":
                        MyFont.Fontfamily = xe.InnerText;
                        break;
                    case "Fontsize":
                        MyFont.Fontsize = Int32.Parse(xe.InnerText);
                        break;
                    case "Fontstyle":
                        if (xe.InnerText == "True")
                        {
                            MyFont.Fontstyle = true;
                        }
                        else
                        {
                            MyFont.Fontstyle = false;
                        }
                        break;
                    case "Fontweight":
                        if (xe.InnerText == "True")
                        {
                            MyFont.Fontweight = true;
                        }
                        else
                        {
                            MyFont.Fontweight = false;
                        }
                        break;
                    case "Fontcolor":
                        MyFont.Fontcolor = xe.InnerText;
                        break;
                    case "BackgroundImage":
                        BackgroundImage = xe.InnerText;
                        break;
                    case "WinOpacity":
                        WinOpacity = Double.Parse(xe.InnerText);
                        break;
                    case "ForeOpacity":
                        ForeOpacity = Double.Parse(xe.InnerText);
                        break;
                    case "SharedDir":
                        SharedDir = xe.InnerText;
                        break;
                    case "DownedDir":
                        DownedDir = xe.InnerText;
                        break;
                    case "SharedWay":
                        if (xe.InnerText == "NeedNothing")
                        {
                            SharedWay = DownloadWay.NeedNothing;
                        }
                        else if (xe.InnerText == "NeedPassword")
                        {
                            SharedWay = DownloadWay.NeedPassword;
                        }
                        break;
                    case "DownPassword":
                        DownPassword = xe.InnerText;
                        break;
                }
            } 
        }
        private void InitialShared()
        {
            SharedDir = System.Environment.CurrentDirectory + "\\YatooShare";
            DownedDir = System.Environment.CurrentDirectory + "\\YatooShare";
            SharedWay = DownloadWay.NeedNothing;
            DownPassword = "";
        }
        private void InitialHoster()
        {
            Hoster = new User();
            Hoster.image = "Images/portrait/12.jpg";
            Hoster.name = Dns.GetHostName();
            Hoster.HostName = Dns.GetHostName();
            IPHostEntry ipEntry = Dns.GetHostByName(Hoster.HostName); //取得本机IP
            Hoster.ip = ipEntry.AddressList[0].ToString(); //假设本地主机为单网卡
            Hoster.talk = "要努力奋斗，养活一家老小";
        }
        private void InitialFont()
        {
            MyFont = new Font();
            MyFont.Fontfamily = "楷体";
            MyFont.Fontsize = 15;
            MyFont.Fontstyle = true;
            MyFont.Fontweight = true;
            MyFont.Fontcolor = "黑色";
        }
        public void UpdateShowMyMsg()
        {
            Username.Content = Hoster.name;

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(Hoster.image, UriKind.Relative);
            bitmap.EndInit();
            Logo.Source = bitmap;

            TalkBox.Text = Hoster.talk;
        }
        public void UpdateShowMySkin()
        {
            if (File.Exists(BackgroundImage) == true)
            {
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.UriSource = new Uri(BackgroundImage, UriKind.RelativeOrAbsolute);
                bi.EndInit();
                ImageBrush ib = new ImageBrush(bi);
                ib.Stretch = Stretch.UniformToFill;
                ib.Opacity = WinOpacity;
                BorderBackground.Background = ib;
            }
            else
            {
                BackgroundImage = "Images/theme/background.jpg";
            }
            foreground.Opacity = ForeOpacity;
        }
        
        #endregion

        #region 窗口缩进
        void AutoHideTimer_Tick(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                return;
            }
            POINT p;
            if (!GetCursorPos(out p))
            {
                return;
            }

            if (p.x >= this.Left && p.x <= (this.Left + this.ActualWidth)
                && p.y >= this.Top && p.y <= (this.Top + this.ActualHeight))
            {
                this.ShowWindow();
            }
            else
            {
                this.HideWindow();
            }
        }

        protected override void OnLocationChanged(EventArgs e)
        {
            
            if (!this._IsHidded)
            {
                if (this.Top <= 0 && this.Left <= 0)
                {
                    this._Location = Location.LeftTop;
                    this.HideWindow();
                }
                else if (this.Top <= 0 && this.Left >= SystemParameters.VirtualScreenWidth - this.ActualWidth)
                {
                    this._Location = Location.RightTop;
                    this.HideWindow();
                }
                else if (this.Top <= 0)
                {
                    this._Location = Location.Top;
                    this.HideWindow();
                }
                else
                {
                    this._Location = Location.None;
                }
            }
            base.OnLocationChanged(e);
        }

        enum Location
        {
            None,
            Top,
            LeftTop,
            RightTop
        }

        private void ShowWindow()
        {
            if (this._IsHidded)
            {
                switch (this._Location)
                {
                    case Location.Top:
                    case Location.LeftTop:
                    case Location.RightTop:
                        this.Top = 0;
                        this.Topmost = false;
                        this._IsHidded = false;
                        this.UpdateLayout();
                        break;
                    case Location.None:
                        break;
                }
            }
        }

        private void HideWindow()
        {
            if (!this._IsHidded)
            {
                switch (this._Location)
                {
                    case Location.Top:
                        this.Top = BORDER - this.ActualHeight;
                        this.Topmost = true;
                        this._IsHidded = true;
                        this._AutoHideTimer.Start();
                        break;
                    case Location.LeftTop:
                        this.Left = 0;
                        this.Top = BORDER - this.ActualHeight;
                        this.Topmost = true;
                        this._IsHidded = true;
                        this._AutoHideTimer.Start();
                        break;
                    case Location.RightTop:
                        this.Left = SystemParameters.VirtualScreenWidth - this.ActualWidth;
                        this.Top = BORDER - this.ActualHeight;
                        this.Topmost = true;
                        this._IsHidded = true;
                        this._AutoHideTimer.Start();
                        break;
                    case Location.None:
                        break;
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;
            public POINT(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetCursorPos(out POINT pt);
        #endregion

        #region 系统按钮

        private void window_move(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if ((e.GetPosition(this).Y < 40))
                {
                    this.DragMove();
                }
            }
        }
        private void OpenTalkWindowClick(object sender, RoutedEventArgs e)
        {
            ListBoxItem user = (ListBoxItem)sender;

            MessageBox.Show(user.Name);
        }
        private void ListDoubleClick(object sender, MouseEventArgs e)
        {
            ListBox user = (ListBox)sender;

            int index = user.SelectedIndex;
            int i = 0;
            if (index < user.Items.Count && index != -1)
            {
                for (i = 0; i < talklist.Count; i++)
                {
                    if (talklist[i].Title == userlist[index].ip)
                    {
                        break;
                    }
                }
                if (i == talklist.Count)
                {
                    TalkWindow talkwindow = new TalkWindow(userlist[index],MyFont,this);
                    talklist.Add(talkwindow);
                    if (GetMsgList.Count != 0)
                    {
                        for (i = GetMsgList.Count-1; i >= 0; i--)
                        {
                            if (GetMsgList[i].user.ip == GetMsgList[0].user.ip)
                            {
                                if (GetMsgList[i].ID == MessageID.UserSendFile)
                                {
                                    talkwindow.AddRecvFile(GetMsgList[i].file, GetMsgList[i].filesize);
                                }
                                else
                                {
                                    talkwindow.ShowMessage(GetMsgList[i].content, GetMsgList[i].font);
                                }
                                GetMsgList.RemoveAt(i);
                            }
                        }
                    }
                    talkwindow.Show();
                    talkwindow.Activate();
                    //请求获得共享文件
                    GetSharedFiles(userlist[index].ip);
                }
                else
                {
                    if (talklist[i].WindowState == WindowState.Minimized)
                    {
                        talklist[i].WindowState = WindowState.Normal;
                    }
                    talklist[i].Activate();
                }
            }
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            UserCheckOut();
            notifyIcon.Dispose();
            SaveMyConfig();
            Environment.Exit(0);
        }

		private void MaximumWindow(object sender, RoutedEventArgs e)
        {   
            if (miniwindow == null)
            {
                miniwindow = new MiniWindow(this);
            }
            while (this.Height > 30)
            {
                this.Height -= 30;
            }
            this.Hide();
            miniwindow.InitialImage();
            miniwindow.userwindow.SetBackground();
            if (GetMsgList.Count != 0)
            {
                miniwindow.FlashImage();
            }
            miniwindow.Show();
        }
        private void MinimumWindow(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }
        private void TalkBoxGotFocusClick(object sender, RoutedEventArgs e)
        {
            TalkBox.SelectAll();
        }
        private void TalkBoxLostFocusClick(object sender, RoutedEventArgs e)
        {
            Hoster.talk = TalkBox.Text;
        }
        #endregion

        #region 改变窗体大小
        protected override void OnSourceInitialized(EventArgs e)
        {
 
 　         base.OnSourceInitialized(e);
 　　       HwndSource hwndSource = PresentationSource.FromVisual(this) as HwndSource;
 　　       if (hwndSource != null)
 　       　{
 　　　　      hwndSource.AddHook(new HwndSourceHook(this.WndProc));
 　         }
         }
        public enum HitTest:int
        {
 　　HTERROR = -2,
 　　HTTRANSPARENT = -1,
 　　HTNOWHERE = 0,
 　　HTCLIENT = 1,
 　　HTCAPTION = 2,
 　　HTSYSMENU = 3,
 　　HTGROWBOX = 4,
 　　HTSIZE = HTGROWBOX,
 　　HTMENU = 5,
  　　HTHSCROLL = 6,
 　　HTVSCROLL = 7,
 　　HTMINBUTTON = 8,
 　　HTMAXBUTTON = 9,
 　　HTLEFT = 10,
 　　HTRIGHT = 11,
 　　HTTOP = 12,
 　　HTTOPLEFT = 13,
 　　HTTOPRIGHT = 14,
 　　HTBOTTOM = 15,
 　　HTBOTTOMLEFT = 16,
 　　HTBOTTOMRIGHT = 17,
 　　HTBORDER = 18,
 　　HTREDUCE = HTMINBUTTON,
 　　HTZOOM = HTMAXBUTTON,
 　　HTSIZEFIRST = HTLEFT,
 　　HTSIZELAST = HTBOTTOMRIGHT,
 　　HTOBJECT = 19,
 　　HTCLOSE = 20,
 　　HTHELP = 21,
 }
         protected virtual IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
         {
             switch (msg)
 　　{
 　　　　case WM_NCHITTEST:
 　　　　this.mousePoint.X = (lParam.ToInt32() &0xFFFF);
 　　　　this.mousePoint.Y = (lParam.ToInt32() >> 16);
 
             if (this.mousePoint.Y - this.Top <= this.agWidth
               　　　　　&& this.mousePoint.X - this.Left <= this.agWidth)
 　　　　{
 　　　　　handled = true;
 　　　　　return new IntPtr((int)HitTest.HTTOPLEFT);
 　　　　}
 　　　　// 窗口左下角　　
　　　　 else if (this.ActualHeight + this.Top - this.mousePoint.Y <= this.agWidth
 　　　　　&& this.mousePoint.X - this.Left <= this.agWidth)
 　　　{
 　　　　　　handled = true;
 　　　　　　return new IntPtr((int)HitTest.HTBOTTOMLEFT);
 　　　　}
 　　　　// 窗口右上角
 　　　　 else if (this.mousePoint.Y - this.Top <= this.agWidth
 　　　　　　&& this.ActualWidth + this.Left - this.mousePoint.X <= this.agWidth)
 　　　　{
 　　　　　　handled = false;
 　　　　　　return new IntPtr((int)HitTest.HTTOPRIGHT);
 　　　　}
 　　　　// 窗口右下角
 　　　　 else if (this.ActualWidth + this.Left - this.mousePoint.X <= this.agWidth
 　　　　　　&& this.ActualHeight + this.Top - this.mousePoint.Y <= this.agWidth)
 　　　　{
 　　　　　　handled = true;
 　　　　　　return new IntPtr((int)HitTest.HTBOTTOMRIGHT);
 　　　　}
 　　　　// 窗口左侧
 　　　　 else if (this.mousePoint.X - this.Left <= this.bThickness)
 　　　　{ 
 　　　　　　handled = true;
 　　　　　　return new IntPtr((int)HitTest.HTLEFT);
 　　　　}
 　　　　// 窗口右侧
 　　　　 else if (this.ActualWidth + this.Left - this.mousePoint.X <= this.bThickness)
 　　　　{
 　　　　　　handled = true;
 　　　　　　return new IntPtr((int)HitTest.HTRIGHT);
 　　　　}
 　　　　// 窗口上方
 　　　　 else if (this.mousePoint.Y - this.Top <= this.bThickness)
 　　　　{
 　　　　　　handled = true;
 　　　　　　return new IntPtr((int)HitTest.HTTOP);
 　　　　}
 　　　　// 窗口下方
 　　　　 else if (this.ActualHeight + this.Top - this.mousePoint.Y <= this.bThickness)
 　　　　{
 　　　　　　handled = true;
 　　　　　　return new IntPtr((int)HitTest.HTBOTTOM);
 　　　　}
 　　　　else // 窗口移动
 　　　　{
 　　　　　　handled = false;
 　　　　　　return new IntPtr((int)HitTest.HTCAPTION);
 　　　    　}

 　        　}
 　　        return IntPtr.Zero;
         }  
        #endregion

        #region 托盘
        public void InitialTray()
        {
            if (File.Exists("Images/Yatoo.ico") == false || File.Exists("Images/yatoomsg.ico") == false)
            {
                MessageBox.Show("没有找到托盘图标，无法生成托盘，请不要删除配置图片，在路径是本地目录下Images/Yatoo.ico和Images/yatoomsg.ico");
                Environment.Exit(0);
            }
            //设置托盘的各个属性
            NormalIcon = new System.Drawing.Icon("Images/Yatoo.ico");
            MessageIcon = new System.Drawing.Icon("Images/yatoomsg.ico");
            notifyIcon = new System.Windows.Forms.NotifyIcon();
            notifyIcon.BalloonTipText = "程序开始运行";
            notifyIcon.Text = "托盘图标";
            notifyIcon.Icon = NormalIcon;
            notifyIcon.Visible = true;
            
            //notifyIcon.ShowBalloonTip(2000);
            notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(notifyIcon_MouseClick);

            //设置菜单项
            System.Windows.Forms.MenuItem menu1 = new System.Windows.Forms.MenuItem("菜单项1");
            System.Windows.Forms.MenuItem menu2 = new System.Windows.Forms.MenuItem("菜单项2");
            System.Windows.Forms.MenuItem menu = new System.Windows.Forms.MenuItem("菜单", new System.Windows.Forms.MenuItem[] { menu1 , menu2 });

            //退出菜单项
            System.Windows.Forms.MenuItem exit = new System.Windows.Forms.MenuItem("退出");
            exit.Click += new EventHandler(exit_Click);

            //关联托盘控件
            System.Windows.Forms.MenuItem[] childen = new System.Windows.Forms.MenuItem[] { menu , exit };
            notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu(childen);

            //窗体状态改变时候触发
            this.StateChanged += new EventHandler(SysTray_StateChanged);

        }
        private void FlashIcon()
        {
            while (GetMsgList.Count != 0)
            {
                notifyIcon.Icon = MessageIcon;

                Thread.Sleep(800);

                notifyIcon.Icon = NormalIcon;

                Thread.Sleep(800);
            }
        }
        ///
        /// 窗体状态改变时候触发
        ///

        private void SysTray_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
            {
                this.Visibility = Visibility.Hidden;
            }
        }

        ///
        /// 退出选项
        ///

        private void exit_Click(object sender, EventArgs e)
        {
            notifyIcon.Dispose();
            SaveMyConfig();
            Environment.Exit(0);

        }

        ///
        /// 鼠标单击
        ///

        private void notifyIcon_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (GetMsgList.Count != 0)
                {
                    TalkWindow talkwindow = new TalkWindow(GetMsgList[0].user, MyFont, this);
                    for (int i = GetMsgList.Count-1; i >= 0; i--)
                    {
                        if (GetMsgList[i].user.ip == GetMsgList[0].user.ip)
                        {
                            if (GetMsgList[i].ID == MessageID.UserSendFile)
                            {
                                talkwindow.AddRecvFile(GetMsgList[i].file, GetMsgList[i].filesize);
                            }
                            else
                            {
                                talkwindow.ShowMessage(GetMsgList[i].content, GetMsgList[i].font);
                            }
                            GetMsgList.RemoveAt(i);
                        }
                    }
                    talklist.Add(talkwindow);
                    talkwindow.Show();
                    talkwindow.Activate();
                    //请求获得共享文件
                    GetSharedFiles(talkwindow.TalkUser.ip);
                }
                else if (this.Visibility == Visibility.Visible)
                {
                    this.Visibility = Visibility.Visible;
                    this.Activate();
                    if (this.Top == TOPPOINT)
                    {
                        this.ShowWindow();
                        this.Top = 1;
                    }
                    else if (this.Top == 1)
                    {
                        this.Top = 0;
                    }
                }
                else if(miniwindow != null)
                {
                    miniwindow.WindowState = System.Windows.WindowState.Normal;
                    miniwindow.Visibility = Visibility.Visible;
                    miniwindow.Activate();
                }

            }
        }
        #endregion

        #region 网络通讯
        //通讯初始化
        private void InitialSocket()
        {
            if (udpClient == null)
                udpClient = new UdpClient(PortServ);
            if (udpReceiver == null)
                udpReceiver = new UdpClient(PortSend, AddressFamily.InterNetwork);

            receiveThread = new Thread(new ThreadStart(ReceiveThread));
            receiveThread.IsBackground = true;
            receiveThread.SetApartmentState(ApartmentState.STA);
            receiveThread.Start();
        }
        //接收线程
        private void ReceiveThread()
        {
            while (receiveThreadIsToRun)
            {
                if (udpReceiver != null)
                {
                    IPEndPoint remoteHost = new IPEndPoint(IPAddress.Any, 0);
                    try
                    {
                        byte[] Data = udpReceiver.Receive(ref remoteHost);
                        //MessageBox.Show(Encoding.Unicode.GetString(Data) + "\r\n" + remoteHost.Address.ToString() + "\r\n" + remoteHost.Port.ToString());
                        var mStream = new MemoryStream(Data);
                        var serializer = new DataContractJsonSerializer(typeof(Message));
                        Message ReadMessage = (Message)serializer.ReadObject(mStream);
                        byte[] bytes;
                        int i = 0;
                        bool BreakFlag = false;
                        switch (ReadMessage.ID)
                        {
                            //用户登入
                            case MessageID.UserCheckIn:
                                //MessageBox.Show(ReceiverIP + "|" + Hoster.ip);
                                if (ReadMessage.user.ip != Hoster.ip)
                                {
                                    bytes = Encoding.Unicode.GetBytes(MakeUserPacket(MessageID.UserBeIn));
                                    udpClient.Send(bytes, bytes.Length, ReadMessage.user.ip, PortSend);
                                }
                                else
                                {
                                    ReadMessage.user.name += "(自己)";
                                }
                                for (i = 0; i < userlist.Count; i++)
                                {
                                    if (userlist[i].ip == ReadMessage.user.ip)
                                    {
                                        break;
                                    }
                                }
                                if (i == userlist.Count)
                                {
                                    userlist.Add(ReadMessage.user);
                                    if (miniwindow != null)
                                    {
                                        miniwindow.userwindow.UpdateUser();
                                    }
                                    User_list.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new NextPrimeDelegate(UpdateUserList));
                                }
                                break;
                            //用户退出
                            case MessageID.UserCheckOut:
                                for (i = 0; i < userlist.Count; i++)
                                {
                                    if (userlist[i].ip == ReadMessage.user.ip)
                                    {
                                        userlist.RemoveAt(i);
                                    }
                                }
                                if (miniwindow != null)
                                {
                                    miniwindow.userwindow.UpdateUser();
                                }
                                User_list.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new NextPrimeDelegate(UpdateUserList));

                                //关闭已经打开的对话框
                                for (i = 0; i < talklist.Count; i++)
                                {
                                    if (talklist[i].TalkUser.ip == ReadMessage.user.ip)
                                    {
                                        talklist[i].Dispatcher.Invoke(new Action(() =>
                                        {
                                            //MessageBox.Show("用户已退出，窗口自动关闭");
                                            talklist[i].Close();
                                        }));
                                        talklist.RemoveAt(i);
                                    }
                                }
                                break;
                            //用户发来文本信息
                            case MessageID.UserSendMessage:
                                PlayMessageSound();
                                for (i = 0; i < talklist.Count; i++)
                                {
                                    this.talklist[i].Dispatcher.Invoke(new Action(() =>
                                    {
                                        if (talklist[i].Title == ReadMessage.user.ip)
                                        {
                                            talklist[i].ShowMessage(ReadMessage.content, ReadMessage.font);
                                            BreakFlag = true;
                                        }
                                    }));
                                    if (BreakFlag == true)
                                    {
                                        break;
                                    }
                                }
                                //如果没有现有对话框的话
                                if (i == talklist.Count)
                                {
                                    GetMsgList.Add(ReadMessage);
                            
                                    if (GetMsgList.Count == 1)
                                    {
                                        //闪动窗口托盘线程
                                        Thread FlashIconThread = new Thread(new ThreadStart(FlashIcon));
                                        FlashIconThread.Start();
                                    }
                                    notifyIcon.BalloonTipText = ReadMessage.user.name + "发来消息";
                                    notifyIcon.ShowBalloonTip(1000);

                                    if (this.Visibility == Visibility.Hidden && miniwindow != null)
                                    {
                                        miniwindow.FlashImage();
                                    }
                                    //MessageBox.Show(ReadMessage.user.name + "发来信息");
                                }
                                break;
                            //用户回复在线
                            case MessageID.UserBeIn:
                                userlist.Add(ReadMessage.user);
                                User_list.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new NextPrimeDelegate(UpdateUserList));
                                break;
                            //对方发送文件过来
                            case MessageID.UserSendFile:
                                PlayMessageSound();
                                for (i = 0; i < talklist.Count; i++)
                                {
                                    this.talklist[i].Dispatcher.Invoke(new Action(() =>
                                    {
                                        if (talklist[i].Title == ReadMessage.user.ip)
                                        {
                                            talklist[i].AddRecvFile(ReadMessage.file, ReadMessage.filesize);
                                            BreakFlag = true;
                                        }
                                    }));
                                    if (BreakFlag == true)
                                    {
                                        break;
                                    }
                                }
                                //如果没有现有对话框的话
                                if (i == talklist.Count)
                                {
                                    //!!!!!!!!!!!!!!!!!!!!!!!!!!多线程对待,会导致资源堵塞
                                    //Thread CreateTalkWindowToFileThread = new Thread(new ParameterizedThreadStart(CreateTalkWindiwToFile)); //创建进程
                                    //CreateTalkWindowToFileThread.SetApartmentState(ApartmentState.STA);
                                    //CreateTalkWindowToFileThread.Start(ReadMessage); 
                                    //MessageBox.Show(ReadMessage.file+"|"+ReadMessage.filesize);
                                    //保存文件信息，提示用户有文件发入
                                    GetMsgList.Add(ReadMessage);

                                    if (GetMsgList.Count == 1)
                                    {
                                        //闪动窗口托盘线程
                                        Thread FlashIconThread = new Thread(new ThreadStart(FlashIcon));
                                        FlashIconThread.Start();
                                    }
                                    notifyIcon.BalloonTipText = ReadMessage.user.name + "发来文件:" + ReadMessage.file;
                                    notifyIcon.ShowBalloonTip(1000);

                                    if (this.Visibility == Visibility.Hidden && miniwindow != null)
                                    {
                                        miniwindow.FlashImage();
                                    }
                                }
                                break;
                            //对方开始接收文件，本地开始发送
                            case MessageID.UserRecvFile:
                                for (i = 0; i < talklist.Count; i++)
                                {
                                    this.talklist[i].Dispatcher.Invoke(new Action(() =>
                                    {
                                        if (talklist[i].Title == ReadMessage.user.ip)
                                        {
                                            talklist[i].listen(ReadMessage.fileid);
                                            BreakFlag = true;
                                        }
                                    }));
                                    if (BreakFlag == true)
                                    {
                                        break;
                                    }
                                }
                                break;
                            //拒绝接收文件
                            case MessageID.UserRefuseFile:
                                for (i = 0; i < talklist.Count; i++)
                                {
                                    this.talklist[i].Dispatcher.Invoke(new Action(() =>
                                    {
                                        if (talklist[i].Title == ReadMessage.user.ip)
                                        {
                                            talklist[i].CloseSendFile(ReadMessage.fileid);
                                            BreakFlag = true;
                                        }
                                    }));
                                    if (BreakFlag == true)
                                    {
                                        break;
                                    }
                                }
                                break;
                            //回复关闭文件信息
                            case MessageID.UserCancelFile:
                                for (i = 0; i < talklist.Count; i++)
                                {
                                    this.talklist[i].Dispatcher.Invoke(new Action(() =>
                                    {
                                        if (talklist[i].Title == ReadMessage.user.ip)
                                        {
                                            talklist[i].CloseSendFile();
                                            BreakFlag = true;
                                        }
                                    }));
                                    if (BreakFlag == true)
                                    {
                                        break;
                                    }
                                }
                                break;
                            //请求获得共享文件
                            case MessageID.UserGetSharedFiles:
                                SharedFileTo(ReadMessage.user.ip, MySharedFiles);
                                break;
                            //获得共享文件
                            case MessageID.UserSharedFiles:
                                for (i = 0; i < talklist.Count; i++)
                                {
                                    this.talklist[i].Dispatcher.Invoke(new Action(() =>
                                    {
                                        if (talklist[i].Title == ReadMessage.user.ip)
                                        {
                                            talklist[i].GetSharedFiles(ReadMessage.sharedfiles,ReadMessage.sharedpassword);
                                            BreakFlag = true;
                                        }
                                    }));
                                    if (BreakFlag == true)
                                    {
                                        break;
                                    }
                                }
                                //如果没有现有对话框的话,显示在主框中
                                if (i == talklist.Count)
                                {
                               
                                }
                                break;
                            case MessageID.UserGetSharedFile:
                                Thread t = new Thread(new ParameterizedThreadStart(listen)); //创建进程
                                t.Start(ReadMessage);                                        //开始进程
                                break;
                            case MessageID.UserGetAllFiles:
                                AllFileTo(ReadMessage.user.ip, MySharedFiles);
                                break;
                            case MessageID.UserSharedALLFiles:
                                if (AllFiles != null)
                                {
                                    this.Down_list.Dispatcher.Invoke(new Action(() =>
                                    {
                                        if (ReadMessage.sharedfiles == null)
                                        {
                                            MessageBox.Show("12");
                                        }
                                        for (i = 0; i < ReadMessage.sharedfiles.Count; i++)
                                        {
                                            SharedFiles file = new SharedFiles();
                                            file.ip = ReadMessage.sharedfiles[i].ip;
                                            file.username = ReadMessage.sharedfiles[i].username;
                                            file.name = ReadMessage.sharedfiles[i].name;
                                            file.SharedWay = ReadMessage.sharedfiles[i].SharedWay;
                                            file.size = ReadMessage.sharedfiles[i].size;
                                            file.filepath = ReadMessage.sharedfiles[i].filepath;
                                            file.image = GetFileImage(file.name);
                                            AllFiles.Add(file);

                                        }
                                        Down_list.ItemsSource = null;
                                        Down_list.ItemsSource = AllFiles;
                                    }));
                                }
                                break;
                            default:
                                break;
                        }
                     }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
            }
        }
        //打开文件窗口
        private void CreateTalkWindiwToFile(Object msg)
        {
            Message ReadMessage = (Message)msg;
            int i = 0;
            TalkWindow talkwindow = new TalkWindow(ReadMessage.user, MyFont, this);
            talklist.Add(talkwindow);
            if (GetMsgList.Count != 0)
            {
                for (i = 0; i < GetMsgList.Count; i++)
                {
                    if (GetMsgList[i].user.ip == GetMsgList[0].user.ip)
                    {
                        talkwindow.ShowMessage(GetMsgList[i].content, GetMsgList[i].font);
                        GetMsgList.RemoveAt(i);
                        i--;
                    }
                }
            }
            notifyIcon.BalloonTipText = ReadMessage.user.name + "发来文件";
            notifyIcon.ShowBalloonTip(1000);

            talkwindow.AddRecvFile(ReadMessage.file, ReadMessage.filesize);
            talkwindow.Show();
            talkwindow.Activate();
            System.Windows.Threading.Dispatcher.Run();
        }
        //同意接收文件
        public bool AcceptRecvFile(string ip, int id)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(MakeUserPacket(MessageID.UserRecvFile, id));
            try
            {
                udpClient.Send(bytes, bytes.Length, ip, PortSend);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        //发送文件信息
        public bool SendFileTo(string ip, string filepath, long filesize)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(MakeUserPacket(MessageID.UserSendFile, filepath, filesize));
            try
            {
                udpClient.Send(bytes, bytes.Length, ip, PortSend);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public bool ReCloseSendFile(string ip, int id)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(MakeUserPacket(MessageID.UserCancelFile, id));
            try
            {
                udpClient.Send(bytes, bytes.Length, ip, PortSend);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public bool CloseSendFile(string ip, int id)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(MakeUserPacket(MessageID.UserRefuseFile, id));
            try
            {
                udpClient.Send(bytes, bytes.Length, ip, PortSend);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        //发送所有共享文件信息
        public bool GetAlLSharedFiles(string ip)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(MakeUserPacket(MessageID.UserGetAllFiles));
            try
            {
                udpClient.Send(bytes, bytes.Length, ip, PortSend);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        //发送共享文件信息
        public bool GetSharedFiles(string ip)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(MakeUserPacket(MessageID.UserGetSharedFiles));
            try
            {
                udpClient.Send(bytes, bytes.Length, ip, PortSend);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public bool SharedFileTo(String ip, List<SharedFilesTo> sharedfiles)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(MakeUserPacket(MessageID.UserSharedFiles, sharedfiles));
            try
            {
                udpClient.Send(bytes, bytes.Length, ip, PortSend);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public bool AllFileTo(String ip, List<SharedFilesTo> sharedfiles)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(MakeUserPacket(MessageID.UserSharedALLFiles, sharedfiles));
            try
            {
                udpClient.Send(bytes, bytes.Length, ip, PortSend);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public bool GetSharedFile(string ip,string filepath)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(MakeUserPacket(MessageID.UserGetSharedFile, filepath));
            try
            {
                udpClient.Send(bytes, bytes.Length, ip, PortSend);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        //发送信息
        public bool SendMsgTo(string ip, string message)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(MakeUserPacket(MessageID.UserSendMessage,message));
            try
            {
                udpClient.Send(bytes, bytes.Length, ip, PortSend);
            }
            catch (Exception ex)
            {
                MessageBox.Show("您发送的图片过大，请使用文件发送方式发送图片");
                return false;
            }
            return true;
        }
        //刷新用户线程
        private void updatethread()
        {
            
            string tempstr = Hoster.ip.Substring(0, Hoster.ip.LastIndexOf('.'));
            string tempstr2 = tempstr.Substring(0, tempstr.LastIndexOf("."));
            string tempstr3 = tempstr2.Substring(0, tempstr2.LastIndexOf("."));
            byte[] bytes = Encoding.Unicode.GetBytes(MakeUserPacket(MessageID.UserCheckIn));
            //广播子网掩码为255.255.255.0的所有IP
            tempstr += ".255";
            udpClient.Send(bytes, bytes.Length, tempstr, PortSend);
            if (Hoster.ip != "127.0.0.1")
            {
                //广播子网掩码为255.255.0.0的所有IP
                tempstr2 += ".255.255";
                udpClient.Send(bytes, bytes.Length, tempstr2, PortSend);
                //广播子网掩码为255.0.0.0的所有IP
                tempstr3 += ".255.255.255";
                udpClient.Send(bytes, bytes.Length, tempstr3, PortSend);
            }
            
        }
        //用户退出
        public void UserCheckOut()
        {
            
            string tempstr = Hoster.ip.Substring(0, Hoster.ip.LastIndexOf('.'));
            string tempstr2 = tempstr.Substring(0, tempstr.LastIndexOf("."));
            string tempstr3 = tempstr2.Substring(0, tempstr2.LastIndexOf("."));
            byte[] bytes = Encoding.Unicode.GetBytes(MakeUserPacket(MessageID.UserCheckOut));
            //广播子网掩码为255.255.255.0的所有IP
            tempstr += ".255";
            udpClient.Send(bytes, bytes.Length, tempstr, PortSend);
            if (Hoster.ip != "127.0.0.1")
            {
                //广播子网掩码为255.255.0.0的所有IP
                tempstr2 += ".255.255";
                udpClient.Send(bytes, bytes.Length, tempstr2, PortSend);
                //广播子网掩码为255.0.0.0的所有IP
                tempstr3 += ".255.255.255";
                udpClient.Send(bytes, bytes.Length, tempstr3, PortSend);
            }
        }
        //更新用户列表
        private void UpdateUserList()
        {
            User_list.ItemsSource = null;
            User_list.ItemsSource = userlist;
        }
        //打包想要的json信息
        private string MakeUserPacket(MessageID id)
        {
            var message = new Message()
            {
                ID = id,
                user = Hoster
            };
            var serializer = new DataContractJsonSerializer(typeof(Message));
            var stream = new MemoryStream();
            serializer.WriteObject(stream, message);

            byte[] dataBytes = new byte[stream.Length];

            stream.Position = 0;

            stream.Read(dataBytes, 0, (int)stream.Length);

            string dataString = Encoding.UTF8.GetString(dataBytes);

            return dataString;
        }
        //打包想要的json信息
        private string MakeUserPacket(MessageID id, string Msg)
        {
            var message = new Message()
            {
                ID = id,
                user = Hoster,
                content = Msg,
                font = MyFont,
            };
            var serializer = new DataContractJsonSerializer(typeof(Message));
            var stream = new MemoryStream();
            serializer.WriteObject(stream, message);

            byte[] dataBytes = new byte[stream.Length];

            stream.Position = 0;

            stream.Read(dataBytes, 0, (int)stream.Length);

            string dataString = Encoding.UTF8.GetString(dataBytes);

            return dataString;
        }
        //打包想要的json信息
        private string MakeUserPacket(MessageID id,string File, long FileSize)
        {
            var message = new Message()
            {
                ID = id,
                user = Hoster,
                font = MyFont,
                file = File,
                filesize = FileSize
            };
            var serializer = new DataContractJsonSerializer(typeof(Message));
            var stream = new MemoryStream();
            serializer.WriteObject(stream, message);

            byte[] dataBytes = new byte[stream.Length];

            stream.Position = 0;

            stream.Read(dataBytes, 0, (int)stream.Length);

            string dataString = Encoding.UTF8.GetString(dataBytes);

            return dataString;
        }
        //打包想要的json信息
        private string MakeUserPacket(MessageID id,int index)
        {
            var message = new Message()
            {
                ID = id,
                user = Hoster,
                font = MyFont,
                fileid = index
            };
            var serializer = new DataContractJsonSerializer(typeof(Message));
            var stream = new MemoryStream();
            serializer.WriteObject(stream, message);

            byte[] dataBytes = new byte[stream.Length];

            stream.Position = 0;

            stream.Read(dataBytes, 0, (int)stream.Length);

            string dataString = Encoding.UTF8.GetString(dataBytes);

            return dataString;
        }
        //打包想要的json信息
        private string MakeUserPacket(MessageID id, List<SharedFilesTo> files)
        {
            var message = new Message()
            {
                ID = id,
                user = Hoster,
                sharedfiles = files,
                sharedpassword = DownPassword
            };
            var serializer = new DataContractJsonSerializer(typeof(Message));
            var stream = new MemoryStream();
            serializer.WriteObject(stream, message);

            byte[] dataBytes = new byte[stream.Length];

            stream.Position = 0;

            stream.Read(dataBytes, 0, (int)stream.Length);

            string dataString = Encoding.UTF8.GetString(dataBytes);

            return dataString;
        }
        #endregion

        #region 刷新用户
        //更新用户列表
        public void UpdateUser()
        {
            userlist.Clear();
            updateThread = new Thread(new ThreadStart(updatethread));
            updateThread.Start();
        }
        //刷新按钮
        private void UpdateClick(object sender, RoutedEventArgs e)
        {
            UpdateUser();
        }
        #endregion

        #region 保存本地文件
        public void SaveMyConfig()
        {
            XmlTextWriter writer = new XmlTextWriter("MyConfig.xml", System.Text.Encoding.UTF8);
            //使用自动缩进便于阅读
            writer.Formatting = Formatting.Indented;
            //XML声明
            writer.WriteStartDocument();
            //书写根元素
            writer.WriteStartElement("Config");
            //添加子元素
            writer.WriteElementString("image", Hoster.image);
            writer.WriteElementString("name", Hoster.name);
            writer.WriteElementString("talk", Hoster.talk);
            writer.WriteElementString("Fontfamily", MyFont.Fontfamily);
            writer.WriteElementString("Fontsize", MyFont.Fontsize.ToString());
            writer.WriteElementString("Fontstyle", MyFont.Fontstyle.ToString());
            writer.WriteElementString("Fontweight", MyFont.Fontweight.ToString());
            writer.WriteElementString("Fontcolor", MyFont.Fontcolor);
            writer.WriteElementString("BackgroundImage", BackgroundImage);
            writer.WriteElementString("WinOpacity", WinOpacity.ToString());
            writer.WriteElementString("ForeOpacity", ForeOpacity.ToString());
            writer.WriteElementString("SharedDir", SharedDir);
            writer.WriteElementString("DownedDir", DownedDir);
            writer.WriteElementString("SharedWay", SharedWay.ToString());
            writer.WriteElementString("DownPassword", DownPassword);
            //关闭Config元素
            writer.WriteEndElement(); // 关闭元素
            //在节点间添加一些空
            writer.Close(); 
        }
        #endregion

        #region 设置组按钮
        public void Setter_Click(object sender, RoutedEventArgs e)
        {
            if (settingwindow == null)
            {
                settingwindow = new SettingWindow(this);
                settingwindow.Show();
            }
            else
            {
                settingwindow.Activate();
            }
        }
        private void Skin_Click(object sender, RoutedEventArgs e)
        {
            if (settingwindow == null)
            {
                settingwindow = new SettingWindow(this, true);
                settingwindow.Show();
            }
            else
            {
                settingwindow.Tabitem2.Focus();
                settingwindow.Activate();
            }
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (Grid_AddFriend.Visibility == Visibility.Hidden)
            {
                Grid_AddFriend.Visibility = Visibility.Visible;
            }
            else
            {
                Grid_AddFriend.Visibility = Visibility.Hidden;
            }
        }
        private void AddFriendClick(object sender, RoutedEventArgs e)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(MakeUserPacket(MessageID.UserCheckIn));
            try
            {
                udpClient.Send(bytes, bytes.Length, TextBox_AddIP.Text, PortSend);
                Grid_AddFriend.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                MessageBox.Show("输入的IP错误,请输入正确的IP格式");
            }
        }

        #endregion

        #region 共享文件
        //获得本地共享文件内容
        public void GetMyShare()
        {
            string[] strDirectorys = Directory.GetDirectories(SharedDir);
            string[] strFiles = Directory.GetFiles(SharedDir);
            //获得一层文件夹下的所有文件
            foreach (string strDirectory in strDirectorys)
            {
                string[] Files = Directory.GetFiles(strDirectory);
                foreach (string strFile in Files)
                {
                    SharedFilesTo file = new SharedFilesTo();
                    file.ip = Hoster.ip;
                    file.username = Hoster.name;
                    file.SharedWay = SharedWay;
                    file.name = strFile.Substring(strFile.LastIndexOf('\\') + 1);
                    file.size = FomatFileSize(GetFileSize(strFile));
                    file.filepath = strFile;
                    MySharedFiles.Add(file);
                }
            }
            //获得当前目录的的所有文件
            foreach (string strFile in strFiles)
            {
                SharedFilesTo file = new SharedFilesTo();
                file.ip = Hoster.ip;
                file.username = Hoster.name;
                file.SharedWay = SharedWay;
                file.name = strFile.Substring(strFile.LastIndexOf('\\') + 1);
                file.size = FomatFileSize(GetFileSize(strFile));
                file.filepath = strFile;
                MySharedFiles.Add(file);
            }
        }
        //获得本地共享文件内容
        public List<SharedFiles> GetMyShare(string Path)
        {
            List<SharedFiles> AllFiles = new List<SharedFiles>();
            string[] strDirectorys = Directory.GetDirectories(Path);
            string[] strFiles = Directory.GetFiles(Path);
            foreach (string strDirectory in strDirectorys)
            {
                string[] Files = Directory.GetFiles(strDirectory);
                foreach (string strFile in Files)
                {
                    SharedFiles file = new SharedFiles();
                    file.name = strFile.Substring(strFile.LastIndexOf('\\') + 1);
                    //file.image = new BitmapImage(new Uri("/Images/directory.png", UriKind.Relative)); ;
                    //file.size = "文件夹";
                    file.image = GetFileImage(file.name);
                    file.size = FomatFileSize(GetFileSize(strFile));
                    AllFiles.Add(file);
                }
            }
            foreach (string strFile in strFiles)
            {
                SharedFiles file = new SharedFiles();
                file.name = strFile.Substring(strFile.LastIndexOf('\\') + 1);
                file.image = GetFileImage(file.name);
                file.size = FomatFileSize(GetFileSize(strFile));
                AllFiles.Add(file);
            }
            //for (int i = 0; i < AllFiles.Count; i++)
            //{
            //    MessageBox.Show(AllFiles[i]);
            //}
            return AllFiles;
        }
        //得到文件大小
        public long GetFileSize(string filepath)
        {
            long size = 0;
            FileStream filestream = File.Open(filepath, FileMode.Open, FileAccess.Read);   //创建文件流
            size = filestream.Length;
            filestream.Close();
            return size;
        }
        //格式化文件大小
        private string FomatFileSize(long size)
        {
            double temp;
            string unit = "B";
            temp = size;
            if (temp >= 1024)
            {
                temp /= 1024;
                unit = "KB";
            }
            if (temp >= 1024)
            {
                temp /= 1024;
                unit = "MB";
            }
            if (temp >= 1024)
            {
                temp /= 1024;
                unit = "GB";
            }
            return temp.ToString().Substring(0, temp.ToString().LastIndexOf('.')+3) + unit;
        }
        //获得发送文件的图标
        public ImageSource GetFileImage(string filepath)
        {
            System.Drawing.Icon icon = IconReader.GetFileIcon(filepath, IconReader.IconSize.Small, true, ref shfileinfo);
            System.Drawing.Bitmap bitmap = icon.ToBitmap();
            IntPtr hBitmap = bitmap.GetHbitmap();

            ImageSource wpfBitmap = Imaging.CreateBitmapSourceFromHBitmap(
                hBitmap,
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            return wpfBitmap;
        }
        //在主窗体上显示共享文件
        public void ShowSharedFile()
        {
            GetMyShare();
        }
        /*
        public void AddSharedFiles(List<SharedFilesTo> files)
        {
            //AllFiles.Clear();
            for (int i = 0; i < files.Count; i++)
            {
                SharedFiles file = new SharedFiles();
                file.name = files[i].name;
                file.SharedWay = files[i].SharedWay;
                file.size = files[i].size;
                file.filepath = files[i].filepath;
                file.image = GetFileImage(file.name);
                AllFiles.Add(file);
            }
            //MessageBox.Show(AllFiles[0].name);
        }
        private void UpdateSharedFilesList()
        {
            Down_list.ItemsSource = null;
            Down_list.ItemsSource = MySharedFiles;
        }
        */

        //发送监听
        public void listen(object message)
        {
            Message msg = (Message)message;
            IPAddress[] ih = Dns.GetHostAddresses(msg.user.ip);       //获得IP列表
            IPAddress UserIp = ih[0];      //获取IP地址    
            IPEndPoint Conncet = new IPEndPoint(UserIp, Port);     //构造结点
            Socket SendFileSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);      //初始化socket 

            try
            {
                SendFileSocket.Connect(Conncet);      //连接远程服务器
                if (SendFileSocket.Connected)         //如果连接成功 s.Connected 则为true 否则为 false
                {
                    //MessageBox.Show("连接成功");
                    SendFile sf = new SendFile();
                    sf.SendPath = msg.content;
                    sf.SendSocket = SendFileSocket;

                    Thread t = new Thread(new ParameterizedThreadStart(SendFile)); //创建进程
                    t.Start(sf);                                   //开始进程
                }
            }
            catch (NullReferenceException e)
            {
                MessageBox.Show(e.ToString());
            }
        }
        //开始发送
        private void SendFile(object sendfile)                       //创建set函数
        {
            SendFile sf = (SendFile)sendfile;
            byte[] data = new byte[65000];            //创建文件缓冲区
            FileStream file = File.Open(sf.SendPath, FileMode.Open, FileAccess.Read);   //创建文件流
            int longer = data.Length;
            int end = (int)file.Length;               //获取文件长度 文件传送如果有需要超过int的范围估计就要改写FileStream类了
            try
            {
                while (end != 0)
                {
                    int count = file.Read(data, 0, longer);                         //把数据写进流

                    int n = sf.SendSocket.Send(data, 0, count, SocketFlags.None);  //用Socket的Send方法发送流

                    end -= count;
                }
                file.Close();     //关闭文件流
                sf.SendSocket.Close();        //关闭Socket
            }
            catch (NullReferenceException e)
            {

            }
        }
        #endregion

        #region 共享文件点击事件

        //首次载入所有共享文件
        private void SharedTabItem_Click(object sender, RoutedEventArgs e)
        {
            if (AllFiles == null)
            {
                //请求获得共享文件
                AllFiles = new List<SharedFiles>();
                AllFiles.Clear();
                for (int i = 0; i < userlist.Count; i++)
                {
                    GetAlLSharedFiles(userlist[i].ip);
                }
            }
        }
        //刷新
        private void UpdateDownClick(object sender, RoutedEventArgs e)
        {
            //请求获得共享文件
            AllFiles.Clear();
            for (int i = 0; i < userlist.Count; i++)
            {
                GetAlLSharedFiles(userlist[i].ip);
            }
        }
        //查看详细信息
        private void ShowMoreFileMessageClick(object sender, RoutedEventArgs e)
        {
            int index = Down_list.SelectedIndex;

            string strtemp = "";
            if (index != -1)
            {
                if (AllFiles[index].SharedWay == DownloadWay.NeedNothing)
                {
                    strtemp = "文件所属人：" + AllFiles[index].username +
                              "\n所属人IP：" + AllFiles[index].ip +
                              "\n文件名：" + AllFiles[index].name +
                              "\n文件路径：" + AllFiles[index].filepath +
                              "\n文件大小：" + AllFiles[index].size +
                              "\n文件下载要求：自由下载";
                }
                else if (AllFiles[index].SharedWay == DownloadWay.NeedPassword)
                {
                    strtemp = "文件所属人：" + AllFiles[index].username +
                              "\n所属人IP：" + AllFiles[index].ip + 
                              "\n文件名：" + AllFiles[index].name +
                              "\n文件路径：" + AllFiles[index].filepath +
                              "\n文件大小：" + AllFiles[index].size +
                              "\n文件下载要求：需要密码下载";
                }
                MessageBox.Show(strtemp);
            }
        }
        private void DownloadSharedFileClick(object sender, RoutedEventArgs e)
        {
            int index = Down_list.SelectedIndex;

            int i = 0;
            if (index != -1)
            {
                for (i = 0; i < talklist.Count; i++)
                {
                    if (talklist[i].Title == AllFiles[index].ip)
                    {
                        break;
                    }
                }
                if (i == talklist.Count)
                {
                    for (i = 0; i < userlist.Count; i++)
                    {
                        if (userlist[i].ip == AllFiles[index].ip)
                        {
                            break;
                        }
                    }
                    if (i != userlist.Count)
                    {
                        TalkWindow talkwindow = new TalkWindow(userlist[i], MyFont, this);
                        talklist.Add(talkwindow);
                        talkwindow.Show();
                        talkwindow.Activate();
                        //请求获得共享文件
                        GetSharedFiles(AllFiles[index].ip);
                        //开始下载文件
                        talkwindow.DownloadFile(AllFiles[index]);
                    }
                    else
                    {
                        MessageBox.Show("拥有该资源的用户已不在!", "错误");
                        UpdateDownClick(sender, e);
                    }
                }
                else
                {
                    if (talklist[i].WindowState == WindowState.Minimized)
                    {
                        talklist[i].WindowState = WindowState.Normal;
                    }
                    talklist[i].Activate();
                    //开始下载文件
                    talklist[i].DownloadFile(AllFiles[index]);
                }
            }
        }
        #endregion

        #region 播放声音
        private void PlayMessageSound()
        { 
            using(SoundPlayer player = new SoundPlayer())
            {
                string location=System.Environment.CurrentDirectory+"\\Images\\msg.wav";
                player.SoundLocation=location;
                player.Play();
            }
        }
        #endregion
    }
}
