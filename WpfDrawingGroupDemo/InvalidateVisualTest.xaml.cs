using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;

namespace WpfDrawingGroupDemo
{
    /// <summary>
    /// InvalidateVisualTest.xaml 的交互逻辑
    /// </summary>
    public partial class InvalidateVisualTest
    {
        private readonly Stopwatch _stopwatch = new();
        private int _frameCount;

        public InvalidateVisualTest()
        {
            InitializeComponent();
            Loaded += (_, _) => { _stopwatch.Start(); };
            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }


        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            //每一秒钟统计一次帧率
            if (_stopwatch.ElapsedMilliseconds >= 1000)
            {
                TextBlockFrame.Text = (_frameCount * 1000d / _stopwatch.ElapsedMilliseconds).ToString("0.00");
                _frameCount = 0;
                _stopwatch.Restart();
            }

            InvalidateVisual();
            _frameCount++;
        }


        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (IsLoaded)
            {
                var isDesignMode = DesignerProperties.GetIsInDesignMode(this);

                if (!isDesignMode)
                {
                    ReRender(drawingContext);
                }
            }
        }

        private readonly Pen _pen = new(Brushes.Blue, 1);
        private readonly Random _random = new();

        public void ReRender(DrawingContext drawingContext)
        {
            for (int i = 0; i < 500; i++)
            {
                var point1 = new Point(_random.Next(0, (int) ActualWidth), _random.Next(0, (int) ActualHeight));
                var point2 = new Point(_random.Next(0, (int) ActualWidth), _random.Next(0, (int) ActualHeight));

                drawingContext.DrawLine(_pen, point1, point2);
            }
        }
    }
}