   M S B A M L   `   `   `  ���� F  BAutoClicker, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null)clr-namespace:AutoClickerAutoClicker  %    AutoClicker.view.OperatingPanel   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     +localclr-namespace:AutoClicker     -     � 	Resources   �� Source�� $ CommonResource.xaml?�%    .���      
LPlayImage �� 
TargetType �� Triggers �� Binding �� Value �� Property  	LStopIcon �� Value  
LStartIcon��%�   &	        7  7  �� " �� ��  �� *	IsPlaying+ true.a���  � ��8   ��  �� *	IsPlaying+	 false.a���  � ��8   � ColumnDefinitions �� $
��Auto ��� $
��Auto ��� $��* �.+�� -   	 �Name$	 RegistrationPane��$��0��    RadioToggle
 �� BasedOn  RadioToggleText��%�   '	�       7  '�� ^     7  �� " 	�8
   .�-   ��  �@d�	IsChecked �� *
IsSelected+��  � ��$
 12,2q��� " ��8
    � RowDefinitions �� �� �� �� �� .+�	� $��0�� � DataContext �� *Registrations[0]+.��(� $��Center=�.���� $��1��	� $��1�� �� *Registrations[1]+.��(� $��Center=�.���� $��2��	� $��2�� �� *Registrations[2]+.��(� $��Center=�.���� $��3��	� $��3�� �� *Registrations[3]+.��(� $��Center=�.���� $��4��	� $��4�� �� *Registrations[4]+.��(� $��Center=�.���� $��5���� $��1��$��3��$��Center=�$��1q�
�������� $��2�� �� $��* ��� $
��Auto �.+�� $��0��.+��� -   .���� #��[  $	��320��$	��320��� $��1��  TBOperatingBg#��[ .+��� $��5q�$��Right=�$��Stretch=� �� Orientation$ 
Horizontal=�.+�� y��� *IsSingleProjectSelection+ " VisibilityHiddenFalseConverter �� 	Converter# [ .+��� $ Vertical=�$��Center=�y��� *	IsPlaying+ ! VisibilityHiddenTrueConverter# [ .+��� 
������� d� 	IsChecked �� *CurrentProject.IsInfinite+ �� Mode$ TwoWay=�.��
Infinite    AutoClicker.NumericTextBox  $��16,4,0,0q����� *CurrentProject.Repeat+$ TwoWay=�y��� *CurrentProject.IsInfinite+# [ �� $ Vertical=�$
��15,0q�$��Center=�.+��� -   $��40��$��60��.��� $��Center=�$��Center=� �� �� .+��� $��0��$��HotKey��  
TBTargetBg#��[  �� TextAlignment$ Center=��� $��1��#��[ $
��Bold"�$ Center=����� *
HotKeyName+�� -    �� IsCancel . .����  	 
FoldUpIcon#��[	 