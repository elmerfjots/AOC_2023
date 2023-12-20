using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeFoundation.Common
{
    public enum Direction
    {
        North,
        East,
        South,
        West,
        Unknown,
    }
    public static class AOCMath
    {
        public static long GCF(long a, long b)
        {
            while (b != 0)
            {
                long temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }
        public static long LCM(long a, long b)
        {
            return (a / GCF(a, b)) * b;
        }
    }
    
}
