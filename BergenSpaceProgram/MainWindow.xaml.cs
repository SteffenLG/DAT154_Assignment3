﻿using System;
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

using SpaceSim;

namespace BergenSpaceProgram
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			SpaceObject selectedObject = null;

			#region Initialize SpaceObjects and Canvas Elements
			InitializeComponent();



			List<SpaceObject> solarSystem = XMLReader.ParseXML();

			List<Ellipse> ellipses = new List<Ellipse>();
			solarSystem.ForEach(so =>
			{
				Ellipse MyLittleEllipse = new Ellipse();

				switch (so)
				{
					case Star s:
						MyLittleEllipse.Width = 50;
						MyLittleEllipse.Height = 50;
						MyLittleEllipse.Fill = new SolidColorBrush(Colors.Cornsilk);
						break;
					case Planet p:
						MyLittleEllipse.Width = 25;
						MyLittleEllipse.Height = 25;
						MyLittleEllipse.Fill = new SolidColorBrush(Colors.DeepPink);
						break;
					case Moon m:
						MyLittleEllipse.Width = 10;
						MyLittleEllipse.Height = 10;
						MyLittleEllipse.Fill = new SolidColorBrush(Colors.Peru);
						break;
					default:
						MyLittleEllipse.Width = 5;
						MyLittleEllipse.Height = 5;
						MyLittleEllipse.Fill = new SolidColorBrush(Colors.OldLace);
						break;

				}
				Canvas.SetLeft(MyLittleEllipse, 150);
				Canvas.SetTop(MyLittleEllipse, 150);
				MyCanvas.Children.Add(MyLittleEllipse);
				ellipses.Add(MyLittleEllipse);

				MyLittleEllipse.MouseDown += (sender, e) => OnSpaceObjectClick(sender, e, so);

				void OnSpaceObjectClick(object sender, EventArgs e, SpaceObject so)
				{
					if(so.Children.Count < 1)
					{
						return;
					}
					selectedObject = so;
				}

				var window = Window.GetWindow(this);

				window.KeyDown += HandleKeyPress;

			});

			#endregion



			#region Start Timer for draw calls

			System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
			dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
			dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
			dispatcherTimer.Start();

			void dispatcherTimer_Tick(object sender, EventArgs e)
			{
				Draw(DateTime.Now.TimeOfDay.TotalMilliseconds / 10);
			}
			#endregion

			#region Draw Calls
			void Draw(double time)
			{
				if (selectedObject != null)
				{
					DrawSelected(time);
				} else
				{
					DrawNoneSelected(time);
				}
			}
			void DrawSelected(double time)
			{
				(double centerX, double centerY) = selectedObject.CalculatePosition(time);

				double maxRadius = selectedObject.Children.OrderByDescending(item => item.OrbitalRadius).First().OrbitalRadius;

				(double, double) SpaceToCanvas(double x, double y)
				{
					double Lerp(double firstFloat, double secondFloat, double by)
					{
						return firstFloat * (1 - by) + secondFloat * by;
					}


					double factorX = (x - (-maxRadius)- centerX) / (maxRadius*2);
					double factorY = (y - (-maxRadius) - centerY) / (maxRadius*2);

					double canvasX = Lerp(0, MyCanvas.ActualWidth, factorX);
					double canvasY = Lerp(0, MyCanvas.ActualHeight, factorY);
					return (canvasX, canvasY);
				}
				for (int i = 0; i < solarSystem.Count; i++)
				{
					(double x, double y) = solarSystem[i].CalculatePosition(time);
					(double canvasX, double canvasY) = SpaceToCanvas(x, y);
					Canvas.SetLeft(ellipses[i], canvasX - ellipses[i].ActualWidth / 2);
					Canvas.SetTop(ellipses[i], canvasY - ellipses[i].ActualHeight / 2);
				}
			}
			void DrawNoneSelected(double time)
			{
				double maxX = 0;
				double maxY = 0;
				double minX = 0;
				double minY = 0;

				for (int i = 0; i < solarSystem.Count; i++)
				{
					(double x, double y) = solarSystem[i].CalculatePosition(time);

					if (x > maxX)
					{
						maxX = x;
					}
					if (x < minX)
					{
						minX = x;
					}
					if (y > maxY)
					{
						maxY = y;
					}
					if (y < minY)
					{
						minY = y;
					}
				}
				double spaceWidth = maxX - minX;
				double spaceHeight = maxY - minY;
				if (spaceWidth > spaceHeight)
				{
					double diff = spaceWidth - spaceHeight;
					maxY += diff / 2;
					minY -= diff / 2;
					spaceHeight = maxY - minY;

				}
				else
				{
					double diff = spaceHeight - spaceWidth;
					maxX += diff / 2;
					minX -= diff / 2;
					spaceWidth = maxX - minX;
				}

				(double, double) SpaceToCanvas(double x, double y)
				{
					double Lerp(double firstFloat, double secondFloat, double by)
					{
						return firstFloat * (1 - by) + secondFloat * by;
					}


					double factorX = (x - minX) / spaceWidth;
					double factorY = (y - minY) / spaceHeight;

					double canvasX = Lerp(0, MyCanvas.ActualWidth, factorX);
					double canvasY = Lerp(0, MyCanvas.ActualHeight, factorY);
					return (canvasX, canvasY);
				}
				for (int i = 0; i < solarSystem.Count; i++)
				{
					(double x, double y) = solarSystem[i].CalculatePosition(time);
					(double canvasX, double canvasY) = SpaceToCanvas(x, y);
					Canvas.SetLeft(ellipses[i], canvasX - ellipses[i].ActualWidth / 2);
					Canvas.SetTop(ellipses[i], canvasY - ellipses[i].ActualHeight / 2);
				}
			}
			#endregion

			#region HandleKeyPress

			void HandleKeyPress(object sender, KeyboardEventArgs e)
			{
				if (e.KeyboardDevice.IsKeyDown(Key.Escape))
				{
					selectedObject = null;
				}
			}
			#endregion

		}
	}
}
