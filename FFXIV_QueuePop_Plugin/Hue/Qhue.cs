using FFXIV_QueuePop_Plugin.Logger;
using Q42.HueApi;
using Q42.HueApi.ColorConverters;
using Q42.HueApi.ColorConverters.Gamut;
using Q42.HueApi.ColorConverters.Original;
using Q42.HueApi.Interfaces;
using Q42.HueApi.Models.Bridge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FFXIV_QueuePop_Plugin.Hue
{
    internal class Qhue
    {
        private static Qhue instance = null;
        private ILocalHueClient client;
        private bool SetupStarted = false;
        System.Timers.Timer resetTimer = new System.Timers.Timer(10000);
        private List<Tuple<RGBColor, string>> lastColor = new List<Tuple<RGBColor, string>>();
        public string prefferedLight { get; set; }

        private Qhue()
        {
            resetTimer.AutoReset = false;
            resetTimer.Elapsed += ResetTimer_Elapsed;
        }

        private async void ResetTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            LightCommand command = new LightCommand();
            command.On = true;

            foreach (Tuple<RGBColor, string> light in lastColor)
            {
                command.TurnOn().SetColor(light.Item1);

                if (light.Item2 == "all")
                {
                    await client.SendCommandAsync(command);
                }
                else
                {
                    await client.SendCommandAsync(command, new List<string> { light.Item2 });
                }                    
            }
        }

        public static Qhue Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Qhue();
                }
                return instance;
            }
        }

        public async Task<bool> InitBridgeAsync(string bridgeId)
        {
            try
            {
                IBridgeLocator locator = new HttpBridgeLocator();
                IEnumerable<LocatedBridge> bridgeIPs = await locator.LocateBridgesAsync(TimeSpan.FromSeconds(5));

                client = new LocalHueClient(bridgeIPs.First<LocatedBridge>().IpAddress);
                client.Initialize(bridgeId);

                SetupStarted = true;

                Log.Write(LogType.Info, "Found Bridge");

            }
            catch (Exception ex)
            {
                Log.Write(LogType.Error, ex);
            }

            return SetupStarted;
        }

        public async Task<Dictionary<string, string>> getLightsAsync()
        {
            Dictionary<string, string> lights = new Dictionary<string, string>();

            IEnumerable<Light> hueLights = await client.GetLightsAsync();
            foreach (Light light in hueLights)
            {
                lights.Add(light.Name, light.Id);
            }
            lights.Add("All Lamps", "all");

            return lights;
        }


        public async Task<string> RegisterBridgeAsync()
        {
            if (SetupStarted == true)
            {
                DialogResult dialogResult = MessageBox.Show("Now Press the button on your Philips Hue bridge", "Philips Hue Register", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    //Make sure the user has pressed the button on the bridge before calling RegisterAsync
                    //It will throw an LinkButtonNotPressedException if the user did not press the button
                    string appKey = await client.RegisterAsync("FFXIV_QHue", "ACT");
                    //Save the app key for later use
                    SetupStarted = false;
                    return appKey;
                }
            }
            else
            {
                Log.Write(LogType.Warning, "Bridge setup not started");
            }

            return null;
        }

        public async void SetLight(List<String> lightList)
        {
            LightCommand command = new LightCommand();
            command.On = true;

            foreach (string lightId in lightList)
            {
                if (lightId == "all")
                {
                    IEnumerable<Light> allLights = await client.GetLightsAsync();
                    foreach (Light light in allLights)
                    {
                        lastColor.Add(new Tuple<RGBColor, string>(light.ToRGBColor(), light.Id));
                    }
                }
                else
                {
                    Light light = await client.GetLightAsync(lightId);
                    if (light != null)
                    {
                        lastColor.Add(new Tuple<RGBColor, string>(light.ToRGBColor(), lightId));
                    }
                }
            }

            //Turn the light on and set a Hex color for the command (see the section about Color Converters)
            command.TurnOn().SetColor(new RGBColor("fe0002"));
            command.Alert = Alert.Multiple;

            if (lightList.First() == "all")
            {
                await client.SendCommandAsync(command);
            }
            else
            {
                await client.SendCommandAsync(command, lightList);
            }

            resetTimer.Start();
        }        

        public void SetLight(String id)
        {
            List<string> selectedLights = new List<string> { id };
            SetLight(selectedLights);
        }

        public void SetLight()
        {
            SetLight(prefferedLight);
        }
    }
}
