using McNaughtonAlgorithm.Model;
using McNaughtonAlgorithm.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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
        private IDictionary<int, Color> _colors;
        private IList<Color> _standardColors;
        private int _usedColor;

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
            _rectangleHeight = 50;
            _rectangleWidth = 50;
            _startX = 100;
            _startY = 50;
            _random = new Random();
            _usedColor = 0;
            PrepareStandardColors();

            InitializeComponent();
            canvas.DataContext = this;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ((MainViewModel)this.DataContext).DrawImage += GantControl_DrawImage;
        }

        private void GantControl_DrawImage(object sender, EventArgs e)
        {
            DrawGant(true);
            this.btnSaveImage.IsEnabled = true;
        }

        private void DrawGant(bool randomColors)
        {
            this.canvas.Children.Clear();
            _usedColor = 0;
            int x = _startX;
            int y = _startY;
            if(randomColors)
                PrepareColors(true);

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

        private void PrepareColors(bool useStandardColors)
        {
            _colors = new Dictionary<int, Color>();
            _colors.Add(new KeyValuePair<int, Color>(0, Colors.Transparent));
            foreach (var machine in Machines)
            {
                foreach (var chunk in machine.Chunks)
                {
                    if (!_colors.ContainsKey(chunk))
                    {
                        if (useStandardColors && _usedColor < _standardColors.Count)
                            _colors.Add(new KeyValuePair<int, Color>(chunk, _standardColors[_usedColor++]));
                        else
                            _colors.Add(new KeyValuePair<int, Color>(chunk, RandomColor()));
                    }
                }
            }
        }

        private void PrepareStandardColors()
        {
            _standardColors = new List<Color>()
            {
                Color.FromRgb(255,0,0),
                Color.FromRgb(0,0,255),
                Color.FromRgb(255,255,0),
                Color.FromRgb(0,255,255),
                Color.FromRgb(0,255,0),
                Color.FromRgb(128,0,255),
                Color.FromRgb(255,128,0),
                Color.FromRgb(255,0,255),
                Color.FromRgb(128,128,128),
                Color.FromRgb(255,0,128),
                Color.FromRgb(128,0,0),
                Color.FromRgb(0,128,0),
                Color.FromRgb(0,0,128),
                Color.FromRgb(128,0,128),
                Color.FromRgb(128,128,0),
                Color.FromRgb(0,128,128)
            };
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

        private void CreateSaveBitmap(Canvas canvas, string filename)
        {
            RenderTargetBitmap renderBitmap = new RenderTargetBitmap(
             (int)canvas.Width, (int)canvas.Height,
             96d, 96d, PixelFormats.Pbgra32);
            // needed otherwise the image output is black
            /*canvas.Measure(new Size((int)canvas.Width, (int)canvas.Height));
            canvas.Arrange(new Rect(new Size((int)canvas.Width, (int)canvas.Height)));*/

            renderBitmap.Render(canvas);

            //JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(renderBitmap));

            using (FileStream file = File.Create(filename))
            {
                encoder.Save(file);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.Filter = "PNG files (*.png)|*.png";
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    CreateSaveBitmap(this.canvas, dialog.FileName);
                }
                catch (Exception exc)
                {
                    MessageBox.Show("Image cannot be saved here. Try again!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                MessageBox.Show("Image saved!", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
