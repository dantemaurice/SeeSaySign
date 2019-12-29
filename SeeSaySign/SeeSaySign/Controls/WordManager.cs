using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SeeSaySign.See;
using Xamarin.Forms;

namespace SeeSaySign.Controls
{
    public static class WordManager
    {
        private static IEnumerable<SightWord> siteWords { get; set; }


        public static IEnumerable<SightWord> GetWords()
        {
            ReadSiteWordsFromJson();
            return siteWords;
        }

        public static IEnumerable<SightWord> GetNouns()
        {
            ReadSiteWordsFromJson();
            return siteWords.Where(s => s.WordType == SightWordType.Noun);
        }

        public static IEnumerable<SightWord> GetVerbs()
        {
            ReadSiteWordsFromJson();
            return siteWords.Where(s => s.WordType == SightWordType.Verb);
        }

        public static SightWord GetWord(int id)
        {
            ReadSiteWordsFromJson();
            return siteWords.FirstOrDefault(s => s.Id == id) ?? throw new Exception("Id requested was not found");
        }

        private static void ReadSiteWordsFromJson()
        {
            if (siteWords == null)
            {
                var assembly = IntrospectionExtensions.GetTypeInfo(typeof(WordListPage)).Assembly;
                Stream stream = assembly.GetManifestResourceStream("SeeSaySign.SightWords.json");
                using (var reader = new StreamReader(stream))
                {
                    var json = reader.ReadToEnd();
                    var Jitems = JArray.Parse(json);
                    siteWords = Jitems.ToObject<SightWord[]>().ToList();
                }
            }

        }

        //Shuffles the list up so its random
        public static List<SightWord> Shuffle(this List<SightWord> list)
        {
            Random rnd = new Random();
            for (var i = 0; i < list.Count - 1; i++)
            {
                int next = rnd.Next(i, list.Count);
                var temp = list[i];
                list[i] = list[next];
                list[next] = temp;
            }
            return list;
        }

        //returns two random words from the list provided
        public static List<SightWord> GetRandom(this List<SightWord> list, int numberToReturn)
        {
            return list.Shuffle().Take(numberToReturn).ToList();
        }

    }
}
