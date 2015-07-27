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
        static IEnumerable<int> RandomSource(BCRandomStream rs)
        {
            while (true)
                yield return rs.Read();
        }


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
                        char[] sep = new char[] { '\r', '\n' };
                        int i = -1;
                        var ReadList = readStream.ReadToEnd()
                            .Split(sep, StringSplitOptions.RemoveEmptyEntries)
                            .Select(x => int.Parse(x))
                            .OrderBy(x => x)
                            .Select(x => { i++; return new { x, i }; })
                            .ToList();

                        int j = -1;
                        var GenerList = RandomSource(rndstream)
                            .Take(ReadList.Count())
                            .OrderBy(y => y)
                            .Select(y=> { j++; return new { y, j }; })
                            .ToList();

                        //var lastList = ReadList.Intersect(GenerList).ToList();

                        var result =
                            from val in ReadList
                            join gen in GenerList
                            on val.x equals gen.y into t
                            from rt in t.DefaultIfEmpty()
                            group t by new { val.x,val.i } into grouped
                            select new { key = grouped.Key, amount = grouped.Count()};

                        //var tt =  result.GroupBy(x => x.key.x);


                        result
                            .Select(x => { var v = Math.Min(x.key.x, x.amount); return x; })
                            .Select(x => { Console.WriteLine(x); return x; }).ToList();


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
