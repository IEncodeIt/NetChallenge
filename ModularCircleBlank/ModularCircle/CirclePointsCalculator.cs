using System;
using System.Windows;

namespace ModularCircle
{
    static class CirclePointsCalculator
    {
        public static Point[] CalcCirclePoints(int circlePointCount, double radius, double x, double y)
        {
            Point[] circlePoints = new Point[circlePointCount];

            double deltaAngle = 2 * Math.PI / circlePointCount;
            for (int i = 0; i < circlePointCount; i++)
            {
                double angle = i * deltaAngle;
                Point p = new Point(x + radius * Math.Cos(angle), y + radius * Math.Sin(angle));
                circlePoints[i] = p;
            }

            return circlePoints;
        }
    }
}
