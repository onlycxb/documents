// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

/// <summary>
/// 关于属性标记
/// </summary>
class Sample2_Property
{
    public static void Run()
    {
        string json = @"{ 'Name': 'John Smith', 'BirthDate': '2000-12-15T22:11:03','Department':'HR'}";
        var person = JsonConvert.DeserializeObject<Person>(json);
        Console.WriteLine("Name=" + person.Name);
        Console.WriteLine("BirthDate=" + person.BirthDate);
        Console.WriteLine("Department=" + person.Department);
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class Person
    {
        // "John Smith"
        [JsonProperty]
        public string Name { get; set; }

        // "2000-12-15T22:11:03"
        [JsonProperty]
        public DateTime BirthDate { get; set; }

        // not serialized because mode is opt-in
        public string Department { get; set; }
    }
}