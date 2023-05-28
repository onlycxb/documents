// See https://aka.ms/new-console-template for more information
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

class Sample1_DeserializeObjec
{
    public static void Run()
    {
        string json = @"{ 'DisplayName': 'John Smith',  'SAMAccountName': 'contoso\\johns'}";

        DirectoryAccount account = JsonConvert.DeserializeObject<DirectoryAccount>(json);

        Console.WriteLine(account.DisplayName);
        // John Smith

        Console.WriteLine(account.Domain);
        // contoso

        Console.WriteLine(account.UserName);
        // johns
    }

    public class DirectoryAccount
    {
        // normal deserialization
        public string DisplayName { get; set; }

        // these properties are set in OnDeserialized
        public string UserName { get; set; }

        public string Domain { get; set; }

        [Newtonsoft.Json.JsonExtensionData]
        private IDictionary<string, JToken> _additionalData;

        //标记该方法在反序列化期间被调用
        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            // SAMAccountName is not deserialized to any property
            // and so it is added to the extension data dictionary
            string samAccountName = (string)_additionalData["SAMAccountName"];

            Domain = samAccountName.Split('\\')[0];
            UserName = samAccountName.Split('\\')[1];
        }

        public DirectoryAccount()
        {
            _additionalData = new Dictionary<string, JToken>();
        }
    }
}