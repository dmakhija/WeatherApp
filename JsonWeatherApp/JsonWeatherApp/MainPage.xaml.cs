using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.Windows;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace JsonWeatherApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
            spWeatherInfo.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            ErrorDisplay.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private async void btnGetData_Click(object sender, RoutedEventArgs e)
        {
            //check if a country has been selected from the list
            if(country.SelectedIndex>-1)
            {
                ErrorDisplay.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                //check if the city name is entered
                if (txtCity.Text != null && txtCity.Text.Trim() != "")
                {
                    ErrorDisplay.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    //try to locate the city in selected country and display the weater forecast, else catch error
                    try
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            pbWeather.Visibility = Windows.UI.Xaml.Visibility.Visible;
                            //http://api.openweathermap.org/data/2.5/forecast/daily?q=Waterloo,CA&mode=json&units=metric&cnt=7
                            //http://api.openweathermap.org/data/2.5/weather?q=Waterloo,ca

                            client.BaseAddress = new Uri("http://api.openweathermap.org");

                            var url = "data/2.5/forecast/daily?q={0}&mode=json&units=metric&cnt=7";

                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                            var countryname = country.SelectedItem.ToString().Trim();
                            var cityname = txtCity.Text.Trim();
                            var location = cityname + "," + countryname;

                            HttpResponseMessage response = await client.GetAsync(String.Format(url, location));

                            if (response.IsSuccessStatusCode)
                            {
                                var data = response.Content.ReadAsStringAsync();
                                var weatherdata = JsonConvert.DeserializeObject<WeatherObject>(data.Result.ToString());                               
                                
                                spWeatherInfo.DataContext = weatherdata;
                                spWeatherInfo.Visibility = Windows.UI.Xaml.Visibility.Visible;

                            }

                            pbWeather.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                            //ErrorDisplay.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

                        }
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show("Some Error Occured");
                        ErrorDisplay.Text = "Oops! Some Error Occured.";
                        ErrorDisplay.Visibility=Windows.UI.Xaml.Visibility.Visible;
                        pbWeather.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                        spWeatherInfo.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    }
                }
                else
                {
                    //show error message to select country first
                    ErrorDisplay.Text = "Please enter a city.";
                    ErrorDisplay.Visibility = Windows.UI.Xaml.Visibility.Visible;
                }
            }
            else
            {
                //show error message to select country first
                ErrorDisplay.Text = "Please select a country.";
                ErrorDisplay.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }

        }


        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        private void country_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // ... Get the ComboBox.
            var comboBox = sender as ComboBox;

            // ... Set SelectedItem as Window Title.
            string country = comboBox.SelectedItem as string;
            ErrorDisplay.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

       
    }
}
