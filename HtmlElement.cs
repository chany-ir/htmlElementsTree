using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConsoleApp1
{
    internal class HtmlElement

    {
        public string Id { get; set; }
        public string Name { get; set; }

        public List<String> Attributes { get; set; }

        public List<String> Classes { get; set; }
        public String InnerHtml { get; set; }
        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; set; }
        // Constructor to initialize the lists for attributes, classes, and children
        public HtmlElement() {
            Classes = new List<string>();
            Attributes = new List<string>();
            Children = new List<HtmlElement>();

        }
        //  Function to return the current element with all its children using yield return
        public IEnumerable<HtmlElement> Descendants()
        {
            HtmlElement current = null;
            var queue = new Queue<HtmlElement>();
            queue.Enqueue(this);
            while (queue.Count > 0) {
                current = queue.Dequeue();
                yield return current;
                foreach (var child in current.Children)
                {
                    queue.Enqueue(child);
                }
            }    
        }
        // Function to return all ancestor elements 
        public IEnumerable<HtmlElement> Ancestors()
        {
           
            while (this.Parent != null)
            {
                yield return this.Parent;

            }
        }
        // Method to search for elements that match a specific CSS selector
        // Uses recursion to explore descendants and match selectors
        public HashSet<HtmlElement> SearchBySelector(Selector selector)
        {
            var result = new HashSet<HtmlElement>();
            SearchBySelectorRecursive(this, selector, result);
            return result;
        }
        // Recursive helper method to search for elements matching the selector in descendants
        public void SearchBySelectorRecursive(HtmlElement htmlElement, Selector selector,HashSet<HtmlElement>result)
        {
            int count = 0;
            // Loop over all descendants of the current element
            foreach (HtmlElement descendant in htmlElement.Descendants())
                {
                // Check if the descendant matches the selector
                if (MatchesSelector(descendant, selector))
                    {
                    count = 0;
                    // If there is a child selector, keep searching through descendants of this element
                    if (selector.Child != null)
                            foreach (HtmlElement descendant1 in descendant.Descendants())
                            {
                            count++;
                            if (count > 1)
                                SearchBySelectorRecursive(descendant1, selector.Child, result);
                            }
                        else
                        // If no child selector, add this descendant to the result
                        result.Add(descendant);
                    }
                }
            }
        // Helper method to check if an element matches the provided selector
        private bool MatchesSelector(HtmlElement element, Selector selector)
        {
            bool matchesName = string.IsNullOrEmpty(selector.TagName) || element.Name == selector.TagName;

            
            bool matchesId = string.IsNullOrEmpty(selector.Id) || element.Id == selector.Id;

            
            bool matchesClasses = selector.Classes.Count == 0 || selector.Classes.All(c => element.Classes.Contains(c));

            return matchesName && matchesId && matchesClasses;
        }
    }
}
