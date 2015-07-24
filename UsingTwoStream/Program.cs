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

                        var ReadList = readStream.ReadToEnd()
                            .Split(sep, StringSplitOptions.RemoveEmptyEntries)
                            .Select(x => int.Parse(x))
                            .OrderBy(x => x);

                        var GenerList = RandomSource(rndstream).Take(ReadList.Count())
                            .OrderBy(x => x);

                        
                        //linq porównaj i zwróć
                        
                        var lastList = ReadList.Intersect(GenerList).ToList();

                        var lastL = from read in ReadList
                                    join gen in GenerList
                                    on read equals gen into t
                                    select new { t};
                        
                        lastL
                        .Select(x =>
                        {
                            Console.WriteLine("{0}", x);
                            return x;
                        })
                        .ToList();


                        lastList
                        .Select(x =>
                        {
                            Console.WriteLine("{0}", x);
                            return x;
                        })
                        .ToList();

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
