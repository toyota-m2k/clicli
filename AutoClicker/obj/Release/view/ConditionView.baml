   M S B A M L   `   `   `  ���� F  BAutoClicker, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null3clr-namespace:AutoClicker.viewAutoClicker.view  $    AutoClicker.view.ConditionView   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     0localclr-namespace:AutoClicker.view     -     � WindowStyle$
  None=�$	��250��$	��150�� � 	Resources  �� Source�� $ CommonResource.xaml?�%    .��� ���#f0f0f0 � RowDefinitions �� $��* ��� $
��Auto �.+��� -    ��Name$ ConditionListView��$��0��$��0q�����  �� View �� -   ConditionGridView��   ListViewHeaderlessStyle! �� ColumnHeaderContainerStyle# [  P���  �� Width$	 100�� �� CellTemplate �� .R��� 	 �� Orientation$	 
Horizontal=�.+�� $��20��$��20��y��� *	IsValid+  BoolToVisibility
 �� 	Converter#
 [ .+��� 
���   �$��1q�����  �� Color �� *ColorRange[8]+� $��	10,10,0,0q�$��10��$��10��y��� *
Negation+#
 [ .+��� 
���   �$��1q����� $ #FFffff���� 	 �� X1$ 0��	 �� X2$ 10��	 �� Y1$ 0��	 �� Y2$ 10��
���   �$��1���� $ 10��$ 0��$ 0��$ 10��
���   �$��1���� $��4,0,0,0q�y��� *	IsValid+#
 [ �� -     NoUnderlineHyperlink#��[ .�.��� .��`�  �� StringFormat$ {0},{1}��.1���  �� Path$ ScreenPoint.Point.X� �� Mode$ OneWay=��� $ ScreenPoint.Point.Y�$ OneWay=�.��� $��1��$	��AND��$	��4,2q�y��� *Combination+  EnumVisibilityConverter#
 [  �� ConverterParameter$	 AND���� $��1��$��OR��$	��4,2q�y��� *Combination+#
 [ $ OR��