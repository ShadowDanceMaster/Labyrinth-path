   M S B A M L   `   `   `  ����H  DLabyrinthPath, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:LabyrinthPathLabyrinthPath  5   	       LabyrinthPath.MainWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:LabyrinthPath     5   	     � Title$  
MainWindow��5   	   $	��465��6   $	��700��6)   .��5	      � .+�5
   
   � -   5         �Name$
 grid��5
      $	��450��6   $	��450��6*   $
��Left=�66    � ShowGridLines$
 True��6Q    � RowDefinitions 5      �� 5      �� 5      �� 5      �� 5      �� 5      �� 5      �� 5       � ColumnDefinitions 5      �� 5      �� 5      �� 5      �� 5      �� 5      �� 5      �� 5      5      �� 5   
   -   5         ��Name$ btnFind��5      $��Найти путь��6"   $��Right=�67   $��	0,35,50,0q�6S   $	��119��6f   $	��Top=�6�   $��59��6�   �� 5    
   -   5        $ btnStart��5       $!��Выбрать начало��6#   $��Right=�6<   $��
0,0,50,141q�6X   $	��119��6l   $��Bottom=�6�   $��66��6�   �� 5!   
   -   5        $ 	btnFinish��5!      $��Выбрать выход��6$   $��Right=�6<   $��	0,0,50,32q�6X   $	��119��6k   $��Bottom=�6�   $��63��6�   �� 5"   
   -   5        $ btnClearPath��5"      $��Очистить путь��6'   $��Right=�6?   $��
0,120,50,0q�6[   $	��119��6o   $	��Top=�6�   $��59��6�   ${�False��6�   5$      5%      