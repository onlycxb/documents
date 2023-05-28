// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

class LinqToJson_Sample_SelectToken
{
    public static void Run()
    {
        {
            JObject o = JObject.Parse(@"{
  'CPU': 'Intel',
  'Drives': [
    'DVD read/writer',
    '500 gigabyte hard drive'
  ]
}");

            string cpu = (string)o["CPU"];
            // Intel

            string firstDrive = (string)o["Drives"][0];
            // DVD read/writer

            IList<string> allDrives = o["Drives"].Select(t => (string)t).ToList();
            // DVD read/writer
            // 500 gigabyte hard drive
        }

        #region Loading JSON from a file

        { //

            using (StreamReader reader = File.OpenText(@"c:\person.json"))
            {
                JObject o = (JObject)JToken.ReadFrom(new JsonTextReader(reader));
                // do stuff
            }
        }

        #endregion Loading JSON from a file

        #region Getting values by Property Name or Collection Index

        {
            string json = @"{
  'channel': {
    'title': 'James Newton-King',
    'link': 'http://james.newtonking.com',
    'description': 'James Newton-King\'s blog.',
    'item': [
      {
        'title': 'Json.NET 1.3 + New license + Now on CodePlex',
        'description': 'Announcing the release of Json.NET 1.3, the MIT license and the source on CodePlex',
        'link': 'http://james.newtonking.com/projects/json-net.aspx',
        'categories': [
          'Json.NET',
          'CodePlex'
        ]
      },
      {
        'title': 'LINQ to JSON beta',
        'description': 'Announcing LINQ to JSON',
        'link': 'http://james.newtonking.com/projects/json-net.aspx',
        'categories': [
          'Json.NET',
          'LINQ'
        ]
      }
    ]
  }
}";

            JObject rss = JObject.Parse(json);

            string rssTitle = (string)rss["channel"]["title"];
            // James Newton-King

            string itemTitle = (string)rss["channel"]["item"][0]["title"];
            // Json.NET 1.3 + New license + Now on CodePlex

            JArray categories = (JArray)rss["channel"]["item"][0]["categories"];
            // ["Json.NET", "CodePlex"]

            IList<string> categoriesText = categories.Select(c => (string)c).ToList();
            // Json.NET
            // CodePlex
        }

        #endregion Getting values by Property Name or Collection Index

    }

    public static void RunSelectToken()
    {
        // Querying JSON with SelectToken
        JObject o = JObject.Parse(@"{
  'Stores': [
    'Lambton Quay',
    'Willis Street'
  ],
  'Manufacturers': [
    {
      'Name': 'Acme Co',
      'Products': [
        {
          'Name': 'Anvil',
          'Price': 50
        }
      ]
    },
    {
      'Name': 'Contoso',
      'Products': [
        {
          'Name': 'Elbow Grease',
          'Price': 99.95
        },
        {
          'Name': 'Headlight Fluid',
          'Price': 4
        }
      ]
    }
  ]
}");

        string name = (string)o.SelectToken("Manufacturers[0].Name");
        // Acme Co

        decimal productPrice = (decimal)o.SelectToken("Manufacturers[0].Products[0].Price");
        // 50

        string productName = (string)o.SelectToken("Manufacturers[1].Products[0].Name");
        // Elbow Grease

    }

    /// <summary>
    /// <see langword="JSONPATH" cref="https://goessner.net/articles/JsonPath/"/>
    /// </summary>
    public static void RunSelectTokenWithJsonPath()
    {
        JObject o = JObject.Parse(@"{
  'Stores': [
    'Lambton Quay',
    'Willis Street'
  ],
  'Manufacturers': [
    {
      'Name': 'Acme Co',
      'Products': [
        {
          'Name': 'Anvil',
          'Price': 50
        }
      ]
    },
    {
      'Name': 'Contoso',
      'Products': [
        {
          'Name': 'Elbow Grease',
          'Price': 99.95
        },
        {
          'Name': 'Headlight Fluid',
          'Price': 4
        }
      ]
    }
  ]
}");

        // manufacturer with the name 'Acme Co'
        JToken acme = o.SelectToken("$.Manufacturers[?(@.Name == 'Acme Co')]");

        Console.WriteLine(acme);
        // { "Name": "Acme Co", Products: [{ "Name": "Anvil", "Price": 50 }] }

        // name of all products priced 50 and above
        IEnumerable<JToken> pricyProducts = o.SelectTokens("$..Products[?(@.Price >= 50)].Name");

        foreach (JToken item in pricyProducts)
        {
            Console.WriteLine(item);
        }
        // Anvil
        // Elbow Grease
    }
}