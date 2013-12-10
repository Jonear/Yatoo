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
using System.Windows.Threading;
using System.IO;
using Microsoft.Win32;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Interop;

namespace Yatoo
{
	public class expression
	{
		public string name {get; set;}
		public string image {get; set;}
	}
    public class sendfile
    {
        public int id { get; set; }
        public ImageSource icon { get; set; }
        public string name { get; set; }
        public string filepath { get; set; }
        public long filesize { get; set; }
        public string size { get; set; }
        public string Button1_name { get; set; }
        public int Progress { get; set; }
        public string Speed { get; set; }
        public Visibility ShowSpeed { get; set; }
        public Visibility ShowButton1 { get; set; }
        public string Button2_name { get; set; }
    }
    public class SaveFile
    {
        public int index { get; set; }
        public string path { get; set; }
    }
    public class SharedFile
    {
        public string SavePath { get; set; }
        public string size { get; set; }
    }
    /// <summary>
    /// TalkWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TalkWindow : Window
    {
		public User TalkUser;
        private MainWindow Parent;
        private const int Port = 8283;

        private Font MyFont;
        private string myname;
        private string SendFilePath = null;
        private Socket SendFileSocket;
        private int SendIndex;
        private long SendAmount;
        private long SendFileSize;
        private long SendSize;
        private bool Is_Sending;
        private bool Is_Send;
        private int DownSharedFileCount = 0;
        private string SharedPassword = "";

		public List<expression> expressionlist = null;     //表情列表
        public List<sendfile> filelist = new List<sendfile>();     //表情列表
        Shell32.SHFILEINFO shfileinfo = new Shell32.SHFILEINFO();
        System.DateTime currentTime = new System.DateTime();

        private List<SharedFiles> SharedFilesList = new List<SharedFiles>();

        public TalkWindow(User user,Font myfont,MainWindow parent)
        {
            InitializeComponent();
            //获得对方的信息
            TalkUser = user;
            //获得父窗口
            Parent = parent;
            //获得我的名字
            myname = parent.Hoster.name;
            //获得字体
            MyFont = myfont;
            //未发送文件中
            Is_Sending = false;
            Is_Send = false;
            //初始化窗体信息
            InitialWindow();
            //初始化字体信息
            SetFontStyle();
            //初始化表情信息
            InitialExpression();
        }

        #region 初始化窗体信息
        private void InitialWindow()
        {
            //设置对方的名字
            UserName.Content = TalkUser.name + "-" + TalkUser.ip;
            //设置对方头像
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(TalkUser.image, UriKind.Relative);
            bitmap.EndInit();
            UserImage.Source = bitmap;
            //设置窗口标题为对方的IP地址
            this.Title = TalkUser.ip;
            //委托聚焦
            this.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                    (Action)(() => { Keyboard.Focus(MyMsg); }));
            //获得当前时间
            currentTime = new System.DateTime();
            //调整行间距
            MyMsg.Document.LineHeight = 0.1;
            ShowMsg.Document.LineHeight = 0.1;
            //清空输入框
            MyMsg.Document.Blocks.Clear();
            //设置窗体主题
            BitmapImage bitmap2 = new BitmapImage();
            bitmap2.BeginInit();
            bitmap2.UriSource = new Uri(Parent.BackgroundImage, UriKind.Relative);
            bitmap2.EndInit();
            WindowBackground.ImageSource = bitmap2;
            WindowBackground.Opacity = Parent.WinOpacity;
        }
        #endregion

        #region 表情处理
        //初始化表情信息
        private void InitialExpression()
		{
			expressionlist = new List<expression>(){
                new expression(){name="[00]",image="/Yatoo;component/Images/expression/[00].gif"},
                new expression(){name="[01]",image="/Yatoo;component/Images/expression/[01].gif"},
                new expression(){name="[02]",image="/Yatoo;component/Images/expression/[02].gif"},
                new expression(){name="[03]",image="/Yatoo;component/Images/expression/[03].gif"},
                new expression(){name="[04]",image="/Yatoo;component/Images/expression/[04].gif"},
                new expression(){name="[05]",image="/Yatoo;component/Images/expression/[05].gif"},
                new expression(){name="[06]",image="/Yatoo;component/Images/expression/[06].gif"},
                new expression(){name="[07]",image="/Yatoo;component/Images/expression/[07].gif"},
                new expression(){name="[08]",image="/Yatoo;component/Images/expression/[08].gif"},
                new expression(){name="[09]",image="/Yatoo;component/Images/expression/[09].gif"},
                new expression(){name="[10]",image="/Yatoo;component/Images/expression/[10].gif"},
                new expression(){name="[11]",image="/Yatoo;component/Images/expression/[11].gif"},
                new expression(){name="[12]",image="/Yatoo;component/Images/expression/[12].gif"},
                new expression(){name="[13]",image="/Yatoo;component/Images/expression/[13].gif"},
                new expression(){name="[14]",image="/Yatoo;component/Images/expression/[14].gif"},
                new expression(){name="[15]",image="/Yatoo;component/Images/expression/[15].gif"},
                new expression(){name="[16]",image="/Yatoo;component/Images/expression/[16].gif"},
                new expression(){name="[17]",image="/Yatoo;component/Images/expression/[17].gif"},
                new expression(){name="[18]",image="/Yatoo;component/Images/expression/[18].gif"},
                new expression(){name="[19]",image="/Yatoo;component/Images/expression/[19].gif"},
                new expression(){name="[20]",image="/Yatoo;component/Images/expression/[20].gif"},
                new expression(){name="[21]",image="/Yatoo;component/Images/expression/[21].gif"},
                new expression(){name="[22]",image="/Yatoo;component/Images/expression/[22].gif"},
                new expression(){name="[23]",image="/Yatoo;component/Images/expression/[23].gif"},
                new expression(){name="[24]",image="/Yatoo;component/Images/expression/[24].gif"},
                new expression(){name="[25]",image="/Yatoo;component/Images/expression/[25].gif"},
                new expression(){name="[26]",image="/Yatoo;component/Images/expression/[26].gif"},
                new expression(){name="[27]",image="/Yatoo;component/Images/expression/[27].gif"},
                new expression(){name="[28]",image="/Yatoo;component/Images/expression/[28].gif"},
                new expression(){name="[29]",image="/Yatoo;component/Images/expression/[29].gif"},
                new expression(){name="[30]",image="/Yatoo;component/Images/expression/[30].gif"},
                new expression(){name="[31]",image="/Yatoo;component/Images/expression/[31].gif"},
                new expression(){name="[32]",image="/Yatoo;component/Images/expression/[32].gif"},
                new expression(){name="[33]",image="/Yatoo;component/Images/expression/[33].gif"},
                new expression(){name="[34]",image="/Yatoo;component/Images/expression/[34].gif"},
                new expression(){name="[35]",image="/Yatoo;component/Images/expression/[35].gif"},
                new expression(){name="[36]",image="/Yatoo;component/Images/expression/[36].gif"},
                new expression(){name="[37]",image="/Yatoo;component/Images/expression/[37].gif"},
                new expression(){name="[38]",image="/Yatoo;component/Images/expression/[38].gif"},
                new expression(){name="[39]",image="/Yatoo;component/Images/expression/[39].gif"},
                new expression(){name="[40]",image="/Yatoo;component/Images/expression/[40].gif"},
                new expression(){name="[41]",image="/Yatoo;component/Images/expression/[41].gif"},
                new expression(){name="[42]",image="/Yatoo;component/Images/expression/[42].gif"},
                new expression(){name="[43]",image="/Yatoo;component/Images/expression/[43].gif"},
                new expression(){name="[44]",image="/Yatoo;component/Images/expression/[44].gif"},
                new expression(){name="[45]",image="/Yatoo;component/Images/expression/[45].gif"},
                new expression(){name="[46]",image="/Yatoo;component/Images/expression/[46].gif"},
                new expression(){name="[47]",image="/Yatoo;component/Images/expression/[47].gif"},
                new expression(){name="[48]",image="/Yatoo;component/Images/expression/[48].gif"},
                new expression(){name="[49]",image="/Yatoo;component/Images/expression/[49].gif"},
			};
            ListBox_Expression.ItemsSource = null;
            ListBox_Expression.ItemsSource = expressionlist;
		}
        private void ShowExpression(object sender, RoutedEventArgs e)
        {
            if (Grid_Expression.Visibility == Visibility.Hidden)
            {
                if (SetFont.Visibility == Visibility.Visible)
                {
                    SetFont.Visibility = Visibility.Hidden;
                }
                Grid_Expression.Visibility = Visibility.Visible;
            }
            else
            {
                Grid_Expression.Visibility = Visibility.Hidden;
            }
        }
        private void MyMsgGotFocusClick(object sender, RoutedEventArgs e)
        {
            if (Grid_Expression.Visibility == Visibility.Visible)
            {
                Grid_Expression.Visibility = Visibility.Hidden;
            }
        }
        private void Expression_LeftUpClick(object sender, MouseEventArgs e)
        {
            ListBox list_expre = (ListBox)sender;

            int index = list_expre.SelectedIndex;

            if (index < list_expre.Items.Count && index != -1)
            {    
                Image image = new Image();
                BitmapImage bitmap = new BitmapImage();
                image.IsEnabled = true;
                bitmap.BeginInit();
                bitmap.UriSource = new Uri("Images/expression/" + expressionlist[index].name + ".gif", UriKind.Relative);
                bitmap.EndInit();
                image.Source = bitmap;
                image.Stretch = Stretch.Uniform;
                image.Height = 25.0;
                image.Width = 25.0;

                new InlineUIContainer(image,MyMsg.Selection.End);

                if (MyMsg.CaretPosition.GetNextInsertionPosition(LogicalDirection.Forward) != null)
                {
                    MyMsg.CaretPosition = MyMsg.CaretPosition.GetNextInsertionPosition(LogicalDirection.Forward);
                }

                Keyboard.Focus(MyMsg);
            }
        }
        #endregion

        #region 窗口菜单
        private void window_move(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
				if ((e.GetPosition(this).Y < 80))
                {
                    this.DragMove();
				}
            }
        }
        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            
            if (Is_Sending)
            {
                MessageBox.Show("文件正在发送中，请取消后再关闭窗口！");
                return;
            }
            else if (Is_Send)
            {
                MessageBox.Show("文件等待发送中，请取消后再关闭窗口！");
                return;
            }
            else if (DownSharedFileCount > 0)
            {
                if (MessageBox.Show("窗口正在下载共享文件，是否停止下载？","Yatoo", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {

                }
                else
                {
                    return;
                }
            }
            Parent.talklist.Remove(this);
            this.Close();
        }

        private void MinimumWindow(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        #endregion

        #region 信息收发
        //发送信息
        private void SendMsgTo(object sender, RoutedEventArgs e)
        {
            SendMsg();
        }
        private void SendMsg()
        {
            TextRange range;
            string String_Mymsg = RTF(MyMsg);

            range = new TextRange(MyMsg.Document.ContentStart, MyMsg.Document.ContentEnd);

            int length = range.Text.Length;

            MyMsg.Document.Blocks.Clear();

            Keyboard.Focus(MyMsg);

            if (length != 0)
            {
                if (Parent.SendMsgTo(TalkUser.ip, StringCompress.Compress(String_Mymsg)))
                {
                    ShowMyMessage(String_Mymsg);
                }
            }


        }
        //显示我发送出去的信息
        private void ShowMyMessage(string message)
        {
            
            currentTime = System.DateTime.Now;

            Paragraph p = new Paragraph();
            Run rtitle = new Run(myname + "    " + currentTime.ToLongTimeString().ToString() + "\n");
            rtitle.Foreground = Brushes.Green;
            p.Inlines.Add(rtitle);

            ShowMsg.Document.Blocks.Add(p);

            LoadFromRTF(ShowMsg,message);

            ShowMsg.Selection.Select(ShowMsg.CaretPosition, ShowMsg.Document.ContentEnd);
            ShowMsg.Selection.ApplyPropertyValue(TextElement.FontFamilyProperty, new FontFamily(MyFont.Fontfamily));
            ShowMsg.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, (double)MyFont.Fontsize);
            if (!MyFont.Fontstyle)
            {
                ShowMsg.Selection.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Italic);
            }
            if (!MyFont.Fontweight)
            {
                ShowMsg.Selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
            }

            ShowMsg.ScrollToEnd();
        }
        //显示我接收的信息
        public void ShowMessage(string message, Font font)
        {
            currentTime = System.DateTime.Now;

            Paragraph p = new Paragraph();
            Run rtitle = new Run(TalkUser.name + "    " + currentTime.ToLongTimeString().ToString() + "\n");
            rtitle.Foreground = Brushes.Blue;
            p.Inlines.Add(rtitle);

            ShowMsg.Document.Blocks.Add(p);

            LoadFromRTF(ShowMsg, StringCompress.Decompress(message));

            ShowMsg.Selection.Select(ShowMsg.CaretPosition, ShowMsg.Document.ContentEnd);
            ShowMsg.Selection.ApplyPropertyValue(TextElement.FontFamilyProperty, new FontFamily(font.Fontfamily));
            ShowMsg.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, (double)font.Fontsize);
            if (!font.Fontstyle)
            {
                ShowMsg.Selection.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Italic);
            }
            if (!font.Fontweight)
            {
                ShowMsg.Selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
            }
            ShowMsg.ScrollToEnd();
        }
        #endregion

        #region 文件接发
        //发送文件
        private void SendFileClick(object sender, RoutedEventArgs e)
        {

            if (TalkUser.ip == Parent.Hoster.ip)
            {
                MessageBox.Show("不能发送文件给自己");
                return;
            }
            if (Is_Send == true)
            {
                MessageBox.Show("一次只能发送一个文件,请先发完这个再继续发送，或者打包发送");
                return;
            }
            OpenFileDialog open = new OpenFileDialog();//定义打开文本框实体
            open.Title = "打开文件";//对话框标题
            open.Filter = "所有文件(*.*)|*.*";//文件扩展名
            if ((bool)open.ShowDialog().GetValueOrDefault())//打开
            {
                Is_Send = true;
                try
                {
                    SendFilePath = open.FileName;
                    AddSendFile();
                    Parent.SendFileTo(TalkUser.ip, SendFilePath.Substring(SendFilePath.LastIndexOf('\\') + 1), GetFileSize(SendFilePath));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }
        //增加发送文件样式
        private void AddSendFile()
        {
            ImageSource fileicon = GetFileImage(SendFilePath);

            sendfile file = new sendfile();
            file.icon = fileicon;
            file.name = SendFilePath.Substring(SendFilePath.LastIndexOf('\\') + 1);
            
            if (filelist.Count != 0)
            {
                file.id = filelist[filelist.Count - 1].id + 1;
            }
            else
            {
                file.id = 0;
            }
            file.filepath = SendFilePath;
            file.filesize = GetFileSize(SendFilePath);
            file.size = FomatFileSize(file.filesize);
            file.Button1_name = "接收文件";
            file.Button2_name = "取消发送";
            file.ShowButton1 = Visibility.Hidden;
            file.ShowSpeed = Visibility.Hidden;
            file.Speed = "0kb/s";
            file.Progress = 0;
            filelist.Add(file);

            File_List.ItemsSource = null;
            File_List.ItemsSource = filelist;

            //显示发送列表
            SharedFile_List.Visibility = Visibility.Hidden;
            File_List.Visibility = Visibility.Visible;
        }
        //增加收取文件样式
        public void AddRecvFile(string filename,long filesize)
        {
            ImageSource fileicon = GetFileImage(filename);

            Is_Send = true; //开始接收

            sendfile file = new sendfile();
            file.icon = fileicon;
            file.name = filename;
            file.filesize = filesize;
            file.size = FomatFileSize(filesize);
            if (filelist.Count != 0)
            {
                file.id = filelist[filelist.Count - 1].id + 1;
            }
            else
            {
                file.id = 0;
            }
            file.Button1_name = "接收文件";
            file.Button2_name = "拒绝接收";
            file.ShowButton1 = Visibility.Visible;
            file.ShowSpeed = Visibility.Hidden;
            file.Speed = "0kb/s";
            file.Progress = 0;
            filelist.Add(file);

            File_List.ItemsSource = null;
            File_List.ItemsSource = filelist;

            //显示发送列表
            SharedFile_List.Visibility = Visibility.Hidden;
            File_List.Visibility = Visibility.Visible;
        }
        //得到文件大小
        public long GetFileSize(string filepath)
        {
            long size=0;
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
            return temp.ToString().Substring(0, temp.ToString().LastIndexOf('.') + 3) + unit;
        }
        //获得发送文件的图标
        public ImageSource GetFileImage(string filepath)
        {
            System.Drawing.Icon icon = IconReader.GetFileIcon(filepath, IconReader.IconSize.Large, true, ref shfileinfo);
            System.Drawing.Bitmap bitmap = icon.ToBitmap();
            IntPtr hBitmap = bitmap.GetHbitmap();

            ImageSource wpfBitmap = Imaging.CreateBitmapSourceFromHBitmap(
                hBitmap,
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            return wpfBitmap;
        }
        //同意接收文件
        private void AcceptRecvFileClick(object sender, RoutedEventArgs e)
        {
            Button clickbutton = (Button)sender;
            int id = Int32.Parse(clickbutton.BorderThickness.ToString().Substring(0, 1));
            int index = GetFileIndex(id);
            if (index != -1)
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.FileName = filelist[index].name;
                dlg.Title = "另存为";
                dlg.Filter = "所有文件(*.*)|*.*";//文件扩展名
                if ((bool)dlg.ShowDialog().GetValueOrDefault())//打开
                {
                    //MessageBox.Show(dlg.FileName);
                    filelist[index].ShowButton1 = Visibility.Hidden;
                    filelist[index].ShowSpeed = Visibility.Visible;
                    File_List.ItemsSource = null;
                    File_List.ItemsSource = filelist;
                    SaveFile sf = new SaveFile();
                    sf.index = index;
                    sf.path = dlg.FileName;
                    Thread g = new Thread(new ParameterizedThreadStart(GetFile)); //创建进程
                    g.Start(sf);                                   //开始进程
                    Parent.AcceptRecvFile(TalkUser.ip, id);
                }
                
            }
            //MessageBox.Show(id.ToString());
        }
        //取消文件发送
        private void RefuseFileClick(object sender, RoutedEventArgs e)
        {
            Button clickbutton = (Button)sender;
            int id = Int32.Parse(clickbutton.BorderThickness.ToString().Substring(0, 1));
            int index = GetFileIndex(id);
            if (index != -1)
            {
                CloseSendFile(index, id);
            }

            //隐藏发送列表
            SharedFile_List.Visibility = Visibility.Visible;
            File_List.Visibility = Visibility.Hidden;
        }
        //关闭发送文件
        public void CloseSendFile(int index,int id)
        {
            if (Is_Sending == false)
            {
                Is_Send = false;
                filelist.RemoveAt(index); //移除发送项
                File_List.Dispatcher.BeginInvoke(
                    DispatcherPriority.Normal,
                    new Yatoo.MainWindow.NextPrimeDelegate(UpdateFileList)); //更新
            }
            Parent.CloseSendFile(TalkUser.ip, id);

            ShowFileMessage("您取消了要发送的文件！");

            //隐藏发送列表
            SharedFile_List.Visibility = Visibility.Visible;
            File_List.Visibility = Visibility.Hidden;
        }
        //对方关闭发送文件
        public void CloseSendFile(int id)
        {
            int index = GetFileIndex(id);
            if (index != -1)
            {
                Is_Send = false;
                filelist.RemoveAt(index); //移除发送项
                File_List.Dispatcher.BeginInvoke(
                    DispatcherPriority.Normal,
                    new Yatoo.MainWindow.NextPrimeDelegate(UpdateFileList)); //更新

                if (Is_Sending == true)
                {
                    Parent.ReCloseSendFile(TalkUser.ip, id);
                }

                ShowFileMessage("对方取消了要发送的文件！");

                //隐藏发送列表
                SharedFile_List.Visibility = Visibility.Visible;
                File_List.Visibility = Visibility.Hidden;
            }
        }
        //关闭发送文件
        public void CloseSendFile()
        {
            Is_Send = false;
            if (filelist.Count > 0)
            {
                filelist.RemoveAt(0); //移除发送项
            }
            File_List.Dispatcher.BeginInvoke(
                DispatcherPriority.Normal,
                new Yatoo.MainWindow.NextPrimeDelegate(UpdateFileList)); //更新

            //隐藏发送列表
            SharedFile_List.Dispatcher.Invoke((Action)(() => {
                SharedFile_List.Visibility = Visibility.Visible;
            }));
            File_List.Dispatcher.Invoke((Action)(() =>
            {
                File_List.Visibility = Visibility.Hidden;
            }));
        }
        //获得文件索引
        private int GetFileIndex(int id)
        {
            int i;
            for (i = 0; i < filelist.Count; i++)
            {
                if (filelist[i].id == id)
                    break;
            }
            if (i == filelist.Count)
            {
                i = -1;
            }
            return i;
        }
        //发送监听
        public void listen(int id)
        {
            int index = GetFileIndex(id);
            IPAddress[] ih = Dns.GetHostAddresses(TalkUser.ip);       //获得IP列表
            IPAddress UserIp = ih[0];      //获取IP地址    
            IPEndPoint Conncet = new IPEndPoint(UserIp, Port);     //构造结点
            SendFileSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);      //初始化socket 

            try
            {
                SendFileSocket.Connect(Conncet);      //连接远程服务器
                if (SendFileSocket.Connected)         //如果连接成功 s.Connected 则为true 否则为 false
                {
                    //MessageBox.Show("连接成功");
                    SaveFile sf = new SaveFile();
                    sf.index = index;
                    sf.path = filelist[index].filepath;
                    filelist[index].ShowSpeed = Visibility.Visible;

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
            SaveFile sf = (SaveFile)sendfile;
            SendIndex = sf.index;
            SendAmount = 0;
            byte[] data = new byte[65000];            //创建文件缓冲区
            FileStream file = File.Open(sf.path, FileMode.Open, FileAccess.Read);   //创建文件流
            int longer = data.Length;
            SendFileSize = file.Length;
            SendSize = 0;
            int end = (int)file.Length;               //获取文件长度 文件传送如果有需要超过int的范围估计就要改写FileStream类了
            try
            {
                System.Timers.Timer timer = new System.Timers.Timer(1000);  //实例化Timer类，设置间隔时间为10000毫秒   
                timer.Elapsed += new System.Timers.ElapsedEventHandler(ShowSpeech);  //到达时间的时候执行事件 
                timer.AutoReset = true;  //设置是执行一次（false）还是一直执行(true)  
                timer.Enabled = true;  //是否执行System.Timers.Timer.Elapsed事件 

                while (end != 0 && Is_Send == true)
                {
                    int count = file.Read(data, 0, longer);                         //把数据写进流

                    int n = SendFileSocket.Send(data, 0, count, SocketFlags.None);  //用Socket的Send方法发送流

                    SendAmount += n;

                    SendSize += n;

                    end -= count;
                }
                if (Is_Send == true)
                { 
                    CloseSendFile();
                    ShowFileMessage("发送文件成功!");
                }
                timer.Close();    //关闭显示
                file.Close();     //关闭文件流
                SendFileSocket.Close();        //关闭Socket
            }
            catch (NullReferenceException e)
            {

            }
        }
        //显示发送速度
        public void ShowSpeech(object source,System.Timers.ElapsedEventArgs e)
        {
            filelist[SendIndex].Speed = FomatFileSize(SendAmount) + "/s";             //更新速度

            filelist[SendIndex].Progress = (int)((SendSize / (Double)SendFileSize) * 100);//更新进度条

            File_List.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Yatoo.MainWindow.NextPrimeDelegate(UpdateFileList)); //更新

            SendAmount = 0;
            //MessageBox.Show("OK!");
        }  
        //开始接收
        private void GetFile(Object savefile)
        {
            SaveFile sf = (SaveFile)savefile;
            SendIndex = sf.index;
            FileStream file = new FileStream(sf.path, FileMode.Create, FileAccess.Write); //写入文件流
            SendFileSize = filelist[sf.index].filesize;
            SendAmount = 0;
            SendSize = 0;
            TcpListener listen = new TcpListener(Port);  //监听端口
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);  //定义Socket并初始化
            try
            {
                listen.Start();        //开始监听
                s = listen.AcceptSocket();            //获取Socket连接
                listen.Stop();
                byte[] data = new byte[65000];      //定义缓冲区
                int longer = data.Length;
                if (s.Connected)             //确定连接
                {
                    Is_Sending = true;
                    System.Timers.Timer timer = new System.Timers.Timer(1000);  //实例化Timer类，设置间隔时间为10000毫秒   
                    timer.Elapsed += new System.Timers.ElapsedEventHandler(ShowSpeech);  //到达时间的时候执行事件 
                    timer.AutoReset = true;  //设置是执行一次（false）还是一直执行(true)  
                    timer.Enabled = true;  //是否执行System.Timers.Timer.Elapsed事件 
                    int count = s.Receive(data, 0, longer, SocketFlags.None);  //把接收到的byte存入缓冲区      
                    file.Write(data, 0, count);
                    SendAmount += count;
                    SendSize += count;
                    while (count != 0 && Is_Send == true)
                    {
                        count = s.Receive(data, 0, longer, SocketFlags.None);
                        file.Write(data, 0, count);
                        SendAmount += count;
                        SendSize += count; 
                    }
                    if (Is_Send == true)
                    {
                        CloseSendFile();
                        ShowFileMessage("接收文件成功!路径：" + sf.path + " 打开文件");
                    }
                    s.Close();
                    file.Close();
                    timer.Close();
                    Is_Sending = false;
                    //隐藏发送列表
                    SharedFile_List.Dispatcher.Invoke((Action)(() =>
                    {
                        SharedFile_List.Visibility = Visibility.Visible;
                    }));
                    File_List.Dispatcher.Invoke((Action)(() =>
                    {
                        File_List.Visibility = Visibility.Hidden;
                    }));
                }
            }
            catch (NullReferenceException e)
            {
            }
        }
        private void ShowFileMessage(String msg)
        {
            ShowMsg.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() =>
            {
                currentTime = System.DateTime.Now;

                Paragraph p = new Paragraph();
                Run rtitle = new Run(currentTime.ToLongTimeString().ToString() + "    " + msg + "\n");
                rtitle.Foreground = Brushes.Gray;
                p.Inlines.Add(rtitle);
                ShowMsg.Document.Blocks.Add(p);
                ShowMsg.ScrollToEnd();
            }));
        }
        private void UpdateFileList()
        {
            File_List.ItemsSource = null;
            File_List.ItemsSource = filelist;
        }
        #endregion 

        #region 设置字体
        //设置字体
        private void SetFontStyle()
        {    
            MyMsg.FontFamily = new FontFamily(MyFont.Fontfamily);
            ComboBox_FontFamily.Text = MyMsg.FontFamily.ToString();
    
            MyMsg.FontSize = MyFont.Fontsize;
            ComboBox_FontSize.Text = MyMsg.FontSize.ToString();

            MyMsg.FontStyle = GetFontStyle(MyFont.Fontstyle);
            MyMsg.FontWeight = GetFontWeight(MyFont.Fontweight);

            MyMsg.Foreground = GetFontColor(MyFont.Fontcolor);
            ComboBox_FontColor.Text = MyFont.Fontcolor;
        }
        //设置字体按钮响应
        private void SetFontClick(object sender, RoutedEventArgs e)
        {
            if (SetFont.Visibility == Visibility.Hidden)
            {
                if (Grid_Expression.Visibility == Visibility.Visible)
                {
                    Grid_Expression.Visibility = Visibility.Hidden;
                }
                SetFont.Visibility = Visibility.Visible;
            }
            else
            {
                SetFont.Visibility = Visibility.Hidden;
            }
        }
        //设置字体粗细
        private void FontWeightClick(object sender, RoutedEventArgs e)
        {
            if (MyFont.Fontweight)
            {
                MyFont.Fontweight = false;
                MyMsg.FontWeight = FontWeights.Bold;
            }
            else
            {
                MyFont.Fontweight = true; 
                MyMsg.FontWeight = FontWeights.Normal;
            }        
        }
        //设置字体倾斜
        private void FontStyleClick(object sender, RoutedEventArgs e)
        {
            if (MyFont.Fontstyle)
            {
                MyFont.Fontstyle = false;
                MyMsg.FontStyle = FontStyles.Italic;
            }
            else
            {
                MyFont.Fontstyle = true;
                MyMsg.FontStyle = FontStyles.Normal;
            }
        }
        private FontStyle GetFontStyle(bool style)
        {
            if (style)
            {
                return FontStyles.Normal;
            }
            return FontStyles.Italic;
        }
        private FontWeight GetFontWeight(bool weight)
        {
            if (weight)
            {
                return FontWeights.Normal;
            }
            return FontWeights.Bold;
        }
        private Brush GetFontColor(string color)
        {
            switch (color)
            {
                case "红色":
                    return Brushes.Red;
                case "蓝色":
                    return Brushes.Blue;
                case "黑色":
                    return Brushes.Black;
                case "黄色":
                    return Brushes.Yellow;
                case "绿色":
                    return Brushes.Green;
                case "紫色":
                    return Brushes.Purple;
                case "粉红":
                    return Brushes.Pink;
                case "浅蓝":
                    return Brushes.LightBlue;
                case "浅绿":
                    return Brushes.LightGreen;
                case "褐色":
                    return Brushes.Brown;
            }
            return Brushes.Black;
        }
        //设置字体颜色
        private void FontColorClick(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem item = (ComboBoxItem)e.AddedItems[0];
            MyFont.Fontcolor = item.Content.ToString();
           
            MyMsg.Foreground = GetFontColor(MyFont.Fontcolor);
        }
        //设置字体大小
        private void FontSizeClick(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem item = (ComboBoxItem)e.AddedItems[0];
            MyFont.Fontsize = int.Parse(item.Content.ToString());

            MyMsg.FontSize = MyFont.Fontsize;
        }
        //设置字体类型
        private void FontFamilyClick(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem item = (ComboBoxItem)e.AddedItems[0];
            MyFont.Fontfamily = item.Content.ToString();
            MyMsg.FontFamily = new FontFamily(MyFont.Fontfamily);
        }
        #endregion

        #region 文件共享
        public void GetSharedFiles(List<SharedFilesTo> files,string password)
        {
            SharedFilesList.Clear();
            for (int i = 0; i < files.Count; i++)
            {
                SharedFiles file = new SharedFiles();
                file.name = files[i].name;
                file.SharedWay = files[i].SharedWay;
                file.size = files[i].size;
                file.filepath = files[i].filepath;
                file.image = GetFileImage(file.name);
                SharedFilesList.Add(file);
            }
            SharedPassword = password;
            ShowSharedFiles();
        }
        public void ShowSharedFiles()
        {
            SharedFile_List.ItemsSource = null;
            SharedFile_List.ItemsSource = SharedFilesList;
        }
        private void UpdateSharedFile(object sender, RoutedEventArgs e)
        {
            Parent.GetSharedFiles(TalkUser.ip);
        }
        private void ShowMoreFileMessage(object sender, RoutedEventArgs e)
        {
            int index = SharedFile_List.SelectedIndex;

            string strtemp = "";
            if (index != -1)
            {
                if (SharedFilesList[index].SharedWay == DownloadWay.NeedNothing)
                {
                    strtemp = "文件名：" + SharedFilesList[index].name +
                              "\n文件路径：" + SharedFilesList[index].filepath +
                              "\n文件大小：" + SharedFilesList[index].size +
                              "\n文件下载要求：自由下载";
                }
                else if (SharedFilesList[index].SharedWay == DownloadWay.NeedPassword)
                {
                    strtemp = "文件名：" + SharedFilesList[index].name +
                              "\n文件路径：" + SharedFilesList[index].filepath +
                              "\n文件大小：" + SharedFilesList[index].size +
                              "\n文件下载要求：需要密码下载";
                }
                MessageBox.Show(strtemp);
            }
        }
        private void ShareFileMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            int index = SharedFile_List.SelectedIndex;

            if (index != -1)
            {
                //DownSharedFileProgress.Visibility = Visibility.Visible;
                DownloadFile(SharedFilesList[index]);
            }
        }
        public void DownloadFile(SharedFiles sharedfile)
        {
            if (sharedfile.SharedWay == DownloadWay.NeedPassword)
            {
                EnterPassword Dlg = new EnterPassword();
                Dlg.ShowDialog();
                if (Dlg.Is_GetPassword)
                {
                    if (Dlg.password.Text == SharedPassword)
                    {
                        Parent.GetSharedFile(TalkUser.ip, sharedfile.filepath);
                        SharedFile sf = new SharedFile();
                        sf.SavePath = Parent.DownedDir + "\\" + sharedfile.name;
                        sf.size = sharedfile.size;
                        Thread g = new Thread(new ParameterizedThreadStart(GetSharedFile)); //创建进程
                        g.Start(sf);                                   //开始进程
                    }
                    else
                    {
                        MessageBox.Show("您输入的口令有误");
                    }
                }
            }
            else
            {
                Parent.GetSharedFile(TalkUser.ip, sharedfile.filepath);
                SharedFile sf = new SharedFile();
                sf.SavePath = Parent.DownedDir + "\\" + sharedfile.name;
                sf.size = sharedfile.size;
                Thread g = new Thread(new ParameterizedThreadStart(GetSharedFile)); //创建进程
                g.Start(sf);                                   //开始进程
            }
        }
        //开始接收
        private void GetSharedFile(Object savefile)
        {
            SharedFile sf = (SharedFile)savefile;
            FileStream file = new FileStream(sf.SavePath, FileMode.Create, FileAccess.Write); //写入文件流
            SendAmount = 0;
            SendSize = 0;
            TcpListener listen = new TcpListener(Port);  //监听端口
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);  //定义Socket并初始化
            try
            {
                listen.Start();        //开始监听
                s = listen.AcceptSocket();            //获取Socket连接
                listen.Stop();
                byte[] data = new byte[65000];       //定义缓冲区
                int longer = data.Length;
                if (s.Connected)                     //确定连接
                {
                    int count = s.Receive(data, 0, longer, SocketFlags.None);  //把接收到的byte存入缓冲区  
                    DownSharedFileCount++;                                     //增加一个下载数
                    file.Write(data, 0, count);
                    SendAmount += count;
                    SendSize += count;
                    ShowSharedMessage("开始下载文件到：" + sf.SavePath + "（" + sf.size + "）" + "\n请勿关闭窗口，耐心等待下载。。。",sf.SavePath);
                    while (count != 0)
                    {
                        try
                        {
                            count = s.Receive(data, 0, longer, SocketFlags.None);
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show("资源拥有者已退出程序！");
                            s.Close();
                            file.Close();
                            return;
                        }
                        file.Write(data, 0, count);
                        SendAmount += count;
                        SendSize += count;
                    }
                    ShowSharedMessage("下载共享文件成功!路径：" + sf.SavePath, sf.SavePath);   //下载完成
                    DownSharedFileCount--;                                      //减少一个下载数
                    s.Close();
                    file.Close();
                }
            }
            catch (NullReferenceException e)
            {
            }
        }
        private void ShowSharedMessage(String msg,String path)
        {
            ShowMsg.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() =>
            {
                currentTime = System.DateTime.Now;

                Paragraph p = new Paragraph();
                Run rtitle = new Run(currentTime.ToLongTimeString().ToString() + "    " + msg+"\t");
                rtitle.Foreground = Brushes.Gray;
                p.Inlines.Add(rtitle);

                Hyperlink lnkSSLink = new Hyperlink();
                lnkSSLink.Inlines.Add("打开文件夹\n");
                lnkSSLink.NavigateUri = new Uri("http://www.google.com");
                lnkSSLink.TargetName = "_blank";
                lnkSSLink.Foreground = Brushes.Blue;
                p.Inlines.Add(lnkSSLink);

                lnkSSLink.RequestNavigate +=
    new System.Windows.Navigation.RequestNavigateEventHandler(RequestNavigateHandler);

                ShowMsg.AddHandler(Hyperlink.RequestNavigateEvent,
    new System.Windows.Navigation.RequestNavigateEventHandler(RequestNavigateHandler));

                ShowMsg.Document.Blocks.Add(p);
                ShowMsg.ScrollToEnd();

            }));
        }
        private void RequestNavigateHandler(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.ToString());
        }
        #endregion

        #region 窗口设置
        private void SettingClick(object sender, RoutedEventArgs e)
        {
            Parent.Setter_Click(sender, e);
        }
        #endregion

        #region richTextBox与RTF的相互转换
        public string RTF(RichTextBox richTextBox)
        {
            string rtf = string.Empty;
            TextRange textRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
            using (MemoryStream ms = new MemoryStream())
            {
                textRange.Save(ms, System.Windows.DataFormats.Rtf);
                ms.Seek(0, SeekOrigin.Begin);
                StreamReader sr = new StreamReader(ms);
                rtf = sr.ReadToEnd();
            }

            return rtf;
        }

        public  void LoadFromRTF(RichTextBox richTextBox, string rtf)
        {
            if (string.IsNullOrEmpty(rtf))
            {
                throw new ArgumentNullException();
            }
            richTextBox.CaretPosition = richTextBox.Document.ContentEnd;
            TextRange textRange = new TextRange(richTextBox.Selection.Start, richTextBox.Selection.End);
            using (MemoryStream ms = new MemoryStream())
            {
                using (StreamWriter sw = new StreamWriter(ms))
                {
                    sw.Write(rtf);
                    sw.Flush();
                    ms.Seek(0, SeekOrigin.Begin);
                    textRange.Load(ms, DataFormats.Rtf);
                }
            }
        }
        #endregion

        #region 发送快捷键
        private bool CtrlDown = false;
        private void MyMsg_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            
            if (e.Key == Key.RightCtrl)
            {
                CtrlDown = true;
            }
            else if (e.Key == Key.Enter && CtrlDown)
            {
                SendMsg();
            }
        }

        private void MyMsg_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.RightCtrl)
            {
                CtrlDown = false;
            }
        }
        #endregion

        #region 拖拽发送文件
        private void File_PreviewDraEnter(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy;
            e.Handled = true;
        }
        private void File_PreviewDrop(object sender, DragEventArgs e)
        {
            Array arr = (Array)e.Data.GetData(DataFormats.FileDrop);
            if (arr.Length > 1)
            {
                MessageBox.Show("无法一次性发送多个文件，请打包发送");
                return;
            }
            if (TalkUser.ip == Parent.Hoster.ip)
            {
                MessageBox.Show("不能发送文件给自己");
                return;
            }
            if (Is_Send == true)
            {
                MessageBox.Show("一次只能发送一个文件,请先发完这个再继续发送，或者打包发送");
                return;
            }
            foreach (var item in arr)
            {   
                Is_Send = true;
                try
                {
                    SendFilePath = item.ToString();
                    AddSendFile();
                    Parent.SendFileTo(TalkUser.ip, SendFilePath.Substring(SendFilePath.LastIndexOf('\\') + 1), GetFileSize(SendFilePath));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }
        private void File_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Move;
            e.Handled = true;
        }
        #endregion

    }
}
