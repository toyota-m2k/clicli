   M S B A M L   `   `   `  ����F  BAutoClicker, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null)clr-namespace:AutoClickerAutoClicker  5      3clr-namespace:AutoClicker.viewAutoClicker.view  5      '    !AutoClicker.view.ProjectEditPanel   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      +localclr-namespace:AutoClicker     5      /viewclr-namespace:AutoClicker.view     5        � 	Resources  5
       �� Source�� 5   
   $ DialogResource.xaml?�6   %    .��5      � .+�5   
   � -   5         �Name$ ProjectEditMain��5   G   $��Center=�6   $��Center=�6,    � RowDefinitions 5      �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6     � ColumnDefinitions 5      �� 5      $
��Auto �6#   �� 5      $
��Auto �6#   �� 5      $
��Auto �6#   .+�5"      �� $��0��6   $��0��6%   $��3��65    �� Padding$ 4q�6I   $��Project Settings��6U   $
��0,10q�6m      
DlgTitleBg#��[  6{   �� 5%      $��1��6   $��0��6%   $
��Name��65     left#��[ 6A   �� 5&      -   5         ��Name$ TxName��5&   3   $��1��6   $��1��6#    	 right#��[ 6Y   ��6C   �� *Name+6Y   �� 5)      $��2��6   $��0��6%   $��Pointing Mode��65   #��[ 6J   �� 5*      $��2��6   $��1��6&   $	��Top=�66   $
��Left=�6N    �� Orientation$ Vertical=�6i   #��[ 6�   .+�5+      	� $��Absolute��6    d� 	IsChecked 61   �� *Mode.IsAbsolute+	� 5,      $��Relative��6    61   �� *Mode.IsRelative+5-      �� 50      $��3��6   $��0��6%   $��
Base Point��65   #��[ 6G   y�6e   �� *Mode.IsRelative+ " VisibilityHiddenFalseConverter	 �� 	Converter#	 [ �� 51      -   5        $��3��51      $��1��6"     StrongButton#��[ 62   $
��auto��6X   y�6}   �� *Mode.IsRelative+#	 [ .��52      �� ��53      `� 54      
 �� StringFormat$
 	({0},{1})��6'   .1�55      ��  �� Path$ Mode.BasePoint.X�6&   �� 56      $ Mode.BasePoint.Y�6&   57      59      5:      �� 5;      $��4��6   $��1��6&   $ 
Horizontal=�66   $��0 5 0 15q�6O   .+�5<      �� -   5          	NorButton#��[ 5<      $��	Show Pos.��6<   $��80��6P   y�6s   �� *Mode.IsRelative+#	 [ �� 5=      -   5        #��[ 5=      $��Adjust��6<   $��80��6M   y�6n   �� *Mode.IsRelative+#	 [ 5>      �� 5A      $��5��6   $��0��6%   $��Repeat��65   #��[ 6C   �� 5B      $��5��6   $��1��6$   $��Infinite��64   #��[ 6G   $��Center=�6f    6�   �� *
IsInfinite+    AutoClicker.NumericTextBox  5C      $��6��6#   $��1��60   #��[ 6@   $��Center=�6w   ��6_   �� *Repeat+y�6�   �� *
IsInfinite+ ! VisibilityHiddenTrueConverter#	 [ �� 5D      $��6��6   $��2��6%   $��times��65   #��[ 6B   y�6a   �� *
IsInfinite+#	 [ �� 5G      $��7��6   $��0��6%   $	��URL��65   #��[ 6A   �� 5H      -   5        $ TxUrl��5H   3   $��7��6   $��1��6#   #��[ 6B   ��6a   �� *URL+�� 5J      $��8��6   $��0��6&   $��3��66   $ 
Horizontal=�6J   $��Center=�6c   $��0,15,0,5q�6�   .+�5K      �� -   5        $��OK��5K      $��80��6&    �� 	IsDefault . $��0,0,10,0q�6B   $��Bottom=�6T     OkButton#��[ 6�   �� 5L      -   5        $��Cancel��5L      $��80��6*    �� IsCancel . $	��Top=�6E     CancelButton#��[ 6s   5M      5N      � 5O   
   -	   5        $ ProjectEditSub��5O      $��Center=�6'   $��Center=�6D   $y�Hidden=�6_   .+�5P      $   AutoClicker.view.PositionTuner  -
   5          Name$ PositionTuner��5P   !   5Q      5R      5S      