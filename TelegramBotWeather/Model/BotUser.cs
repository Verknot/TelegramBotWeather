using SKitLs.Bots.Telegram.Stateful.Model;
using SKitLs.Bots.Telegram.Stateful.Prototype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBotWeather.Model
{
    public class BotUser: IStatefulUser
    {
         internal List<GeoCoderInfo> Favs { get; } = new();

        public IUserState State { get; set; }
        public long TelegramId { get; set; }

        public BotUser(long telegramId)
        {
            State = new DefaultUserState();
            TelegramId = telegramId;
        }

        public void ResetState() => State = new DefaultUserState();
    }
}

