   M S B A M L   `   `   `  ���� 	  Yatoo    Yatoo.UserWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Dec5http://schemas.microsoft.com/expression/2010/controls     -     �Name$  Window�� � Title$ 
UserWindow��$	��570��$	��400�� � WindowStyle$
 None=� � 
ResizeMode$ NoResize=� � ShowInTaskbar .   � AllowsTransparency .  � 	Resources  �� 
TargetType �� Property	 �� Value
 �� 
TargetType �� CornerRadius �� 
StartPoint �� EndPoint � Offset � Color �� RelativeSource �� Path �� Orientation �� Triggers P� Property P� Value �� 
TargetName J� Opacity��%�  '��       �� " ��.���  � ��	 �� "
 ��.R��� $
��back��$ 10,10,10,10��
�������$��0q�$��80��$��80������ $	 0,0&�$	 0,1&���� � $	 0.0Q� ��  �� *TemplatedParent+$1 +(Button.Background).(SolidColorBrush.Color)�� $	 0.5Q� ��  �� *TemplatedParent+$1 +(Button.Background).(SolidColorBrush.Color)�� $
 0.51Q� ��  �� *TemplatedParent+$1 +(Button.Background).(SolidColorBrush.Color)�� $ 1Q� ��  �� *TemplatedParent+$1 +(Button.Background).(SolidColorBrush.Color)�._��� $ Vertical=�$��Center=�.+��� $��50��$��50������ $ image��� $��12(�$��Center=�$��Black"����� $
 name� P�  �@J�IsMouseOver . . ��� $
 back�� � ��	 �� $	 0,0&�$	 0,1&���� � $
 Gray��$	 0.0Q�� $	 0.5Q� ��  �� *TemplatedParent+$1 +(Button.Background).(SolidColorBrush.Color)�� $
 Gray��$ 1Q���  �@�ContextMenu	 �� .@�m� $��发送信息��m� $��发送文件��m� $��查看共享资源���� $	 0.8Q�m� $��皮肤��m� $��设置��m� $��添加用户���� $	 0.8Q�m� $��刷新������  �� ImageSource$, &/Yatoo;component/Images/background.jpg�� j� Stretch$ UniformToFill=� j� TileMode$
 None=� �� Opacity$ 0Q�.��� -    �Name$ 
LayoutRoot��.+��� -   $��BorderBackground��$ 10,10,10,10��$	��400��
���   �$��1q�$	��Top=����� $, &/Yatoo;component/Images/background.jpg��$ UniformToFill=�$
 None=�$	 0.7Q�._��� -   $��	User_List������ ���� "
 ��.R��� $��Disabled=�$��Hidden=�.���  � Orientation$ 
Horizontal=� K� IsItemsHost . ��.  � ContextMenu �� .@�m� $��皮肤��m� $��设置��m� $��添加用户���� $	 0.8Q�m� $��刷新��