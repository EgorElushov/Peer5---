using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace peer
{
    /// <summary>
    /// Базовый класс фрактала.
    /// </summary>
    internal class Fractal
    {
        public int MinFractalWidthBound;
        public int MinFractalHeightBound;
        private float _segmentLength;
        private int _recursionDepth;
        private Color[] _gradientColors;
        private Color _startColor;
        private Color _endColor;

        /// <summary>
        /// Длина сегмента.
        /// </summary>
        public float SegmentLength
        {
            get => _segmentLength;
            private set
            {
                if (value <= 0 || value > 600)
                {
                    throw new ArgumentException("Длина отрезка должна быть больше 0 и не превышать 600");
                }
                _segmentLength = value;
            }
        }

        /// <summary>
        /// Глубина рекурсии.
        /// </summary>
        public int RecursionDepth
        {
            get => _recursionDepth;
            set
            {
                _recursionDepth = value;
                SetGradientColors(RecursionDepth);
            }
        }

        public virtual int MaxRecursionDepth { get; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public Fractal() { }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public Fractal(float segmentLength, int recursionDepth, Color startColor, Color endColor)
        {
            SegmentLength = segmentLength;
            RecursionDepth = recursionDepth;
            _startColor = startColor;
            _endColor = endColor;
            SetGradientColors(RecursionDepth);
        }

        /// <summary>
        /// Получение градиента цвета.
        /// </summary>
        /// <param name="colorCount"></param>
        protected void SetGradientColors(int colorCount)
        {
            _gradientColors = new Color[colorCount];

            for (int i = 0; i < colorCount; i++)
            {
                int rAverage = _endColor.R + (int)((_startColor.R - _endColor.R) * i / (double)colorCount);
                int gAverage = _endColor.G + (int)((_startColor.G - _endColor.G) * i / (double)colorCount);
                int bAverage = _endColor.B + (int)((_startColor.B - _endColor.B) * i / (double)colorCount);
                _gradientColors[i] = Color.FromRgb((byte)rAverage, (byte)gAverage, (byte)bAverage);
            }
        }

        /// <summary>
        /// Рисование фрактала.
        /// </summary>
        /// <param name="points">Точки для рисования.</param>
        /// <param name="recursionLevel">Уровень рекурсии.</param>
        /// <param name="fractalPlace">Канвас для рисования.</param>
        public virtual void DrawFractal(System.Drawing.PointF[] points, int recursionLevel, Canvas fractalPlace) {
            if (this is FractalTree || this is KochSnowflake || this is SierpinskiTriangle)
            {
                fractalPlace.Children.Add(new Line { X1 = points[0].X, Y1 = points[0].Y, X2 = points[1].X, Y2 = points[1].Y, 
                                                    Stroke = new SolidColorBrush(_gradientColors[recursionLevel - 1])});
            }
            else if (this is CantorSet || this is SierpinskiCarpet)
            {
                Rectangle rectangle = new Rectangle { Width = points[1].X - points[0].X, Height = points[1].Y - points[0].Y,
                                                      Stroke = new SolidColorBrush(_gradientColors[recursionLevel - 1]),
                                                      Fill = new SolidColorBrush(_gradientColors[recursionLevel - 1]),
                                                      Margin = new Thickness(points[0].X, points[0].Y, 0, 0)
                                                      };
                fractalPlace.Children.Add(rectangle);
            }
        }
    }
}
