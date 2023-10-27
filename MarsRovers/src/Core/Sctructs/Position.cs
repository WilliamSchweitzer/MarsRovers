using MarsRovers.src.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MarsRovers.src.Core.Sctructs
{
    public struct Position
    {
        public ulong X { get; set; }
        public ulong Y { get; set; }
        private ulong XMax { get; }
        private ulong YMax { get; }
        public Heading Heading { get; set; }

        public Position(ulong x, ulong y, ulong xMax, ulong yMax, Heading heading)
        {
            X = x;
            Y = y;
            XMax = xMax;
            YMax = yMax;
            Heading = heading;
        }

        // The position should never exceed the max nor the minimum of zero defined by the 2d plane with only positive values given by the problem at hand
        public void IncrementX()
        {
            if (X < XMax)
            {
                X++;
            }
        }

        public void IncrementY()
        {
            if (Y < YMax)
            {
                Y++;
            }
        }

        public void DecrementX()
        {
            if (X > 0)
            {
                X--;
            }
        }

        public void DecrementY()
        {
            if (Y > 0)
            {
                Y--;
            }
        }

        public void Move()
        {
            if (Heading == Heading.N)
            {
                IncrementY();
            }
            else if (Heading == Heading.E)
            {
                IncrementX();
            }
            else if (Heading == Heading.S)
            {
                DecrementY();
            }
            else if (Heading == Heading.W)
            {
                DecrementX();
            }
        }

        public void SetHeading(Heading heading)
        {
            Heading = heading;
        }

        public override string ToString() => $"{X} {Y} {Heading}";
    }
}
