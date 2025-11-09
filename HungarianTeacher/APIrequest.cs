using Google.Cloud.Translation.V2; // Google APIS


class APIrequest // This class is responsiable for working with API
{
    private string _apiKey = ""; // Key to work with API
    public string apiKey
    {
        get
        {
            return _apiKey;
        }
    }

    private string _languageCode = "en";

    public string languageCode
    {
        get
        {
            return _languageCode;
        }
        set
        {
            _languageCode = value;
        }
    }
    public string SendRequestTranslation(string text, string languageCode) // Send api request to translate a text
    {
        TranslationClient client = TranslationClient.CreateFromApiKey(apiKey); // Create a client to work with API using key
        var response = client.TranslateText(text, languageCode); // Send request to translate a text

        return response.TranslatedText; // Return translated text
    }
}