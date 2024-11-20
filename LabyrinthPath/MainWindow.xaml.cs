using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LabyrinthPath
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int squareSize = 7;
        //переменная изображения точки начала
        Rectangle rectStart = new Rectangle();
        //переменная изображения точки выхода
        Rectangle rectEnd = new Rectangle();
        //здесь будут начальные эл-ты
        ObservableCollection<Rectangle> startingRects = new ObservableCollection<Rectangle>();
        //здесь будет фон
        Rectangle rectBG = new Rectangle();
        //здесь будет точка нажатия
        Point point = new Point();
        //цвет стен
        Color wallColor = Colors.BlueViolet;
        //цвет пути
        Color pathColor = Colors.Chocolate;
        (int, int) pointStartRowCol = (-1, -1);
        (int, int) pointEndRowCol = (-1, -1);
        UserPointTypeEnum pointType = UserPointTypeEnum.Default;
        //путь
        Stack<(int, int)> path = new Stack<(int, int)>();
        List<(int, int)> history = new List<(int, int)>();
        (int, int) lastOdd = (-1, -1);
        //карта, по ней определяем где стены(1) и где пусто(0)
        static int[,] testMap = new int[,]{
            {0,0,0,0,0,0,0},
            {0,0,1,1,0,0,1},
            {1,1,1,1,1,0,1},
            {0,0,1,0,0,0,0},
            {0,0,1,0,1,0,0},
            {0,0,0,0,1,0,0},
            {0,0,0,0,1,0,0}
        };
        public MainWindow()
        {
            InitializeComponent();
            AddChildren();
            DrawWalls();
            
        }
        private void AddChildren()
        {
            //добавляем невидимые точки
            rectStart.Visibility = Visibility.Hidden;
            grid.Children.Add(rectStart);
            rectEnd.Visibility = Visibility.Hidden;
            grid.Children.Add(rectEnd);
            //отключаем кнопку, пока она не нужна
            btnFind.IsEnabled = false;
            //создаём фон
            grid.Children.Add(rectBG);
            rectBG.Visibility = Visibility.Visible;
            rectBG.Width = grid.Width;
            rectBG.Height = grid.Height;
            rectBG.Fill = new SolidColorBrush(Colors.White);
            rectBG.SetValue(Grid.RowProperty, 0);
            rectBG.SetValue(Grid.ColumnProperty, 0);
            rectBG.SetValue(Grid.RowSpanProperty, grid.RowDefinitions.Count);
            rectBG.SetValue(Grid.ColumnSpanProperty, grid.ColumnDefinitions.Count);
            rectBG.SetValue(Grid.ZIndexProperty, -1);
        }
        /// <summary>
        /// рисуем стены / drawing walls
        /// </summary>
        private void DrawWalls()
        {
            for (int i = 0; i < squareSize; i++)
            {
                for (int j = 0; j < squareSize; j++)
                {
                    if (testMap[i, j] != 0)
                    {
                        Rectangle rect = new Rectangle();
                        grid.Children.Add(rect);
                        CreateSquare(rect, 60, i, j, wallColor);
                    }
                }
            }
        }
        /// <summary>
        /// Создание точки
        /// </summary>
        /// <param name="width"></param>
        /// <param name="desiredRow"></param>
        /// <param name="desiredColumn"></param>
        /// <param name="color"></param>
        void CreateSquare(Rectangle rect,double width, int desiredRow, int desiredColumn, Color color)
        {
            rect.Visibility = Visibility.Visible;
            rect.Width = width;
            rect.Height = width;
            rect.Fill = new SolidColorBrush(color);
            rect.SetValue(Grid.RowProperty, desiredRow);
            rect.SetValue(Grid.ColumnProperty, desiredColumn);
        }


        private void Grid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (pointType == UserPointTypeEnum.Default) return;
            //сохраняем позицию курсора
            point = Mouse.GetPosition(grid);
            

            int row = 0;
            int col = 0;
            double accumulatedHeight = 0.0;
            double accumulatedWidth = 0.0;

            // считаем ряды в сетке
            foreach (var rowDefinition in grid.RowDefinitions)
            {
                accumulatedHeight += rowDefinition.ActualHeight;
                if (accumulatedHeight >= point.Y)
                    break;
                row++;
            }

            // считаем колонке в сетке
            foreach (var columnDefinition in grid.ColumnDefinitions)
            {
                accumulatedWidth += columnDefinition.ActualWidth;
                if (accumulatedWidth >= point.X)
                    break;
                col++;
            }
            
            point.Y = (grid.RowDefinitions[row].ActualHeight * (row + 1)) - grid.RowDefinitions[row].ActualHeight / 2;
            point.X = (grid.ColumnDefinitions[col].ActualWidth * (col + 1)) - grid.ColumnDefinitions[col].ActualWidth / 2;
            if (pointType == UserPointTypeEnum.End && (row != grid.RowDefinitions.Count - 1 && col != grid.ColumnDefinitions.Count - 1 && row != 0 && col != 0))
                return;//в случае, если выход не с краю, не рисуем точку
            //создаём изображение точки
            Rectangle rect = new Rectangle();
            Color color = new Color();
            if (pointType == UserPointTypeEnum.Start)
            {
                pointStartRowCol = (row, col);
                color = Colors.OrangeRed;
                rect = rectStart;
            }
            if (pointType == UserPointTypeEnum.End)
            {
                rect = rectEnd;
                pointEndRowCol = (row, col);
                color = Colors.ForestGreen;
            }
            if (testMap[row, col] != 1)
                CreateSquare(rect, (double)20, row, col, color);
            if (rectStart.Visibility == Visibility.Hidden || rectEnd.Visibility == Visibility.Hidden)
                btnFind.IsEnabled = false;
            else btnFind.IsEnabled = true;

        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            
            pointType = UserPointTypeEnum.Start;
        }

        private void btnFinish_Click(object sender, RoutedEventArgs e)
        {
            
            pointType = UserPointTypeEnum.End;
        }
        private void FindWay()
        {
            while (path.Peek() != pointEndRowCol)
            {

                //проверяем, можно ли пройти вниз, и ходили ли мы туда
                if (CheckMove((path.Peek().Item1 + 1, path.Peek().Item2)))
                {
                    history.Add((path.Peek().Item1 + 1, path.Peek().Item2));
                    path.Push((path.Peek().Item1 + 1, path.Peek().Item2));
                    continue;
                }
                //вправо
                if (CheckMove((path.Peek().Item1, path.Peek().Item2 + 1)))
                {
                    history.Add((path.Peek().Item1, path.Peek().Item2 + 1));
                    path.Push((path.Peek().Item1, path.Peek().Item2 + 1));
                    continue;
                }
                //вверх
                if (CheckMove((path.Peek().Item1 - 1, path.Peek().Item2)))
                {
                    history.Add((path.Peek().Item1 - 1, path.Peek().Item2));
                    path.Push((path.Peek().Item1 - 1, path.Peek().Item2));
                    continue;
                }
                //влево
                if (CheckMove((path.Peek().Item1, path.Peek().Item2 - 1)))
                {
                    history.Add((path.Peek().Item1, path.Peek().Item2 - 1));
                    path.Push((path.Peek().Item1, path.Peek().Item2 - 1));
                    continue;
                }
                //возврат
                if (path.Peek() != pointStartRowCol)
                {
                    lastOdd = path.Pop();
                }
            }
            //отображаем путь
            foreach (var q in path)
            {
                if (q != pointStartRowCol && q != pointEndRowCol)
                {
                    Rectangle rect = new Rectangle();
                    grid.Children.Add(rect);
                    CreateSquare(rect, 50, q.Item1, q.Item2, pathColor);
                }
            }
            btnClearPath.IsEnabled = true;
        }
        /// <summary>
        /// Можно ли продвинуться
        /// </summary>
        /// <param name="point">Куда продвинуться</param>
        /// <returns></returns>
        private bool CheckMove((int, int) point)
        {
            return !path.Where(p=>p.Item1==point.Item1&&p.Item2==point.Item2).Any() &&
                !history.Where(p => p.Item1 == point.Item1 && p.Item2 == point.Item2).Any() &&
                (point != lastOdd) &&
                (point.Item1 >= 0) &&
                (point.Item2 >= 0) &&
                (point.Item1 < squareSize) &&
                (point.Item2 < squareSize) &&
                (testMap[point.Item1, point.Item2] == 0);

        }
        private void btnFind_Click(object sender, RoutedEventArgs e)
        {
            if (rectStart.Visibility == Visibility.Hidden || rectEnd.Visibility == Visibility.Hidden) return;
            if (pointStartRowCol.Item1 < 0 || pointStartRowCol.Item2 < 0 || pointEndRowCol.Item1 < 0 || pointEndRowCol.Item2 < 0)
                return;//проверка значений>=0
            btnClearPath.IsEnabled = false;
            path.Push(pointStartRowCol);
            history.Add(pointStartRowCol);
            FindWay();
            pointType = UserPointTypeEnum.Default;
        }

        private void btnClearPath_Click(object sender, RoutedEventArgs e)
        {
            path = new Stack<(int, int)>();
            history = new List<(int, int)>();
            lastOdd = (-1, -1);
            grid.Children.Clear();
            AddChildren();
            DrawWalls();
            pointType = UserPointTypeEnum.Default;
            rectStart.Visibility = Visibility.Hidden;
            rectEnd.Visibility = Visibility.Hidden;
        }
    }
}
