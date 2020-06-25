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
        /// <summary>
        /// Calculates the Manhattan distance between two points in plane.
        /// </summary>
        /// <param name="pointA">The given point A in plane</param>
        /// <param name="pointB">The given point B </param>
        /// <returns>Manhattan distance between point A and point B.</returns>
        public static float ManhattanDistance(Vector2 pointA, Vector2 pointB)
        {
            return Math.Abs(pointB.X - pointA.X) + Math.Abs(pointB.Y - pointA.Y);
        }
    }
}
