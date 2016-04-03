using System;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace AirController
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private bool isWorking = false;
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            test();

            try
            {
                var mode = await DataManager.GetCurrentMode();
                var modeByte = Convert.ToByte(mode);

                if (modeByte == 0)
                {
                    isWorking = false;
                    CurrentMode.Text = "Current Mode: Off";
                }
                else if (modeByte == 1)
                {
                    isWorking = true;
                    CurrentMode.Text = "Current Mode: Manual";
                }
                else if (modeByte == 2)
                {
                    isWorking = true;
                    CurrentMode.Text = "Current Mode: Auto";
                }

                buttonManual.Content = isWorking ? "Turn Off" : "Turn On";
            }
            catch
            {

            }
            
        }


        private async Task test()
        {
            while (true)
            {
                try
                {
                    var value = await DataManager.GetCO2LastValue();
                    var newValue = Convert.ToInt32(value);

                    var currentValue = Convert.ToInt32(Percent.Text.Replace("%", ""));
                    if (newValue == 0)
                    {
                        newValue = currentValue;
                    }
                    var difference = newValue - currentValue;

                    var differenceAbs = Math.Abs(difference);

                    for (var i = 0; i <= differenceAbs; i++)
                    {
                        await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            if (difference < 0)
                            {
                                Percent.Text = (currentValue--).ToString() + "%";
                            }
                            else if (difference > 0)
                            {
                                Percent.Text = (currentValue++).ToString() + "%";
                            }

                            Color color = new Color();

                            color.A = 255;
                            color.B = 0;
                            if (currentValue < 70)
                            {
                                color.G = Convert.ToByte(255 - (currentValue * 3));
                            }
                            else
                            {
                                color.G = 0;
                            }

                            if (currentValue < 50)
                            {
                                color.R = Convert.ToByte(currentValue * 5);
                            }
                            else
                            {
                                color.R = 255;
                            }

                            Percent.Foreground = new SolidColorBrush(color);
                            Tlen.Opacity = Convert.ToDouble(currentValue) / 70;
                        });
                        await Task.Delay(100 + (i*5));
                    }

                    await Task.Delay(TimeSpan.FromSeconds(2));
                }
                catch { }   
            }
        }

        private void buttonManualControl_Click(object sender, RoutedEventArgs e)
        {
            isWorking = !isWorking;
            var mode = Convert.ToByte(isWorking);

            buttonManual.Content = isWorking ? "Turn Off" : "Turn On";
            CurrentMode.Text = isWorking ? "Current Mode: Manual" : "Current Mode: Off";

            DataManager.SetStatus(mode);
        }

        private void buttonAuto_Click(object sender, RoutedEventArgs e)
        {
            buttonManual.Content = "Turn Off";
            isWorking = true;
            CurrentMode.Text = "Current Mode: Auto";

            DataManager.SetStatus(2);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(BlankPage1));
        }
    }
}
