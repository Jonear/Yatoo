   M S B A M L   `   `   `  ����	  Yatoo    Yatoo.UserWindow   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      d `Microsoft.Expression.Controls, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35>ec5http://schemas.microsoft.com/expression/2010/controls  5      -   5          �Name$  Window��5       � Title$ 
UserWindow��5      $	��570��5      $	��400��6    � WindowStyle$
 None=�5	       � 
ResizeMode$ NoResize=�5
       � ShowInTaskbar .   � AllowsTransparency .  � 	Resources 5       �� 
TargetType �� Property	 �� Value
 �� 
TargetType �� CornerRadius �� 
StartPoint �� EndPoint � Offset � Color �� RelativeSource �� Path �� Orientation �� Triggers P� Property P� Value �� 
TargetName J� Opacity��%e  '��       �� 5   
   " ��6   .�5      ��  � ��	 5      �� 5      "
 ��6&   .R�5      �� $
��back��6!   $ 10,10,10,10��6-   
�������$��0q�6[   $��80��6o   $��80��6{   ��5      �� 5   "   $	 0,0&�66   $	 0,1&�6G   ��5   *   � � 5   .   $	 0.0Q�6�    6;   ��  �� *TemplatedParent+$1 +(Button.Background).(SolidColorBrush.Color)�6�   � 5   .   $	 0.5Q�6�    6;   ��  �� *TemplatedParent+$1 +(Button.Background).(SolidColorBrush.Color)�6�   � 5   .   $
 0.51Q�6�    6;   ��  �� *TemplatedParent+$1 +(Button.Background).(SolidColorBrush.Color)�6�   � 5   .   $ 1Q�6�    6;   ��  �� *TemplatedParent+$1 +(Button.Background).(SolidColorBrush.Color)�6�   5   +   5   #   ._�5       �� $ Vertical=�6*   $��Center=�6A   .+�5!   "   �� $��50��6(   $��50��63   ��6?   �� $ image��� 5"   "   $��12(�6G   $��Center=�6U   $��Black"�6r   ��6,   �� $
 name�6r   5#      5$       5%      P� 5&       �@J�IsMouseOver . . �5'   "   �� $
 back��6?    � ��	 5(   &   �� 5)   *   $	 0,0&�6>   $	 0,1&�6O   ��5+   2   � � 5,   6   $
 Gray��6C   $	 0.0Q�6P   � 5-   6   $	 0.5Q�6�    6C   ��  �� *TemplatedParent+$1 +(Button.Background).(SolidColorBrush.Color)�6�   � 5.   6   $
 Gray��6C   $ 1Q�6P   5/   3   51   +   53   #   54      56      58      �� 59       �@�ContextMenu	 5:      �� 5;      .@�5<      m� $��发送信息��6#   m� 5=      $��发送文件��6#   m� 5>      $��查看共享资源��6#   �� 5?      $	 0.8Q�6$   65   m� 5@      $��皮肤��6#   m� 5A      $��设置��6#   m� 5B      $��添加用户��6#   �� 5C      $	 0.8Q�6$   65   m� 5D      $��刷新��6#   5E      5G      5H      5I      ��5J      �� 5K   
    �� ImageSource$, &/Yatoo;component/Images/background.jpg��6    j� Stretch$ UniformToFill=�6J    j� TileMode$
 None=�6b    �� Opacity$ 0Q�6r   .��5M      � -   5         �Name$ 
LayoutRoot��5M      .+�5N   
   �� -   5        $��BorderBackground��5N      $ 10,10,10,10��6)   $	��400��6D   
���   �$��1q�6e   $	��Top=�6y   ��5O      �� 5P      $, &/Yatoo;component/Images/background.jpg��6   $ UniformToFill=�6R   $
 None=�6j   $	 0.7Q�6z   ._�5R      �� -   5        $��	User_List��5R      ��6'   �� ��5S      �� 5T      "
 ��6&   .R�5U      �� $��Disabled=�6(   $��Hidden=�6Q   .��5V      �  � Orientation$ 
Horizontal=�6(    K� IsItemsHost . ��. 6T   5W      5X       � ContextMenu 5[      �� 5\      .@�5]      m� $��皮肤��6#   m� 5^      $��设置��6#   m� 5_      $��添加用户��6#   �� 5`      $	 0.8Q�6$   65   m� 5a      $��刷新��6#   5b      5d      5e      5f      5g      