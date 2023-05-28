// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;

class Sample5_Setting
{

    /// <summary>
    /// 空值处理示例
    /// </summary>
    public static void Run()
    {
        Movie movie = new Movie();
        movie.Name = "Bad Boys III";
        movie.Description = "It's no Bad Boys";

        string included = JsonConvert.SerializeObject(movie,
            Formatting.Indented,
            new JsonSerializerSettings { });
        var move = """
            {
               "Name": "Bad Boys III",
              "Description": "It's no Bad Boys",
              "Classification": null,
              "Studio": null,
              "ReleaseDate": null,
              "ReleaseCountries": null
            }
            """;

        string ignored = JsonConvert.SerializeObject(movie, Formatting.Indented,
                                    new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

        // {
        //   "Name": "Bad Boys III",
        //   "Description": "It's no Bad Boys"
        // }
        Console.WriteLine(ignored);
    }

    public class Movie
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Classification { get; set; }
        public string Studio { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public List<string> ReleaseCountries { get; set; }
    }
}