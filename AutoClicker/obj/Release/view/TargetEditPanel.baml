   M S B A M L   `   `   `  ���� F  BAutoClicker, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null)clr-namespace:AutoClickerAutoClicker  3clr-namespace:AutoClicker.viewAutoClicker.view  5clr-namespace:AutoClicker.modelAutoClicker.model  &     AutoClicker.view.TargetEditPanel   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     +localclr-namespace:AutoClicker     /viewclr-namespace:AutoClicker.view     1modelclr-namespace:AutoClicker.model       � 	Resources   �� Source�� $ DialogResource.xaml?�%    .��� $��Center=�$��Center=�.+�� -    �Name$ TargetEditMain�� � RowDefinitions �� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto � � ColumnDefinitions �� $	��100 ��� $	��200 ��� $
��auto �   �� 
TargetType   RadioToggle �� BasedOn �� Property �� Value��%K   '	�       7   �� " 	�8   .���  � ��$ 4q���  � ��$ 50��.+��� $��0��$��0��$��3��	 �� Padding$	 4q�$��Target Item Setting��$
��0,10q�  
DlgTitleBg#��[ �� $��1��$��0��$��Action Type��  left#��[ �� $��1��$��1��
 �� Orientation$
 Vertical=� 	 right#��[ .+��� ����   
ClickTypes �� Source# [  �� SelectedValue �� *Type+�� $
 
Horizontal=�$��0,5,0,0q�y��� *Type+  EnumVisibilityConverter �� 	Converter# [  �� ConverterParameter$ KEYPRESS��.+��� $��Key: ��$��Center=��� $��0q�$
��Left=�����   KeyTypes# [  �� *
PressKey+�� $
 
Horizontal=�$��0,5,0,0q�y��� *Type+# [ $ WHEEL��.+��� $��	Amounts: ��$��Center=�    AutoClicker.NumericTextBox  $��60��$��0q�$
��Left=����� *WheelAmount+ �� UpdateSourceTrigger$ PropertyChanged=��� $��2��$��0��$��Point��#��[ y��� *Type+   TypePointVisibilityConverter# [ �� -   $��2��$��1��  StrongButton#��[ $
��auto��y��� *Type+# [ .���� ��`�  �� StringFormat$ {0},{1}��.1���  �� Path$ ScreenPoint.Point.X��� $ ScreenPoint.Point.Y��� $��2��$��2��$
��Move��$��Center=� d� 	IsChecked �� *	MoveMouse+y��� *Type+# [ �� $��3��$��0��$
��Wait��#��[   -     Name$ NtWait��$��3��$��1��#��[ ���� *Wait+$ PropertyChanged=�� $��4��$��1��.+��� $
 
Horizontal=�.+��� -   $
��3000�� 	 	NorButton#��[	 �� -   $
��1000��#��[	 �� -   $	��500��#��[	 �� -   $	��100��#��[	 �� -   $��50��#��[	 �� $��3��$��2��$
��msec��#��[ �� $��5��$��0��$��Repeat��#��[   -	   $ NtRepeat��$��5��$��1��#��[ ���� *Repeat+$ PropertyChanged=��� $��5��$��2��$��times��#��[ �� $��6��$��0��$��Comment��$��Right=�#��[ �� -
    ��Name$ 	TxComment��$��6��$��1��#��[ ���� *	Comment+$ PropertyChanged=��� $��7��$��0��$��3��$
 
Horizontal=�$��Center=�$��0,10,0,5q�y��� *Type+# [ .+��� -   #��[	 $��	Show Pos.��$��80��$��Bottom=��� -   #��[	 $��Try now��$��80��$	��Top=��� -   #��[	 $��Adjust��$��80��$	��Top=��� $��8��$��0��$��	Condition��$��Right=�#��[ �� $��8��$��1��$
 
Horizontal=�$��0,0,5,0q�$��Center=�.+�	� $
��None�� 	� 	GroupName$ ConditionType��$	��0,4q� �� *ConditionList.Type+ 
 EnumBooleanConverter# [
 $
 NONE�� �� Mode$ TwoWay=�	� $
��Skip��$ ConditionType��$	��0,4q� �� *ConditionList.Type+# [
 $
 SKIP��$ TwoWay=�	� $
��Wait��$ ConditionType��$	��0,4q� �� *ConditionList.Type+# [
 $
 WAIT��$ TwoWay=�� $��9��$��1��y��� *ConditionList.Type+  NegEnumVisibilityConverter# [ $
 NONE��.+��� -    ��Name$ ConditionListView��$��0,0,5,0q�$	��100������  �� View �� -   ConditionGridView��  ListViewHeaderlessStyle! �� ColumnHeaderContainerStyle# [ P���  �� Width$	 100�� �� CellTemplate �� .R��� $
 
Horizontal=�.+�� $��20��$��20��y��� *	IsValid+  BoolToVisibility# [ .+��� 
���   �$��1q�����  �� Color �� *ColorRange[8]+� $��	10,10,0,0q�$��10��$��10��y��� *
Negation+# [ .+��� 
���   �$��1q����� $ #FFffff���� 	 �� X1$ 0��	 �� X2$ 10��	 �� Y1$ 0��	  �� Y2$  10��
���   �$��1���� $ 10��$ 0��$ 0��$  10��
���   �$��1���� $��4,0,0,0q�y��� *	IsValid+# [ �� -     NoUnderlineHyperlink#��[ .�.��� .��`� $ {0},{1}��.1��� $! ScreenPoint.AbsolutePoint.X�$ OneWay=��� $! ScreenPoint.AbsolutePoint.Y�$ OneWay=�.��� -     
MiniButton#��[ y��� *	IsValid+  NegBoolToVisibility# [ .����   AddIcon#��[ ���� " ��.�-   �� $��9��$��2��$
 Vertical=�$��Bottom=�.+��� -   $
��Edit��#��[	 y��� *ConditionList.Type+# [ $
 NONE���� -   $	��Del��#��[	 $��0,5,0,0q�y��� *ConditionList.Type+# [ $
 NONE���� $��10��$��1��$
 
Horizontal=�$��0,0,5,0q�$��Center=�y��� *ConditionList.IsMulti+# [ .+�	� $	��AND��$ CombinationType��$	��0,4q� �� *ConditionList.Combination+# [
 $	 AND��$ TwoWay=�	� $��OR��$ CombinationType��$	��0,4q� �� *ConditionList.Combination+# [
 $ OR��$ TwoWay=��� $��11��$��0��$��3��$
 
Horizontal=�$��Center=�$��0,5,0,5q�.+��� -     OkButton#��[ $��OK��$��80��! �� 	IsDefault! . $��0,0,10,0q�$��Bottom=��� -     CancelButton#��[ $��Cancel��$��80��" �� IsCancel" . $	��Top=�� -   $ TargetEditSub��$y�Hidden=�.+�$   AutoClicker.view.PositionTuner  -   #  Name$# PositionTuner��