   M S B A M L   `   `   `  ���� F  BAutoClicker, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null3clr-namespace:AutoClicker.viewAutoClicker.view  $    AutoClicker.view.PositionTuner   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     0localclr-namespace:AutoClicker.view       � 	Resources   �� Source�� $ CommonResource.xaml?�%    .����  �� Orientation$ 
Horizontal=�$��Center=�.+��� -   $	��400��$	��400��.+��� -    ��Name$ CapturedImage��$
��Fill=����� *
MapWidth+���� *	MapHeight+�� 
���   � J� Opacity$	 0.1Q����� *	ZoomRatio+���� *	MapHeight+ �� Left �� *CrosshairOffset.X+�� 
���   �$	 0.1Q����� *
MapWidth+���� *	ZoomRatio+
 �� Top �� *CrosshairOffset.Y+�� 
��� ������� *	ZoomRatio+���� *	ZoomRatio+ �� *CrosshairOffset.X+ �� *CrosshairOffset.Y+�� $ Vertical=�$��15,0,0,0q�$��Center=�.+��� -   $��0,0,0,10q�   OkButton#��[  $��OK��$��80�� �� 	IsDefault . �� -   $��0,0,0,10q�  CancelButton#��[ $��Cancel��$��80�� �� IsCancel . �� -   $��0,0,0,10q�  	NorButton#��[ $��Update��$��80��	� $��0,5,0,2q�	 	� 	GroupName$	 Magnification��$	��x10��
 d� 	IsChecked
 �� *ViewMagnification+ �� Mode$ TwoWay=�  EnumBooleanConverter �� 	Converter# [  �� ConverterParameter$ SMALL��	� $��0,0,0,2q�$	 Magnification��$	��x15��
 �� *ViewMagnification+$ TwoWay=�# [ $ MEDIUM��	� $��0,0,0,2q�$	 Magnification��$	��x20��
 �� *ViewMagnification+$ TwoWay=�# [ $ LARGE��	� $��0,0,0,2q�$	 Magnification��$	��x30��
 �� *ViewMagnification+$ TwoWay=�# [ $ EXTRA��