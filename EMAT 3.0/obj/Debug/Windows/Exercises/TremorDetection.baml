   M S B A M L   `   `   `  ����  EMAT 3.03    -EMAT3.Windows.Exercises.TremorDetectionWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   X TXceed.Wpf.Toolkit, Version=2.4.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  5   	     � Title$  Tremor Detection��5   	   $	��662��6"   $��Center=�6/   $��Center=�6L    � WindowStyle$
 None=�6g   
���   � � AllowsTransparency . ��6�   Z� .��5      � -   5         �Name$ grid_background��5      $	��609��6$   $	��747��61   .+�5   
   � -   5        $ grid_resultsHolder��5      $��494,206,16,212q�6+   ��5	      �� 5
       �� Color$ White��6"    �� Opacity$
 0.75Q�60   .+�5      �� -   5         ��Name$ groupBox_results��5      $��Results��61   $
��Left=�6B   $	��Top=�6]   $	��237��6u   $	��191��6�   .��5      � -   5        $ grid_results��5      $
��Left=�6-   $	��169��6H   $	��Top=�6U   $	��225��6m   .+�5      �� -   5         ��Name$ lbl_tremorAmp��5      $��
Amplitude:��63   $
��Left=�6H   $��	10,72,0,0q�6c   $	��Top=�6v   �� 5      -   5        $ 
lbl_tremor��5      $��
Frequency:��60   $
��Left=�6E   $��	10,41,0,0q�6`   $	��Top=�6s    J� RenderTransformOrigin$ 2.658,0.231&�6�   �� 5      -   5        $ lbl_countDown��5      $��
Countdown:��63   $
��Left=�6H   $	��Top=�6c   $ 4.237,3.769&�6{   $��	10,10,0,0q�6�   5      5      5      � 5   
   -   5        $! grid_advancedSettingsHolder��5      $��10,206,500,212q�64   ��5      �� 5      $ White��6"   $
 0.75Q�60   .+�5      �� -	   5        $ groupBox_advancedSettings��5      $��Advanced Settings��6:   $
��Left=�6U   $	��Top=�6p   $	��237��6�   $	��191��6�   .��5      � -
   5        $ grid_advancedSettings��5      $
��Left=�66   $	��169��6Q   $	��Top=�6^   $	��225��6v   .+�5      �� -   5        $ lbl_thresholdFreq��5      $��Frequency Threshold:��67   $
��Left=�6V   $��	10,10,0,0q�6q   $	��Top=�6�   $ 1.263,1.462&�6�   �� 5      -   5        $ lbl_average��5      $��	Average: ��61   $
��Left=�6E   $��	10,41,0,0q�6`   $	��Top=�6s   �� 5      -   5        $ lbl_averageThreshold��5      $��Average x Threshold: ��6:   $
��Left=�6Z   $��	10,72,0,0q�6u   $	��Top=�6�   �� 5      -   5        $ lbl_thresholdAmp��5      $��Amplitude Threshold:��66   $
��Left=�6U   $��
10,134,0,0q�6p   $	��Top=�6�   �� 5      -   5        $ 
lbl_maxBin��5      $��Largest Value: ��60   $
��Left=�6J   $��
10,103,0,0q�6e   $	��Top=�6y   %  Xceed.Wpf.Toolkit.DecimalUpDown  5      -   5        	  Name$	 numUpDown_ThresholdFreq��5   )   $
��Left=�6J   $��
138,16,0,0q�6e   $	��Top=�6y   $��60��6�   �  �Xceed.Wpf.Toolkit.Primitives.UpDownBase`1[[System.Nullable`1[[System.Decimal, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]
   Maximum$
 20X�6�      Minimum$ 2X�6�      DefaultValue$
 2.00X�6�   %   DisplayDefaultValueOnEmptyText . �  �Xceed.Wpf.Toolkit.NumericUpDown`1[[System.Nullable`1[[System.Decimal, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]   	Increment$
 0.25X�6�     5       -   5        $	 numUpDown_ThresholdAmp��5    )   $
��Left=�6I   $��138,140,0,0q�6d   $	��Top=�6y   $��60��6�   $
 15X�6�   $ 2X�6�   $	 0.5X�6�   $ 2X�6�    . 6�   5!      5"      5#      � 5$   
   -   5        $ grid_graphHolder��5$      $��252,402,16,16q�6)   ��5%      �� 5&      $ White��6"   $
 0.75Q�60   .+�5(      �� -   5        $ groupBox_graph��5(      $��Graph��6/   $
��Left=�6>   $	��Top=�6Y   $	��479��6q   $	��191��6}   5)      �� 5*   
   $��GroupBox��6   $
��Left=�6%   $��	10,10,0,0q�6@   $	��Top=�6S   $	��237��6k   $	��191��6w   $y�Hidden=�6�   �� 5+   
   $��GroupBox��6   $
��Left=�6%   $��
252,10,0,0q�6@   $	��Top=�6T   $	��237��6l   $	��191��6x   $y�Hidden=�6�   � 5,   
   -   5        $ grid_settingsHolder��5,      $��252,206,258,212q�6,   ��5-      �� 5.      $ White��6"   $
 0.75Q�60   .+�50      �� -   5        $ groupBox_settings��50      $��Settings��62   $
��Left=�6D   $	��Top=�6_   $	��237��6w   $	��191��6�   .��51      � -   5        $ grid_settings��51      $
��Left=�6.   $	��169��6I   $	��Top=�6V   $	��225��6n   .+�52      �� -   5         ��Name$ combobox_nodeSelect��52      $
��Left=�6<   $��35��6W   $	��Top=�6c   $	��123��6{   $��	92,10,0,0q�6�   ��53      �� 54       �� EndPoint$	 0,1&�62    �� 
StartPoint$	 0,0&�6A   .��55   "   �  � Color$ 	#FFF3F3F3��6/    � Offset$ 0Q�6A   � 56   "   $ 	#FFEBEBEB��6/   $	 0.5Q�6A   � 57   "   $ 	#FFDDDDDD��6/   $	 0.5Q�6A   � 58   "   $ 	#FFCDCDCD��6/   $ 1Q�6A   59      5;      �� 5<      -   5         ��Name$ checkBox_graph��5<      $��Graph��67   $
��Left=�6G   $��	10,50,0,0q�6b   $	��Top=�6u   $	��108��6�   6�   �� 5=      -   5        $ lbl_node��5=      $��Select Node:��6.   $
��Left=�6E   $��	10,10,0,0q�6`   $	��Top=�6s   �� 5>      -   5        $ checkBox_advancedSettings��5>      $��Advanced Settings��6B   $
��Left=�6^   $��	10,71,0,0q�6y   $	��Top=�6�   6�   �� 5?      -   5         ��Name$ 	btn_start��5?      $��Start Collection��60   $
��Left=�6K   $��
151,50,0,0q�6f   $	��Top=�6z   $��64��6�   $	��109��6�   
������6�   �� 5@      -   5        $ 	btn_break��5@      $��Insert Break��60   $
��Left=�6G   $��
10,50,0,10q�6b   $��Bottom=�6v   $	��100��6�   $��55��6�   
������6�   5A      5B      5D      �� 5E   
   -   5        $ btn_exit��5E      $
��Exit��6#   $
��Left=�62   $��207,161,0,0q�6M   $	��Top=�6b   $��40��6z   $��40��6�   
���  ��6�   5G      5H      