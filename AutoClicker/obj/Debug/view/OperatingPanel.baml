   M S B A M L   `   `   `  ����F  BAutoClicker, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null)clr-namespace:AutoClickerAutoClicker  5      %    AutoClicker.view.OperatingPanel   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      +localclr-namespace:AutoClicker     5      -   5          � 	Resources  5       �� Source�� 5   
   $ CommonResource.xaml?�6   %    .��5      �   5   
      
LPlayImage �� 
TargetType �� Triggers �� Binding �� Value �� Property  	LStopIcon �� Value  
LStartIcon��%	  &	        7  7  �� 5      " ��6'    5      �� 5       6"   �� *	IsPlaying+ true6@   .a�5      ��  � ��8   63   5      �� 5       6"   �� *	IsPlaying+	 false6@   .a�5      ��  � ��8  63   5      5      5       � ColumnDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5"   
   � -   5        	 �Name$	 RegistrationPane��5"      $��0��6     5#        RadioToggle
 �� BasedOn  RadioToggleText��%�   '	�       7  '�� �     7  �� 5$      " 	�6   8
   .�5%      -   5        �� 5'       �@d�	IsChecked 62   �� *
IsSelected+�� 5(       � ��$
 12,2q�60   5)      �� 5*      " ��6   8
   6/   5+       � RowDefinitions 5,      �� 5-      �� 5.      �� 5/      �� 50      �� 51      .+�53      	� $��0��6    � DataContext 6'   �� *Registrations[0]+.��54      (� $��Center=�6   .��55      �� $��1��6    56      57      	� 58      $��1��6    6'   �� *Registrations[1]+.��59      (� $��Center=�6   .��5:      �� $��2��6    5;      5<      	� 5=      $��2��6    6'   �� *Registrations[2]+.��5>      (� $��Center=�6   .��5?      �� $��3��6    5@      5A      	� 5B      $��3��6    6'   �� *Registrations[3]+.��5C      (� $��Center=�6   .��5D      �� $��4��6    5E      5F      	� 5G      $��4��6    6'   �� *Registrations[4]+.��5H      (� $��Center=�6   .��5I      �� $��5��6    5J      5K      5L      �� 5M   
   $��1��6   $��3��6'   $��Center=�61   $��1q�6N   
�������6b   � 5N   
   $��2��6    5O      �� 5P      $��* �6    �� 5Q      $
��Auto �6    .+�5T      � $��0��6   .+�5U      �� -   5        .��5V      �� #��[  6   $	��320��6@   $	��320��6O   5W      5X      � 5Y      $��1��6     TBOperatingBg#��[ 6    .+�5Z      �� $��5q�5[      $��Right=�5\      $��Stretch=�5]       �� Orientation$ 
Horizontal=�5^      .+�5a      � y�6   �� *IsSingleProjectSelection+ " VisibilityHiddenFalseConverter �� 	Converter# [ .+�5b      �� $ Vertical=�6%   $��Center=�6<   y�6W   �� *	IsPlaying+ ! VisibilityHiddenTrueConverter# [ .+�5c      �� 
������� d� 	IsChecked 6:   �� *CurrentProject.IsInfinite+ �� Mode$ TwoWay=�.��6w   
Infinite6�       AutoClicker.NumericTextBox  5d      $��16,4,0,0q�6�   ��63   �� *CurrentProject.Repeat+$ TwoWay=�y�6g   �� *CurrentProject.IsInfinite+# [ 6�   5e      5f      �� 5h      $ Vertical=�6!   $
��15,0q�68   $��Center=�6F   .+�5i      �� -   5        $��40��5j   %   $��60��5k   %   .��5m      � $��Center=�6#   $��Center=�6>    5n   "   �� 5o   &   �� 5p   &   .+�5r   "   �� $��0��6,   $��HotKey��69     
TBTargetBg#��[ 6G    �� TextAlignment$ Center=�6p   �� 5s   "   $��1��6,   #��[ 6U   $
��Bold"�6~   $ Center=�6�   ��69   �� *
HotKeyName+6�   5t      5u      5v      �� 5w      -   5         �� IsCancel . .��5x      ��  	 
FoldUpIcon#��[	 6    5y      5{      5|      5~      5      5�      