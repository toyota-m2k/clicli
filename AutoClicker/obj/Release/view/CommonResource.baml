   M S B A M L   `   `   `  ���� O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  F BAutoClicker, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null)clr-namespace:AutoClickerAutoClicker 5clr-namespace:AutoClicker.modelAutoClicker.model \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089   ToolBtnWidth  ToolBtnHeight  ToolBtnMargin  u� Left
 u� Top u� Right u� Bottom  ToolBtnSpMargin  BoolToVisibility,   &AutoClicker.NegBoolVisibilityConverter  NegBoolToVisibility0  *AutoClicker.VisibilityHiddenFalseConverter " VisibilityHiddenFalseConverter/  )AutoClicker.VisibilityHiddenTrueConverter ! VisibilityHiddenTrueConverter#  AutoClicker.TypeNameConverter  TypeNameConverter&   AutoClicker.EnumBooleanConverter 	 EnumBooleanConverter,  &AutoClicker.NegEnumVisibilityConverter 
 NegEnumVisibilityConverter)  #AutoClicker.EnumVisibilityConverter  EnumVisibilityConverter.  (AutoClicker.TypePointVisibilityConverter   TypePointVisibilityConverter*  $AutoClicker.IntToVisibilityConverter  IntToVisibilityConverter 	 DlgBg �� Color  ButtonBg  StatusBarBg  
DlgTitleBg 
 TBtnBg  TBProjectBg  
TBTargetBg  TBTargetBgActive  TBOperatingBg  	StartIcon �� 	UriSource  StopIcon  
LStartIcon  	LStopIcon  AddIcon  
RemoveIcon 
 UpIcon  DownIcon  RelocateIcon   MenuIcon ! EnabledIcon " DisabledIcon # NoopIcon $ 	ClickIcon % 	WheelIcon & KeyPressIcon ' DoubleClickIcon ( AbsIcon ) RelIcon * WaitIcon + SkipIcon , NotIcon - MoreIcon . 	BlankIcon / LinkIcon 0 
FoldUpIcon 1 FoldDownIcon U� Source U� 	Transform �� Angle 2 FoldToLeftIcon 3 FoldToRightIcon 4 BtnTemplate	 �� 
TargetType
 ��Name �� CornerRadius �� Padding'	  !System.Windows.VisualStateManager 	  VisualStateGroups%
  System.Windows.VisualStateGroup 
  States   System.Windows.VisualState   
Storyboard �� 
TargetName �� TargetProperty	 �� To g� Duration 
  Transitions%  System.Windows.VisualTransition   From   GeneratedDuration �� 
TargetType �� Property �� Value 5 PlainButton �� BasedOn 6 	NorButton 7 OkButton 8 CancelButton 9 StrongButton : 
MiniButton ; 
MenuButton J� SnapsToDevicePixels �� Triggers P� Property P� Value �� 
TargetName < RadioToggle = RadioToggleText  �� Triggers! �� Binding" �� Value > ImageButtonStyle ? KeyTypes# S� 
MethodName$ S� 
ObjectType   System.Enum% S� MethodParameters& M� TypeName @ 
ClickTypes A ListViewHeaderlessStyle B NoUnderlineHyperlink�� D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     1system&clr-namespace:System;assembly=mscorlib  #localclr-namespace:AutoClicker  )modelclr-namespace:AutoClicker.model  %q  &	        &	      &	      &	 =     &	 c     &	 h     &	 m     &	 r     &	 w     &		 |     &	
 �     &	 �     &	 �     &	 �     &	 �     &	 �     &	 �     &	 �     &	 �     &	 �     &	     &	 &    &	 ;    &	 P    &	 r    &	 �    &	 �    &	 �    &	     &	 %    &	 D    &	 e    &	  �    &	! �    &	" �    &	# �    &	$     &	% 7    &	& Y    &	' ~    &	( �    &	) �    &	* �    &	+     &	, #    &	- C    &	. d    &	/ �    &	0 �    &	1 �    &	2 �    &	3 0    &	4 c    '��      7 4 &	5 �    &	6 �    7��7  &	7     7��&	8 `    7��&	9 �    7 6 &	: 	    7��7  &	; �	    &	< �
    7d�&	= �
    &	> a    &	? �    &	@ �    &	A     &	B +    X�50Q�X�50Q�u� $  2Q�$ 0Q�$ 0Q�$ 0Q�u� $  10Q�$ 0Q�$ 0Q�$ 0Q���                    �� $ 	#FFEAFFBC���� $ 	#FFFFE8B0���� $ 	#FFFFDEFF���� $ 	#FFCEBCFF���� $ #FFFFFF���� $ 	#FF3A0553���� $ 	#FF152455���� $ 	#FF6688FB���� $ 	#FF6D7803���� $ ../resources/start.png?��� $ ../resources/stop.png?��� $" ../resources/start-large.png?��� $! ../resources/stop-large.png?��� $ ../resources/add.png?��� $ ../resources/remove.png?��� $ ../resources/up.png?��� $ ../resources/down.png?��� $ ../resources/relocate.png?��� $ ../resources/menu.png?��� $ ../resources/enabled.png?��� $ ../resources/disabled.png?��� $ ../resources/noop.png?��� $ ../resources/click.png?��� $ ../resources/wheel.png?��� $ ../resources/keypress.png?��� $ ../resources/dblclk.png?��� $ ../resources/abs.png?��� $ ../resources/rel.png?��� $ ../resources/wait.png?��� $ ../resources/skip.png?��� $ ../resources/not.png?��� $ ../resources/more.png?��� $ ../resources/blank.png?��� $ ../resources/link.png?��� $ ../resources/fold.png?�U� $ ../resources/fold.png�� �� $	 180Q�U� $ ../resources/fold.png�� �� $	 -90Q�U� $ ../resources/fold.png�� �� $ 90Q��� "	 ��.R��� $
 border��#��z��$ 3��# z��$��0,0,4,0q�#��z��#��z�� 
  CommonStates��.   
Normal��  	MouseOver��. �� .���� $ border��$1 +(Border.Background).(SolidColorBrush.Color)�$ 	#FFFFFF8EX�$ 0:0:0.2D�  Pressed��. �� .���� $ border��$1 +(Border.Background).(SolidColorBrush.Color)�$ 	#FF80FF96X�$ 0D�   $ Pressed��$ 0:0:0.2D�._��� $��Center=�$��Center=��� " ��.���  � ��
 �������  � ��8   ��  �@�Cursor$
 Hand����  � |� .  �� " ��.���  � ��
 �������  �@�Cursor$
 Hand����  � |� .  �� " ��8   .���  � ��8  ��  � ��$ 4q��� " ��8   .���  � �� �	#FFACFF9E��  � ��$ 4q��� " ��8   .���  � �� �	#FFFFECEC��  � ��$ 4q��� " ��8   .���  � �� �	#FFC8FDE7��  � �� �	#FF00DED4��  � ��$ 3q��� " ��8   .���  � ��8  ��  � ��$ 2q���  � ��$ 24����  � ��$ 20����  � ��$ 0q��� " d�.���  � �� �� "	 d�.R��� $
 border��$ 2q�$ 3�����	#00FFFFFF$��0q� . ._���  P�  �@d�	IsChecked$
 trueY�. ��� $ border�� � �� �	#20FFFFFFP�  �@J�IsMouseOver . . ��� $ border�� � �� �	#30FFFFFF�� " 	�8   .���  � �� �	#FFF5F0FF�� " ��.���  � ��$ Arial Black)���  � �� �	#FF541099  �� ! �� *IsRegistered+	" false.a���  � �� �	#FFDFB9FF�� " ��.���  � �� �� "	 ��.R��� S� $# 	GetValues��"$  % M� $& model:KeyType��S� $# 	GetValues��"$  % M� $& model:ClickType���� " ��.���  � y�$ 	Collapsed=��� " ��  P�  �@��IsMouseOver . . ���  � ��
 ��=H�.���  � ��
 ���F���  � �� Z� 