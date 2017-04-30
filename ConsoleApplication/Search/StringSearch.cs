using System;
using System.Collections.Generic;

namespace ConsoleApplication.Search
{
    public class StringSearch
    {
        public static string AutoComplete(string s, string[] array)
        {
            var sLower = s.ToLower();
            List<string> tempList = new List<string>();
            string current;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].Length > sLower.Length)
                {
                    current = array[i].Substring(0, sLower.Length).ToLower();

                    if (current.Equals(sLower))
                    {
                        tempList.Add(array[i]);
                    }
                }
            }
            if (tempList.Count == 1)
            {
                return tempList[0];
            }
            else
            {
                return s;
            }
    }
    }
}