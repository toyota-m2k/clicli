   M S B A M L   `   `   `  ����   AutoClicker"    AutoClicker.view.HotKeyPanel   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008       � 	Resources   �� Source�� $ CommonResource.xaml?�%    .��� $	��250��.+���  �� Orientation$ Vertical=�$��Stretch=�.+��� $��HotKey for Start/Stop��   
DlgTitleBg#��[   �� Padding$ 5q��  � ColumnDefinitions �� $��20 ��� �� �� ��  � RowDefinitions �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto �.+�	� $��0��$��1��$
��None��$
��0, 5q� 	� 	GroupName$ HotKey�� d� 	IsChecked �� *HotKey+  EnumBooleanConverter �� 	Converter# [ 	 �� ConverterParameter$	 HK_NONE��
 �� Mode$
 TwoWay=��� $��1��$��0��$��5��$��NUMPAD��$ 5q�$
��0, 5q�	� $��2��$��2��$��/��$
��0, 5q�$ HotKey�� �� *HotKey+# [ $	 HK_DIV��$
 TwoWay=�	� $��2��$��3��$��*��$
��0, 5q�$ HotKey�� �� *HotKey+# [ $	 HK_MUL��$
 TwoWay=�	� $��2��$��4��$��-��$
��0, 5q�$ HotKey�� �� *HotKey+# [ $	 HK_SUB��$
 TwoWay=�	� $��3��$��1��$��7��$
��0, 5q�$ HotKey�� �� *HotKey+# [ $		 HK7��	� $��3��$��2��$��8��$
��0, 5q�$ HotKey�� �� *HotKey+# [ $		 HK8��	� $��3��$��3��$��9��$
��0, 5q�$ HotKey�� �� *HotKey+# [ $		 HK9��	� $��3��$��4��$��+��$
��0, 5q�$ HotKey�� �� *HotKey+# [ $	 HK_PLS��	� $��4��$��1��$��4��$
��0, 5q�$ HotKey�� �� *HotKey+# [ $		 HK4��	� $��4��$��2��$��5��$
��0, 5q�$ HotKey�� �� *HotKey+# [ $		 HK5��	� $��4��$��3��$��6��$
��0, 5q�$ HotKey�� �� *HotKey+# [ $		 HK6��	� $��5��$��1��$��1��$
��0, 5q�$ HotKey�� �� *HotKey+# [ $		 HK1��	� $��5��$��2��$��2��$
��0, 5q�$ HotKey�� �� *HotKey+# [ $		 HK2��	� $��5��$��3��$��3��$
��0, 5q�$ HotKey�� �� *HotKey+# [ $		 HK3��	� $��6��$��1��$��0��$
��0, 5q�$ HotKey�� �� *HotKey+# [ $		 HK0��$
 TwoWay=�	� $��6��$��3��$��.��$
��0, 5q�$ HotKey�� �� *HotKey+# [ $	 HK_DOT���� $��7��$��0��$��5��$��Cursor��$ 5q�$
��0, 5q�	� $��8��$��1��$
��Home��$
��0, 5q�$ HotKey�� �� *HotKey+# [ $	 HK_HOME��	� $��9��$��1��$	��End��$
��0, 5q�$ HotKey�� �� *HotKey+# [ $	 HK_END��	� $��8��$��2��$
��PgUp��$
��0, 5q�$ HotKey�� �� *HotKey+# [ $	 HK_PGUP��	� $��9��$��2��$
��PgDn��$
��0, 5q�$ HotKey�� �� *HotKey+# [ $	 HK_PGDN���� $��10��$��0��$��5��$ 
Horizontal=�$��Center=�$��0,15,0,5q�.+��� -   $��OK��$��80�� �� 	IsDefault . $��0,0,10,0q�$��Bottom=�  OkButton#��[ �� -   $��Cancel��$��80�� �� IsCancel . $	��Top=�  CancelButton#��[ �  �� $��20 ��� �� �� 