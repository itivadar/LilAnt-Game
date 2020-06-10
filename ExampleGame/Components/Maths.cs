using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheTirelessLilAnt.Components
{
    static class Maths
    {
        public static float ManhattanDistance(Vector2 pointA, Vector2 pointB)
        {
            return Math.Abs(pointB.X - pointA.X) + Math.Abs(pointB.Y - pointA.Y);
        }
    }
}
