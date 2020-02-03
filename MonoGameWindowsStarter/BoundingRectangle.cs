using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter
{
    public struct BoundingRectangle
    {
        public float X, Y, Width, Height;

        public BoundingRectangle(float x, float y, float width, float height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        public static implicit operator Rectangle(BoundingRectangle br)
        {
            return new Rectangle(
                (int)br.X,
                (int)br.Y,
                (int)br.Width, 
                (int)br.Height);
        }

        public bool CollidesWith(BoundingRectangle other)
        {
            return !(this.X > other.X + other.Width
                    || this.X + this.Width < other.X
                    || this.Y > other.Y + other.Height
                    || this.Y + this.Height < other.Y);
        }
    }
}
