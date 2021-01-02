using GameOfLife.Models;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GameOfLife
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public float SpeedValue { get; set; } = 0.05f;
        public int SizeValue { get; set; } = 32;

        private DispatcherTimer timer;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            timer = new DispatcherTimer(DispatcherPriority.Render);
            timer.Interval = TimeSpan.FromSeconds(SpeedValue);
            timer.Tick += Next;
            GoLV.GridHeight = SizeValue;
            GoLV.GridWidth = 2 * SizeValue;
            GoLV.UpdateDimensions();
            SizeLabel.Content = String.Format("Size: {0}x{1}", GoLV.GridWidth, GoLV.GridHeight);
            GenerationLabel.Content = "Generation " + GoLV.Generation;
        }

        private void Next(object sender, EventArgs e)
        {
            GoLV.StepGeneration();
            GenerationLabel.Content = "Generation " + GoLV.Generation;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (timer.IsEnabled)
                {
                    timer.Stop();
                }
                else
                {
                    timer.Start();
                } 
            }
            else if (e.Key == Key.Space)
            {
                GoLV.StepGeneration();
                GenerationLabel.Content = "Generation " + GoLV.Generation;
            }
        }

        private void gridSlider_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            timer.Interval = TimeSpan.FromSeconds(SpeedValue);
        }

        private void UpdateGridSize(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            GoLV.GridHeight = SizeValue;
            GoLV.GridWidth = 2 * SizeValue;
            GoLV.UpdateDimensions();
            VB.UpdateLayout();
            SizeLabel.Content = String.Format("Size: {0}x{1}", GoLV.GridWidth, GoLV.GridHeight);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GoLV.StepGeneration();
            GenerationLabel.Content = "Generation " + GoLV.Generation;
        }

        private void StartButtonClick(object sender, RoutedEventArgs e)
        {
            if (timer.IsEnabled)
            {
                timer.Stop();
                (sender as Button).Content = "Start";
            }
            else
            {
                timer.Start();
                (sender as Button).Content = "Stop";
            }
        }

        private void ClearButtonClick(object sender, RoutedEventArgs e)
        {
            GoLV.Clear();
            timer.Stop();
            GenerationLabel.Content = "Generation " + GoLV.Generation;

        }
    }
}
