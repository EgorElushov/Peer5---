using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace peer
{
    /// <summary>
    /// Класс кривой Коха.
    /// </summary>
    internal class KochSnowflake : Fractal
    {
        private const float segmentFraction = 3;
        private const double rotateAngle = Math.PI / 3;

        /// <summary>
        /// Максимальная глубина рекурсии.
        /// </summary>
        public override int MaxRecursionDepth => 5;


        /// <summary>
        /// Конструктор.
        /// </summary>
        public KochSnowflake() { }


        /// <summary>
        /// Конструктор.
        /// </summary>
        public KochSnowflake(float segmentLength, int recursionDepth, Color startColor, Color endColor)
            : base(segmentLength, recursionDepth, startColor, endColor) { }


        /// <summary>
        /// Рисование фрактала.
        /// </summary>
        /// <param name="points">Точки для рисования.</param>
        /// <param name="recursionLevel">Уровень рекурсии.</param>
        /// <param name="fractalPlace">Канвас для рисования.</param>
        public override void DrawFractal(System.Drawing.PointF[] points, int recursionLevel, Canvas fractalPlace)
        {
            float dx = (points[1].X - points[0].X) / segmentFraction;
            float dy = (points[1].Y - points[0].Y) / segmentFraction;

            System.Drawing.PointF endOfFirstSegment = new System.Drawing.PointF(points[0].X + dx, points[0].Y + dy);
            System.Drawing.PointF endOfThirdSegment = new System.Drawing.PointF(points[0].X + dx * 2, points[0].Y + dy * 2);

            float apexDx = endOfThirdSegment.X - endOfFirstSegment.X;
            float apexDy = endOfThirdSegment.Y - endOfFirstSegment.Y;
            float apexX = (float)(apexDx * Math.Cos(rotateAngle) + apexDy * Math.Sin(rotateAngle));
            float apexY = (float)(apexDy * Math.Cos(rotateAngle) - apexDx * Math.Sin(rotateAngle));

            System.Drawing.PointF endOfSecondSegment = new System.Drawing.PointF(endOfFirstSegment.X + apexX, endOfFirstSegment.Y + apexY);

            if (recursionLevel == RecursionDepth)
            {
                base.DrawFractal(new System.Drawing.PointF[] { points[0], points[1] }, recursionLevel, fractalPlace);
            }

            if (recursionLevel > 1)
            {
                base.DrawFractal(new System.Drawing.PointF[] { endOfFirstSegment, endOfSecondSegment }, recursionLevel - 1, fractalPlace);
                base.DrawFractal(new System.Drawing.PointF[] { endOfSecondSegment, endOfThirdSegment }, recursionLevel - 1, fractalPlace);
                fractalPlace.Children.Add(new Line { Stroke = Brushes.White, X1 = endOfFirstSegment.X, Y1 = endOfFirstSegment.Y, X2 = endOfThirdSegment.X, Y2 = endOfFirstSegment.Y });

                DrawFractal(new System.Drawing.PointF[] { points[0], endOfFirstSegment }, recursionLevel - 1, fractalPlace);
                DrawFractal(new System.Drawing.PointF[] { endOfFirstSegment, endOfSecondSegment }, recursionLevel - 1, fractalPlace);
                DrawFractal(new System.Drawing.PointF[] { endOfSecondSegment, endOfThirdSegment }, recursionLevel - 1, fractalPlace);
                DrawFractal(new System.Drawing.PointF[] { endOfThirdSegment, points[1] }, recursionLevel - 1, fractalPlace);
            }
        }
    }
}
