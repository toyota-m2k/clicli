   M S B A M L   `   `   `  ���� F  BAutoClicker, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null3clr-namespace:AutoClicker.viewAutoClicker.view  (    "AutoClicker.view.TargetMouseCursor   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     0localclr-namespace:AutoClicker.view     $��60��$��60��  � 	Resources   �� Source�� $ CommonResource.xaml?�%    .+�� $��48��$��48��
������  J� RenderTransformOrigin$ 0.5, 0.5&�.+��� 	 �� X1$ 24��	 �� Y1$ 0��	 �� X2$ 24��	 �� Y2$ 21��
�������$��2���� $ 24��$ 0��$ 24��$ 21��
���   ��� $ 24��$ 27��$ 24��$ 48��
�������$��2���� $ 24��$ 27��$ 24��$ 48��
���   ��� $ 24��$ 0��$ 24��$ 21��
�������$��2���� $ 24��$ 0��$ 24��$ 21��
���   ��� $ 24��$ 27��$ 24��$ 48��
�������$��2���� $ 24��$ 27��$ 24��$ 48��
���   �A� 
�������$��1��$��3q�A� 
���   �$��1��$��4q�A� 
�������$��1��$��12q�A� 
���   �$��1��$��13q�A� 
���   �$��1��$��20q�A� 
�������$��1��$��19q�����  �� Color$ Transparent��-   TargetFocus��(� $��20��$��	28,38,0,2q�y��� *DecisionEnabled+   BoolToVisibility �� 	Converter# [  .���� $��1q�
���   �._�� .+��� $
��True�����	#FF5FFF03
���   �	 �� Padding$		 5,0q�y��� *
Decision+# [  �� $��False�����	#FFFF7CB2
���   �$		 5,0q�y��� *
Decision+  NegBoolToVisibility# [ z�V� .~��� �� 
 � Triggers
 :�  :� RoutedEvent$ Loaded��.[��� .���� .��W�  �� TargetProperty$V P(Rectangle.RenderTransform).(TransformGroup.Children)[1].(ScaleTransform.ScaleX)� W� From$	 0.8X�	 W� To$
 1.25X� g� Duration$ 0:0:1D� g� AutoReverse .  g� RepeatBehavior$ Forever��W� $V P(Rectangle.RenderTransform).(TransformGroup.Children)[1].(ScaleTransform.ScaleY)�$	 0.8X�$
 1.25X�$ 0:0:1D� . $ Forever����  �� 
TargetName$ TargetFocus��$ Color�	 �� To$	 RedX�$ 0:0:0.5D�$ Forever��