﻿using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Model;
using SKitLs.Bots.Telegram.Core.Prototype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBotWeather.Model
{
    public class UserManager : IUsersManager
    {

        public UserDataChanged? SignedEventHandled => null;

        public List<BotUser> Users { get; } = new();

        public UserDataChanged? SignedUpdateHandled => null;


        public async Task<IBotUser?> GetUserByIdAsync(long telegramId) => await Task.FromResult(Users.Find(x => x.TelegramId == telegramId));


        public async Task<bool> IsUserRegisteredAsync(long telegramId) => await GetUserByIdAsync(telegramId) is not null;

        public async Task<IBotUser?> RegisterNewUserAsync(ICastedUpdate update)
        {
            var user = ChatScanner.GetSender(update.OriginalSource, this)!;
            var @new = new BotUser(user.Id);
            Users.Add(@new);
            return await Task.FromResult(@new);
        }

    }
}
