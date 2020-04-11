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
        int f;

        double XTransform, YTransform;

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

            XTransform = Surface.Width / (2 * l);
            YTransform = Surface.Height / (2 * l);

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
            Surface.Children.Clear();
            DrawAxis();
        }

        private void Parabola_Checked(object sender, RoutedEventArgs e)
        {
            f = 1;
        }
                
        private void Sinus_Checked(object sender, RoutedEventArgs e)
        {
            f = 2;
        }

        private void SinusDivX_Checked(object sender, RoutedEventArgs e)
        {
            f = 3;
        }

        private void ParamsBtn_Click(object sender, RoutedEventArgs e)
        {
            InputParams();

            if (f == 1) DrawFunc(ParabolaFunc);
            else if (f == 2) DrawFunc(SinusFunc);
            else if (f == 3) DrawFunc(SinDivXFunc);
        }

        private void NewBtn_Click(object sender, RoutedEventArgs e)
        {
            Parameters.Visibility = System.Windows.Visibility.Hidden;
            Input.Visibility = System.Windows.Visibility.Visible;
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

            int centerX = width / 2;
            int centerY = height / 2;

            // рисую вертикальные линии координатной сетки
            for (double x = 0; x <= centerX ; x += XTransform)
            {
                DrawLine(centerX + x, 0, centerX + x, height, 1, System.Windows.Media.Brushes.Gray);
                DrawLine(centerX - x, 0, centerX - x, height, 1, System.Windows.Media.Brushes.Gray);
            }

            // рисую горизонтальные линии координатной сетки
            for (double y = 0; y <= centerY; y += YTransform)
            {
                DrawLine(0, centerY + y, width, centerY + y, 1, System.Windows.Media.Brushes.Gray);
                DrawLine(0, centerY - y, width, centerY - y, 1, System.Windows.Media.Brushes.Gray);
            }

            //рисую ось Х
            DrawLine(0, centerY, width, centerY, 2, System.Windows.Media.Brushes.Black);

            // рисую ось у
            DrawLine( centerX, 0, centerX, height, 2, System.Windows.Media.Brushes.Black);
        }

        private void DrawFunc(Func<double, double> f)
        {
            double x1 = -l;
            double y1 = (l - f(x1)) * YTransform;
            double x2 = -l;
            double y2;

            do
            {
                x2 += h;
                y2 = (l - f(x2)) * YTransform;

                DrawLine((x1 + l) * XTransform, y1,
                         (x2 + l) * XTransform, y2,
                         2, System.Windows.Media.Brushes.Blue);

                x1 = x2;
                y1 = y2;

            } while (x2 < l);
        }

        public MainWindow()
        {
            InitializeComponent();

            a = 0;
            b = 0;
            c = 0;
            f = 0;
        }
    }
}
