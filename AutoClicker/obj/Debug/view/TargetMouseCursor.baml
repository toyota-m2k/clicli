   M S B A M L   `   `   `  ����F  BAutoClicker, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null3clr-namespace:AutoClicker.viewAutoClicker.view  5      (    "AutoClicker.view.TargetMouseCursor   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:AutoClicker.view     5      $��60��5      $��60��5	        � 	Resources  5       �� Source�� 5   
   $ CommonResource.xaml?�6   %    .+�5      � $��48��6   $��48��6   
������  J� RenderTransformOrigin$ 0.5, 0.5&�6;   .+�5   
   �� 	 �� X1$ 24��6   	 �� Y1$ 0��6   	 �� X2$ 24��6   	 �� Y2$ 21��6&   
�������$��2��6=   �� 5   
   $ 24��6   $ 0��6   $ 24��6   $ 21��6&   
���   �6.   �� 5   
   $ 24��6   $ 27��6   $ 24��6   $ 48��6'   
�������$��2��6>   �� 5   
   $ 24��6   $ 27��6   $ 24��6   $ 48��6'   
���   �6/   �� 5   
   $ 24��6   $ 0��6   $ 24��6   $ 21��6&   
�������$��2��6=   �� 5   
   $ 24��6   $ 0��6   $ 24��6   $ 21��6&   
���   �6.   �� 5   
   $ 24��6   $ 27��6   $ 24��6   $ 48��6'   
�������$��2��6>   �� 5   
   $ 24��6   $ 27��6   $ 24��6   $ 48��6'   
���   �6/   A� 5M   
   
�������$��1��6!   $��3q�65   A� 5N   
   
���   �$��1��6!   $��4q�65   A� 5P   
   
�������$��1��6!   $��12q�65   A� 5Q   
   
���   �$��1��6!   $��13q�65   A� 5S   
   
���   �$��1��6!   $��20q�65   A� 5T   
   
�������$��1��6!   $��19q�65   ��5U      �� 5V       �� Color$ Transparent��6"   -   5        TargetFocus��5V   6   5X      (� 5Z   
   $��20��6   $��	28,38,0,2q�6   y�60   �� *DecisionEnabled+   BoolToVisibility �� 	Converter# [  .��5[      �� $��1q�6   
���   �._�5\      � .+�5]      �� $
��True��6    ���	#FF5FFF03
���   �	 �� Padding$		 5,0q�6V   y�6d   �� *
Decision+# [  �� 5^      $��False��6    ���	#FFFF7CB2
���   �$		 5,0q�6W   y�6f   �� *
Decision+  NegBoolToVisibility# [ 5_      5`      5a      z�5d   
   V� 5e      .~�5f      �� �� 5g      5h      
 � Triggers
 5j   
   :� 5k       :� RoutedEvent$ Loaded��6   .[�5l      �� .��5m      �� .��5o      W�  �� TargetProperty$V P(Rectangle.RenderTransform).(TransformGroup.Children)[1].(ScaleTransform.ScaleX)�6*    W� From$	 0.8X�6�   	 W� To$
 1.25X�6�    g� Duration$ 0:0:1D�6�    g� AutoReverse .  g� RepeatBehavior$ Forever��6�   W� 5p      $V P(Rectangle.RenderTransform).(TransformGroup.Children)[1].(ScaleTransform.ScaleY)�6*   $	 0.8X�6�   $
 1.25X�6�   $ 0:0:1D�6�    . $ Forever��6�   �� 5q       �� 
TargetName$ TargetFocus��6)   $ Color�6M   	 �� To$	 RedX�6o   $ 0:0:0.5D�6x   $ Forever��6�   5r      5s      5t      5w      5x      