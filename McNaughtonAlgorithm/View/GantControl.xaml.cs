using McNaughtonAlgorithm.Model;
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

namespace McNaughtonAlgorithm.View
{
    /// <summary>
    /// Interaction logic for GantControl.xaml
    /// </summary>
    public partial class GantControl : UserControl
    {
        private int _rectangleWidth;
        private int _rectangleHeight;
        private int _startX;
        private int _startY;
        private Random _random;
        private IList<Machine> _machines;
        private IDictionary<int, Color> _colors;

        public IList<Machine> Machines
        {
            get { return (IList<Machine>)GetValue(MachinesProperty); }
            set { SetValue(MachinesProperty, value); }
        }

        public static readonly DependencyProperty MachinesProperty =
            DependencyProperty.Register("Machines", typeof(IList<Machine>),
              typeof(GantControl), new PropertyMetadata(null));

        public GantControl()
        {
            _rectangleHeight = 25;
            _rectangleWidth = 95;
            _startX = 100;
            _startY = 50;
            _random = new Random();

            InitializeComponent();
            canvas.DataContext = this;
            //DrawGant();
            //DrawRectangle(GetRectangle(3, Colors.Red), 0, 0);
        }

        private void DrawGant(bool randomColors)
        {
            this.canvas.Children.Clear();
            int x = _startX;
            int y = _startY;
            if(randomColors)
                PrepareColors();

            for (int m = 0; m < Machines.Count; m++)
            {
                var test = Machines[m].Chunks.GroupBy(k => k, v => Machines[m].Chunks.Count(c => c == v));

                foreach (var item in test) //item.Key - job name, item.First() - job time
                {
                    DrawRectangle(GetRectangle(item.First(), _colors[item.Key]), x, y);
                    if(item.Key != 0)
                        AddJobName(item.Key.ToString(), x + item.First()*_rectangleWidth / 2, y);
                    x += item.First() * _rectangleWidth;
                }
                x = _startX;
                y += _rectangleHeight;
            }

            AddMachineNames();
            AddTimeLine();
        }

        private void PrepareColors()
        {
            _colors = new Dictionary<int, Color>();
            _colors.Add(new KeyValuePair<int, Color>(0, Colors.Transparent));
            foreach (var machine in Machines)
            {
                foreach (var chunk in machine.Chunks)
                {
                    if (!_colors.ContainsKey(chunk))
                        _colors.Add(new KeyValuePair<int, Color>(chunk, RandomColor()));
                }
            }
        }

        private Color RandomColor()
        {
            return new Color()
            {
                A = 0xFF,
                R = (byte)_random.Next(1, 255),
                G = (byte)_random.Next(1, 255),
                B = (byte)_random.Next(1, 255)
            };
        }

        private void DrawRectangle(Rectangle rectangle, int x, int y)
        {
            Canvas.SetLeft(rectangle, x);
            Canvas.SetTop(rectangle, y);
            this.canvas.Children.Add(rectangle);
        }

        private Rectangle GetRectangle(int length, Color color)
        {
            Rectangle rect = new Rectangle();
            rect.Width = length * _rectangleWidth;
            rect.Height = _rectangleHeight;
            rect.Stroke = new SolidColorBrush(Colors.Black);
            rect.StrokeThickness = 1;
            rect.Fill = new SolidColorBrush(color);
            return rect;
        }

        private void AddMachineNames()
        {
            for (int machine = 0; machine<Machines.Count; machine++)
            {
                Viewbox box = new Viewbox();

                string text = "P" + (machine+1).ToString();
                TextBlock label = new TextBlock();
                label.Text = text;
                box.Height = _rectangleHeight;
                box.Width = _rectangleWidth;
                box.HorizontalAlignment = HorizontalAlignment.Right;
                ///label.FontSize = _rectangleHeight;
                Canvas.SetLeft(box, _startX - box.Width - 2);
                Canvas.SetTop(box, _startY + _rectangleHeight * machine);

                box.Child = label;

                canvas.Children.Add(box);
            }
        }

        private void AddTimeLine()
        {
            //Timeline
            int cMax = Machines.First().Chunks.Count;
            Line line = new Line();
            line.X1 = _startX;
            line.Y1 = _startY + Machines.Count * _rectangleHeight;
            line.X2 = _startX + cMax * _rectangleWidth;
            line.Y2 = _startY + Machines.Count * _rectangleHeight;
            line.StrokeThickness = 1;
            line.Stroke = new SolidColorBrush(Colors.Black);
            canvas.Children.Add(line);

            for (int i = 0; i <= Machines.First().Chunks.Count; i++)
            {
                Line step = new Line();
                step.X1 = _startX + i * _rectangleWidth;
                step.Y1 = _startY + Machines.Count * _rectangleHeight - _rectangleHeight / 4;
                step.X2 = _startX + i * _rectangleWidth;
                step.Y2 = _startY + Machines.Count * _rectangleHeight + _rectangleHeight / 4;
                step.StrokeThickness = 1;
                step.Stroke = new SolidColorBrush(Colors.Black);
                canvas.Children.Add(step);
            }

            for (int i = 0; i <= Machines.First().Chunks.Count; i++)
            {
                Viewbox box = new Viewbox();
                string text = i.ToString();
                TextBlock label = new TextBlock();
                label.Text = text;
                box.Height = _rectangleHeight;
                box.Width = _rectangleWidth;
                ///label.FontSize = _rectangleHeight;
                Canvas.SetLeft(box, _startX + i * _rectangleWidth - box.Width/2);
                Canvas.SetTop(box, _startY + Machines.Count * _rectangleHeight);

                box.Child = label;

                canvas.Children.Add(box);
            }
        }

        private void AddJobName(string name, int x, int y)
        {
            Viewbox box = new Viewbox();
            string text = "Z" + name;
            TextBlock label = new TextBlock();
            label.Text = text;
            box.Height = _rectangleHeight;
            box.Width = _rectangleWidth;
            Canvas.SetLeft(box, x-box.Width/2);
            Canvas.SetTop(box, y);

            box.Child = label;

            canvas.Children.Add(box);
        }

        private void przycisk_Click(object sender, RoutedEventArgs e)
        {
            DrawGant(true);
        }

        private void ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                _rectangleWidth = (int)this.rectWidth.Value;
                _rectangleHeight = (int)this.rectHeight.Value;
                DrawGant(false);
            }
            catch (Exception)
            { }
        }
    }
}
