using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SmartClasses
{
    public static class RandomProvider
    {    
         private static int seed = Environment.TickCount;

         private static ThreadLocal<System.Random> randomWrapper = new ThreadLocal<System.Random>(() =>
             new System.Random(Interlocked.Increment(ref seed))
         );

         public static System.Random GetThreadRandom()
         {
             return randomWrapper.Value;
         }
    }
}
