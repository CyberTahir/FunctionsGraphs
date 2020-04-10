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
        double h, x1, x2, a, b, c;
        int f;

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
            h = double.Parse(IntervalLength.Text);
            x1 = double.Parse(StartX.Text);
            x2 = double.Parse(EndX.Text);

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
            Parameters.Visibility = System.Windows.Visibility.Hidden;

            if (f == 1) DrawFunc(ParabolaFunc);
            else if (f == 2) DrawFunc(SinusFunc);
            else if (f == 3) DrawFunc(SinDivXFunc);
            
        }

        private void DrawAxis()
        {
            Line XAxis = new Line();

            XAxis.X1 = 0;
            XAxis.Y1 = Surface.Height / 2;
            XAxis.X2 = Surface.Width;
            XAxis.Y2 = XAxis.Y1;

            XAxis.Stroke = System.Windows.Media.Brushes.Black;
            XAxis.StrokeThickness = 4;

            Surface.Children.Add(XAxis);
        }

        private void DrawFunc(Func<double, double> f)
        {
            DrawAxis();
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
