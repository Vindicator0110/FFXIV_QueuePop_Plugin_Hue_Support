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
        private int red = 0;
        private int green = 255;
        private int blue = 0;
        private int i = 0;

        private static Qhue instance = null;
        private ILocalHueClient client;
        private bool SetupStarted = false;
        System.Timers.Timer resetTimer = new System.Timers.Timer(45000); // 45 seconds queuetime
        System.Timers.Timer blinkTimer = new System.Timers.Timer(2500); // 1 sec
        private List<Tuple<RGBColor, string>> lastColor = new List<Tuple<RGBColor, string>>();
        public string prefferedLight { get; set; }

        private Qhue()
        {
            resetTimer.AutoReset = false;
            resetTimer.Elapsed += ResetTimer_Elapsed;

            blinkTimer.Elapsed += BlinkTimer_Elapsed;
        }

        private void BlinkTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            BlinkGradually();
        }        

        private void BlinkGradually()
        {
            if (i < 3)
            {
                green -= 25;
                red += 5;
            }
            else
            {
                green -= 10;
                red += 15;
            }

            RGBColor color = new RGBColor(red, green, blue);

            Alert blink = Alert.None;
            if ((i % 4) == 0)
            {
                blink = Alert.Multiple;
            }

            SetLight(color, lastColor, blink);
            
            // Increment
            i++;
        }

        private void ResetTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            ResetLights();
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
                if(!lights.ContainsKey(light.Name))
                {
                    lights.Add(light.Id, light.Name);
                }
            }

            if (!lights.ContainsKey("all"))
            {
                lights.Add("all", "All Lamps");
            }

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

        public async void BlinkLightRed(List<String> lightList)
        {
            if(!blinkTimer.Enabled)
            {
                red = 0;
                green = 255;
                blue = 0;
                i = 0;
                blinkTimer.Start();
                resetTimer.Start();

                foreach (string lightId in lightList)
                {
                    if (lightId == "all")
                    {
                        IEnumerable<Light> allLights = await client.GetLightsAsync();
                        foreach (Light light in allLights)
                        {
                            lastColor.Add(new Tuple<RGBColor, string>(light.ToRGBColor(), light.Id));
                        }
                        break;
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

                BlinkGradually();
            }
        }        

        public void BlinkLightRed(String id)
        {
            List<string> selectedLights = new List<string> { id };
            BlinkLightRed(selectedLights);
        }

        public void BlinkLightRed()
        {
            BlinkLightRed(prefferedLight);
        }

        public async void CancelBlink()
        {
            if(blinkTimer.Enabled)
            {
                blinkTimer.Stop();
                resetTimer.Stop();

                SetLight(new RGBColor("2403fc"), lastColor);
                await Task.Delay(3000);
                ResetLights();
            }
        }
        
        public async void ResetLights()
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

            blinkTimer.Stop();
            lastColor.Clear();
        }


        public void SetLight(RGBColor color, List<Tuple<RGBColor, string>> lightList, Alert blink = Alert.None)
        {
            List<String> ids = new List<string>();
            foreach (Tuple<RGBColor, string> lightInfo in lightList)
            {
                ids.Add(lightInfo.Item2);
            }

            SetLight(color, ids, blink);
        }

        public async void SetLight(RGBColor color, List<string> lightList, Alert blink = Alert.None)
        {
            LightCommand command = new LightCommand();
            command.On = true;
            //Turn the light on and set a Hex color for the command (see the section about Color Converters)
            command.TurnOn().SetColor(color);
            command.Alert = blink;

            if (lightList == null || lightList.First() == "all")
            {
                await client.SendCommandAsync(command);
            }
            else
            {
                await client.SendCommandAsync(command, lightList);
            }
        }

        public void SetLight(string hexCode, List<string> lightList, Alert blink = Alert.None)
        {
            SetLight(new RGBColor(hexCode), lightList, blink);
        }       
    }
}
