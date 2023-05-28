// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

class Sample6_DefaultContractResolver
{
    public static void Run()
    {
        Book book = new Book
        {
            BookName = "The Gathering Storm",
            BookPrice = 16.19m,
            AuthorName = "Brandon Sanderson",
            AuthorAge = 34,
            AuthorCountry = "United States of America"
        };

        //利用自定义接口，筛选属性
        string startingWithA = JsonConvert.SerializeObject(book, Formatting.Indented,
            new JsonSerializerSettings { ContractResolver = new DynamicContractResolver('A') });

        // {
        //   "AuthorName": "Brandon Sanderson",
        //   "AuthorAge": 34,
        //   "AuthorCountry": "United States of America"
        // }

        string startingWithB = JsonConvert.SerializeObject(book, Formatting.Indented,
            new JsonSerializerSettings { ContractResolver = new DynamicContractResolver('B') });

        // {
        //   "BookName": "The Gathering Storm",
        //   "BookPrice": 16.19
        // }
    }

    public class DynamicContractResolver : DefaultContractResolver
    {
        private readonly char _startingWithChar;

        public DynamicContractResolver(char startingWithChar)
        {
            _startingWithChar = startingWithChar;
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            IList<JsonProperty> properties = base.CreateProperties(type, memberSerialization);

            // only serializer properties that start with the specified character
            properties =
                properties.Where(p => p.PropertyName.StartsWith(_startingWithChar.ToString())).ToList();

            return properties;
        }
    }

    public class Book
    {
        public string BookName { get; set; }
        public decimal BookPrice { get; set; }
        public string AuthorName { get; set; }
        public int AuthorAge { get; set; }
        public string AuthorCountry { get; set; }
    }
}