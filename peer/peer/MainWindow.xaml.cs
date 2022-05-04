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
using System.Media;
using System.Globalization;

namespace peer
{
    /// <summary>
    /// Перечисление названий фракталов.
    /// </summary>
    enum FractalName
    {
        FractalTree,
        KochSnowflake,
        SierpinskiCarpet,
        SierpinskiTriangle,
        CantorSet
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly int[] NumericMaximum = {
            new FractalTree().MaxRecursionDepth,
            new KochSnowflake().MaxRecursionDepth,
            new SierpinskiCarpet().MaxRecursionDepth,
            new SierpinskiTriangle().MaxRecursionDepth,
            new CantorSet().MaxRecursionDepth
        };

        private readonly string[] IncorrectInputMessages = {
            "Длина отрезка должна быть вещественным числом",
            "Угол наклона правого отрезка должен быть вещественным числом",
            "Угол наклона левого отрезка должен быть вещественным числом",
            "Коэффициент отношения отрезков должен быть вещественным числом",
            "Высота отрезка должна быть вещественным числом",
            "Расстояние между отрезками должно быть вещественным числом"
        };
        FractalName CurrentFractalName;

        private Fractal _fractal;

        /// <summary>
        /// Инициализация компонентов.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            segmentLengthTB.PreviewTextInput += new TextCompositionEventHandler(textBox_PreviewTextInput);
            rBeginTB.PreviewTextInput += new TextCompositionEventHandler(textBox_PreviewTextInput);
            gBeginTB.PreviewTextInput += new TextCompositionEventHandler(textBox_PreviewTextInput);
            bBeginTB.PreviewTextInput += new TextCompositionEventHandler(textBox_PreviewTextInput);
            rEndTB.PreviewTextInput += new TextCompositionEventHandler(textBox_PreviewTextInput);
            gEndTB.PreviewTextInput += new TextCompositionEventHandler(textBox_PreviewTextInput);
            bEndTB.PreviewTextInput += new TextCompositionEventHandler(textBox_PreviewTextInput);
        }

        /// <summary>
        /// Запрет на ввод не цифр.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
        }

        /// <summary>
        /// Нажатие на фрактальное дерево.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fractalTreeRB_Checked(object sender, RoutedEventArgs e)
        {
            fractalTree.Visibility = Visibility.Visible;
            CurrentFractalName = FractalName.FractalTree;
            maxRecurtionDepth.Text = NumericMaximum[0].ToString();
            recurtionDepthSlider.Maximum = NumericMaximum[0];
        }

        /// <summary>
        /// Скрытие информации о фрактальном дереве.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fractalTreeRB_Unchecked(object sender, RoutedEventArgs e)
        {
            fractalTree.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Вывод информации о множестве кантора.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cantorSetRB_Checked(object sender, RoutedEventArgs e)
        {
            cantor.Visibility = Visibility.Visible;
            maxRecurtionDepth.Text = NumericMaximum[4].ToString();
            recurtionDepthSlider.Maximum = NumericMaximum[4];
        }

        /// <summary>
        /// Скрытие информации о множестве кантора.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cantorSetRB_Unchecked(object sender, RoutedEventArgs e)
        {
            cantor.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Проверка ввода на корректность.
        /// </summary>
        /// <param name="input">Ввод.</param>
        /// <param name="number"></param>
        /// <param name="message">Вывод сообщения об ошибке.</param>
        /// <returns></returns>
        private bool IsCorrectInputNumber(string input, out float number, string message)
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-US");
            if (!float.TryParse(input, out number))
            {
                MessageBox.Show(message, "Некорректный ввод");
                number = 0;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Нажатие на кнопку Рисования фрактала.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void drawButton_Click(object sender, RoutedEventArgs e)
        {
            fractalPlace.Children.Clear();
            if (!IsCorrectInputNumber(segmentLengthTB.Text, out float segmentLength, IncorrectInputMessages[0]))
            {
                return;
            }
            int recursionDepth = (int)recurtionDepthSlider.Value;
            Color startColor = Color.FromRgb((byte)int.Parse(rBeginTB.Text), (byte)int.Parse(gBeginTB.Text), (byte)int.Parse(bBeginTB.Text));
            Color endColor = Color.FromRgb((byte)int.Parse(rEndTB.Text), (byte)int.Parse(gEndTB.Text), (byte)int.Parse(bEndTB.Text));
            try
            {
                switch (CurrentFractalName)
                {
                    case FractalName.FractalTree:
                        if (!IsCorrectInputNumber(rightAngleTB.Text, out float rightAngle, IncorrectInputMessages[1])
                            || !IsCorrectInputNumber(leftAngleTB.Text, out float leftAngle, IncorrectInputMessages[2])
                            || !IsCorrectInputNumber(coefficientTB.Text, out float coefficient, IncorrectInputMessages[3]))
                        {
                            return;
                        }
                        _fractal = new FractalTree(segmentLength, recursionDepth, startColor, endColor, rightAngle, leftAngle, coefficient);
                        break;
                    case FractalName.KochSnowflake:
                        _fractal = new KochSnowflake(segmentLength, recursionDepth + 1, startColor, endColor);
                        break;
                    case FractalName.SierpinskiCarpet:
                        _fractal = new SierpinskiCarpet(segmentLength, recursionDepth, startColor, endColor);
                        break;
                    case FractalName.SierpinskiTriangle:
                        _fractal = new SierpinskiTriangle(segmentLength, recursionDepth, startColor, endColor);
                        break;
                    case FractalName.CantorSet:
                        if (!IsCorrectInputNumber(cantorHeightTB.Text, out float height, IncorrectInputMessages[4])
                            || !IsCorrectInputNumber(cantorDistanceTB.Text, out float distance, IncorrectInputMessages[5]))
                        {
                            return;
                        }
                        _fractal = new CantorSet(segmentLength, recursionDepth, startColor, endColor, (float)height, (float)distance);
                        break;
                }
                DrawFractal();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Некорректный ввод");
            }
        }

        /// <summary>
        /// Получение стартовой позиции фрактала.
        /// </summary>
        /// <returns></returns>
        private System.Drawing.PointF[] GetStartPoints()
        {
            System.Drawing.PointF[] points = new System.Drawing.PointF[0];
            float centerX = (float)fractalPlace.Width / 2f;
            float centerY = (float)fractalPlace.Height / 2f;

            if (_fractal is FractalTree)
            {
                points = new System.Drawing.PointF[] {
                    new System.Drawing.PointF(centerX, centerY + _fractal.SegmentLength),
                    new System.Drawing.PointF(centerX, centerY)
                };
            }
            if (_fractal is KochSnowflake)
            {
                points = new System.Drawing.PointF[] {
                    new System.Drawing.PointF(centerX - _fractal.SegmentLength / 2, centerY),
                    new System.Drawing.PointF(centerX + _fractal.SegmentLength / 2, centerY)
                };
            }
            if (_fractal is SierpinskiCarpet)
            {
                points = new System.Drawing.PointF[] {
                    new System.Drawing.PointF(centerX - _fractal.SegmentLength / 2, centerY - _fractal.SegmentLength / 2)
                };
            }
            if (_fractal is SierpinskiTriangle)
            {
                points = new System.Drawing.PointF[] {
                    new System.Drawing.PointF(centerX - _fractal.SegmentLength / 2, centerY + _fractal.SegmentLength / 2),
                    new System.Drawing.PointF(centerX + _fractal.SegmentLength / 2, centerY + _fractal.SegmentLength / 2),
                    new System.Drawing.PointF(centerX, centerY + _fractal.SegmentLength / 2 - _fractal.SegmentLength / 2 * (float)Math.Tan(Math.PI / 3))
                };
            }
            if (_fractal is CantorSet)
            {
                points = new System.Drawing.PointF[] { new System.Drawing.PointF(centerX - _fractal.SegmentLength / 2, 10) };
            }
            return points;
        }

        /// <summary>
        /// Рисование фрактала.
        /// </summary>
        private void DrawFractal()
        {
            _fractal.MinFractalHeightBound = (int)fractalPlace.Height;
            _fractal.MinFractalWidthBound = (int)fractalPlace.Width;

            try
            {
                _fractal.DrawFractal(GetStartPoints(), _fractal.RecursionDepth, fractalPlace);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка");
            }

        }

        /// <summary>
        /// Нажатие на кривую Коха.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void kochSnowflakeRB_Checked(object sender, RoutedEventArgs e)
        {
            CurrentFractalName = FractalName.KochSnowflake;
            maxRecurtionDepth.Text = NumericMaximum[1].ToString();
            recurtionDepthSlider.Maximum = NumericMaximum[1];
        }

        /// <summary>
        /// Нажатие на ковер Сперинского.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sierpinskiCarpetRB_Checked(object sender, RoutedEventArgs e)
        {
            CurrentFractalName = FractalName.SierpinskiCarpet;
            maxRecurtionDepth.Text = NumericMaximum[2].ToString();
            recurtionDepthSlider.Maximum = NumericMaximum[2];
        }

        /// <summary>
        /// Нажатие на треугольник Сперинского.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sierpinskiTriangleRB_Checked(object sender, RoutedEventArgs e)
        {
            CurrentFractalName = FractalName.SierpinskiTriangle;
            maxRecurtionDepth.Text = NumericMaximum[3].ToString();
            recurtionDepthSlider.Maximum = NumericMaximum[3];
        }
    }
}
