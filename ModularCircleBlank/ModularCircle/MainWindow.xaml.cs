using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace ModularCircle
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double _circleRadius;
        private int _circlePointCount;
        private double _pointRadius;

        private readonly Brush _mainCircleBrush = Brushes.OrangeRed;
        private readonly Brush _pointBrush = Brushes.CornflowerBlue;
        private readonly Brush _lineBrush = Brushes.CornflowerBlue;

        private const double _factor = 150;

        public MainWindow()
        {
            Loaded += (sender, args) => Start();
            InitializeComponent();

            _circleRadius = (canvas.Height - 20) / 2;
            _circlePointCount = 200;
            _pointRadius = 2;
        }

        private void Start()
        {
            double marginX = (canvas.Width - 2 * _circleRadius) / 2;
            double marginY = (canvas.Height - 2 * _circleRadius) / 2;

            //рассчитываем координаты точек на окружности
            Point[] circlePoints = CirclePointsCalculator.CalcCirclePoints(_circlePointCount, _circleRadius, marginX + _circleRadius, marginY + _circleRadius);

            //рисуем основную окружность
            DrawCircle(_circleRadius, _mainCircleBrush, marginX, marginY);

            //рисуем точки на окружности ввиде маленьких кружочков
            foreach (var point in circlePoints)
            {
                DrawCircle(_pointRadius, _pointBrush, point.X - _pointRadius, point.Y - _pointRadius, true);
            }

            //todo: из каждой точки на окружности провести линию, используя метод DrawLine
            //Номер точки, в которую нужно вести линию вычисляется по формуле из видео
            for (int i = 0; i < _circlePointCount; i++)
            {
                int endPointIndex = i * (int)_factor % _circlePointCount;
                DrawLine(circlePoints[i], circlePoints[endPointIndex], _lineBrush);
            }

        }

        private void DrawLine(Point from, Point to, Brush brush)
        {
            Line line = new Line()
            {
                X1 = from.X,
                Y1 = from.Y,
                X2 = to.X,
                Y2 = to.Y,
                Stroke = brush,
                StrokeThickness = 1
            };

            canvas.Children.Add(line);
        }

        private void DrawCircle(double radius, Brush brush, double x, double y, bool fill = false)
        {
            var circle = new Ellipse()
            {
                Stroke = brush,
                Width = 2 * radius,
                Height = 2 * radius,
                StrokeThickness = 1
            };

            if (fill)
                circle.Fill = brush;

            Canvas.SetLeft(circle, x);
            Canvas.SetTop(circle, y);

            canvas.Children.Add(circle);
        }


    }
}
