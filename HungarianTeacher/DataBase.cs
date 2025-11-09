using Microsoft.Data.SqlClient;
using System.Configuration;


class DataBase // This class is responsiable for working with database
{
    public void AddNewChatID(long chatID) // Add new Telegram chat id 
    {
        string query = @" 
        IF NOT EXISTS (SELECT 1 FROM Users WHERE ChatID = @chatID)
        BEGIN
            INSERT INTO [Users] (ChatID) VALUES (@chatID)
        END"; // Make SQL request to datbase. @ - mark parameter place for fiuture setting of our value

        // using allows to close connection with data base automaticly and awoid exeptions
        using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TelegramBotDataBase"].ConnectionString))  // connect database using it's name to get connection string
        {
            connection.Open(); // Open connection with database
            using (SqlCommand command = new SqlCommand(query, connection)) // Create command. Using is used to control memory and connection
            {
                command.Parameters.AddWithValue("@chatID", chatID.ToString()); // Set our value on parameter place

                command.ExecuteNonQuery(); // Execute command and return only of affected rows
            }
        }
    }

    public List<string> GetAllChatIDs() // Get all users' Telegram chat IDs
    {
        var chatIDs = new List<string>(); // New list for users' Telegram chat IDs. Var is more convinient and it makes code easier (instead of List<string>)

        string query = @"SELECT ChatID FROM Users"; // Make SQL request to datbase. @ - mark parameter place for fiuture setting of our valuee

        // using allows to close connection with data base automaticly and awoid exeptions
        using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TelegramBotDataBase"].ConnectionString))  // connect database using it's name to get connection string
        {
            connection.Open(); // Open connection with database
            using (SqlCommand command = new SqlCommand(query, connection)) // Create command. Using is used to control memory and connection
            {
                using (SqlDataReader reader = command.ExecuteReader()) // Create command. Using is used to control memory and connection
                {
                    while (reader.Read()) // While reader can read. It is used to take all srings
                    {
                        if (!reader.IsDBNull(0))  // If captured result doesn't equels null
                        {
                            string chatID = reader.GetString(0); // Capture the first string with result in case it exists

                            chatIDs.Add(chatID);// Take value from captured string and add it to list
                        }
                    }
                }
            }
        }

        return chatIDs; // Return list
    }

    public void SetIsWaitingForLanguageMessage(long chatID, bool isWaitingForLanguageMessage) // Set value to check if Telegram bot is waitng for user's message to select language to translate into
    {
        string query = @"
        UPDATE Users
        SET IsWaitingForLanguageMessage = @isWaitingForLanguageMessage
        WHERE ChatID = @chatID"; // Make SQL request to datbase. @ - mark parameter place for fiuture setting of our valuee

        // using allows to close connection with data base automaticly and awoid exeptions
        using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TelegramBotDataBase"].ConnectionString))  // connect database using it's name to get connection string
        {
            connection.Open(); // Open connection with database
            using (SqlCommand command = new SqlCommand(query, connection)) // Create command. Using is used to control memory and connection
            {
                command.Parameters.AddWithValue("@chatID", chatID.ToString()); // Set our value on parameter place
                command.Parameters.AddWithValue("@isWaitingForLanguageMessage", isWaitingForLanguageMessage); // Set our value on parameter place

                command.ExecuteNonQuery(); // Execute command and return only of affected rows
            }
        }
    }

    public bool GetIsWaitingForLanguageMessage(long chatID) // Set value to check if Telegram bot is waitng for user's message to set time between messages
    {
        bool isWaitingForLanguageMessage = false;
        string query = "SELECT IsWaitingForLanguageMessage FROM Users WHERE ChatID = @chatID"; // Make SQL request to datbase. @ - mark parameter place for fiuture setting of our value

        // using allows to close connection with data base automaticly and awoid exeptions
        using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TelegramBotDataBase"].ConnectionString))  // connect database using it's name to get connection string
        {
            connection.Open(); // Open connection with database
            using (SqlCommand command = new SqlCommand(query, connection)) // Create command. Using is used to control memory and connection
            {
                command.Parameters.AddWithValue("@chatID", chatID.ToString()); // Set our value on parameter place

                using (SqlDataReader reader = command.ExecuteReader()) // Create command. Using is used to control memory and connection
                {
                    if (reader.Read()) // Capture the first string with result in case it exists
                    {
                        if (!reader.IsDBNull(0)) // If captured result doesn't equels null
                        {
                            isWaitingForLanguageMessage = reader.GetBoolean(0); // Take value from captured string
                        }
                    }
                }
            }
        }

        return isWaitingForLanguageMessage; // Return received value
    }

    public void SetIsWaitingForMinutesMessage(long chatID, bool isWaitingForMinutesMessage) // Set value to check if Telegram bot is waitng for user's message to set time between messages
    {
        string query = @"
        UPDATE Users
        SET IsWaitingForMinutesMessage = @isWaitingForMinutesMessage
        WHERE ChatID = @chatID"; // Make SQL request to datbase. @ - mark parameter place for fiuture setting of our value

        // using allows to close connection with data base automaticly and awoid exeptions
        using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TelegramBotDataBase"].ConnectionString))  // connect database using it's name to get connection string
        {
            connection.Open(); // Open connection with database
            using (SqlCommand command = new SqlCommand(query, connection)) // Create command. Using is used to control memory and connection
            {
                command.Parameters.AddWithValue("@chatID", chatID.ToString()); // Set our value on parameter place
                command.Parameters.AddWithValue("@isWaitingForMinutesMessage", isWaitingForMinutesMessage); // Set our value on parameter placee

                command.ExecuteNonQuery(); // Execute command and return only of affected rows
            }
        }
    }

    public bool GetIsWaitingForMinutesMessage(long chatID) // Set value to check if Telegram bot is waitng for user's message to set time between messages
    {
        bool isWaitingForMinutesMessage = false;
        string query = "SELECT IsWaitingForMinutesMessage FROM Users WHERE ChatID = @chatID"; // Make SQL request to datbase. @ - mark parameter place for fiuture setting of our value

        // using allows to close connection with data base automaticly and awoid exeptions
        using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TelegramBotDataBase"].ConnectionString))  // connect database using it's name to get connection string
        {
            connection.Open(); // Open connection with database
            using (SqlCommand command = new SqlCommand(query, connection)) // Create command. Using is used to control memory and connection
            {
                command.Parameters.AddWithValue("@chatID", chatID.ToString()); // Set our value on parameter place

                using (SqlDataReader reader = command.ExecuteReader()) // Start reading data from database
                {
                    if (reader.Read()) // Capture the first string with result in case it exists
                    {
                        if (!reader.IsDBNull(0)) // If captured result doesn't equels null
                        {
                            isWaitingForMinutesMessage = reader.GetBoolean(0); // Take value from captured string
                        }
                    }
                }
            }
        }

        return isWaitingForMinutesMessage; // Return received value
    }

    public void SetTimeBetweenMessage(long chatID, int minutes) // Set value to set time between sending messages
    {
        string query = @"
        UPDATE Users
        SET Minutes = @minutes
        WHERE ChatID = @chatID"; // Make SQL request to datbase. @ - mark parameter place for fiuture setting of our value

        // using allows to close connection with data base automaticly and awoid exeptions
        using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TelegramBotDataBase"].ConnectionString))  // connect database using it's name to get connection string
        {
            connection.Open(); // Open connection with database
            using (SqlCommand command = new SqlCommand(query, connection)) // Create command. Using is used to control memory and connection
            {
                command.Parameters.AddWithValue("@chatID", chatID.ToString()); // Set our value on parameter place
                command.Parameters.AddWithValue("@minutes", minutes); // Set our value on parameter place

                command.ExecuteNonQuery(); // Execute command and return only of affected rows
            }
        }
    }

    public int GetTimeBetweenMessage(long chatID) // Get value to set time between sending messages
    {
        int minutes = 30; // Set based value of varibable
        string query = "SELECT Minutes FROM Users WHERE ChatID = @chatID"; // Make SQL request to datbase. @ - mark parameter place for fiuture setting of our value

        // using allows to close connection with data base automaticly and awoid exeptions
        using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TelegramBotDataBase"].ConnectionString))  // connect database using it's name to get connection string
        {
            connection.Open(); // Open connection with database
            using (SqlCommand command = new SqlCommand(query, connection)) // Create command. Using is used to control memory and connection
            {
                command.Parameters.AddWithValue("@chatID", chatID.ToString()); // Set our value on parameter place

                using (SqlDataReader reader = command.ExecuteReader()) // Start reading data from database
                {
                    if (reader.Read()) // Capture the first string with result in case it exists
                    {
                        if (!reader.IsDBNull(0)) // If captured result doesn't equels null
                        {
                            minutes = reader.GetInt32(0); // Take value from captured string
                        }
                    }
                }
            }
        }

        return minutes; // Return received value
    }

    public void SetTargetTime(long chatID, DateTime targetTime) // Set target time to send message
    {
        string query = @"UPDATE users
                        SET TargetTime = @targetTime
                        WHERE ChatID = @chatID"; // Make SQL request to datbase. @ - mark parameter place for fiuture setting of our value

        // using allows to close connection with data base automaticly and awoid exeptions
        using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TelegramBotDataBase"].ConnectionString))  // connect database using it's name to get connection string
        {
            connection.Open(); // Open connection with database
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@chatID", chatID.ToString()); // Set our value on parameter place
                command.Parameters.AddWithValue("@targetTime", targetTime); // Set our value on parameter place

                command.ExecuteNonQuery(); // Execute command and return only of affected rows
            }
        }
    }

    public DateTime GetTargetTime(long chatID) // Get target time to send message
    {
        DateTime targetTime = DateTime.Now.AddMinutes(30);
        string query = @"SELECT TargetTime FROM Users WHERE ChatID = @chatID";

        // using allows to close connection with data base automaticly and awoid exeptions
        using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TelegramBotDataBase"].ConnectionString))  // connect database using it's name to get connection string
        {
            connection.Open(); // Open connection with database
            using (SqlCommand command = new SqlCommand(query, connection)) // Create command. Using is used to control memory and connection
            {
                command.Parameters.AddWithValue("@chatID", chatID.ToString()); // Set our value on parameter place

                using (SqlDataReader reader = command.ExecuteReader()) // Start reading data from database
                {
                    if (reader.Read()) // Capture the first result if it exists
                    {
                        if (!reader.IsDBNull(0)) // If captured result doesn't equels null
                        {
                            targetTime = reader.GetDateTime(0); // Take value from captured string
                        }
                    }
                }
            }
        }

        return targetTime; // Return received value
    }
}
