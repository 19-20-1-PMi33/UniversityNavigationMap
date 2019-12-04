using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using System.Collections.Generic;
using UniversityProgramm.Helpers;
using System.Windows.Data;

namespace UniversityProgramm
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public double MaximumHeight { get => Height; }
        public double MapHeight { get; set; } = 800;
        public ApplicationModel CurrentDataContext { get => DataContext as ApplicationModel; }

        private int _delta = 120;
        private float _persantage = 0.1f;
        private float _maxPersantage = 2f;
        private float _currentPersantage = 1f;
        private double _normalHeight = 960;
        private double _normalWidth = 1280;
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
            _draggedImage = new Image() { Source = bitmap };

            _draggedImage.Name = "MapPicture";

            _normalHeight = bitmap.Height;
            _normalWidth = bitmap.Width;

            Canvas.SetLeft(_draggedImage, 0);
            Canvas.SetTop(_draggedImage, 0);

            canvas.Children.Add(_draggedImage);
            SetBindings(_draggedImage);
        }

        private void SetBindings(Image image)
        {
            Binding heightBinding = new Binding
            {
                Source = (DataContext as ApplicationModel),
                Path = new PropertyPath("MapHeight"),
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };

            Binding widthBinding = new Binding
            {
                Source = (DataContext as ApplicationModel),
                Path = new PropertyPath("MapWidth"),
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };

            BindingOperations.SetBinding(image, Image.HeightProperty, heightBinding);
            BindingOperations.SetBinding(image, Image.WidthProperty, widthBinding);
        }

        private Image _draggedImage;
        private Point _mousePosition;
        private bool _isDragged = false;

        private void CanvasMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var image = _draggedImage;//e.Source as Image;

            if (image != null && canvas.CaptureMouse())
            {
                Mouse.OverrideCursor = Cursors.ScrollAll;
                _mousePosition = e.GetPosition(canvas);
                _draggedImage = image;
                Panel.SetZIndex(_draggedImage, 1);

                _isDragged = true;
            }
        }

        private void CanvasMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_draggedImage != null)
            {
                _isDragged = false;

                Mouse.OverrideCursor = Cursors.Arrow;
                canvas.ReleaseMouseCapture();
                Panel.SetZIndex(_draggedImage, 0);
            }
        }

        private void CanvasMouseMove(object sender, MouseEventArgs e)
        {
            if (_draggedImage != null && _isDragged)
            {
                var position = e.GetPosition(canvas);
                var offset = position - _mousePosition;
                _mousePosition = position;

                Point relativePoint = _draggedImage.TransformToAncestor(Map).Transform(new Point(0, 0));

                double toX = relativePoint.X + offset.X;
                double toY = relativePoint.Y + offset.Y;

                if ((offset.X > 0 && toX <= 0) || (offset.X < 0 && -toX + Map.ActualWidth <= _draggedImage.ActualWidth))
                {
                    Canvas.SetLeft(_draggedImage, Canvas.GetLeft(_draggedImage) + offset.X);
                }

                if ((offset.Y > 0 && toY <= 0) || (offset.Y < 0 && -toY + Map.ActualHeight <= _draggedImage.ActualHeight))
                {
                    Canvas.SetTop(_draggedImage, Canvas.GetTop(_draggedImage) + offset.Y);
                }
            }
        }

        private void Find(object sender, RoutedEventArgs e)
        {
            From.Visibility = Visibility.Visible;
            To.Visibility = Visibility.Visible;
            Search.Visibility = Visibility.Visible;
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
                    float newPersantage = ((e.Delta / _delta) * _persantage + _currentPersantage);
                    newPersantage = (newPersantage <= _maxPersantage ? newPersantage : _maxPersantage);

                    double newPersantageHeight = newPersantage * _normalHeight;
                    double newPersantageWidth = newPersantage * _normalWidth;

                    bool isNormalHeight = newPersantageHeight >= Map.ActualHeight;
                    bool isNormalWidth = newPersantageHeight >= Map.ActualWidth;
                    bool isNormalImage = image != null && image.Name == "MapPicture";

                    if (isNormalImage && (isNormalHeight || isNormalWidth))
                    {
                        _currentPersantage = newPersantage;

                        if (_normalHeight < _normalWidth)
                        {
                            CurrentDataContext.MapHeight = newPersantageHeight;
                        }
                        else
                        {
                            CurrentDataContext.MapWidth = newPersantageWidth;
                        }

                        break;
                    }
                }
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            From.Visibility = Visibility.Hidden;
            To.Visibility = Visibility.Hidden;
            Search.Visibility = Visibility.Hidden;
        }
    }
}
