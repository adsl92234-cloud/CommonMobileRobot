using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AML
{
    public delegate void Refresh(string table);
    public class WT_Library
    {
        public static string StringArrayCombine(string[] Array, string key)
        {
            try
            {
                return (StringArrayCombine(Array, key, 0, Array.Length));
            }
            catch
            { return "StringArrayCombine Fail."; }
        }
         public static string StringArrayCombine(string[] Array, string key, int Start)
         {
             try
             {
                 return (StringArrayCombine(Array, key, Start, Array.Length - Start));
             }
             catch
             { return "StringArrayCombine Fail.."; }
         }
        public static string StringArrayCombine(string[] Array, string key, int Start, int count)
        {
            try
            {
                string ReturnString = "";
                for (int i = 0; i < (count - 1); i++)
                    ReturnString = ReturnString + Array[Start + i] + key;

                return (ReturnString + Array[Start + count - 1]);
            }
            catch
            {
                return "StringArrayCombine Fail...";
            }
        }
        public static string[] SplitByCount(string Text, int Count)//把字串每"Count"分一組回傳(Tag reader用)
        { 
            List<string> Return_Array = new List<string>();

            while (Text.Length >= 16)
            {
                Return_Array.Add(Text.Substring(0, 16));
                Text = Text.Substring(16);
            }
            if(Text!= "")
                Return_Array.Add(Text);
            return Return_Array.ToArray();
        }
    }
}
