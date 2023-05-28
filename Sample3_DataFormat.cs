// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

class Sample3_DataFormat
{
    public class LogEntry
    {
        public string Details { get; set; }

        [Newtonsoft.Json.JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime LogDate { get; set; }
    }

    public static void Run()
    {
        LogEntry entry = new LogEntry
        {
            LogDate = new DateTime(2009, 2, 15, 0, 0, 0, DateTimeKind.Utc),
            Details = "Application started."
        };

        // default as of Json.NET 4.5
        string isoJson = JsonConvert.SerializeObject(entry);
        // {"Details":"Application started.","LogDate":"2009-02-15T00:00:00Z"}

        JsonSerializerSettings microsoftDateFormatSettings = new JsonSerializerSettings
        {
            DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
        };
        string microsoftJson = JsonConvert.SerializeObject(entry, microsoftDateFormatSettings);
        // {"Details":"Application started.","LogDate":"\/Date(1234656000000)\/"}

        string javascriptJson = JsonConvert.SerializeObject(entry, new JavaScriptDateTimeConverter());
        // {"Details":"Application started.","LogDate":new Date(1234656000000)}

        //var entry2 = JsonConvert.DeserializeObject<LogEntry>(javascriptJson);//异溃
        var entry2 = JsonConvert.DeserializeObject<LogEntry>(javascriptJson, new JavaScriptDateTimeConverter());//正确的方式
        var entry3 = JsonConvert.DeserializeObject<LogEntry>(microsoftJson);//正确的方式

    }

    public static void Run2()
    {
        string json = @"[
  '7 December, 2009',
  '1 January, 2010',
  '10 February, 2010'
]";

        IList<DateTime> dateList = JsonConvert.DeserializeObject<IList<DateTime>>(json, new JsonSerializerSettings
        {
            DateFormatString = "d MMMM, yyyy"
        });

        foreach (DateTime dateTime in dateList)
        {
            Console.WriteLine(dateTime.ToLongDateString());
        }
        // Monday, 07 December 2009
        // Friday, 01 January 2010
        // Wednesday, 10 February 2010
    }
}