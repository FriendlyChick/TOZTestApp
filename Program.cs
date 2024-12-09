using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
namespace TOZTestApp
{
    class Program
    {
        const string htmlLinkToNearestMatches = @"https://tula.nhliga.org/calendar?division=0";
        const string htmlLinkToResults = @"https://tula.nhliga.org/calendar/results?division=0";
        const int UpdateTime = 10000; //Тут стоит час 3600000
        private static Message message; //Нужно что бы получить id чата кудаписать
        private static TelegramBotClient botClient;
        
        static async Task Main(string[] args)
        {
            HtmlParser htmlParser = new HtmlParser();
            TimerCallback tm = new TimerCallback(ParsePage);
            Timer timer = new Timer(tm, htmlParser, 15, UpdateTime);
            //Ниже бот
            using var cts = new CancellationTokenSource();
            var bot = new TelegramBotClient("8170346717:AAGuUli9FlzvpPdEc6AoWTxaoprlwX-ERZk", cancellationToken: cts.Token);
            botClient = bot;
            var me = await bot.GetMe();

            await bot.DeleteWebhook();          
            await bot.DropPendingUpdates(); 
            bot.OnError += OnError;
            bot.OnMessage += OnMessage;
            bot.OnUpdate += OnUpdate;

            Console.WriteLine($"@{me.Username} is running... Press Escape to terminate");

            while (Console.ReadKey(true).Key != ConsoleKey.Escape) ;
            cts.Cancel(); 

            async Task OnError(Exception exception, HandleErrorSource source)
            {
                Console.WriteLine(exception);
                await Task.Delay(2000, cts.Token);
            }

            async Task OnMessage(Message msg, UpdateType type)
            {
                if (msg.Text is not { } text)
                    Console.WriteLine($"Received a message of type {msg.Type}");
                else if (text.StartsWith('/'))
                {
                    var space = text.IndexOf(' ');
                    if (space < 0) space = text.Length;
                    var command = text[..space].ToLower();
                    if (command.LastIndexOf('@') is > 0 and int at)
                        if (command[(at + 1)..].Equals(me.Username, StringComparison.OrdinalIgnoreCase))
                            command = command[..at];
                        else
                            return; 
                    await OnCommand(command, text[space..].TrimStart(), msg);
                }
                else
                    await OnTextMessage(msg);
            }

            async Task OnTextMessage(Message msg)
            {

            }

            async Task OnCommand(string command, string args, Message msg)
            {
                switch (command)
                {
                    case "/start":
                        if(message == null)
                        {
                            message = msg;
                            await botClient.SendMessage(message.Chat, $"Парсинг обновлён",
                             parseMode: ParseMode.Html, linkPreviewOptions: true,
                                replyMarkup: new ReplyKeyboardRemove());                            
                        }
                        break;
                }
            }
            async Task OnUpdate(Update update)
            {
                switch (update)
                {
                    case { CallbackQuery: { } callbackQuery }: await OnCallbackQuery(callbackQuery); break;
                    case { PollAnswer: { } pollAnswer }: await OnPollAnswer(pollAnswer); break;
                    default: Console.WriteLine($"Received unhandled update {update.Type}"); break;
                };
            }

            async Task OnCallbackQuery(CallbackQuery callbackQuery)
            {
            }

            async Task OnPollAnswer(PollAnswer pollAnswer)
            {
            }
        }
        async public static void SendAlert(int status, MatchCalendarItemSQLite match, MatchCalendarItem lastMatch = null)
        {
            /* 0 - Через неделю
             * 1 - Через день
             * 2 - Сыграли
             */
            switch (status)
            {
                case 0:
                    await Task.Run(async () =>
                    {
                        if (message != null)
                            await botClient.SendMessage(message.Chat, $"Через неделю играют\r\n{match.TeamName} - {match.TeamName1}\r\nЛига:{match.MatchType}\r\nДата и время:\r\n{match.MatchDate}\r\nМесто проведения : {match.MatchPlace}",
                                     parseMode: ParseMode.Html, linkPreviewOptions: true,
                                        replyMarkup: new ReplyKeyboardRemove());
                    });
                    break;
                case 1:
                    await Task.Run(async () =>
                    {
                        if (message != null)
                            await botClient.SendMessage(message.Chat, $"Через день играют\r\n{match.TeamName} - {match.TeamName1}\r\nЛига:{match.MatchType}\r\nДата и время:\r\n{match.MatchDate}\r\nМесто проведения : {match.MatchPlace}",
                                     parseMode: ParseMode.Html, linkPreviewOptions: true,
                                        replyMarkup: new ReplyKeyboardRemove());
                    });
                    break;
                case 2:
                    await Task.Run(async () =>
                    {
                        if (message != null)
                            await botClient.SendMessage(message.Chat, $"Сыграли\r\n{lastMatch.TeamName} - {lastMatch.TeamName1}\r\nЛига:{lastMatch.MatchType}\r\nДата и время:\r\n{lastMatch.MatchDate}\r\nМесто проведения : {lastMatch.MatchPlace}\r\nРезультат:\r\n{lastMatch.TeamScore} - {lastMatch.TeamScore1}\r\nПодробнее:{lastMatch.HrefToResult}",
                                     parseMode: ParseMode.Html, linkPreviewOptions: true,
                                        replyMarkup: new ReplyKeyboardRemove());
                    });
                    break;
            }
        }
        public static  bool isCanSendAlert()
        {
            if (message != null)
            {
                return true;
            }
            return false;
        }
        public static void ParsePage(object obj)
        {
            HtmlParser htmlParser = (HtmlParser)obj;
            htmlParser.CheckMatches(htmlLinkToNearestMatches);
            htmlParser.CheckResults(htmlLinkToResults);
        }
    }
}
