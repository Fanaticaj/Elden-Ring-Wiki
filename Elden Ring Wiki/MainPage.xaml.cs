using RestSharp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Text;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Elden_Ring_Wiki
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        public class Weapon
        {
            public string id { get; set; }
            public string name { get; set; }
            public string image { get; set; }
            public string description { get; set; }
            public List<Attribute> attack { get; set; }
            public List<Attribute> defence { get; set; }
            public List<Attribute> scalesWith { get; set; }
            public List<Attribute> requiredAttributes { get; set; }
        }

        public class Attribute
        {
            public string name { get; set; }
            public int amount { get; set; }
            public string scaling { get; set; } // only for scalesWith attribute
        }

        private async void WSearch_click(object sender, RoutedEventArgs e)
        {
            var Weapon_Search_Text = WSearch_text.Text;

            var client = new RestClient("https://eldenring.fanapis.com/api/weapons?name=" + Weapon_Search_Text);
            var request = new RestRequest();

            var response = await client.ExecuteAsync(request);
            if (response.IsSuccessful)
            {
                dynamic apiResponse = JObject.Parse(response.Content);

                if (apiResponse.data.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();

                    for (int i = 0; i < apiResponse.data.Count; i++)
                    {
                        string weaponName = apiResponse.data[i].name;
                        string weaponDescription = apiResponse.data[i].description;

                        sb.AppendLine($"Name: {weaponName}\nDescription: {weaponDescription}");

                        // Iterate through attack array
                        sb.AppendLine("Attack:");
                        foreach (var attack in apiResponse.data[i].attack)
                        {
                            sb.AppendLine($"{attack.name}: {attack.amount}");
                        }

                        // Iterate through defence array
                        sb.AppendLine("Defence:");
                        foreach (var defence in apiResponse.data[i].defence)
                        {
                            sb.AppendLine($"{defence.name}: {defence.amount}");
                        }

                        // Iterate through scalesWith array
                        sb.AppendLine("Scales With:");
                        foreach (var scale in apiResponse.data[i].scalesWith)
                        {
                            sb.AppendLine($"{scale.name}: {scale.scaling}");
                        }

                        // Iterate through requiredAttributes array
                        sb.AppendLine("Required Attributes:");
                        foreach (var attr in apiResponse.data[i].requiredAttributes)
                        {
                            sb.AppendLine($"{attr.name}: {attr.amount}");
                        }

                        sb.AppendLine(); // add an empty line for separation
                    }

                    WSearch_return.Text = sb.ToString();
                }
                else
                {
                    WSearch_return.Text = "No weapons found matching your search.";
                }
            }
            else
            {
                WSearch_return.Text = "Request failed. Please try again.";
            }
        }


        /*private async void WSearch_click(object sender, RoutedEventArgs e)
        {
            var Weapon_Search_Text = WSearch_text.Text;

            var client = new RestClient("https://eldenring.fanapis.com/api/weapons?&name=" + Weapon_Search_Text);
            var request = new RestRequest();

            var response = await client.ExecuteAsync(request);
            if (response.IsSuccessful)
            {
                dynamic apiResponse = JObject.Parse(response.Content);

                // Access the data you need from the apiResponse object. For example,
                string weaponName = "";
                string weaponDescription = "";
                for (int i = 0; i < (int)apiResponse.count; i++)
                {
                    weaponName += apiResponse.data[i].name + "\n";
                    weaponDescription += apiResponse.data[i].description + "\n";

                    // And then assign these to the TextBox
                }
                WSearch_return.Text = $"Name:\n{weaponName}\nDescription: {weaponDescription}";
            }
            else
            {
                WSearch_return.Text = "Request failed. Please try again.";
            }
        }
        */
    }
}
