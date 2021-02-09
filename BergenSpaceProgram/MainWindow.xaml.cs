using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using SpaceSim;

namespace BergenSpaceProgram
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		SpaceObject selectedObject = null;
		double timeScale = 1;
		double currentTime = 0;

		List<SpaceObject> solarSystemData = XMLReader.ParseXML();
		double pixelsPerMegameter = 1;
		double zoomFactor = 1.0;

		List<SpaceObjectHolder> spaceObjectHolders;
		public MainWindow()
		{
			
			#region Initialize SpaceObjects and Canvas Elements
			InitializeComponent();



			pixelsPerMegameter = PixelsPerMegameter(solarSystemData, MyCanvas.ActualWidth, MyCanvas.ActualHeight);

			MyCanvas.SizeChanged += (object sender, SizeChangedEventArgs e) =>
			{
				if(selectedObject == null || selectedObject.Children.Count < 1)
					pixelsPerMegameter = PixelsPerMegameter(solarSystemData, MyCanvas.ActualWidth, MyCanvas.ActualHeight);
				else
					pixelsPerMegameter = PixelsPerMegameter(selectedObject.Children, MyCanvas.ActualWidth, MyCanvas.ActualHeight);
			};




			spaceObjectHolders = new List<SpaceObjectHolder>();
			solarSystemData.ForEach(so =>
			{
				SpaceObjectHolder currentSpaceObjectHolder = new SpaceObjectHolder(MyCanvas, so);
				spaceObjectHolders.Add(currentSpaceObjectHolder);
				currentSpaceObjectHolder.Ellipse.MouseDown += (sender, e) => OnSpaceObjectClick(sender, e, so, MyCanvas.ActualWidth, MyCanvas.ActualHeight);
				currentSpaceObjectHolder.Ellipse.MouseMove += (sender, e) => OnSpaceObjectHover(sender, e, so, currentSpaceObjectHolder.Ellipse);
			});

			var window = Window.GetWindow(this);

			window.KeyDown += HandleKeyPress;

			TimeScaleSlider.ValueChanged += TimeScaleSlider_ValueChanged;
			#endregion



			#region Start Timer for draw calls

			System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
			dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
			dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
			dispatcherTimer.Start();

			void dispatcherTimer_Tick(object sender, EventArgs e)
			{
				currentTime += timeScale;
				Draw(currentTime);
			}
			#endregion

			#region Draw Calls
			void Draw(double time)
			{
				if (selectedObject != null)
				{
					DrawSystem(time, selectedObject);
				} else
				{
					DrawSystem(time, solarSystemData[0]);
				}
			}
			void DrawSystem(double time, SpaceObject so)
			{
				(double centerX, double centerY) = so.CalculatePosition(time);

				foreach (SpaceObjectHolder item in spaceObjectHolders)
				{
					(double x, double y) = item.spaceObjectData.CalculatePosition(time);
					(double canvasX, double canvasY) = Space2Canvas2ElectricBogaloo(
						time,
						pixelsPerMegameter * zoomFactor,
						x, y,
						MyCanvas.ActualWidth, MyCanvas.ActualHeight,
						centerX,
						centerY);
					item.SetX(canvasX);
					item.SetY(canvasY);
					//Canvas.SetLeft(item.canvas, canvasX - item.canvas.ActualWidth / 2);
					//Canvas.SetTop(item.canvas, canvasY - item.canvas.ActualHeight / 2);
				}
				/*
				for (int i = 0; i < spaceObjectHolders.Count; i++)
				{
					(double x, double y) = solarSystem[i].CalculatePosition(time);
					(double canvasX, double canvasY) = Space2Canvas2ElectricBogaloo(
						time, 
						pixelsPerMegameter*zoomFactor,
						x, y,
						MyCanvas.ActualWidth, MyCanvas.ActualHeight,
						centerX,
						centerY);
					Canvas.SetLeft(grids[i], canvasX - grids[i].ActualWidth / 2);
					Canvas.SetTop(grids[i], canvasY - grids[i].ActualHeight / 2);
					//draw text next to the ellipse if label is enabled
				}
				*/
			}
			
            #endregion
            
			#region ScaleCalculations

			//finne den største radiusen
			//lage en skaleringsfaktor som lar oss oversette fra avstand fra solen til avstand fra sentrum i vinduet
			//regne ut pixel per Megameter (ppM)

			double PixelsPerMegameter(List<SpaceObject> spaceObjects, double screenWidth, double screenHeight)
            {
				double maxRadius = 0;
				spaceObjects.ForEach(so =>
				{
					if (so.OrbitalRadius > maxRadius)
					{
						maxRadius = so.OrbitalRadius;
					}
				});

				return (screenWidth > screenHeight ? screenWidth : screenHeight) / (2*maxRadius);
			}

			(double, double) Space2Canvas2ElectricBogaloo (
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

				return ((-spaceX + x) * pixelsPerMegameter + screenWidth/2, (-spaceY + y) * pixelsPerMegameter + screenHeight/2);
            }


            #endregion
            #region HandleEvents

            void HandleKeyPress(object sender, KeyboardEventArgs e)
			{
				if (e.KeyboardDevice.IsKeyDown(Key.Escape))
				{
					selectedObject = null;
					pixelsPerMegameter = PixelsPerMegameter(solarSystemData, MyCanvas.ActualWidth, MyCanvas.ActualHeight);
					InfoPanel.Visibility = Visibility.Hidden;
				}
				else if (e.KeyboardDevice.IsKeyDown(Key.Up))
                {
					zoomFactor += 0.10;
                }
				else if (e.KeyboardDevice.IsKeyDown(Key.Down))
				{
					
					zoomFactor -= 0.10;
					if (zoomFactor <= 0.0001)
						zoomFactor = 0.10;
				}
			}

			void OnSpaceObjectHover(object semder,MouseEventArgs e, SpaceObject so, Ellipse ellipse)
			{

			}

			void OnSpaceObjectClick(object sender, EventArgs e, SpaceObject so, double screenWidth, double screenHeight)
			{
				if (so.Children.Count < 1)
				{
					return;
				}
				selectedObject = so;
				pixelsPerMegameter = PixelsPerMegameter(so.Children, screenWidth, screenHeight);
				//TODO: add show and hide info panel
				InfoPanel.Visibility = Visibility.Visible;

				InfoPanelName.Content = selectedObject.Name;
				InfoPanelOrbitalRadius.Content = selectedObject.OrbitalRadius;
				InfoPanelOrbitalPeriod.Content = selectedObject.OrbitalPeriod;
				InfoPanelRotationalPeriod.Content = selectedObject.RotationalPeriod;
				InfoPanelDadBod.Content = selectedObject.DadBod.Name;

			}

			void TimeScaleSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
			{
				timeScale = e.NewValue;
			}
			#endregion

		}

		private void ShowPlanetLabels_Checked(object sender, RoutedEventArgs e)
		{
			spaceObjectHolders?.Where(soh => soh.spaceObjectData.GetType() == typeof(Planet)
			|| soh.spaceObjectData.GetType() == typeof(Star)).ToList()
				.ForEach(soh => soh.ToggleLabel(true));
		}

		private void ShowPlanetLabels_Unchecked(object sender, RoutedEventArgs e)
		{
			spaceObjectHolders?.Where(soh => soh.spaceObjectData.GetType() == typeof(Planet) 
			|| soh.spaceObjectData.GetType()== typeof(Star)).ToList()
				.ForEach(soh => soh.ToggleLabel(false));
		}

		private void ShowMoonLabels_Checked(object sender, RoutedEventArgs e)
		{
			spaceObjectHolders?.Where(soh => soh.spaceObjectData.GetType() == typeof(Moon)).ToList()
				.ForEach(soh => soh.ToggleLabel(true));
		}

		private void ShowMoonLabels_Unchecked(object sender, RoutedEventArgs e)
		{
			spaceObjectHolders?.Where(soh => soh.spaceObjectData.GetType() == typeof(Moon)).ToList()
				.ForEach(soh => soh.ToggleLabel(false));
		}

		private void ZoomScaleSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			zoomFactor = e.NewValue;
		}
	}
}
