   M S B A M L   `   `   `  ���� F  BAutoClicker, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null)clr-namespace:AutoClickerAutoClicker  3clr-namespace:AutoClicker.viewAutoClicker.view  '    !AutoClicker.view.GetPositionPanel   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     +localclr-namespace:AutoClicker     /viewclr-namespace:AutoClicker.view       � 	Resources   �� Source�� $ CommonResource.xaml?�%    .��� $��	0,0,0,-51q�.+��� -    ��Name$ GetPositionMain��$��Center=�$��Center=�.+��� $
��Bold"�$
��Wrap=� �� TextAlignment$ Center=�Move mouse to target point,�� .,and press 'A' key or '+' key in numeric pad.�� -    ��Name$ GPTChk��$��Center=�$��0,20,0,0q�.���� ���� *	RemainSec+ �� StringFormat$% get position after {0} seconds.��� $��Center=�$��0,20,0,0q����	#FFCFDFEAy��� *EnableRatio+   BoolToVisibility �� 	Converter# [   � RowDefinitions �� $
��Auto ��� $
��Auto ��� $
��Auto �.+�� $��0�� � ColumnDefinitions �� $	��50* ��� $	��50* �  	 �� 
TargetType  RadioToggle
 �� BasedOn �� Property �� Value��%5   '	�       7  �� "	 	�8
   .���  � ��$ 4q�.+�	� $��0��$��Get Position�� d� 	IsChecked$
 TrueY� 	� 	GroupName$ OperationMode��$��0q�	� $��1��$��Move Target��$ OperationMode��$��0q� �� *SettingTargetWinPos+ �� Mode$ TwoWay=��� $��1�� �� Orientation$ Vertical=�$��Center=�$	��350��$	��0,5q�y��� *SettingTargetWinPos+  NegBoolToVisibility# [ .+��� $ 
Horizontal=�$��Center=�$��0,5,0,15q�.+��� $��Center=��� .�
Based at �� -     NoUnderlineHyperlink#��[ .�.��� .��`� $ 	({0},{1})��.1���  �� Path$ 
OrgPoint.X�$ OneWay=��� $ 
OrgPoint.Y�$ OneWay=�.��� -     	NorButton#��[ $��Adjust��$��5,0,0,0q�$	��8,2q��� $ 
Horizontal=�$��Center=�.+��� $��	Exp.Ratio��$��0,0,10,0q�    AutoClicker.NumericTextBox  -     Name$ 	RatioEdit��$	��100������ *Ratio+�� $��%��$��Center=��� $��2��$��1��$��2��$ 
Horizontal=�$��Center=�.+��� -   $	��100��$��35��#��[ $��0,5,2,0q�$	��0,2q��� -   $��90��$��35��#��[ $��0,5,2,0q�$	��0,2q��� -   $��75��$��35��#��[ $��0,5,2,0q�$	��0,2q��� -	   $��67��$��35��#��[ $��0,5,2,0q�$	��0,2q��� -
   $��50��$��35��#��[ $��0,5,2,0q�$	��0,2q��� $��2��$	��350��$ Vertical=�$��Center=�$��Center=�$	��0,5q�y��� *SettingTargetWinPos+# [  .+��� $��Center=���`� $, &Move target to ({0},{1}) - {2}W x {3}H��.1��� $ TargetWinPos.X�$ OneWay=��� $ TargetWinPos.Y�$ OneWay=��� $ TargetWinPos.Width�$ OneWay=��� $ TargetWinPos.Height�$ OneWay=�� $��0,10,0,0q�$��Center=�y��� *SettingTargetWinPos+# [   �� $
��Auto ��� $
��Auto ��� $
��Auto � �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto �.+��� $��0��$��0��$	��X =��$��Right=�$��Center=��� $��1��$��0��$	��Y =��$��Right=�$��Center=��� $��0��$��2��$��Width =��$
��10,0q�$��Right=�$��Center=��� $��1��$��2��$��Height =��$
��10,0q�$��Right=�$��Center=�  $��0��$��1��$��60��$	��0,2q����� *TargetWinPos.X+$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�  $��1��$��1��$��60��$	��0,2q����� *TargetWinPos.Y+$ TwoWay=�$ PropertyChanged=�  $��0��$��3��$��60��$	��0,2q����� *TargetWinPos.Width+$ TwoWay=�$ PropertyChanged=�  $��1��$��3��$��60��$	��0,2q����� *TargetWinPos.Height+$ TwoWay=�$ PropertyChanged=��� -   $��2��$��4��$��Left/Top corner��$	��8,2q�#��[ $��0,10,0,0q�$��Center=��� $ 
Horizontal=�$��Center=�y��� *SettingTargetWinPos+# [ .+��� -   $��OK��$��80��$��5,20,5,0q�  OkButton#��[ y��� *EnableRatio+# [  �� -   $��Cancel��$��80��$��5,20,5,0q�  CancelButton#��[ �� $ 
Horizontal=�$��Center=�y��� *SettingTargetWinPos+# [  .+��� -   $��Close��$��80��$��0,20,0,0q�#��[ � -    �Name$ GetPositionSub��$y�Hidden=�.+�$   AutoClicker.view.PositionTuner  -     Name$ PositionTuner��