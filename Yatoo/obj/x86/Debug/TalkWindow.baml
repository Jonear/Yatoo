   M S B A M L   `   `   `  ����	  Yatoo    Yatoo.TalkWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     6F   -   5          � Title$  
TalkWindow��5   	   $	��500��6   $	��700��6)    � WindowStyle$
 None=�65    � 
ResizeMode$ NoResize=�6H    � AllowsTransparency .  � 	Resources 5         Button_Close �� 
TargetType �� Property �� Value �� 
TargetType	 �Name	
 �� X1	 �� Y1	 �� X2	 �� Y2 �� 
StartPoint �� EndPoint � Color � Offset �� RelativeSource ��Name J� SnapsToDevicePixels J� OpacityMask �� Triggers P� Property P� Value �� 
TargetName  Button_Window  Button_Voice  Button_Camera  Button_Sendfile  Button_Setter  Button_characters ��Name �� CornerRadius �� Path  Button_Expression  Button_setfont 	 Button_SendFile��%�"  &	        &	 �    &	 �	    &	 \    &	 �    &	     &	 �    &	 �    &	 �    &		 !    �� 5   
   " ��6%   .�5	      ��  � ��
 �����6+   �� 5
       � ��$ 16(�6    �� 5       � ��$ 	ExtraBold"�6"   �� 5       � �� 5      �� 5      " ��6&   .R�5      � $
	 grid��6   $��25��6-   .+�5      �� $��1��6#   $
 0��67   $ 0��6>   $ 0��6E   $ 25��6L   ���	#FF786F6F��5      �� 5   #   $	 0,0&�67   $ 0,1.5&�6H   ��5   3   � � 5   8   $ 	#FF786F6F��6E   $ 0Q�6W   � 5   8   $ 1Q�6�    6E   �� *.,(Control.Background).(SolidColorBrush.Color)+ �� *TemplatedParent+6�   5   4   5   +   5   
   �� 5      $ ContentPresenter��6A   $��10,0,0,0q�6/   # z 6[   #��z��6�   #��z��5      ��. $��38.37��6t    �	#FFD816166�   5       5!      P� 5"       �@J�IsMouseOver . . �5#   "   �� $
 grid��6?    � �� 5$   &   �� 5%   *   $	 0,0&�6>   $ 0,1.5&�6O   ��5'   3   � � 5(   8   $ 0Q�6�    6E   �� *.,(Control.Background).(SolidColorBrush.Color)+ �� *TemplatedParent+6�   � 5)   8   $ 	#FFFF0000��6E   $ 1Q�6W   5*   4   5,   +   5.   #   5/      51      53      �� 54       � �� 6+   Z� 55      �� 57   
   " ��6&   .�58      ��  � ��
 �����6+   �� 59       � ��$ 17(�6    �� 5:       � ��$
 Bold"�6"   �� 5;       � �� 5<      �� 5=      " ��6&   .R�5>      � $
	 grid��6   .+�5?      �� $��1��6#   $
 0��67   $ 0��6>   $ 0��6E   $ 25��6L   ���	#FF786F6F$��0,0,8,0q�6e   ��5@      �� 5A   #   $	 0,0&�67   $ 0,1.5&�6H   ��5C   3   � � 5D   8   $ 	#FF786F6F��6E   $ 0Q�6W   � 5E   8   $ 1Q�6�    6E   �� *.,(Control.Background).(SolidColorBrush.Color)+ �� *TemplatedParent+6�   5F   4   5H   +   5J   
   �� 5K      $ ContentPresenter��6/   # z 6I   #��z��6�   #��z��5L      ��. $��30��6t    �	#FFD816166   5M       5O      P� 5P       �@J�IsMouseOver . . �5Q   "   �� $
 grid��6?    � �� 5R   &   �� 5S   *   $	 0,0&�6>   $ 0,1.5&�6O   ��5U   3   � � 5V   8   $ 	#FF3ED0A5��6E   $ 0Q�6W   � 5W      $	 0.6Q�6�    6   �� *.,(Control.Background).(SolidColorBrush.Color)+ �� *TemplatedParent+6�   � 5X   8   $ 1Q�6�    6E   �� *.,(Control.Background).(SolidColorBrush.Color)+ �� *TemplatedParent+6�   5Y   4   5[   +   5]   #   5^      5`      5b      �� 5c       � �� 6+   Z� 5d      �� 5f      " ��6   .�5g      ��  � ��
 �����6+   �� 5h       � ��$ 17(�6    �� 5i       � ��$
 Bold"�6"   �� 5j       � �� 5k      �� 5l      " ��6&   .R�5m      � $
	 grid��6   .+�5n      �� $'��!/Yatoo;component/Images/voice.png��6#   5o       5q      P� 5r       �@J�IsMouseOver . . �5s   "   �� $
 grid��6?    � �� 5t   &   �� 5u   *   $	 0,0&�6>   $	 0,1&�6O   ��5w   3   � � 5x   8   $ 0Q�6�    6E   �� *.,(Control.Background).(SolidColorBrush.Color)+ �� *TemplatedParent+6�   � 5y      $ White��6   $	 0.6Q�6,   � 5z   8   $ 1Q�6�    6E   �� *.,(Control.Background).(SolidColorBrush.Color)+ �� *TemplatedParent+6�   5{   4   5}   +   5   #   5�      5�      5�      �� 5�       � �� 6+   Z� 5�      �� 5�      " ��6    .�5�      ��  � ��
 �����6+   �� 5�       � ��$ 17(�6    �� 5�       � ��$
 Bold"�6"   �� 5�       � �� 5�      �� 5�      " ��6&   .R�5�      � $
	 grid��6   .+�5�      �� $(��"/Yatoo;component/Images/camera.png��6#   5�       5�      P� 5�       �@J�IsMouseOver . . �5�   "   �� $
 grid��6?    � �� 5�   &   �� 5�   *   $	 0,0&�6>   $	 0,1&�6O   ��5�   3   � � 5�   8   $ 0Q�6�    6E   �� *.,(Control.Background).(SolidColorBrush.Color)+ �� *TemplatedParent+6�   � 5�      $ White��6   $	 0.6Q�6,   � 5�   8   $ 1Q�6�    6E   �� *.,(Control.Background).(SolidColorBrush.Color)+ �� *TemplatedParent+6�   5�   4   5�   +   5�   #   5�      5�      5�      �� 5�       � �� 6+   Z� 5�      �� 5�      " ��6"   .�5�      ��  � ��
 �����6+   �� 5�       � ��$ 17(�6    �� 5�       � ��$
 Bold"�6"   �� 5�       � �� 5�      �� 5�      " ��6&   .R�5�      � $
	 grid��6   .+�5�      �� $&�� /Yatoo;component/Images/file.png��6#   5�       5�      P� 5�       �@J�IsMouseOver . . �5�   "   �� $
 grid��6?    � �� 5�   &   �� 5�   *   $	 0,0&�6>   $	 0,1&�6O   ��5�   3   � � 5�   8   $ 0Q�6�    6E   �� *.,(Control.Background).(SolidColorBrush.Color)+ �� *TemplatedParent+6�   � 5�      $ White��6   $	 0.6Q�6,   � 5�   8   $ 1Q�6�    6E   �� *.,(Control.Background).(SolidColorBrush.Color)+ �� *TemplatedParent+6�   5�   4   5�   +   5�   #   5�      5�      5�      �� 5�       � �� 6+   Z� 5�      �� 5�      " ��6    .�5�      ��  � ��
 �����6+   �� 5�       � ��$ 17(�6    �� 5�       � ��$
 Bold"�6"   �� 5�       � �� 5�      �� 5�      " ��6&   .R�5�      � $
	 grid��6   .+�5�      �� $(��"/Yatoo;component/Images/setter.png��6#   5�       5�      P� 5�       �@J�IsMouseOver . . �5�   "   �� $
 grid��6?    � �� 5�   &   �� 5�   *   $	 0,0&�6>   $	 0,1&�6O   ��5�   3   � � 5�   8   $ 0Q�6�    6E   �� *.,(Control.Background).(SolidColorBrush.Color)+ �� *TemplatedParent+6�   � 5�      $ White��6   $	 0.6Q�6,   � 5�   8   $ 1Q�6�    6E   �� *.,(Control.Background).(SolidColorBrush.Color)+ �� *TemplatedParent+6�   5�   4   5�   +   5�   #   5�      5�      5�      �� 5�       � �� 6+   Z� 5�      �� 5�   
   " ��6*   .�5�      ��  � �� 5�      �� 5�      " ��6&   .R�5�      �� $ bd��6S   $ 13,13,13,13��6!   $��27��6<   $��27��6G   
�������._�5�      �� $��20��6$   $��20��6/   $,��&/Yatoo;component/Images/characters.png��6;   6m   5�       5�      P� 5�       �@J�IsMouseOver . . �5�      �� $ bd��6A    � ��$ 1q�6Q   �� 5�   "   $ bd��6?    � �� 5�   &   �� 5�   *   $	 0,0&�6>   $ 0,1.5&�6O   ��5�   3   � � 5�   8   $
 #FFF��6E   $ 0Q�6R   � 5�   6   $	 0.5Q�6�    6C   ��  �� *TemplatedParent+$1 +(Button.Background).(SolidColorBrush.Color)�6�   � 5   6   $
 #FFF��6C   $ 1Q�6P   5  4   5  +   5  #   5     5     5
     �� 5      � �� 6+   Z� 5     �� 5  
   " ��6*   .�5     ��  � �� 5     �� 5     " ��6&   .R�5     �� $ bd��6S   $ 13,13,13,13��6!   $��27��6<   $��27��6G   
�������._�5     �� $��20��6$   $��20��6/   $+��%/Yatoo;component/Images/facesmile.png��6;   6l   5      5     P� 5      �@J�IsMouseOver . . �5     �� $ bd��6A    � ��$ 1q�6Q   �� 5  "   $ bd��6?    � �� 5  &   �� 5  *   $	 0,0&�6>   $ 0,1.5&�6O   ��5  3   � � 5  8   $
 #FFF��6E   $ 0Q�6R   � 5  6   $	 0.5Q�6�    6C   ��  �� *TemplatedParent+$1 +(Button.Background).(SolidColorBrush.Color)�6�   � 5   6   $
 #FFF��6C   $ 1Q�6P   5!  4   5#  +   5%  #   5&     5(     5*     �� 5+      � �� 6+   Z� 5,     �� 5.  
   " ��6'   .�5/     ��  � �� 50     �� 51     " ��6&   .R�52     �� $ bd��6S   $ 13,13,13,13��6!   $��22��6<   $��22��6G   
�������._�53     �� #��z��6$   $��30��6H   $��30��6S   $��0,-4,0,0q�6`   $��14(�6r   54      56     P� 57      �@J�IsMouseOver . . �58  "   �� $ bd��6D    � ��$ 1q�6T   �� 59  "   $ bd��6?    � �� 5:  &   �� 5;  *   $	 0,0&�6>   $ 0,1.5&�6O   ��5=  2   � � 5>  6   $
 #FFF��6C   $ 0Q�6P   � 5?  6   $	 0.5Q�6�    6C   ��  �� *TemplatedParent+$1 +(Button.Background).(SolidColorBrush.Color)�6�   � 5@  6   $
 #FFF��6C   $ 1Q�6P   5A  3   5C  +   5E  #   5F     5H     5J     �� 5K      � �� 6+   Z� 5L     �� 5N  
   " ��6(   .�5O     ��  � �� 5P     �� 5Q     " ��6&   .R�5R     �� $ bd��68   $��70��6!   $��22��6,   
�������._�5S     �� #��z��6$   $��70��6H   $��30��6S   $��0,-4,0,0q�6`   
��� � �$��14(�6�   5T     5U     5W     �� 5X      � �� 6+   Z� 5Y     5Z     ��5[     �� 5\  
    �� ImageSource$, &/Yatoo;component/Images/background.jpg��6    j� Stretch$ UniformToFill=�6J    j� TileMode$
 None=�6b     �� Opacity$  0Q�6r   .��5^     � .+�5_     �� 
���   �$��1q�6   $ 10,10,10,10��63   ��5`     �� 5a     -   5        WindowBackground��$, &/Yatoo;component/Images/background.jpg��5a  +   $ UniformToFill=�6`   $
 None=�6x   5c     �� 5d  
   -   5        ! ��Name$! button2��5d  �   #��[ 6   $��  ━��68   $��33��6F   $��Right=�6R   $��0,3,40,0q�6n   $	��Top=�6�   $��34��6�   6�   �� 5e     -   5        $! Button_Close��5e  �   #��[  6   $��  ╳��6F   $��33��6T   $��Right=�6`   $��0,0,0,0q�6|   $	��Top=�6�   $��39��6�   �� 5f     -   5        $��	UserImage��5f  
   $+��%/Yatoo;component/Images/未命名.jpg��6   $��	18,16,0,0q�6D   $
��Left=�6W   $��60��6r   $	��Top=�6~   $��60��6�   �� 5g  
   -   5        $��UserName��5g  9   $
��Bold"�6   $�� ��6"   $��28��6-   $��91.5,17.666,0,0q�6I   $	��Top=�6b   $
��Left=�6z   $��308.5��6�   �� 5h  
   -   5        $��MyMsg��5h  �   $
��Auto=�6�   $��3,0,0,31q�6�   $��91��6�   $��Bottom=�6�   $
��Left=�6�   $��395.5��6  6m  �� 5i  
   -   5        #��[ 5i     $��Button��6<   $��10.5,0,0,123q�6M   $
��Left=�6c   $
��26.5��6~   $��Bottom=�6�   $
��28.5��6�   6�   �� 5j  
   -	   5        #��[ 5j     $��Button��6<   $��
43,0,0,123q�6M   $
��Left=�6a   $��26��6|   $��Bottom=�6�   $��28.167��6�   6�   �� 5k  
   -
   5        $��ShowMsg��5k  F   $
��Auto=�6   $��
4,88,0,150q�6U   " �� 
IsReadOnly" . $
��Left=�6{   $��394.5��6�   � 5l  
   -   5        $��Grid_Expression��5l     $y�Hidden=�6'   $��4,245,302,149q�6;   
�������.+�5m     �� -   5        $��ListBox_Expression��5m     $��5,0,0,0q�6H   $	��105��6Y   ��60   �� ��5n     �� 5o     " ��6&   .R�5p     �� $��Disabled=�6(   $��Hidden=�6Q   .��5q     � # � Orientation$# 
Horizontal=�6(   $ K� IsItemsHost$ . ��. 6T   5r     5s     ��5u     �� 5v     .R�5w     �� $
��back��6!   
�������$��0q�6@   $��25��6T   $��25��6`   ._�5x     �� $��25��6$   $��25��6/   ��6;   �� $ image�5y     % �� Triggers% 5z     P� 5{      �@J�IsMouseOver . . �5|  "   �� $
 back��6D    � ��$ 1q�6V   5}     5     5�     5�     �� 5�  
   -   5        $��SetFont��5�  )   $y�Hidden=�6   & �� Orientation$& 
Horizontal=�68   $��4,0,302.5,149q�6Q   $��22��6h   $��Bottom=�6t   ���	#FFB7B7B7.+�5�     �� -   5        $��ComboBox_FontFamily��5�  *   
�������$	��100��6E   $��10,0,0,0q�6R   .@�5�     �� $��宋体��6   $��宋体)�6&   �� 5�     $��楷体��6   $��楷体)�6    �� 5�     $��黑体��6   $��黑体)�6    �� 5�     $��仿宋��6   $��仿宋)�6    �� 5�     $��微软雅黑��6   $��微软雅黑)�6"   �� 5�     $��	新宋体��6   $��	新宋体)�6"   5�     �� 5�     -   5        $��ComboBox_FontSize��5�  +   
�������$��50��6E   $��8,0,0,0q�6P   .@�5�     �� $��13��6   �� 5�     $��14��6   �� 5�     $��15��6   �� 5�     $��16��6   �� 5�     $��17��6   �� 5�     $��18��6   �� 5�     $��19��6   �� 5�     $��20��6   �� 5�     $��21��6   �� 5�     $��22��6   5�     �� 5�     -   5        $��Button_FontWeight��5�     #��[ 6.   $��B��6V   $��25��6b   $��8,0,0,0q�6n   $
��Bold"�6   6�   �� 5�     -   5        $��Button_FontStyle��5�     #��[ 6.   $��I��6V   $��25��6b   $��8,0,0,0q�6n   $��Italic$�6�   6�   �� 5�     -   5        $��Button_UnderLine��5�     #��[ 6.   $��25��6V   $��8,0,0,0q�6b   .��5�     �� $��	Underline{�6   $
��Bold"�68   U6M   5�     �� 5�     -   5        $��ComboBox_FontColor��5�  +   
�������$��80��6G   $��8,0,0,0q�6S   .@�5�     �� $��红色��6   
���  ��6    �� 5�     $��蓝色��6   
����  �6    �� 5�     $��黑色��6   
���   �6    �� 5�     $��黄色��6   
��� ���6    �� 5�     $��绿色��6   
��� � �6    �� 5�     $��紫色��6   
���� ��6    �� 5�     $��粉红��6   
�������6    �� 5�     $��浅蓝��6   
����ح�6    �� 5�     $��褐色��6   
���**��6    �� 5�     $��浅绿��6   
������6    5�     5�     �� 5�  
   -   5        $��关闭��5�     $��222.168,0,0,4q�62   $��23��6I   $��Bottom=�6U   $
��Left=�6p   $��65.666��6�   �� 5�     -   5        $��发送��5�     $��65.666��6   $��303.668,0,0,4q�6'   $��23��6>   $��Bottom=�6J   $
��Left=�6e   6�   �� 5�     $	��▼��6   $��369.334,0,0,4q�6   $��23��6.   $��Bottom=�6:   $
��Left=�6U   $��21.666��6p   �� 5�     #��[ 6   $
��Left=�61   $��96.829,45.832,0,0q�6L   $	��Top=�6g   $��40��6   $��40��6�   �� 5�     #��[ 6   $
��Left=�62   $��156.829,45.832,0,0q�6M   $	��Top=�6i   $��40��6�   $��40��6�   �� 5�     -   5        $��Button_SendFile��5�     #��[ 68   $��217.997,45.832,0,0q�6a   $	��Top=�6}   $��40��6�   $
��Left=�6�   $��40��6�   �� 5�  
   -   5        #��[ 5�     $
��Left=�6M   $��280.996,45.832,0,0q�6h   $	��Top=�6�   $��40��6�   $��40��6�   �� 5�  
   -   5        $��	File_List��5�     $y�Hidden=�6#   $
��Left=�67   $��405,88,0,30q�6R   $��289.5��6g   ��5�     �� 5�     .R�5�     �� ' ��Name$' 
stackpanel��6   $��80��61   $& 
Horizontal=�6=   .+�5�     �� $��	ImageIcon��6    $��50��61   $��50��6<   ��6H   �� $
 icon��� 5�     $��10,10,10,10q�6%   $& Vertical=�6:   $	��200��6Q   .+�5�     � $��20��6*   ( � Value( 66   �� $ Progress�� 5�     .+�5�  "   �� $
��name��6,   $	��150��68   $
��Left=�6_   $��12(�6z   $��Black"�6�   ��6D   �� $
 name�6�   �� 5�  "   $��12(�6G   $��Right=�6U   $��Black"�6q   ��6,   �� $
 size�6q   5�     � 5�     .+�5�  "   �� -   5        #��[	 5�  *   ) � Cursor$
) Hand��6x   $
��Left=�6�   ��6T   �� $ id���6�   �� $ Button1_name�y�6�   �� $ ShowButton1�6�   �� 5�  "   $
��Left=�6n   ��6(   �� $ Speed�y�6H   �� $ 	ShowSpeed�6n   �� 5�  "   -   5        #��[	 5�  )   $
) Hand��6v   $��Right=�6�   ��6R   �� $ id���6�   �� $ Button2_name�6�   5�     5�     5�     5�     5�  	   �� 5�  
   -   5        $��SharedFile_List��5�     $
��Left=�6)   $��405,88,0,30q�6D   $	��295��6Y   ��5�     �� 5�     .R�5�     �� $' 
stackpanel��6!   $��20��65   $	��270��6A   $& 
Horizontal=�6M   .+�5�     �� $��17��6    $��17��6+   ��67   �� $ image��� 5�     $
��name��66   $��10,0,0,0q�6$   $��12(�6]   $��Black"�6k   ��6B   �� $
 name�6k   �� 5�     $��(��6$   �� 5�     $��12(�6@   ��6%   �� $
 size�6@   �� 5�     $��)��6$   5�     5�     * � ContextMenu* 5�     �� 5�     .@�5�     m� -   5        $��下载文件��5�     6-   m� 5�     -   5        $��查看详细信息��5�     6/   �� 5�     + J� Opacity$	+ 0.8Q�6    61   m� 5�     -   5        $��刷新��5�     6+   5�     5�     � 5�     -   5        $��DownSharedFileProgress��5�  	   $y�Hidden=�6'   $��Right=�6;   $��0,0,14,4q�6W   $	��273��6i   $��22��6u   $��Bottom=�6�   .+�5�     � $
��Left=�6   $��0,0.336,0,0q�6,   $��118.319��6A   �� 5�     $	��12%��6   $��122.319,0.336,118.332,0q�6   $��10(�6:   �� 5�     $��122m/s��6   $��10(�6&   $��Right=�64   $��60��6P   $��0,0,69,0q�6[   �� 5�     #��[ 6   $	��╳��6=   $��0,0.336,-4,0q�6I   $��Right=�6_   $��26��6{   5�     5�     5�     