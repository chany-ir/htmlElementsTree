using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Selector
    {
        private static htmlHelper tags = htmlHelper.SingleTon;
        public  string TagName { get; set; }
        public string Id { get; set; }
        public List<string> Classes { get; set; }

        public Selector Parent { get; set; }

        public Selector Child { get; set; }
       

        public Selector() { 
            Classes = new List<string>();
        }
        public static Selector Create(string s )
       {
            var rootSelector = new Selector();
            Selector currentSelector=null;
             var str = s.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < str.Length; i++)
            {
                var newSelector = new Selector();
                var ch = "";
                Console.WriteLine(str[i]);
                if (char.IsLetter(str[i][0]))
                {
                    if (str[i].Contains('#'))
                        ch = str[i].Substring(0, str[i].IndexOf('#'));
                    else
                        ch = str[i];
                    if(ch.Contains('.'))
                          ch = str[i].Substring(0, str[i].IndexOf('.'));
                        if(Regex.IsMatch(ch, "^[a-zA-Z]+$"))
                        {
                            if (tags.Tags.Contains(ch))
                                newSelector.TagName = ch;
                        }
                        str[i] = str[i].Substring(ch.Length);
                }
                if (str[i].StartsWith('#'))
                {
                    if (!str[i].Contains('.'))
                        newSelector.Id = str[i];
                    else
                    {
                        ch= str[i].Substring(1, str[i].IndexOf('.')-1);
                        newSelector.Id = ch;
                        str[i]= str[i].Substring(ch.Length+1);
                    }
                }
                if (str[i].StartsWith('.'))
                {
                    if (str[i].Contains('#'))
                        ch = str[i].Substring(0, str[i].IndexOf('#'));
                    else
                        ch = str[i];
                    var temp =ch.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string st in temp)
                    {
                        newSelector.Classes.Add(st);
                    }
                    ch = str[i].Substring(ch.Length);
                    if (ch.Contains('#'))
                        newSelector.Id = ch.Substring(1);
                    
                }            
                if (currentSelector == null)
                {
                    currentSelector = newSelector;
                    rootSelector = currentSelector;
                  
                }
                else
                {
                    newSelector.Parent = currentSelector;
                    currentSelector.Child = newSelector;
                    currentSelector = newSelector;
                }
            }
            return rootSelector;
        }
    }
}
