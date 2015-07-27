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
                            .Select(y => { j++; return new { y, j }; })
                            .ToList();

                        //var lastList = ReadList.Intersect(GenerList).ToList();


                        var bcres = ReadList.Join(GenerList, rl => rl.x, gl => gl.y, (rl, gl) => new { rl, gl });
                        var bcr2 = bcres.GroupBy(res => res.rl);
                        var bcr3 = bcr2.GroupBy(res => res.Key.x);

                        var bcfinal = bcr3.Select(res => new { k= res.Key, val=Math.Min(res.Count(), res.Max(l => l.Count())) });
                        
                        var groupsY = bcr3.Select(g => g.Count());
                        var groupsX = bcr3.Select(g => g.Max(l => l.Count()));



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
