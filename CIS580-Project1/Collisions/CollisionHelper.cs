using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace CIS580_Project.Collisions
{
    /// <summary>
    /// Helps check and handle collisions
    /// </summary>
    public class CollisionHelper
    {
        /// <summary>
        /// Detects collisions between two bounding circles
        /// </summary>
        /// <param name="a">one circle</param>
        /// <param name="b">the second circle</param>
        /// <returns>a bool representing if the circles collided</returns>
        public static bool Collides(BoundingCircle a, BoundingCircle b)
        {
            return Math.Pow(a.Radius + b.Radius, 2) >= Math.Pow(a.Center.X - b.Center.X, 2) + Math.Pow(a.Center.Y - b.Center.Y, 2);
        }
    }
}
