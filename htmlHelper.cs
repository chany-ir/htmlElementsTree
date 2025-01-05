using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace ConsoleApp1
{
    internal class htmlHelper
    {
        // Singleton pattern: creates a single instance of htmlHelper to be reused throughout the application
        private readonly static htmlHelper _singleTon =new htmlHelper();
        public static htmlHelper SingleTon => _singleTon;

        public string[] Tags {  get; set; }
        public string[] VoidTags { get; set; }
        private htmlHelper()
        {
            // Deserialize the content of the "HtmlTags.json" file into an array of strings
            Tags = JsonSerializer.Deserialize<string []>(File.ReadAllText("jsonFile/HtmlTags.json"));
            // Deserialize the content of the "HtmlVoidTags.json" file into an array of strings
            VoidTags = JsonSerializer.Deserialize<string[]>(File.ReadAllText("jsonFile/HtmlVoidTags.json"));
        }
    }
}
