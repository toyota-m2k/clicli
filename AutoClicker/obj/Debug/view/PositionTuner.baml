   M S B A M L   `   `   `  ����F  BAutoClicker, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null3clr-namespace:AutoClicker.viewAutoClicker.view  5      $    AutoClicker.view.PositionTuner   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:AutoClicker.view     5        � 	Resources  5	       �� Source�� 5
   
   $ CommonResource.xaml?�6   %    .��5      ��  �� Orientation$ 
Horizontal=�6   $��Center=�6*   .+�5   
   �� -   5        $	��400��5      $	��400��6   .+�5      �� -   5         ��Name$ CapturedImage��5      $
��Fill=�6c   ��6+   �� *
MapWidth+��6F   �� *	MapHeight+6c   �� 5      
���   � J� Opacity$	 0.1Q�6�   ��6   �� *	ZoomRatio+��64   �� *	MapHeight+ �� Left 6^   �� *CrosshairOffset.X+6�   �� 5      
���   �$	 0.1Q�6�   ��6   �� *
MapWidth+��63   �� *	ZoomRatio+
 �� Top 6]   �� *CrosshairOffset.Y+6�   �� 5      
��� �����6   �� *	ZoomRatio+��64   �� *	ZoomRatio+ 6_   �� *CrosshairOffset.X+ 6�   �� *CrosshairOffset.Y+5      �� 5   
   $ Vertical=�6   $��15,0,0,0q�6,   $��Center=�6>   .+�5      �� -   5        $��0,0,0,10q�5         OkButton#��[  6'   $��OK��6I   $��80��6V    �� 	IsDefault . 6r   �� 5      -   5        $��0,0,0,10q�5        CancelButton#��[ 6'   $��Cancel��6M   $��80��6^    �� IsCancel . 6y   �� 5      -   5        $��0,0,0,10q�5        	NorButton#��[ 6'   $��Update��6J   $��80��6[   6f   	� 5      $��0,5,0,2q�6   	 	� 	GroupName$	 Magnification��6+   $	��x10��6E   
 d� 	IsChecked
 6S   �� *ViewMagnification+ �� Mode$ TwoWay=�  EnumBooleanConverter �� 	Converter# [  �� ConverterParameter$ SMALL��	� 5      $��0,0,0,2q�6   $	 Magnification��6+   $	��x15��6E   
 6S   �� *ViewMagnification+$ TwoWay=�# [ $ MEDIUM��	� 5      $��0,0,0,2q�6   $	 Magnification��6+   $	��x20��6E   
 6S   �� *ViewMagnification+$ TwoWay=�# [ $ LARGE��	� 5      $��0,0,0,2q�6   $	 Magnification��6+   $	��x30��6E   
 6S   �� *ViewMagnification+$ TwoWay=�# [ $ EXTRA��5      5      5      