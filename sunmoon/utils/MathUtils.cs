using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sunmoon.utils
{
    public static class MathUtils
    {
        public static int FloorDiv(int a, int n)
        {
            return (int)Math.Floor((double)a / n);
        }

        public static int Mod(int a, int n)
        {
            return ((a % n) + n) % n;
        }
    }
}