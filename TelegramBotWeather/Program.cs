using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SKitLs.Bots.Telegram.AdvancedMessages.AdvancedDelivery;
using SKitLs.Bots.Telegram.AdvancedMessages.Model;
using SKitLs.Bots.Telegram.AdvancedMessages.Model.Menus;
using SKitLs.Bots.Telegram.AdvancedMessages.Model.Messages;
using SKitLs.Bots.Telegram.AdvancedMessages.Model.Messages.Text;
using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.ArgedInteractions;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation;
using SKitLs.Bots.Telegram.ArgedInteractions.Interactions.Model;
using SKitLs.Bots.Telegram.Core;
using SKitLs.Bots.Telegram.Core.Model.Building;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.Interactions.Defaults;
using SKitLs.Bots.Telegram.Core.Model.Management.Defaults;
using SKitLs.Bots.Telegram.Core.Model.UpdateHandlers.Defaults;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.PageNavs;
using SKitLs.Bots.Telegram.PageNavs.Model;
using SKitLs.Bots.Telegram.Stateful.Model;
using SKitLs.Bots.Telegram.Stateful.Prototype;
using SKitLs.Utils.Localizations.Prototype;
using System.Globalization;
using Telegram.Bot;
using TelegramBotWeather.Model;

namespace TelegramBotWeather
{

    internal class Program
    {

        private static readonly bool RequestApi = true;
        private static readonly string GeocoderApiKey = "7b43886b-f21b-4377-8462-2fdf487aba3c";
        private static readonly string WeatherApiKey = "9ee2120a-4078-4e31-b969-47f7ceb95721";

        private static readonly string BotApiKey = "2050743043:AAEj4RZetxQ3C97sTY1GVllbUOGKVdWFaew";

        public static DefaultUserState DefaultState = new(0, "default");
        public static DefaultUserState InputCityState = new(10, "typing");
        static async Task Main(string[] args)
        {
            BotBuilder.DebugSettings.DebugLanguage = LangKey.RU;
            BotBuilder.DebugSettings.UpdateLocalsPath("resources/locals");
            var privateMessages = new DefaultSignedMessageUpdateHandler();
            var privateTexts = new DefaultSignedMessageTextUpdateHandler
            {
                CommandsManager = new DefaultActionManager<SignedMessageTextUpdate>()
            };

            var mm = GetMenuManager();
            var privateCallbacks = new DefaultCallbackHandler()
            {
                CallbackManager = new DefaultActionManager<SignedCallbackUpdate>(),
            };

            privateCallbacks.CallbackManager.AddSafely(StartSearching);

            mm.ApplyTo(privateCallbacks.CallbackManager);

            privateTexts.CommandsManager.AddSafely(StartCommand);
            privateMessages.TextMessageUpdateHandler = privateTexts;

            ChatDesigner privates = ChatDesigner.NewDesigner()
               .UseUsersManager(new UserManager())
               .UseMessageHandler(privateMessages)
             .UseCallbackHandler(privateCallbacks);

            await BotBuilder.NewBuilder("2050743043:AAEj4RZetxQ3C97sTY1GVllbUOGKVdWFaew")
            .EnablePrivates(privates)
            .AddService<IArgsSerializeService>(new DefaultArgsSerializeService())
    .AddService<IMenuManager>(mm)
    .CustomDelivery(new AdvancedDeliverySystem())
    .Build()
    .Listen();
        }


        private static DefaultCommand StartCommand => new("start", Do_StartAsync);

        private static async Task Do_StartAsync(SignedMessageTextUpdate update)
        {
            if (update.Sender is IStatefulUser sender)
            {
                var mm = update.Owner.ResolveService<IMenuManager>();

                // Получаем определённую страницу по id
                // ...StaticPage( { это id -> } "main", "Главная"...
                var page = mm.GetDefined("main");

                await mm.PushPageAsync(page, update);

                //int st = (sender.State.StateId + 1) % 2;
                //var text = st == 0
                //    ? "Start command запущена из состояния 0"
                //    : "Абсолютно другой текст, в котором сказано, что состояния - 1";
                //await update.Owner.DeliveryService.ReplyToSender(text, update);
                //sender.State = new DefaultUserState(st);
            }
        }


        private static IMenuManager GetMenuManager()
        {
            var mm = new DefaultMenuManager();

            var mainBody = new OutputMessageText("Добро пожаловать!\n\nЧего желаете?");
            var mainMenu = new PageNavMenu();
            var mainPage = new StaticPage("main", "Главная", mainBody, mainMenu);

            var savedBody = new DynamicMessage(u =>
            {
                return new OutputMessageText("_Здесь будут отображаться избранные..._");
            });
            var savedPage = new WidgetPage("saved", "Избранное", savedBody);

            mainMenu.PathTo(savedPage);
            mainMenu.AddAction(StartSearching);

            mm.Define(mainPage);
            mm.Define(savedPage);

            return mm;
        }

        // Этот коллбэк будет вызывать поиск. Пока что прототип.
        private static DefaultCallback StartSearching => new("startSearch", "Найти", Do_SearchAsync);
        private static async Task Do_SearchAsync(SignedCallbackUpdate update) { }


        private static DefaultTextInput ExitInput => new("Выйти", Do_ExitInputCityAsync);
        private static async Task Do_ExitInputCityAsync(SignedMessageTextUpdate update)
        {
            // Просто меняем состояние пользователя на исходное
            if (update.Sender is IStatefulUser stateful)
            {
                stateful.State = DefaultState;

                var message = new OutputMessageText($"Вы больше ничего не вводите.")
                {
                    Menu = new ReplyCleaner(),
                };
                await update.Owner.DeliveryService.ReplyToSender(message, update);
            }
        }
    }


}