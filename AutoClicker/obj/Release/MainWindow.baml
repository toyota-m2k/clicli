   M S B A M L   `   `   `  ���� O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  F BAutoClicker, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null)clr-namespace:AutoClickerAutoClicker 3clr-namespace:AutoClicker.viewAutoClicker.view    AutoClicker.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     9system&clr-namespace:System;assembly=mscorlib     +localclr-namespace:AutoClicker     /viewclr-namespace:AutoClicker.view     -     � Title$*  $edit AssemblyInfo.cs to modify title��$	��450��$	��868�� � 	Resources  �� Source�� $ view/CommonResource.xaml?�%     J� CommandBindings )  #System.Windows.Input.CommandBinding  -     AutoClicker.MainWindow   CopyProject   Command# Z   -      
CutProject# Z   -      PasteProject# Z   -      EditProject# Z   -   	   TranslateProject# Z	   -   
   InsertProject# Z
   -      DeleteProject# Z   -	      ExportProject# Z   -
      ImportProject# Z   -      
CopyTarget# Z   -      	CutTarget# Z   -      PasteTarget# Z   -      
EditTarget# Z   -      InsertTarget# Z   -      DeleteTarget# Z   -      EnableTarget# Z   -      DisableTarget# Z   -      FileNew# Z   -      FileOpen# Z   -      FileSave# Z   -      
FileSaveAs# Z .��� -    �Name$ MainContainer��  �� 
TargetType �� Property �� Value 
  AbsRel �� Triggers �� Binding  �� Value  	PlayImage  StopIcon  	StartIcon  PlayBar  TBTargetBgActive  
TBTargetBg  gridViewUtilizationColumn! �� Width" �� CellTemplate��%�  '��       &	  -     &	 �     7  7  &	 q    7  7  &	 �    �� " ��.�-   ��  � ��$ Stretch=��� " �� ��  �� * CurrentProject.Mode.IsRelative+  true.a���  � ��$ Relative����  �� * CurrentProject.Mode.IsRelative+	  false.a���  � ��$ Absolute���� " �� ��  �� *	IsPlaying+  true.a���  � ��8   ��  �� *	IsPlaying+	  false.a���  � ��8  �� " � ��  �� *	IsPlaying+  true.a���  � ��8   ��  �� *	IsPlaying+	  false.a���  � ��8  �� $��Utilization��$! 50��" �� .R��� $��Right=����� *UtilizationCount+.+�� -   $ SettingPanel��$y�Visible=�# � ColumnDefinitions# �� $
��Auto ��� $
��Auto ��� $��* �.+�� -   $ ProjectPane��$��0��.+�� $ � RowDefinitions$ �� $
��Auto ��� $
��Auto ��� $��* ��� $
��Auto �.+�� $��0��  TBProjectBg#��[ # �� $
��Auto ��� $��* ��� $
��Auto �.+��� $��0��$��5q�$
��Left=�$��Stretch=�% �� Orientation$% 
Horizontal=�.+��� -   & ��Name$& 
BtnAddProj��.����  	 AddIcon#��[	 �� $% Vertical=�.+��� -   $��24��.����  

 UpIcon#��[
 �� -   $��24��.����   DownIcon#��[ �� $��2��$	��0,5q�$��Right=�$��Stretch=�$% 
Horizontal=�.+�)  #AutoClicker.view.DropDownMenuButton  -   '  Name$' BtnMenu��  
MenuButton#��[ .����   MenuIcon#��[ (   DropDownContextMenu( �� -   ) ��Name$) FileMenu��
�������.@�m� $��New(_N)��* m� Command#* Z m� $��Open(_O)��#* Z m� $��Save(_S)��#* Z m� $��Save As(_A)��#* Z � $��1��   RadioToggle+ �� BasedOn  RadioToggleText��%�   '	�       7  '�� ]     7  �� " 	�8+   .�-    ��  � ��$	 0,2q���  �@d�	IsChecked �� *
IsSelected+�� " ��8+   # �� �� �� �� �� .+�	� $��0��, � DataContext, �� *Registrations[0]+.���� $��1��	� $��1��, �� *Registrations[1]+.���� $��2��	� $��2��, �� *Registrations[2]+.���� $��3��	� $��3��, �� *Registrations[3]+.���� $��4��	� $��4��, �� *Registrations[4]+.���� $��5���� -!   - ��Name$- ProjectListView��$��2������ . � ContextMenu. �� -"   $
��Proj��
�������.@�m� $��Copy(_C)��/ m� InputGestureText$/ Ctrl+C��#* Z m� $��Cut(_T)��$/ Ctrl+X��#* Z m� $��	Paste(_P)��$/ Ctrl+P��#* Z �� m� $��Base Point(_B)��$/ Enter��#* Z	 m� $��Edit(_E)��$/ 
Ctrl+Enter��#* Z m� $��
Insert(_I)��#* Z
 m� $��
Delete(_D)��#* Z �� m� $��
Export(_X)��#* Z m� $��
Import(_M)��#* Z �� m� $��Registration��.@�m� -#   $��Regist to 1��0 m� CommandParameter$0 1��m� -$   $��Regist to 2��$0 2��m� -%   $��Regist to 3��$0 3��m� -&   $��Regist to 4��$0 4��m� -'   $��Regist to 5��$0 5��1 J� InputBindings1 �� 2 �� 	Modifiers$
2 Ctrlf�
3 �� Key$3 C��4 �� Command#4 Z �� $
2 Ctrlf�$3 X��#4 Z �� $
2 Ctrlf�$3 V��#4 Z �� $3 Delete��#4 Z �� $3 Return��#4 Z	 �� $
2 Ctrlf�$3 Return��#4 Z 5 �� View5 ��   ListViewHeaderlessStyle!6 �� ColumnHeaderContainerStyle#6 [ P��� $	! 200��" �� .R��� $% 
Horizontal=�.+��� ����  ��  �� *Mode.IsRelative+  True.a���  � ��  RelIcon# [ ��  �� *Mode.IsRelative+	  False.a���  � ��  AbsIcon# [ �� $% 
Horizontal=�$
��Left=�.+��� $��Center=����� *Name+�� ���	#FF0A641F$��Center=�$��15,0,0,0q����� *Ratio+7 �� StringFormat$7 ({0})��y��� *RatioEnabled+  BoolToVisibility8 �� 	Converter#8 [ �� $! 28��" �� .R��� -(     ImageButtonStyle#��[ |�.  y��� *HasURL+#8 [ .����   LinkIcon#��[ � $��3��#��[ # �� $
��Auto ��� $��* �.+��� $% 
Horizontal=�$��0��$	��2 2q�.+��� #��Z	 y��� *
IsRelative+#8 [ .����   RelocateIcon#��[ � $��2q�$��1��.+��� y��� *IsSingleProjectSelection+#8 [ .+�� $ �� �� �� # �� �� $
��Auto ��� .+��� $��0��$��1��$��:��$	��5,0q�$��Center=�
��������� $��1��$��1��$��:��$	��5,0q�$��Center=�
��������� $��0��$��0��$��Project:��$��Right=�
��������� $��1��$��0��$��Mode:��$��Right=�
��������� $��0��$��2��$
��Left=�
����������� *CurrentProject.Name+�� $��1��$��2��#��[  $
��Left=�
��������� $��(Marged Project)��$��Center=�
�������y��� *IsMargedTarget+#8 [ �� $��(not selected)��$��Center=�
�������y��� *NoProjectSelected+#8 [ �� $��1��$��3��$��Center=�$��1q�� -)   $ TargetsPanel��$��2��$y�Visible=�$��0,0,0,0q�$ �� $
��Auto ��� $��* ��� $
��Auto �.+�� $��0��#��[ # �� $
��Auto ��� $��* ��� $
��Auto �.+��� $��0��$��5q�$��Stretch=�$��Stretch=�$% 
Horizontal=�.+��� -*   .���� #��[ �� -+   $& BtnAdd��.���� #��[	 �� $% Vertical=�.+��� -,   $& BtnUp��$��24��.���� #��[
 �� --   $& BtnDown��$��24��.���� #��[ �� $��Center=�$% Vertical=�$
��Left=�.+��� $% 
Horizontal=�$
��Left=�$��Center=�.+��� -.   9 ��Name$9 	checkMode��$��
Check Mode��
�������: d� 	IsChecked: �� *	CheckMode+�� -/   $9 utilizationiMode��$��Utilization��
�������$��10,0,0,0q�: �� *UtlizationMode+�� -0   $��0,6,0,0q�$	��100��$
��Left=�.���� 
Test All�� $��2��$��5q�$��Right=�$��Stretch=�$% 
Horizontal=�.+�� y��� *IsSingleProjectSelection+ " VisibilityHiddenFalseConverter#8 [ .+��� $% Vertical=�$��Center=�y��� *	IsPlaying+ ! VisibilityHiddenTrueConverter#8 [ .+��� 
�������: �� *CurrentProject.IsInfinite+; �� Mode$; TwoWay=�.��
Infinite   AutoClicker.NumericTextBox  $��16,4,0,0q����� *CurrentProject.Repeat+$; TwoWay=�y��� *CurrentProject.IsInfinite+#8 [ �� $% Vertical=�$
��15,0q�$��Center=�.+��� -1   $��40��$��60��.��� $��Center=�$��Center=�$ �� �� .+��� $��0��$��HotKey��#��[ < �� TextAlignment$< Center=��� $��1��#��[ $
��Bold"�$< Center=����� *
HotKeyName+�� -2   = �� 	IsDefault= . .����   FoldDownIcon#��[ � -3   $ 
TargetGrid��$��1��$ �� $��* ��� $
��Auto ��� $
��Auto �.+��� -4   $- TargetListView��$��0��$��Stretch=�$��Stretch=����� . �� -5   $��Target��
�������.@�m� $��Copy(_C)��$/ Ctrl+C��#* Z m� $��Cut(_T)��$/ Ctrl+X��#* Z m� $��	Paste(_P)��$/ Ctrl+P��#* Z �� m� $
��Edit��#* Z m� $��Insert��#* Z m� $��Delete��#* Z �� m� $��Enable��#* Z m� $��Disable��#* Z 1 �� $
2 Ctrlf�$3 C��#4 Z �� $
2 Ctrlf�$3 X��#4 Z �� $
2 Ctrlf�$3 V��#4 Z �� $3 Delete��#4 Z �� $3 Return��#4 Z �� $
2 Ctrlf�$3 E��#4 Z �� $
2 Ctrlf�$3 D��#4 Z 5 �� -6   TargetGridView��.P��� $! 40��.���� {�.  " �� .R��� ����  ��  �� *	Enabled+  True.a���  � ��  EnabledIcon# [ ��  �� *	Enabled+	  False.a���  � ��  DisabledIcon# [ �� $
��Type��$! 40��" �� .R�� .+��� ����  ��  �� *Type+	  CLICK.a���  � ��  	ClickIcon# [ ��  �� *Type+
  DBLCLK.a���  � ��  DoubleClickIcon# [ ��  �� *Type+  NOOP.a���  � ��  NoopIcon# [ ��  �� *Type+	  WHEEL.a���  � ��  	WheelIcon# [ ��  �� *Type+  KEYPRESS.a���  � ��   KeyPressIcon# [  �� $��Center=�$��Center=����	#FF05A839���� *WheelAmount+y��� *Type+ ! EnumVisibilityConverter#8 [! > �� ConverterParameter$> WHEEL��(� $��4q�.���� $��Center=�$��Center=����	#FF05A839���� *KeyShortLabel+y��� *Type+#8 [! $> KEYPRESS���� $��Point (abs)��$	! 100��" �� .R�� $��24��$��Stretch=�.+��� $��Center=�$��Center=��� -7    " NoUnderlineHyperlink#��[" ��.  .�.��� .��`� $7 {0},{1}��.1��� ? �� Path$!? ScreenPoint.AbsolutePoint.X�$; TwoWay=��� $!? ScreenPoint.AbsolutePoint.Y�$; TwoWay=�.��� $
��Wait��$! 50��" �� .R��� $��Right=����� *Wait+�� $��Repeat��$! 50��" �� .R��� $��Right=����� *Repeat+�� $��	Condition��$	! 160��" �� .R�� $��Stretch=�����  ��  �� @ �� ElementName$@ 	checkMode��$? 	IsChecked�  True.a���  � �� �	#FFFDD98D.+�� $��Center=�# �� $��24 ��� $��80 ��� $��20 ��� $��24 �.+��� $��0������  ��  �� *ConditionList.Type+  NONE.a���  � ��# [ ��  �� *ConditionList.Type+  WAIT.a���  � �� # WaitIcon# [# ��  �� *ConditionList.Type+  SKIP.a���  � �� $ SkipIcon# [$ �� $��1��$��Center=�$��Center=�y��� *ConditionList.Type+ % NegEnumVisibilityConverter#8 [% $
> NONE���� -8   #��[" ��.  .�.��� .��`� $7 {0},{1}��.1��� $4? .ConditionList.Head.ScreenPoint.AbsolutePoint.X��� $4? .ConditionList.Head.ScreenPoint.AbsolutePoint.Y�.�� $��2��$��20��$��20��$��Center=�y��� *ConditionList.Type+#8 [% $
> NONE��.+��� 
���   �$��1q����� A �� ColorA �� *" ConditionList.Head.ColorRange[8]+� $��	10,10,0,0q�$��10��$��10��y��� *ConditionList.Head.Negation+#8 [ .+��� 
���   �$��1q����� $A #FFffff���� 	B �� X1$B 0��	C �� X2$C 10��	D �� Y1$D 0��	E �� Y2$E 10��
���   �$��1���� $B 10��$C 0��$D 0��$E 10��
���   �$��1���� -9   $��24��$��24��$��0q� & PlainButton#��[& $��3��$��0q�$��0q�
������ y��� *ConditionList.IsMulti+#8 [ .��� .+�(� $	��Top=�$��4q�.���� ���#3f51b5���� *ConditionList.CombinationName+��  ' MoreIcon#��[' $��0,14,0,0q�$��10��$��10���� $��Comment��$	! 200��F �� DisplayMemberBindingF �� *	Comment+�� $��1��$��3��$��Center=�$��Stretch=�$��1q�y��� $@ 	checkMode��$? 	IsChecked�#8 [ �� -:   G ��Name$G DebugOutput��$��2��$��Stretch=�H �� IsReadOnlyCaretVisibleH . y��� $@ 	checkMode��$? 	IsChecked�#8 [ '  !AutoClicker.view.GetPositionPanel  -;   I  Name$I GettingPointPanel��$��10q�$y�Hidden=��� -<   J ��Name$J Dialog��$y�Hidden=�$
��Auto=�.��� $��Center=�$��Center=� 	( DlgBg#��[( .+�'  !AutoClicker.view.ProjectEditPanel  -=   K  Name$K EditProjectPanel��$��10q�$y�	Collapsed=�&   AutoClicker.view.TargetEditPanel  ->   L  Name$L EditTargetPanel��$��10q�$y�	Collapsed=�"  AutoClicker.view.HotKeyPanel  -?   M  Name$M HotKeyPanel��$��10q�$y�	Collapsed=�$	  AutoClicker.view.SamplingColor	  -@   N 	 Name$N SamplingColor��$��10q�$y�	Collapsed=�%
  AutoClicker.view.OperatingPanel
  -A   O 
 Name$O OperatingPanel��$y�Hidden=�