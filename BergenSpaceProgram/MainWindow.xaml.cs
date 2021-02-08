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
			double timeScale = 1;
			double currentTime = 0;
			#region Initialize SpaceObjects and Canvas Elements
			InitializeComponent();



			List<SpaceObject> solarSystem = XMLReader.ParseXML();
			double pixelsPerMegameter = PixelsPerMegameter(solarSystem, MyCanvas.ActualWidth, MyCanvas.ActualHeight);
			double zoomFactor = 1.0;

			MyCanvas.SizeChanged += (object sender, SizeChangedEventArgs e) =>
			{
				if(selectedObject == null || selectedObject.Children.Count < 1)
					pixelsPerMegameter = PixelsPerMegameter(solarSystem, MyCanvas.ActualWidth, MyCanvas.ActualHeight);
				else
					pixelsPerMegameter = PixelsPerMegameter(selectedObject.Children, MyCanvas.ActualWidth, MyCanvas.ActualHeight);
			};


				

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

				MyLittleEllipse.MouseDown += (sender, e) => OnSpaceObjectClick(sender, e, so, MyCanvas.ActualWidth, MyCanvas.ActualHeight);
				MyLittleEllipse.MouseMove += (sender, e) => OnSpaceObjectHover(sender, e, so, MyLittleEllipse);

				
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
					DrawSystem(time, solarSystem[0]);
				} else
				{
					DrawSystem(time, selectedObject);
				}
			}
			void DrawSystem(double time, SpaceObject so)
			{
				(double centerX, double centerY) = so.CalculatePosition(time);

				for (int i = 0; i < solarSystem.Count; i++)
				{
					(double x, double y) = solarSystem[i].CalculatePosition(time);
					(double canvasX, double canvasY) = Space2Canvas2ElectricBogaloo(
						time, 
						pixelsPerMegameter*zoomFactor,
						x, y,
						MyCanvas.ActualWidth, MyCanvas.ActualHeight,
						centerX,
						centerY);
					Canvas.SetLeft(ellipses[i], canvasX - ellipses[i].ActualWidth / 2);
					Canvas.SetTop(ellipses[i], canvasY - ellipses[i].ActualHeight / 2);
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
					pixelsPerMegameter = PixelsPerMegameter(solarSystem, MyCanvas.ActualWidth, MyCanvas.ActualHeight);
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
			}

			void TimeScaleSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
			{
				timeScale = e.NewValue;
			}
			#endregion

		}

	}
}
