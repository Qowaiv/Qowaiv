using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Qowaiv
{
    internal static class Parsing
    {
        private static Regex SpacePattern = new Regex(@"\s", RegexOptions.Compiled);

        /// <summary>Clears spacing.</summary>
        public static string ClearSpacing(string str)
        {
            return SpacePattern.Replace(str, "");
        }
        /// <summary>Clears spacing and uppercases.</summary>
        public static string ClearSpacingToUpper(string str)
        {
            return ClearSpacing(str).ToUpperInvariant();
        }


        /// <summary>Unifies the string.</summary>
        public static string ToUnified(string str)
        {
            return ToNonDiacritic(str).ToLowerInvariant().Replace(" ", "");
        }

        /// <summary>Replaces diacrtic characters by non diacritic ones.</summary>
        public static string ToNonDiacritic(string str)
        {
            var sb = new StringBuilder();

            foreach (var ch in str)
            {
                String chs;

                if (DiacriticLookup.TryGetValue(ch, out chs))
                {
                    sb.Append(chs);
                }
                else
                {
                    sb.Append(ch);
                }
            }
            return sb.ToString();
        }

        private static Dictionary<Char, String> DiacriticLookup = new Dictionary<char, string>()
        {
            // A
            {'À',"A"},{'Á',"A"},{'Â',"A"},{'Ã',"A"},{'Ä',"A"},{'Å',"A"},{'Ā',"A"},{'Ă',"A"},{'Ą',"A"},{'Ǎ',"A"},{'Ǻ',"A"},{'Æ',"AE"},{'Ǽ',"A"},
            {'à',"a"},{'á',"a"},{'â',"a"},{'ã',"a"},{'ä',"a"},{'å',"a"},{'ā',"a"},{'ă',"a"},{'ą',"a"},{'ǎ',"a"},{'ǻ',"a"},{'æ',"ae"},{'ǽ',"a"},
            
            // C
            {'Ç',"C"},{'Ć',"C"},{'Ĉ',"C"},{'Ċ',"C"},{'Č',"C"},
            {'ç',"c"},{'ć',"c"},{'ĉ',"c"},{'ċ',"c"},{'č',"c"},
            
            // E
            {'È',"E"},{'É',"E"},{'Ê',"E"},{'Ë',"E"},{'Ē',"E"},{'Ĕ',"E"},{'Ė',"E"},{'Ę',"E"},{'Ě',"E"},
            {'è',"e"},{'é',"e"},{'ê',"e"},{'ë',"e"},{'ē',"e"},{'ĕ',"e"},{'ė',"e"},{'ę',"e"},{'ě',"e"},

            // Eszett
            {'ß',"sz"},
            

            
            
            
            
           
           
           
           

//{'Ì
//{'Í
//{'Î
//{'Ï
//{'Ð
//{'Ñ
//{'Ò
//{'Ó
//{'Ô
//{'Õ
//{'Ö
//{'Ø
//{'Ù
//{'Ú
//{'Û
//{'Ü
//{'Ý

//{'ì
//{'í
//{'îïðñòóôõöøùúûüýÿĎďĐđĜĝĞğĠġĢģĤĥĦħĨĩĪīĬĭĮįİıĲĳĴĵĶķĸĹĺĻļĽľĿŀŁłŃńŅņŇňŉŊŋŌōŎŏŐőŒœŔŕŖŗŘřŚśŜŝŞşŠšŢţŤťŦŧŨũŪūŬŭŮůŰűŲųŴŵŶŷŸŹźŻżŽžƠơƯưǏǐǑǒǓǔǕǖǗǘǙǚǛǜǾǿ
        };
    }
}
