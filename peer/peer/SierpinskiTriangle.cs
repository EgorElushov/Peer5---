using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace peer
{
    /// <summary>
    /// Класс треугольника сперинского.
    /// </summary>
    internal class SierpinskiTriangle : Fractal
    {
        public override int MaxRecursionDepth => 7;


        /// <summary>
        /// Конструктор.
        /// </summary>
        public SierpinskiTriangle() { }


        /// <summary>
        /// Конструктор.
        /// </summary>
        public SierpinskiTriangle(float segmentLength, int recursionDepth, Color startColor, Color endColor)
            : base(segmentLength, recursionDepth, startColor, endColor) { }


        /// <summary>
        /// Рисование фрактала.
        /// </summary>
        /// <param name="points">Точки для рисования.</param>
        /// <param name="recursionLevel">Уровень рекурсии.</param>
        /// <param name="fractalPlace">Канвас для рисования.</param>
        public override void DrawFractal(System.Drawing.PointF[] points, int recursionLevel, Canvas fractalPlace)
        {
            if (recursionLevel == 0)
            {
                return;
            }

            System.Drawing.PointF bottomApex = new System.Drawing.PointF((points[0].X + points[1].X) / 2, (points[0].Y + points[1].Y) / 2);
            System.Drawing.PointF leftApex = new System.Drawing.PointF((points[0].X + points[2].X) / 2, (points[0].Y + points[2].Y) / 2);
            System.Drawing.PointF rightApex = new System.Drawing.PointF((points[1].X + points[2].X) / 2, (points[1].Y + points[2].Y) / 2);

            DrawFractal(new System.Drawing.PointF[] { points[0], bottomApex, leftApex }, recursionLevel - 1, fractalPlace);
            DrawFractal(new System.Drawing.PointF[] { bottomApex, points[1], rightApex }, recursionLevel - 1, fractalPlace);
            DrawFractal(new System.Drawing.PointF[] { leftApex, rightApex, points[2] }, recursionLevel - 1, fractalPlace);

            for (int i = 0; i <= points.Length - 1; i++)
            {
                base.DrawFractal(new System.Drawing.PointF[] { points[i % points.Length], points[(i + 1) % points.Length] }, recursionLevel, fractalPlace);
            }
        }
    }
}
