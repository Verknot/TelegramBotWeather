﻿using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Prototype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBotWeather.Model
{
 
        internal class GeoCoderInfo
        {
            [BotActionArgument(0)]
            public string Name { get; set; } = null!;
            [BotActionArgument(1)]
            public double Longitude { get; set; }
            [BotActionArgument(2)]
            public double Latitude { get; set; }

            public GeoCoderInfo() { }
            public GeoCoderInfo(string name, double longitude, double latitude)
            {
                Name = name;
                Longitude = longitude;
                Latitude = latitude;
            }

            public string GetDisplay() => $"Город: {Name} ({Latitude.ToString().Replace(',', '.')}, {Longitude.ToString().Replace(',', '.')})";
        }
}
