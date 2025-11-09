using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;


class TelegramBotLogic // This class is responsiable for logic
{
    private static TelegramBotHost hungarianStudingBot = new TelegramBotHost(""); // Create instance with token

    private static  List<long> chatIds = new List<long>(); // Thsi list is used to check if user's Teelegram chat ID is already added to database

    private static void Main() // Main method, where 
    {
        hungarianStudingBot.Start(); // Lanching bot
        DataBase dataBase = new DataBase(); // instance of DataBase class

        foreach (string stringChatId in dataBase.GetAllChatIDs()) // Get every Telegram chat ID
        {
            if (long.TryParse(stringChatId, out long chatId)) // Get long type from string type
            {
                chatIds.Add(chatId); // Add users' Tellegram chat IDs to the list
                Task.Run(() => CheckTimerLoop(hungarianStudingBot.telegramBotClient, chatId)); // Lanch async task for time loop
            }
        }

        hungarianStudingBot.OnMessage += OnMessage; // Method withc is used after getting a message

        Console.ReadLine(); // Finish working after pressing "Enter"
    }

    private static async void OnMessage(ITelegramBotClient client, Update update) // Method which is used to work with user's messages
    {
        long chatId = 0; // User's chat Id who sent a message
        if (update.Message != null) // If new update is a message 
        {
            chatId = update.Message.Chat.Id; // Take Telegram chat ID from the message
        }
        if (update.CallbackQuery?.Message != null) // If new update is a pressed button
        {
            chatId = update.CallbackQuery.Message.Chat.Id; // Take Telegram chat ID from the pressed button
        }

        if (chatId != 0) // If Telegram chat ID exist
        {
            DataBase dataBase = new DataBase(); // Instance of DataBase class
            APIrequest aPIrequest = new APIrequest(); // Instance of APIrequest class

            if (!chatIds.Contains(chatId)) // Chexk if user's Telegram chat ID is already added to database or not using that lsit
            {
                dataBase.AddNewChatID(chatId);
                _ = Task.Run(() => CheckTimerLoop(hungarianStudingBot.telegramBotClient, chatId)); // Lanch async task for time loop. _ is needed to show, that We don't have to wait for its finishing
            }
            if (dataBase.GetIsWaitingForMinutesMessage(chatId) && update.Message?.Text != null) // If bot is waiting user's message
            {
                if (int.TryParse(update.Message.Text, out int minutes)) // Try to convert user's message to int
                {
                    dataBase.SetTimeBetweenMessage(chatId, minutes);
                    dataBase.SetTargetTime(chatId, DateTime.Now.AddMinutes(minutes)); // Set time between sneding messages
                    dataBase.SetIsWaitingForMinutesMessage(chatId, false); // Now not ended waiting for user's message

                    await client.SendMessage(update.Message.Chat.Id, "Great! Messages will come once  " + dataBase.GetTimeBetweenMessage(chatId) + " mins."); // Send a message, that time is set
                }
                else // In case of exception
                {
                    await client.SendMessage(update.Message.Chat.Id, "Incorect input! Try again"); // Send a message, that input is incorect
                }
            }
            if (dataBase.GetIsWaitingForLanguageMessage(chatId) && update.Message?.Text != null) // If bot is waiting user's message
            {
                dataBase.SetIsWaitingForLanguageMessage(chatId, false); // Now not ended waiting for user's message
                await client.SendMessage(update.Message.Chat.Id, "Translated text: " + aPIrequest.SendRequestTranslation(update.Message.Text, aPIrequest.languageCode)); // Send translated text
            }
            if (update.Message?.Text == "/start") // If message = "/start"
            {
                dataBase.SetIsWaitingForMinutesMessage(chatId, false); // Now not ended waiting for user's message
                dataBase.SetIsWaitingForLanguageMessage(chatId, false); // Now not ended waiting for user's message

                await client.SendMessage(update.Message.Chat.Id, "Welcome! I was created to review Hungarian lessons already completed by my creator. Once every set time, a random rule or set of Hungarian words will appear. Errors are possible, because my creator is a CRAP!"); // Send the first message
            }
            if (update.Message?.Text == "/set_time") // If message = /set_time"
            {
                dataBase.SetIsWaitingForMinutesMessage(chatId, false); // Now not ended waiting for user's message
                dataBase.SetIsWaitingForLanguageMessage(chatId, false); // Now not ended waiting for user's message

                await client.SendMessage(update.Message.Chat.Id, "Great! Specify the number of minutes (an integer) you want to receive messages every. The default is 30 minutes."); // Ask user to set time

                dataBase.SetIsWaitingForMinutesMessage(chatId, true); // Bot started waiting for user's answer

            }
            if (update.Message?.Text == "/translate") // If message = "/translate"
            {
                dataBase.SetIsWaitingForMinutesMessage(chatId, false); // Now not ended waiting for user's message
                dataBase.SetIsWaitingForLanguageMessage(chatId, false); // Now not ended waiting for user's message

                var inLineKeyboard = new InlineKeyboardMarkup(new[] // New buttons under bot's message
                {
                new [] //   New buttons
                {
                    InlineKeyboardButton.WithCallbackData("Hungarian", "callbackHungarian"), // Button to translate into hungarian
                    InlineKeyboardButton.WithCallbackData("English", "callbackEnglish"), // Button to translate into english
                    InlineKeyboardButton.WithCallbackData("Russian", "callbackRussian") // Button to translate into russian
                }
            });
                await client.SendMessage(update.Message.Chat.Id, "Select language to translate into.", replyMarkup: inLineKeyboard); // Send a message  to ask user's send his text
            }
            if (update.Type == UpdateType.CallbackQuery) // If button was pressed
            {
                var callbackQuery = update.CallbackQuery; // Data of pressed button
                if (callbackQuery != null) // Data of pressed button exists
                {
                    await client.AnswerCallbackQuery(callbackQuery.Id); // Start working with this data

                    if (callbackQuery.Data == "callbackHungarian" || callbackQuery.Data == "callbackEnglish" || callbackQuery.Data == "callbackRussian") // If data of pressed button = "*"
                    {
                        dataBase.SetIsWaitingForLanguageMessage(chatId, true); // Now bot started waiting for user's message
                        aPIrequest.languageCode = callbackQuery.Data.Substring(8, 2).ToLower(); // Get language code from data of pressed button 
                        await client.SendMessage(chatId, "Good! Write your text!"); // Send a message to ask user to send his text
                    }
                }
            }
        }
    }

    private static async Task CheckTimerLoop(ITelegramBotClient client, long chatId) // Time loop
    {
        DataBase dataBase = new DataBase(); // Instance of DataBase class
        FileHolder fileHolder = new FileHolder(); // Instance of DataBase class

        dataBase.SetTargetTime(chatId, DateTime.Now.AddMinutes(dataBase.GetTimeBetweenMessage(chatId))); // Set traget time from database for user 
        while (true) // Loop
        {
            DateTime currentTime = DateTime.Now; // Get current time

            if (currentTime >= dataBase.GetTargetTime(chatId)) // If current time > or = target time
            {
                // "using" - is used to work with threads, it automaticly open and close it to avoid problems
                using (var thread = File.OpenRead(fileHolder.GetPictureFile())) // Creating new thread to open file
                {
                    var inputFile = new Telegram.Bot.Types.InputFileStream(thread, Path.GetFileName(fileHolder.GetPictureFile())); // Create thread to send a file in Telegram
                    await client.SendPhoto(chatId, inputFile); // Finish task after sending file (inputFile) to user (chatId)
                }
                dataBase.SetTargetTime(chatId, DateTime.Now.AddMinutes(dataBase.GetTimeBetweenMessage(chatId))); // Set traget time from database for user again
            }

            await Task.Delay(1000); // Delay to not have much preasure on CPU
        }
    }
}