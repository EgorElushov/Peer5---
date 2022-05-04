using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace peer
{
    /// <summary>
    /// Класс фрактального дерева.
    /// </summary>
    internal class FractalTree : Fractal
    {
        private double _rightAngle;
        private double _leftAngle;
        private double _coefficient;
        private double _currentAngle;
        private float _treeStartX;
        private float _treeEndX;
        private float _treeStartY;
        private float _treeEndY;

        public override int MaxRecursionDepth => 13;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public FractalTree() { }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public FractalTree(float segmentLength, int recursionDepth, Color startColor, Color endColor,
            double rightAngle, double leftAngle, double coefficient) : base(segmentLength, recursionDepth, startColor, endColor)
        {
            RightAngle = rightAngle;
            LeftAngle = leftAngle;
            Coefficient = coefficient;
        }

        /// <summary>
        /// Правый угол.
        /// </summary>
        private double RightAngle
        {
            get => _rightAngle;
            set
            {
                if (value <= -180 || value >= 180)
                {
                    throw new ArgumentException("Угол наклона правого отрезка не может быть больще 180 и меньше -180 градусов");
                }
                _rightAngle = value * Math.PI / 180;
            }
        }

        /// <summary>
        /// Левый угол.
        /// </summary>
        private double LeftAngle
        {
            get => _leftAngle;
            set
            {
                if (value <= -180 || value >= 180)
                {
                    throw new ArgumentException("Угол наклона левого отрезка не может быть больще 180 и меньше -180 градусов");
                }
                _leftAngle = value * Math.PI / 180;
            }
        }

        /// <summary>
        /// Коэфициент отношения отрезков.
        /// </summary>
        private double Coefficient
        {
            get => _coefficient;
            set
            {
                if (value >= 1 || value <= 0)
                {
                    throw new ArgumentException("Коэффициент должен принадлежать отрезку (0;1]");
                }
                _coefficient = value;
            }
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
            base.DrawFractal(points, recursionLevel, fractalPlace);

            SetStartAndEndPoints(points, recursionLevel);
            points[0] = points[1];
            double segmentLength = SegmentLength * Math.Pow(Coefficient, RecursionDepth - recursionLevel + 1);

            BuildBranch(points[1], segmentLength, LeftAngle, 1, recursionLevel, fractalPlace);
            BuildBranch(points[1], segmentLength, RightAngle, -1, recursionLevel, fractalPlace);
        }

        /// <summary>
        /// Set start and end points of the fractal.
        /// </summary>
        /// <param name="points">Points for setting start and end points.</param>
        /// <param name="recursionLevel">A recursion level.</param>
        private void SetStartAndEndPoints(System.Drawing.PointF[] points, int recursionLevel)
        {
            if (recursionLevel == RecursionDepth)
            {
                _treeStartX = -1;
                _treeEndX = -1;
                _treeStartY = -1;
                _treeEndY = -1;
            }
            foreach (var point in points)
            {
                if (point.X < _treeStartX || _treeStartX == -1)
                    _treeStartX = point.X;
                if (point.X > _treeEndX || _treeEndX == -1)
                    _treeEndX = point.X;
                if (point.Y < _treeStartY || _treeStartY == -1)
                    _treeStartY = point.Y;
                if (point.Y > _treeEndY || _treeEndY == -1)
                    _treeEndY = point.Y;
            }
        }

        /// <summary>
        /// Построение ветки дерева.
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="segmentLength"></param>
        /// <param name="angle"></param>
        /// <param name="direction"></param>
        /// <param name="recursionLevel"></param>
        /// <param name="fractalPlace"></param>
        private void BuildBranch(System.Drawing.PointF startPoint, double segmentLength, double angle, int direction, int recursionLevel, Canvas fractalPlace)
        {
            _currentAngle += angle * direction;

            float x = (float)(startPoint.X - direction * (segmentLength * Math.Sin(_currentAngle * direction)));
            float y = (float)(startPoint.Y - (segmentLength * Math.Cos(_currentAngle * direction)));

            DrawFractal(new System.Drawing.PointF[] { startPoint, new System.Drawing.PointF(x, y) }, recursionLevel - 1, fractalPlace);

            _currentAngle -= angle * direction;
        }
    }
}
