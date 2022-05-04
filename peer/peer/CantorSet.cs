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
    /// Класс множества Кантора.
    /// </summary>
    internal class CantorSet : Fractal
    {
        private float _height;
        private float _distance;

        /// <summary>
        /// Максимальная глубина рекурсии.
        /// </summary>
        public override int MaxRecursionDepth => 7;

        /// <summary>
        /// Высота одного сегмента.
        /// </summary>
        public float Height
        {
            get => _height;
            set
            {
                if (value <= 0 || value > 50)
                {
                    throw new ArgumentException("Высота прямоугольника должна быть больше 0 и не превышать 50");
                }
                _height = value;
            }
        }

        /// <summary>
        /// Расстояния между двумя отрезками.
        /// </summary>
        public float Distance
        {
            get => _distance;
            set
            {
                if (value <= 0 || value > 100)
                {
                    throw new ArgumentException("Расстояние между отрезками должно быть больше 0 и не превышать 100");
                }
                _distance = value;
            }
        }


        /// <summary>
        /// Конструктор.
        /// </summary>
        public CantorSet() { }


        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="height"></param>
        /// <param name="distance"></param>
        public CantorSet(float segmentLength, int recursionDepth, Color startColor, Color endColor, float height, float distance)
            : base(segmentLength, recursionDepth, startColor, endColor)
        {
            Height = height;
            Distance = distance;
        }


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
            float currentSegmentLength = SegmentLength / (float)Math.Pow(3, RecursionDepth - recursionLevel);

            System.Drawing.PointF endPoint = new System.Drawing.PointF(points[0].X + currentSegmentLength, points[0].Y + Height);

            base.DrawFractal(new System.Drawing.PointF[] { points[0], endPoint }, recursionLevel, fractalPlace);

            DrawFractal(new System.Drawing.PointF[] { new System.Drawing.PointF(points[0].X, points[0].Y + Distance + Height) }, recursionLevel - 1, fractalPlace);
            DrawFractal(new System.Drawing.PointF[] { new System.Drawing.PointF(points[0].X + currentSegmentLength * 2 / 3, points[0].Y + Distance + Height) }, recursionLevel - 1, fractalPlace);
        }
    }
}
