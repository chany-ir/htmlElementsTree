// See https://aka.ms/new-console-template for more information
using ConsoleApp1;
using System;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
// https://hebrewbooks.org/
https://netfree.link/shortcuts/
var html1 = await CreateHtmlElementTree("https://netfree.link/shortcuts/");
PrintHtmlElement(html1, 0);
var s = Selector.Create("div div.infont div a");
var temp1 = html1.SearchBySelector(s);
//Function to print details of an HTML element from the hashSet
static void PrintHtmlElementFromHashSet(HashSet<HtmlElement> element)
{
    var count = 0;
    // For each element in the HashSet
    foreach (var e in element) { 
    count++;
    // Prints the element name, ID, class (if any), and a snippet of InnerHtml (if any)
    Console.WriteLine($" {count} <,{e.Name}, Id: {e.Id}, Classes: {(e.Classes.Any() ? e.Classes.First() : "None")}" +
        $",InnerHtml: {(string.IsNullOrEmpty(e.InnerHtml) ? "" : (e.InnerHtml.Length > 15 ? e.InnerHtml.Substring(0, 15) : e.InnerHtml))}>");
}
}
// Function to print details of an HTML element with all his children
static void PrintHtmlElement(HtmlElement element, int indent)
{
    // Create indentation (spaces) based on the element's depth level
    string indentSpaces = new string(' ', indent);
    // Prints the element's name, ID, class, and a snippet of InnerHtml
    Console.WriteLine($"{indentSpaces}Element: {element.Name}, Id: {element.Id}, Classes: {(element.Classes.Any() ? element.Classes.First() : "None")}" +
        $",InnerHtml: {(string.IsNullOrEmpty(element.InnerHtml) ? "" : (element.InnerHtml.Length > 15 ? element.InnerHtml.Substring(0, 15) : element.InnerHtml))}");
    // For each child element of the current element, call the function to print its details
    foreach (var child in element.Children)
    {
        // Increase depth by 2
        PrintHtmlElement(child, indent + 2);
    }
}
//Function to create a tree of objects of type HtmlElement 
async Task<HtmlElement> CreateHtmlElementTree(string url)
{
    //Creating a SingleTon that contains all the html tags
    htmlHelper h = htmlHelper.SingleTon;
    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    // Get the HTML content of the page as a string
    var html = await response.Content.ReadAsStringAsync();
    // Print the raw HTML content for debugging
    Console.WriteLine(html);
    // Clean up the HTML by replacing multiple spaces with a single space
    var cleanhtml = new Regex(@"(?<=\S)\s+(?=\S)").Replace(html, " ");
    // Split the cleaned HTML into individual tags
    var htmllines = new Regex("<(.*?)>").Split(cleanhtml).Where(s => s.Length > 0);
    // Create a root HTML element to hold the page structure
    HtmlElement root = new HtmlElement();
    HtmlElement currentElement = root;
    // Loop through each line in the HTML
    foreach (var line in htmllines)
    {
        // If the line is not empty
        if (!string.IsNullOrWhiteSpace(line))
        {
            // Extract the first tag from the line
            var tagParts = new Regex(@"^/?\w+").Match(line).Value;
            var remainingParts = line.Substring(tagParts.Length).Trim();
            // If we encounter the closing </html> tag, we finish the parsing
            if (tagParts == "/html")
            {
                currentElement = currentElement.Parent;
                PrintHtmlElement(root, 0);
                return root;
            }
            // If it's a closing tag, we go back to the parent element
            if (tagParts.StartsWith("/"))
            {
                currentElement = currentElement.Parent;
                continue;
            }
            // If it's a valid tag (one of the known tags)
            if (h.Tags.Contains(tagParts))
            {
                // Create a new HTML element
                HtmlElement newElement = new HtmlElement
                {
                    Name = tagParts,
                    Parent = currentElement
                };
                // Search for attributes (e.g., id, class) in the tag
                var attributes = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(remainingParts);
                for (int i = 0; i < attributes.Count(); i++)
                {
                    if (attributes[i].Value.StartsWith("id"))
                        newElement.Id = attributes[i].Value.Substring(3);
                    else if (attributes[i].Value.StartsWith("class"))
                    {
                        var c = attributes[i].Value.Substring(7).Trim('"').TrimEnd('\\');
                        newElement.Classes.AddRange(c.Split(' '));
                    }
                    else
                    {
                        newElement.Attributes.Add(attributes[i].Value);
                    }
                }
                // If the element is <html>, update the root and currentElement
                if (newElement.Name.Equals("html"))
                {
                    root = newElement;
                    currentElement = newElement;
                }
                else
                {
                    currentElement.Children.Add(newElement);
                    currentElement = newElement;
                }
                // If it's a VOID tag (e.g., <img>) or if the tag is self-closing, return to the parent
                if (h.VoidTags.Contains(tagParts) || remainingParts.EndsWith("/"))
                {
                    currentElement = currentElement.Parent;
                }
            }
            else
            {
                // If it's content (not a tag), assign it to the current element's InnerHtml
                if (currentElement!=null&&currentElement.Name != null)
                    currentElement.InnerHtml = line;
            }
        }
    }

    // Return the root element after finishing parsing the page
    return root;
}
PrintHtmlElementFromHashSet(temp1);
