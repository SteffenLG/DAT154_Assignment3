using SpaceSim;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BergenSpaceProgram
{
	class SpaceObjectHolder
	{
		public Label label;
		public Canvas canvas;
		Ellipse ellipse;
		public Ellipse Ellipse { get { return ellipse; } }
		public SpaceObject spaceObjectData;
		private const double ELLIPSE_SHIFT = 150;
		private const double LABEL_SHIFT = 100;
		private Line line;

		public SpaceObjectHolder(Canvas parentCanvas,SpaceObject spaceObject)
		{
			this.spaceObjectData = spaceObject;
			line = new Line();
			canvas = new Canvas();
			ellipse = new Ellipse();
			label = new Label();
			label.Content = spaceObjectData.Name;
			label.IsHitTestVisible = false;
			label.Foreground = new SolidColorBrush(Colors.Goldenrod);
			switch (spaceObjectData)
			{

				case Star s:
					ellipse.Width = 50;
					ellipse.Height = 50;
					ellipse.Fill = new SolidColorBrush(Colors.Cornsilk);
					break;
				case Planet p:
					ellipse.Width = 25;
					ellipse.Height = 25;
					ellipse.Fill = new SolidColorBrush(Colors.DeepPink);
					break;
				case Moon m:
					ellipse.Width = 10;
					ellipse.Height = 10;
					ellipse.Fill = new SolidColorBrush(Colors.Peru);
					label.Visibility = Visibility.Hidden;
					break;
				default:
					ellipse.Width = 5;
					ellipse.Height = 5;
					ellipse.Fill = new SolidColorBrush(Colors.OldLace);
					break;

			}
			line.X1 = LABEL_SHIFT+2;
			line.Y1 = LABEL_SHIFT+2;
			line.X2 = ELLIPSE_SHIFT;
			line.Y2 = ELLIPSE_SHIFT;
			line.StrokeThickness = 1;
			line.Stroke = new SolidColorBrush(Colors.Blue);
			canvas.Children.Add(line);
			Canvas.SetLeft(label, LABEL_SHIFT);
			Canvas.SetTop(label, LABEL_SHIFT);
			Canvas.SetLeft(ellipse, ELLIPSE_SHIFT);
			Canvas.SetTop(ellipse, ELLIPSE_SHIFT);
			canvas.Children.Add(ellipse);
			canvas.Children.Add(label);
			parentCanvas.Children.Add(canvas);
		}

		public void SetX(double x)
        {
			Canvas.SetLeft(canvas, x  - ELLIPSE_SHIFT - ellipse.ActualWidth/2);
			
        }
		public void SetY(double y)
        {
			Canvas.SetTop(canvas, y - ELLIPSE_SHIFT - ellipse.ActualHeight / 2);
        }

		public double GetX()
        {
			return Canvas.GetLeft(canvas) + ELLIPSE_SHIFT + ellipse.ActualWidth / 2;
        }
		public double GetY()
        {
			return Canvas.GetTop(canvas) + ELLIPSE_SHIFT + ellipse.ActualHeight / 2;

		}

		public void ToggleLabel(bool state)
        {
			if(state == false)
            {
				label.Visibility = Visibility.Hidden;
				line.Visibility = Visibility.Hidden;
            }
			else
            {
				label.Visibility = Visibility.Visible;
				line.Visibility = Visibility.Visible;
            }
        }
	}

}
