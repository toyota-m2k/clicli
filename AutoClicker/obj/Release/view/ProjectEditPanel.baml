   M S B A M L   `   `   `  ���� F  BAutoClicker, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null)clr-namespace:AutoClickerAutoClicker  3clr-namespace:AutoClicker.viewAutoClicker.view  '    !AutoClicker.view.ProjectEditPanel   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     +localclr-namespace:AutoClicker     /viewclr-namespace:AutoClicker.view       � 	Resources   �� Source�� $ DialogResource.xaml?�%    .��� .+�� -    �Name$ ProjectEditMain��$��Center=�$��Center=� � RowDefinitions �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto � � ColumnDefinitions �� $
��Auto ��� $
��Auto ��� $
��Auto �.+��� $��0��$��0��$��3�� �� Padding$ 4q�$��Project Settings��$
��0,10q�   
DlgTitleBg#��[  �� $��1��$��0��$
��Name��  left#��[ �� -    ��Name$ TxName��$��1��$��1�� 	 right#��[ ���� *Name+�� $��2��$��0��$��Pointing Mode��#��[ �� $��2��$��1��$	��Top=�$
��Left=� �� Orientation$ Vertical=�#��[ .+�	� $��Absolute�� d� 	IsChecked �� *Mode.IsAbsolute+	� $��Relative�� �� *Mode.IsRelative+�� $��3��$��0��$��
Base Point��#��[ y��� *Mode.IsRelative+ " VisibilityHiddenFalseConverter	 �� 	Converter#	 [ �� -   $��3��$��1��  StrongButton#��[ $
��auto��y��� *Mode.IsRelative+#	 [ .���� ��`� 
 �� StringFormat$
 	({0},{1})��.1���  �� Path$ Mode.BasePoint.X��� $ Mode.BasePoint.Y��� $��4��$��1��$ 
Horizontal=�$��0 5 0 15q�.+��� -     	NorButton#��[ $��	Show Pos.��$��80��y��� *Mode.IsRelative+#	 [ �� -   #��[ $��Adjust��$��80��y��� *Mode.IsRelative+#	 [ �� $��5��$��0��$��Repeat��#��[ �� $��5��$��1��$��Infinite��#��[ $��Center=� �� *
IsInfinite+    AutoClicker.NumericTextBox  $��6��$��1��#��[ $��Center=����� *Repeat+y��� *
IsInfinite+ ! VisibilityHiddenTrueConverter#	 [ �� $��6��$��2��$��times��#��[ y��� *
IsInfinite+#	 [ �� $��7��$��0��$	��URL��#��[ �� -   $ TxUrl��$��7��$��1��#��[ ���� *URL+�� $��8��$��0��$��3��$ 
Horizontal=�$��Center=�$��0,15,0,5q�.+��� -   $��OK��$��80�� �� 	IsDefault . $��0,0,10,0q�$��Bottom=�  OkButton#��[ �� -   $��Cancel��$��80�� �� IsCancel . $	��Top=�  CancelButton#��[ � -	   $ ProjectEditSub��$��Center=�$��Center=�$y�Hidden=�.+�$   AutoClicker.view.PositionTuner  -
     Name$ PositionTuner��