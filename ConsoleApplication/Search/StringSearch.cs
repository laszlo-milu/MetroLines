using System;
using System.Collections.Generic;

namespace ConsoleApplication.Search
{
    public class StringSearch
    {
        public static string AutoComplete(string s, string[] array, bool listOptions=false)
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
            else if (tempList.Count > 1)
            {
                if (listOptions)
                {
                    Program.ClearCurrentConsoleLine();
                    Console.Write(tempList[0] + Environment.NewLine);
                }
                bool b = true;
                var counter = 0;
                while (b)
                {
                    counter++;
                    for (int i = 1; i < tempList.Count; i++)
                    {
                        if (listOptions)
                        {
                            Console.WriteLine(tempList[i]);
                        }

                        if (!tempList[i - 1]
                            .ToLower()
                            .Substring(0, s.Length + counter)
                            .Equals(tempList[i].ToLower().Substring(0, s.Length + counter)))
                        {
                            b = false;
                        }
                    }
                }
                return tempList[0].Substring(0,s.Length+counter-1);
            }
            else
            {
                return s;
            }
    }
    }
}