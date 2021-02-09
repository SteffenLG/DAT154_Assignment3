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
				if (selectedObject == null || selectedObject.Children.Count < 1)
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
				currentSpaceObjectHolder.Ellipse.MouseEnter += (sender, e) => OnSpaceObjectHover(sender, e, currentSpaceObjectHolder, true);
				currentSpaceObjectHolder.Ellipse.MouseLeave += (sender, e) => OnSpaceObjectHover(sender, e, currentSpaceObjectHolder, false);
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
				}
				else
				{
					DrawSystem(time, solarSystemData[0]);
				}
			}
			void DrawSystem(double time, SpaceObject so)
			{
				(double centerX, double centerY) = so.CalculatePosition(time);

				foreach (SpaceObjectHolder item in spaceObjectHolders)
				{
					item.UpdatePosition(time,pixelsPerMegameter * zoomFactor,
						MyCanvas.ActualWidth, MyCanvas.ActualHeight, 
						centerX, centerY);
				}
				
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

				return (screenWidth > screenHeight ? screenWidth : screenHeight) / (2 * maxRadius);
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

			void OnSpaceObjectHover(object sender, MouseEventArgs e, SpaceObjectHolder soh, bool entering)
			{
				
				if (entering)
				{
					if (soh.showingLabel) return;
					soh.ToggleLabel(true);

					
				}
				else //leaving
				{
					if (!soh.showingLabel) return;

					switch (soh.spaceObjectData)
					{
						case Moon m:
							soh.ToggleLabel(ShowMoonLabels.IsChecked ?? false);
							break;
						case Star star:
							soh.ToggleLabel(ShowPlanetLabels.IsChecked ?? false);
							break;
						case Planet planet:
							soh.ToggleLabel(ShowPlanetLabels.IsChecked ?? false);
							break;
						default:
							break;
					}

				}

				
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
				InfoPanelDadBod.Content = (selectedObject.DadBod != null) ? selectedObject.DadBod.Name : "";

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
			|| soh.spaceObjectData.GetType() == typeof(Star)).ToList()
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
