   M S B A M L   `   `   `  ����F  BAutoClicker, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null3clr-namespace:AutoClicker.viewAutoClicker.view  5   	   $    AutoClicker.view.ConditionView   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:AutoClicker.view     5   	   -   5          � WindowStyle$
  None=�5	   	   $	��250��5   	   $	��150��6    � 	Resources 5       �� Source�� 5   
   $ CommonResource.xaml?�6   %    .��5      � ���#f0f0f0 � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� -   5         ��Name$ ConditionListView��5       $��0��6   $��0q�6S   ��6;   ��  �� View 5      �� 5      -   5        ConditionGridView��   ListViewHeaderlessStyle! �� ColumnHeaderContainerStyle# [  5   6   P�5      �� 5       �� Width$	 100��6)    �� CellTemplate 5      �� 5   "   .R�5   &   �� 	 �� Orientation$	 
Horizontal=�61   .+�5   *   � $��20��6/   $��20��6:   y�6F   �� *	IsValid+  BoolToVisibility
 �� 	Converter#
 [ .+�5   .   �� 
���   �$��1q�6I   ��5   2   �� 5   6    �� Color 6F   �� *ColorRange[8]+5!   /   � 5"   .   $��	10,10,0,0q�63   $��10��6F   $��10��6Q   y�5#   ?   �� *
Negation+#
 [ .+�5%   2   �� 
���   �$��1q�6M   ��5&   6   �� 5'   :   $ #FFffff��6J   5)   3   �� 5*   2   	 �� X1$ 0��67   	 �� X2$ 10��6>   	 �� Y1$ 0��6F   	 �� Y2$ 10��6M   
���   �$��1��6d   �� 5+   2   $ 10��67   $ 0��6?   $ 0��6F   $ 10��6M   
���   �$��1��6d   5,   /   5.   +   �� 5/   *   $��4,0,0,0q�64   y�6E   �� *	IsValid+#
 [ �� 50   *   -   5          NoUnderlineHyperlink#��[ 50   4   .�6�   .�51   .   �� .�62   �52   2   `� 53   6    �� StringFormat$ {0},{1}��6C   .1�54   :   ��  �� Path$ ScreenPoint.Point.X�6B    �� Mode$ OneWay=�6]   �� 55   :   $ ScreenPoint.Point.Y�6B   $ OneWay=�6]   56   7   .�57   <   58   /   59   +   5:   +   5;   '   5<   #   5>      5@      5B      �� 5C   
   $��1��6   $	��AND��6!   $	��4,2q�6�   y�6,   �� *Combination+  EnumVisibilityConverter#
 [  �� ConverterParameter$	 AND��6�   �� 5D   
   $��1��6   $��OR��6!   $	��4,2q�6�   y�6+   �� *Combination+#
 [ $ OR��6�   5E      5F      