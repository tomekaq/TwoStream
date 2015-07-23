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
            var range = int.Parse(Console.ReadLine());
            using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader readStream = new StreamReader(fileStream))
                {
                    using (BCRandomStream rndstream = new BCRandomStream(range))
                    {
                        HashSet<int> listFile1 = new HashSet<int>();

                        HashSet<int> hashSet = new HashSet<int>();

                        char[] sep = new char[]{ '\r', '\n' };
                        readStream.ReadToEnd()
                            .Split(sep, StringSplitOptions.RemoveEmptyEntries)
                            .Select(x => int.Parse(x))
                            .ToList()
                            .ForEach(x=>hashSet.Add(x));

                      


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
