using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ConsoleFtpClient.Core
{
    public class FileParser
    {
        public static List<Item> Parse(string s)
        {
            string[] dataRecords = s.Split('\n');
            List<string> list = new List<string>();
            List<Item> result = new List<Item>();
            result.Add(new Item
            {
                Name = "..",
                FullLink = "..",
                IsDirectory = true
            });
            if (s.Length == 0)
            {
                return result;
            }

            list.AddRange(dataRecords.Select(i => i.Trim()).ToList());
            var pattern = @"[A-Za-zА-ЯЁа-яё._\-0-9]+$";
            foreach (var i in list)
            {
                if(i.Length != 0) {
                    var item = new Item();
                    if (i[0].ToString() == "d")
                    {
                        item.IsDirectory = true;
                    }
                    else
                    {
                        item.IsDirectory = false;
                    }

                    var a = Regex.Match(i, pattern);
                    item.Name = a.Value;
                    item.FullLink = i;
                    result.Add(item);
                }
            }

            

            return result;
        }
    }
}