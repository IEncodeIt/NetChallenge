using System;
using System.Collections.Generic;
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

namespace Graphics
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            int[,] coordinates = new int[22, 4]; 

            int rowInd = -1;
            for (int i = 0; i <= 400; i = i + 40)
            {
                rowInd++;

                int j = 400 - i;
                DrawLine(0, i, j, 0);

                coordinates[rowInd, 0] = coordinates[rowInd, 3] = 0;
                coordinates[rowInd, 1] = i;
                coordinates[rowInd, 2] = j;
            }

            rowInd = coordinates.GetLength(0)/2-1;
            for (int i = 0; i <= 400; i = i + 40)
            {
                rowInd++;

                int j = 400 - i;
                DrawLine(i, 400, 400, j);

                coordinates[rowInd, 0] = i;
                coordinates[rowInd, 1] = coordinates[rowInd, 2] = 400;
                coordinates[rowInd, 3] = j;
            }
        }

        private void DrawLine(int a, int b, int c, int d)
        {
            var line = new Line();
            line.Stroke = Brushes.CadetBlue;

                    line.X1 = a;
                    line.Y1 = b;
                    line.X2 = c;
                    line.Y2 = d;

            canvas.Children.Add(line);
        }

        /*private double CrossPoint(int x1, int y1, int x2, int y2, int x3, int y3, int x4, int y4, int index)
        {
            double crossX = ((x1 * y2 - y1 * x2) * (x3 - x4) - (x1 - x2) * (x3 * y4 - y3 * x4)) /
                            ((x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4));

            double crossY = ((x1 * y2 - y1 * x2) * (y3 - y4) - (y1 - y2) * (x3 * y4 - y3 * x4)) /
                            ((x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4));

            return crossX;
        }*/
    }
}