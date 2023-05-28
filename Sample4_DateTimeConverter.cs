// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

class Sample4_DateTimeConverter
{
    /// <summary>
    /// 应用 JsonConverter 示例
    /// </summary>
    public static void Run()
    {
        var json2 = "{'Name':'C# cooking','dateAdded':1538205295000,'lastModified':1538205295 ,'SaleTime':1538205295}";
        var bookmarks = JsonConvert.DeserializeObject<Test>(json2);
        //2018-09-29 15:14:55
    }

    public class Test
    {
        public string Name { get; set; }

        [JsonProperty(PropertyName = "dateAdded")]
        [Newtonsoft.Json.JsonConverter(typeof(MyDateTimeConverter))]
        public DateTime dateAdded;

        [Newtonsoft.Json.JsonConverter(typeof(MyDateTimeConverter))]
        public DateTime SaleTime;

        [JsonProperty(PropertyName = "lastModified")]
        [Newtonsoft.Json.JsonConverter(typeof(UnixDateTimeConverter))]//会差8个小时
        public DateTime lastModified;

        public Test(DateTime lastModified)
        {
            this.lastModified = lastModified.ToLocalTime();//当应用 UnixDateTimeConverter 反序列化后时间因为时区的原因少8个小时
        }
    }

    public class MyDateTimeConverter : Newtonsoft.Json.JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var t = (long)reader.Value;
            if (t.ToString().Length == 10)
            {
                return new DateTime(1970, 1, 1, 8, 0, 0, DateTimeKind.Utc).AddSeconds(t).ToLocalTime();
            }
            else
            {
                return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(t).ToLocalTime();
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {

        }
    }
}