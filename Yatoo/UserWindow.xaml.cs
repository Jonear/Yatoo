using System;
using System.Collections.Generic;
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

namespace Yatoo
{
	/// <summary>
	/// UserWindow.xaml 的交互逻辑
	/// </summary>
	public partial class UserWindow : Window
	{
        MainWindow parent;
        public UserWindow(MainWindow parent)
		{
			this.InitializeComponent();

            this.parent = parent;

            SetBackground();

			UpdateUserList();
		}

        //失焦消失
        private void deactivated(object sender, EventArgs e)
        {
            this.Hide();
        }
        //点击出现聊天框
        private void mousedown(object sender, MouseButtonEventArgs e)
        {
            ListBox user = (ListBox)sender;

            int index = user.SelectedIndex;

            //MessageBox.Show(index.ToString());
            int i = 0;
            if (index < user.Items.Count && index != -1)
            {
                for (i = 0; i < parent.talklist.Count; i++)
                {
                    if (parent.talklist[i].Title == parent.userlist[index].ip)
                    {
                        break;
                    }
                }
                if (i == parent.talklist.Count)
                {
                    TalkWindow talkwindow = new TalkWindow(parent.userlist[index],parent.MyFont, parent);
                    parent.talklist.Add(talkwindow);
                    if (parent.GetMsgList.Count != 0)
                    {
                        for (i = 0; i < parent.GetMsgList.Count; i++)
                        {
                            if (parent.GetMsgList[i].user.ip == parent.GetMsgList[0].user.ip)
                            {
                                talkwindow.ShowMessage(parent.GetMsgList[i].content, parent.GetMsgList[i].font);
                                parent.GetMsgList.RemoveAt(i);
                                i--;
                            }
                        }
                    }
                    talkwindow.Show();
                    //请求获得共享文件
                    parent.GetSharedFiles(parent.userlist[index].ip);
                }
                else
                {
                    if (parent.talklist[i].WindowState == WindowState.Minimized)
                    {
                        parent.talklist[i].WindowState = WindowState.Normal;
                    }
                    parent.talklist[i].Activate();
                }
            }
        }
        public void UpdateUser()
        {
            User_List.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Yatoo.MainWindow.NextPrimeDelegate(UpdateUserList));
        }
        private void UpdateUserList()
        {
            User_List.ItemsSource = null;
            User_List.ItemsSource = parent.userlist;
        }
        public void SetBackground()
        {
            BorderBackground.Background = parent.BorderBackground.Background;
        }

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