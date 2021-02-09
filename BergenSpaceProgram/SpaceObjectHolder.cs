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
		private Line underLine;

		public bool showingLabel = true;

		Ellipse orbitEllipse;
		public SpaceObjectHolder(Canvas parentCanvas,SpaceObject spaceObject)
		{
			this.spaceObjectData = spaceObject;
			line = new Line();
			underLine = new Line();
			canvas = new Canvas();
			ellipse = new Ellipse();
			label = new Label();


			orbitEllipse = new Ellipse();
			orbitEllipse.Visibility = Visibility.Hidden;
			orbitEllipse.Stroke = new SolidColorBrush(Colors.White);
			orbitEllipse.StrokeThickness = 2;
			orbitEllipse.Fill = new SolidColorBrush(Colors.Transparent);
			parentCanvas.Children.Add(orbitEllipse);
			orbitEllipse.IsHitTestVisible = false;

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

					orbitEllipse.Visibility = Visibility.Visible;

					orbitEllipse.Width = spaceObjectData.OrbitalRadius * 2;
					orbitEllipse.Height = spaceObjectData.OrbitalRadius * 2;


					break;
				case Moon m:
					ellipse.Width = 10;
					ellipse.Height = 10;
					ellipse.Fill = new SolidColorBrush(Colors.Peru);
					label.Visibility = Visibility.Hidden;
					line.Visibility = Visibility.Hidden;
					underLine.Visibility = Visibility.Hidden;
					showingLabel = false;
					break;
				default:
					ellipse.Width = 5;
					ellipse.Height = 5;
					ellipse.Fill = new SolidColorBrush(Colors.OldLace);
					break;

			}
			line.X1 = LABEL_SHIFT+40;
			line.Y1 = LABEL_SHIFT+20;
			line.X2 = ELLIPSE_SHIFT + ellipse.Width/2;
			line.Y2 = ELLIPSE_SHIFT  +  ellipse.Height/2;
			line.StrokeThickness = 1;
			line.Stroke = new SolidColorBrush(Colors.White);
			canvas.Children.Add(line);


			underLine.X1 = LABEL_SHIFT + 10;
			underLine.Y1 = LABEL_SHIFT + 20;
			underLine.X2 = LABEL_SHIFT + 40;
			underLine.Y2 = LABEL_SHIFT + 20;
			underLine.StrokeThickness = 1;
			underLine.Stroke = new SolidColorBrush(Colors.White);
			canvas.Children.Add(underLine);


			Canvas.SetLeft(label, LABEL_SHIFT);
			Canvas.SetTop(label, LABEL_SHIFT);
			Canvas.SetLeft(ellipse, ELLIPSE_SHIFT);
			Canvas.SetTop(ellipse, ELLIPSE_SHIFT);
			canvas.Children.Add(ellipse);
			canvas.Children.Add(label);
			parentCanvas.Children.Add(canvas);
		}



		internal void UpdatePosition(double time, double ppmZ, double actualWidth, double actualHeight, double centerX, double centerY)
		{
			(double x, double y) = spaceObjectData.CalculatePosition(time);
			(double canvasX, double canvasY) = Space2Canvas2ElectricBogaloo(
				time,
				ppmZ,
				x, y,
				actualWidth, actualHeight,
				centerX,
				centerY);
			Canvas.SetLeft(canvas, canvasX - ELLIPSE_SHIFT - ellipse.ActualWidth / 2);
			Canvas.SetTop(canvas, canvasY - ELLIPSE_SHIFT - ellipse.ActualHeight / 2);

			if (spaceObjectData.DadBod == null) return;
			(double dadX, double dadY) = spaceObjectData.DadBod.CalculatePosition(time);
			(double canvasDadX, double canvasDadY) = Space2Canvas2ElectricBogaloo(
				time,
				ppmZ,
				dadX, dadY,
				actualWidth, actualHeight,
				centerX,
				centerY);
			Canvas.SetLeft(orbitEllipse, canvasDadX  - orbitEllipse.ActualWidth / 2);
			Canvas.SetTop(orbitEllipse, canvasDadY - orbitEllipse.ActualHeight / 2);

			orbitEllipse.Width = spaceObjectData.OrbitalRadius * 2 * ppmZ;
			orbitEllipse.Height = spaceObjectData.OrbitalRadius * 2 * ppmZ;
		}


		public void ToggleLabel(bool state)
        {
			if (showingLabel == state) return;
			showingLabel = state;
			if (state == false)
            {
				label.Visibility = Visibility.Hidden;
				line.Visibility = Visibility.Hidden;
				underLine.Visibility = Visibility.Hidden;
            }
			else
            {
				label.Visibility = Visibility.Visible;
				line.Visibility = Visibility.Visible;
				underLine.Visibility = Visibility.Visible;
            }
        }


		(double, double) Space2Canvas2ElectricBogaloo(
				double time,
				double pixelsPerMegameter,
				double x,
				double y,
				double screenWidth,
				double screenHeight,
				double spaceX = 0,
				double spaceY = 0)
		{
			//(0,0) is center of space
			//if you have selected another planet, (spaceX, spaceY) is center of space
			//(width/2, height/2) is center of canvas

			return ((-spaceX + x) * pixelsPerMegameter + screenWidth / 2, (-spaceY + y) * pixelsPerMegameter + screenHeight / 2);
		}
	}

}
