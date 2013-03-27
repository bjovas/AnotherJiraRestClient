using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleClassLibrary
{
    public class HeavyCalculations
    {
        public List<int> DoSomeHeavyStuff(int arg1, int arg2, int arg3)
        {
            
            // Lets calculate doing something that runs out of memory
            throw new OutOfMemoryException("This is a fake exception created at " + DateTime.Now.ToShortTimeString());


        }
    }
}
