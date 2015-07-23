using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsingTwoStream
{
    public class BCRandomStream : TextReader
    {
        Random rnd;
        int rndrange;

        public BCRandomStream(int range)
        {
            rndrange = range;
            rnd = new Random();
        }

        public override int Read()
        {
            return rnd.Next(rndrange);
        }
    }
}


