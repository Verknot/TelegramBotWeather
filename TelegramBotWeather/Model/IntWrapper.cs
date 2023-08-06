using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Prototype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBotWeather.Model
{
        internal class IntWrapper
        {
            [BotActionArgument(0)]
            public int Value { get; set; }

            public IntWrapper() { }
            public IntWrapper(int value) => Value = value;
        }

}
