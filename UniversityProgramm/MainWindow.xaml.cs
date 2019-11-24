using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using System.Collections.Generic;

namespace UniversityProgramm
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public double MaximumHeight { get => Height; }

        private int _delta = 120;
        private float _persantage = 0.1f;
        private float _maxPersantage = 1.5f;
        private float _currentPersantage = 1f;
        private double _normalHeight = 800;
        private double _normalWidth = 1024;
        private Image _map;

        public MainWindow()
        {
            InitializeComponent();

            var picturePath = "pack://application:,,,/Images/1.1.jpg";

            AddPicture(picturePath);
        }

        //private void Expander_Button_click(object sender, RoutedEventArgs e)
        //{
        //    if (LeftExpandPanel.Visibility == Visibility.Collapsed)
        //    {
        //        LeftExpandPanel.Visibility = Visibility.Visible;
        //        ((Button)sender).Content = "-->";
        //    }
        //    else
        //    {
        //        LeftExpandPanel.Visibility = Visibility.Collapsed;
        //        ((Button)sender).Content = "<--";
        //    }
        //}

        private void Exit(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        public void DrawLine(Point firstPoint, Point secondPoint)
        {
            Line line = new Line
            {
                X1 = firstPoint.X,
                X2 = secondPoint.X,
                Y1 = firstPoint.Y,
                Y2 = secondPoint.Y,
                Stroke = Brushes.Blue,
                StrokeThickness = 2
            };

            Map.Children.Add(line);
        }

        public bool ClearAllLines()
        {
            bool isCLeared = false;

            List<Line> lines = new List<Line>();

            foreach (var item in Map.Children)
            {
                if(item is Line)
                {
                    lines.Add(item as Line);
                }
            }

            foreach (var item in lines)
            {
                Map.Children.Remove(item);
            }

            return isCLeared;
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image Files (*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp"
            };

            if ((bool)dialog.ShowDialog())
            {
                AddPicture(dialog.FileName);
            }
        }

        private void AddPicture(string path)
        {
            var bitmap = new BitmapImage(new Uri(path));
            var image = new Image() { Source = bitmap };

            image.Name = "MapPicture";

            _normalHeight = bitmap.Height;
            _normalWidth = bitmap.Width;

            Canvas.SetLeft(image, 0);
            Canvas.SetTop(image, 0);

            canvas.Children.Add(image);
        }

        private Image draggedImage;
        private Point mousePosition;

        private void CanvasMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var image = e.Source as Image;

            if (image != null && canvas.CaptureMouse())
            {
                Mouse.OverrideCursor = Cursors.ScrollAll;
                mousePosition = e.GetPosition(canvas);
                draggedImage = image;
                Panel.SetZIndex(draggedImage, 1);
            }
        }

        private void CanvasMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (draggedImage != null)
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                canvas.ReleaseMouseCapture();
                Panel.SetZIndex(draggedImage, 0);
                draggedImage = null;
            }
        }

        private void CanvasMouseMove(object sender, MouseEventArgs e)
        {
            if (draggedImage != null)
            {
                var position = e.GetPosition(canvas);
                var offset = position - mousePosition;
                mousePosition = position;

                Point relativePoint = draggedImage.TransformToAncestor(Map).Transform(new Point(0, 0));

                double toX = relativePoint.X + offset.X;
                double toY = relativePoint.Y + offset.Y;

                if ((offset.X > 0 && toX <= 0) || (offset.X < 0 && -toX + Map.ActualWidth <= draggedImage.ActualWidth))
                {
                    Canvas.SetLeft(draggedImage, Canvas.GetLeft(draggedImage) + offset.X);
                }

                if ((offset.Y > 0 && toY <= 0) || (offset.Y < 0 && -toY + Map.ActualHeight <= draggedImage.ActualHeight))
                {
                    Canvas.SetTop(draggedImage, Canvas.GetTop(draggedImage) + offset.Y);
                }
            }
        }

        private void Find(object sender, RoutedEventArgs e)
        {

        }

        private void ToMap(object sender, RoutedEventArgs e)
        {
            ClearAllLines();
        }

        private void CanvasMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (_map == null)
            {
                foreach (var item in canvas.Children)
                {
                    Image image = item as Image;
                    if (image != null && image.Name == "MapPicture")
                    {
                        (item as Image).Height += ((e.Delta / _delta) * _persantage * _normalHeight);
                        (item as Image).Width += ((e.Delta / _delta) * _persantage * _normalWidth);
                        break;
                    }
                }
            }
        }
    }
}
