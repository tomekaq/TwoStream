using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UsingTwoStream
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = @"C:\Users\user\Documents\Zapis.txt";
            Console.WriteLine("Podaj maksymalna wartosc");
            var range = int.Parse(Console.ReadLine());
            using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader readStream = new StreamReader(fileStream))
                {
                    using (BCRandomStream rndstream = new BCRandomStream(range))
                    {
                        List<int> listFile = new List<int>();

                        HashSet<int> hashSet = new HashSet<int>();

                        HashSet<int> hashSet2 = new HashSet<int>();
                        char[] sep = new char[] { '\r', '\n' };

                        List<int> ReadList = readStream.ReadToEnd()
                            .Split(sep, StringSplitOptions.RemoveEmptyEntries)
                            .Select(x => int.Parse(x))
                            .ToList();
                        
                        ReadList.ForEach(x => hashSet.Add(x));

                        Enumerable.Range(0, ReadList.Count)
                            .Select(x => rndstream.Read())
                            .Where(x => hashSet.Contains(x))
                            .ToList()
                            .ForEach(x =>hashSet2.Add(x));

                        foreach (var t in hashSet2)
                            Console.WriteLine(t);

                        rndstream.Close();
                    }
                    readStream.Close();
                }
                fileStream.Close();
            }
            Console.ReadLine();
        }
    }
}
