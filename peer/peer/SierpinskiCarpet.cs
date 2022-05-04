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
    /// Класс ковра сперинского.
    /// </summary>
    internal class SierpinskiCarpet : Fractal 
    {
        private int _colorIndex;

        public override int MaxRecursionDepth => 5;


        /// <summary>
        /// Конструктор.
        /// </summary>
        public SierpinskiCarpet() { }


        /// <summary>
        /// Конструктор.
        /// </summary>
        public SierpinskiCarpet(float segmentLength, int recursionDepth, Color startColor, Color endColor)
            : base(segmentLength, recursionDepth, startColor, endColor) { }


        /// <summary>
        /// Рисование фрактала.
        /// </summary>
        /// <param name="points">Точки для рисования.</param>
        /// <param name="recursionLevel">Уровень рекурсии.</param>
        /// <param name="fractalPlace">Канвас для рисования.</param>
        public override void DrawFractal(System.Drawing.PointF[] points, int recursionLevel, Canvas fractalPlace)
        {
            if (recursionLevel == RecursionDepth)
            {
                _colorIndex = 1;
                SetGradientColors((int)Math.Pow(8, RecursionDepth));
            }
            float currentSegmentLength = SegmentLength / (float)Math.Pow(3, RecursionDepth - recursionLevel);

            if (recursionLevel == 0)
            {
                System.Drawing.PointF endPoint = new System.Drawing.PointF(points[0].X + currentSegmentLength, points[0].Y + currentSegmentLength);
                base.DrawFractal(new System.Drawing.PointF[] { points[0], endPoint }, RecursionDepth == 1 ? 1 : _colorIndex++, fractalPlace);
                return;
            }

            float startX = points[0].X;
            float middleX = startX + currentSegmentLength / 3;
            float endX = startX + currentSegmentLength * 2 / 3;
            float startY = points[0].Y;
            float middleY = startY + currentSegmentLength / 3;
            float endY = startY + currentSegmentLength * 2 / 3;

            DrawFractal(new System.Drawing.PointF[] { new System.Drawing.PointF(startX, startY) }, recursionLevel - 1, fractalPlace);
            DrawFractal(new System.Drawing.PointF[] { new System.Drawing.PointF(middleX, startY) }, recursionLevel - 1, fractalPlace);
            DrawFractal(new System.Drawing.PointF[] { new System.Drawing.PointF(endX, startY) }, recursionLevel - 1, fractalPlace);
            DrawFractal(new System.Drawing.PointF[] { new System.Drawing.PointF(startX, middleY) }, recursionLevel - 1, fractalPlace);
            DrawFractal(new System.Drawing.PointF[] { new System.Drawing.PointF(endX, middleY) }, recursionLevel - 1, fractalPlace);
            DrawFractal(new System.Drawing.PointF[] { new System.Drawing.PointF(startX, endY) }, recursionLevel - 1, fractalPlace);
            DrawFractal(new System.Drawing.PointF[] { new System.Drawing.PointF(middleX, endY) }, recursionLevel - 1, fractalPlace);
            DrawFractal(new System.Drawing.PointF[] { new System.Drawing.PointF(endX, endY) }, recursionLevel - 1, fractalPlace);
        }
    }
}
