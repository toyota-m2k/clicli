   M S B A M L   `   `   `  ����O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  5      F BAutoClicker, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null)clr-namespace:AutoClickerAutoClicker 5      5clr-namespace:AutoClicker.modelAutoClicker.model 5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089   ToolBtnWidth  ToolBtnHeight  ToolBtnMargin  u� Left
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
ClickTypes A ListViewHeaderlessStyle B NoUnderlineHyperlink�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     5      1system&clr-namespace:System;assembly=mscorlib  5      #localclr-namespace:AutoClicker  5      )modelclr-namespace:AutoClicker.model  5      %�  &	        &	      &	 4     &	 v     &	 �     &	 �     &	 �     &	 �     &	     &		     &	
 +    &	 >    &	 Q    &	 d    &	 w    &	 �    &	 �    &	 �    &	     &	 $    &	 G    &	 j    &	 �    &	 �    &	 �    &	     &	 E    &	 z    &	 �    &	 �    &	     &	 5    &	  h    &	! �    &	" �    &	# �    &	$ +    &	% [    &	& �    &	' �    &	( �    &	)     &	* K    &	+ z    &	, �    &	- �    &	.     &	/ 6    &	0 e    &	1 �    &	2 �    &	3 X    &	4 �    '�� �    7 4 &	5 �    &	6 "    7��7  &	7 �    7��&	8     7��&	9 ~    7 6 &	: "    7��7  &	; �    &	< �    7d�&	= Q    &	> O    &	? �    &	@ ?    &	A �    &	B �    X�5      50Q�6-   X�5	      50Q�6.   u� 5      $  2Q�6&   $ 0Q�6/   $ 0Q�67   $ 0Q�6A   u� 5      $  10Q�6(   $ 0Q�62   $ 0Q�6:   $ 0Q�6D   �� 5      6#      5      6'     5      6+     5      6*     5      6     5      6!     5      6'     5      6$     5      6)     5      6%   �� 5      $ 	#FFEAFFBC��6$   �� 5      $ 	#FFFFE8B0��6'   �� 5      $ 	#FFFFDEFF��6*   �� 5      $ 	#FFCEBCFF��6)   �� 5      $ #FFFFFF��6%   �� 5      $ 	#FF3A0553��6*   �� 5      $ 	#FF152455��6)   �� 5       $ 	#FF6688FB��6/   �� 5!      $ 	#FF6D7803��6,   �� 5#      $ ../resources/start.png?�6$   �� 5$      $ ../resources/stop.png?�6#   �� 5%      $" ../resources/start-large.png?�6%   �� 5&      $! ../resources/stop-large.png?�6$   �� 5'      $ ../resources/add.png?�6"   �� 5(      $ ../resources/remove.png?�6%   �� 5)      $ ../resources/up.png?�6!   �� 5*      $ ../resources/down.png?�6#   �� 5+      $ ../resources/relocate.png?�6'   �� 5,      $ ../resources/menu.png?�6#   �� 5.      $ ../resources/enabled.png?�6&   �� 5/      $ ../resources/disabled.png?�6'   �� 50      $ ../resources/noop.png?�6#   �� 51      $ ../resources/click.png?�6$   �� 52      $ ../resources/wheel.png?�6$   �� 53      $ ../resources/keypress.png?�6'   �� 54      $ ../resources/dblclk.png?�6*   �� 55      $ ../resources/abs.png?�6"   �� 56      $ ../resources/rel.png?�6"   �� 57      $ ../resources/wait.png?�6#   �� 58      $ ../resources/skip.png?�6#   �� 59      $ ../resources/not.png?�6"   �� 5:      $ ../resources/more.png?�6#   �� 5;      $ ../resources/blank.png?�6$   �� 5<      $ ../resources/link.png?�6#   �� 5?      $ ../resources/fold.png?�6%   U� 5B      $ ../resources/fold.png��6-    5C   
   �� 5D      $	 180Q�6   5F      U� 5G      $ ../resources/fold.png��6/    5H   
   �� 5I      $	 -90Q�6   5K      U� 5L      $ ../resources/fold.png��60    5M   
   �� 5N      $ 90Q�6   5P      �� 5R      "	 ��6+   .R�5S   
   �� $
 border��5X      #��z��5T      $ 3��5U      # z��5V      $��0,0,4,0q�5W      #��z��5Y      #��z��5Z       5]      
  5^      CommonStates��. 5_        
Normal��6"     5`      	MouseOver��. 5a      �� .��5b      �� $ border��6-   $1 +(Border.Background).(SolidColorBrush.Color)�5c   #   $ 	#FFFFFF8EX�5d   #   $ 0:0:0.2D�62   5e      5f        5h      Pressed��. 5i      �� .��5j      �� $ border��6-   $1 +(Border.Background).(SolidColorBrush.Color)�5k   %   $ 	#FF80FF96X�5l   %   $ 0D�64   5m      5n       5p        5q      $ Pressed��6+   $ 0:0:0.2D�6:   5s      ._�5u      �� $��Center=�5v      $��Center=�5w      5x      5y      �� 5{      " ��6   .�5|   
   ��  � ��
 �����6'   �� 5}   
    � ��8   6%   �� 5~   
    �@�Cursor$
 Hand��6#   �� 5   
    � |� .  6&   5�      �� 5�      " ��6   .�5�   
   ��  � ��
 �����6'   �� 5�   
    �@�Cursor$
 Hand��6#   �� 5�   
    � |� .  6&   5�      �� 5�      " ��6   8   .�5�   
   ��  � ��8  6'   �� 5�   
    � ��$ 4q�6$   5�      �� 5�      " ��6   8   .�5�   
   ��  � �� �	#FFACFF9E6'   �� 5�   
    � ��$ 4q�6$   5�      �� 5�      " ��6   8   .�5�   
   ��  � �� �	#FFFFECEC6'   �� 5�   
    � ��$ 4q�6$   5�      �� 5�      " ��6   8   .�5�   
   ��  � �� �	#FFC8FDE76'   �� 5�   
    � �� �	#FF00DED46(   �� 5�   
    � ��$ 3q�6,   5�      �� 5�      " ��6   8   .�5�   
   ��  � ��8  6'   �� 5�   
    � ��$ 2q�6$   �� 5�   
    � ��$ 24��6"   �� 5�   
    � ��$ 20��6#   �� 5�   
    � ��$ 0q�6#   5�      �� 5�      " d�6   .�5�   
   ��  � �� 5�      �� 5�      "	 d�6"   .R�5�      �� $
 border��6   $ 2q�5�      $ 3��5�      ���	#00FFFFFF$��0q�5�       . ._�5�      �� 5�       5�      P� 5�       �@d�	IsChecked$
 trueY�67   . �5�      �� $ border��5�   %    � �� �	#20FFFFFF5�   %   5�      P� 5�       �@J�IsMouseOver . . �5�      �� $ border��5�   %    � �� �	#30FFFFFF5�   %   5�      5�      5�      5�      �� 5�      " 	�6   8   .�5�   
   ��  � �� �	#FFF5F0FF6'   5�      �� 5�      " ��6   .�5�   
   ��  � ��$ Arial Black)�6'   �� 5�   
    � �� �	#FF5410996'     5�   
   �� 5�      ! 6   �� *IsRegistered+	" false6;   .a�5�      ��  � �� �	#FFDFB9FF6/   5�      5�      �� 5�      " ��6   .�5�   
   ��  � �� 5�      �� 5�      "	 ��6"   .R�5�      �� 5�      5�      5�      S� 5�      $# 	GetValues��6*   "$  5�      % 5�   
   M� 5�      $& model:KeyType��6   5�      S� 5�      $# 	GetValues��6,   "$  5�      % 5�   
   M� 5�      $& model:ClickType��6   5�      �� 5�      " ��6,   .�5�   
   ��  � y�$ 	Collapsed=�6'   5�      �� 5�      " ��6)     5�   
   P� 5�       �@��IsMouseOver . . �5�      ��  � ��
 ��=H�6/   5�      .�5�   
   ��  � ��
 ���F�6'   �� 5�   
    � �� 66   Z� 5�      5�      