using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
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

namespace FunctionsGraphs
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        double h, l, a, b, c;
        double cellSize, centerX, centerY;
        const double epsilon = 0.8;

        Func<double, double> f;
        bool hasDrawn;

        Ellipse dot;
        TranslateTransform tt;

        private void setDefaultParams() {
            h = 0;
            l = 0;
            a = 0;
            b = 0;
            c = 0;

            centerX = 0;
            centerY = 0;
            cellSize = 0;

            f = ParabolaFunc;
            hasDrawn = false;

            dot = new Ellipse();
            tt = new TranslateTransform();
        }

        private void CreateDot()
        {
            dot.Width = 7;
            dot.Height = 7;

            dot.StrokeThickness = 2;

            dot.Stroke = Brushes.Black;
            dot.Fill = Brushes.White;

            dot.HorizontalAlignment = HorizontalAlignment.Left;
            dot.VerticalAlignment = VerticalAlignment.Top;

            dot.Visibility = Visibility.Hidden;

            tt = new TranslateTransform(50, 50);
            dot.RenderTransform = tt;

            Surface.Children.Add(dot);
        }

        private double ParabolaFunc(double x)
        {
            return (a * x + b) * x + c;
        }

        private double SinusFunc(double x)
        {
            return a * Math.Sin(2 * Math.PI * b * x + c);
        }

        private double SinDivXFunc(double x)
        {
            return Math.Sin(x) / x;
        }

        private void InputData()
        {
            l = double.Parse(IntervalLength.Text);
            h = double.Parse(H.Text);

            centerX = Surface.Width / 2;
            centerY = Surface.Height / 2;

            cellSize = Math.Min( Surface.Width / (2 * l), Surface.Height / (2 * l) );
            l = Surface.Width / cellSize * 0.5;

            Input.Visibility = System.Windows.Visibility.Hidden;
            Parameters.Visibility = System.Windows.Visibility.Visible;
        }

        private void InputParams()
        {
            a = double.Parse(Param1.Text);
            b = double.Parse(Param2.Text);
            c = double.Parse(Param3.Text);
        }

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            InputData();
            DrawAxis();
        }

        private void Parabola_Checked(object sender, RoutedEventArgs e)
        {
            f = ParabolaFunc;
        }
                
        private void Sinus_Checked(object sender, RoutedEventArgs e)
        {
            f = SinusFunc;
        }

        private void SinusDivX_Checked(object sender, RoutedEventArgs e)
        {
            f = SinDivXFunc;
        }

        private void NewBtn_Click(object sender, RoutedEventArgs e)
        {
            Parameters.Visibility = System.Windows.Visibility.Hidden;
            Input.Visibility = System.Windows.Visibility.Visible;

            DotX.Content = "0.00";
            DotY.Content = "0.00";
        }

        private void Surface_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!hasDrawn) return;

            Point p = e.GetPosition(Surface);

            double mouseX = p.X;
            double mouseY = p.Y;

            double x = (mouseX - centerX) / cellSize;
            double y = (centerY - mouseY) / cellSize;
            double y0 = f(x);

            if (Math.Abs(y0 - y) < epsilon)
            {
                DotX.Content = x;
                DotY.Content = y0;

                tt.X = mouseX - 3;
                tt.Y = centerY - y0 * cellSize - 3;

                dot.RenderTransform = tt;
                dot.Visibility = Visibility.Visible;
            }
            else
            {
                dot.Visibility = Visibility.Hidden;
            }
        }

        private void DrawBtn_Click(object sender, RoutedEventArgs e)
        {
            InputParams();
            Surface.Children.Clear();
            DrawAxis();
            DrawFunc();
            CreateDot();

            DotX.Content = "0.00";
            DotY.Content = "0.00";
        }

        private void DrawLine(double x1, double y1, double x2, double y2, double l, Brush color)
        {
            Line line = new Line
            {
                X1 = x1,
                Y1 = y1,
                X2 = x2,
                Y2 = y2,
                Stroke = color,
                StrokeThickness = l
            };

            Surface.Children.Add(line);
        }

        private void DrawAxis()
        {
            int width = (int)Surface.Width;
            int height = (int)Surface.Height;

            // рисую вертикальные линии координатной сетки
            for (double x = 0; x <= centerX ; x += cellSize)
            {
                DrawLine(centerX + x, 0, centerX + x, height, 1, Brushes.Gray);
                DrawLine(centerX - x, 0, centerX - x, height, 1, Brushes.Gray);
            }

            // рисую горизонтальные линии координатной сетки
            for (double y = 0; y <= centerY; y += cellSize)
            {
                DrawLine(0, centerY + y, width, centerY + y, 1, Brushes.Gray);
                DrawLine(0, centerY - y, width, centerY - y, 1, Brushes.Gray);
            }

            //рисую ось Х
            DrawLine(0, centerY, width, centerY, 2, Brushes.Black);

            // рисую ось у
            DrawLine( centerX, 0, centerX, height, 2, Brushes.Black);

            // рисую границы
            DrawLine(0, 0, width, 0, 1, Brushes.Black);
            DrawLine(0, 0, 0, height, 1, Brushes.Black);
            DrawLine(width, 0, width, height, 1, Brushes.Black);
            DrawLine(0, height, width, height, 1, Brushes.Black);
        }

        private void DrawFunc()
        {
            double yMax = centerY / cellSize;

            double x1 = -l;
            double y1 = f(x1);
            double x2 = -l;
            double y2;

            while( Math.Abs(y1) > yMax )
            {
                x1 += h;
                y1 = f(x1);
            }

            y1 = centerY - y1 * cellSize;

            do
            {
                x2 += h;
                y2 = f(x2);

                if (Math.Abs(y2) > yMax) continue;

                y2 = centerY - y2 * cellSize;

                DrawLine(centerX + x1 * cellSize, y1,
                         centerX + x2 * cellSize, y2,
                         2, System.Windows.Media.Brushes.Blue);

                x1 = x2;
                y1 = y2;

            } while (x2 < l);

            hasDrawn = true;
        }

        public MainWindow()
        {
            InitializeComponent();
            setDefaultParams();
        }
    }
}
