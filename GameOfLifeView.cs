using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GameOfLife
{
    public class GameOfLifeView : FrameworkElement
    {
		#region Private fields

		private GameOfLifeLogic gameLogic;
		private int lastX,lastY;

		private List<Visual> visuals = new List<Visual>();
		private DrawingVisual gridVisual = new DrawingVisual();
		private DrawingVisual cellsVisual = new DrawingVisual();

		public int GridWidth { get; set; } = 8;
		public int GridHeight { get; set; } = 4;
		#endregion
		#region Properties
		public int CellSize
		{
			get { return (int)GetValue(CellSizeProperty); }
			set { SetValue(CellSizeProperty, value); }
		}

		public static readonly DependencyProperty CellSizeProperty =
			DependencyProperty.Register("CellSize", typeof(int), typeof(GameOfLifeView), new FrameworkPropertyMetadata(12, FrameworkPropertyMetadataOptions.AffectsMeasure));

		public int Generation
		{get; private set;}

		public static readonly DependencyProperty BackgroundProperty;
		public Brush Background
		{
			get { return (Brush)GetValue(BackgroundProperty); }
			set { SetValue(BackgroundProperty, value); }
		}

		public static readonly DependencyProperty ForegroundProperty;
		public Brush Foreground
		{
			get { return (Brush)GetValue(ForegroundProperty); }
			set { SetValue(ForegroundProperty, value); }
		}

		public static readonly DependencyProperty PaddingProperty;
		public Thickness Padding
		{
			get { return (Thickness)GetValue(PaddingProperty); }
			set { SetValue(PaddingProperty, value); }
		}

		#endregion

		#region Constructors
		static GameOfLifeView()
		{
			BackgroundProperty = Control.BackgroundProperty.AddOwner(typeof(GameOfLifeView),
				new FrameworkPropertyMetadata(Brushes.Transparent, FrameworkPropertyMetadataOptions.AffectsRender));
			ForegroundProperty = Control.ForegroundProperty.AddOwner(typeof(GameOfLifeView), new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender));
			PaddingProperty = Control.PaddingProperty.AddOwner(typeof(GameOfLifeView), new FrameworkPropertyMetadata(new Thickness(10), FrameworkPropertyMetadataOptions.AffectsRender));
		}

		public GameOfLifeView()
		{
			AddVisual(gridVisual);
			AddVisual(cellsVisual);
		}
		#endregion

		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);
			UpdateDimensions();
		}

		public void UpdateDimensions()
		{
			if (gameLogic == null)
            {
				gameLogic = new GameOfLifeLogic(GridWidth, GridHeight);
			}
			else
				gameLogic.SetGridSize(GridWidth, GridHeight);
			InvalidateMeasure();
			DrawGrid();
			DrawCells();
		}
		protected override Size MeasureOverride(Size availableSize)
		{
			int cellSize = this.CellSize;
			Thickness padding = this.Padding;
			return new Size(GridWidth * cellSize + padding.Left + padding.Right, GridHeight * cellSize + padding.Top + padding.Bottom);
		}
		protected override void OnRender(DrawingContext drawingContext)
		{
			DrawGrid();
			DrawCells();
		}

		void DrawGrid()
        {
            using (DrawingContext dCtx = gridVisual.RenderOpen())
            {
				//Grid border
				Thickness padding = this.Padding;
				int cellSize = this.CellSize;
				Rect gridBorder = new Rect(padding.Left, padding.Top, GridWidth*cellSize, GridHeight*cellSize);

				dCtx.DrawRectangle(this.Background, new Pen(this.Foreground, 1), gridBorder);

				//Vertical Lines
				double x = padding.Left;
                for (int i = 0; i < GridWidth; i++)
                {
					dCtx.DrawLine(new Pen(this.Foreground, 1), new Point(x, padding.Top), new Point(x, padding.Top+GridHeight*cellSize));
					x += cellSize;
                }
				//Horizontal Lines
				double y = padding.Top;
				for (int i = 0; i <= GridHeight; i++)
				{
					dCtx.DrawLine(new Pen(this.Foreground, 1), new Point(padding.Left, y), new Point(padding.Left + GridWidth * cellSize, y));
					y += cellSize;
				}

			}
		}

		public void DrawCells()
        {
            using (DrawingContext dCtx = cellsVisual.RenderOpen())
            {
                Thickness padding = this.Padding;
				int cellSize = this.CellSize;
				Brush cellColor = this.Foreground;
                for (int i = 0; i < GridWidth; i++)
                {
                    for (int j = 0; j < GridHeight; j++)
                    {
                        if (gameLogic.Cells[i, j].IsAlive)
                        {
							dCtx.DrawRectangle(cellColor, null, new Rect(padding.Left + i * cellSize , padding.Top + j * cellSize , cellSize , cellSize ));
						}
                    }
                }
            }
        }
		
		public void StepGeneration()
        {
			gameLogic.Step();
			DrawCells();
			this.Generation++;
        }

		protected override void OnMouseDown(System.Windows.Input.MouseButtonEventArgs e)
		{
			base.OnMouseDown(e);
			if(e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
				Thickness padding = this.Padding;
				Point pt = e.GetPosition(this);
				int cellSize = this.CellSize;
				int x = (int)((pt.X - padding.Left) / cellSize);
				int y = (int)((pt.Y - padding.Top) / cellSize);
				gameLogic.Cells[x, y].SwitchStatus();
				DrawCells();
			}
			
		}

		protected override void OnMouseMove(System.Windows.Input.MouseEventArgs e)
		{
			base.OnMouseMove(e);

			Point pt = e.GetPosition(this);
			Thickness padding = this.Padding;
			int cellSize = this.CellSize;
			int x = (int)((pt.X - padding.Left) / cellSize);
			int y = (int)((pt.Y - padding.Top) / cellSize);
			if (lastX == x && lastY == y) return;
			lastX = x;
			lastY = y;
			if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
			{
				gameLogic.Cells[x, y].IsAlive = true;
				DrawCells();
			} else if(e.RightButton == System.Windows.Input.MouseButtonState.Pressed)
            {
				gameLogic.Cells[x, y].IsAlive = false;
				DrawCells();
			}
		}

		void AddVisual(Visual child)
		{
			this.AddLogicalChild(child);
			this.AddVisualChild(child);
			visuals.Add(child);
		}

		public void Clear()
		{
			gameLogic.Cells.Clear();
			this.Generation = 0;
			DrawGrid();
			DrawCells();
		}

		protected override Visual GetVisualChild(int index)
		{
			return visuals[index];
		}

		protected override int VisualChildrenCount
		{
			get
			{
				return visuals.Count;
			}
		}
	}
}
