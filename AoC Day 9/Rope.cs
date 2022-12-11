using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace AoC_Day_9
{
    public struct Coords
    {
        public Coords()
        {
            x = 0;
            y = 0;
        }
        public Coords(int X, int Y)
        {
            x = X;
            y = Y;
        }
        public int x;
        public int y;
    }
    class CoordsComparer : IEqualityComparer<Coords>
    {
        public bool Equals(Coords coord1, Coords coord2)
        {
            return coord1.x == coord2.x && coord1.y == coord2.y;
        }
        public int GetHashCode(Coords coord)
        {
            return coord.x.GetHashCode() ^ coord.y.GetHashCode();
        }
    }
    public struct RopeKnot
    {
        public Coords coordinate;
        public Coords[] history = new Coords[] { };

        public RopeKnot()
        {
            coordinate = new Coords(0, 0);
            history = history.Append(coordinate).ToArray();
        }
        public RopeKnot(Coords coord)
        {
            coordinate = coord;
            history = history.Append(coordinate).ToArray();
        }
        public void Move(int x_, int y_)
        {
            coordinate.x += x_;
            coordinate.y += y_;
            history=history.Append(coordinate).ToArray();
        }
        public string Log()
        {
            return String.Format("at {0},{1}", coordinate.x, coordinate.y);
        }
        public double Distance(RopeKnot otherKnot)
        {
            return (Math.Pow(Math.Pow(coordinate.x - otherKnot.coordinate.x, 2) + Math.Pow(coordinate.y - otherKnot.coordinate.y, 2), 0.5));
        }
        public bool Touching(RopeKnot rope1)
        {
            int minX = coordinate.x-1;
            int minY = coordinate.y-1;
            int maxX = coordinate.x+1;
            int maxY = coordinate.y+1;
            if ((minX <= rope1.coordinate.x) && (rope1.coordinate.x <= maxX))
            {
                //Debug.WriteLine("X touching");
                if ((minY <= rope1.coordinate.y) && (rope1.coordinate.y <= maxY))
                {
                    //Debug.WriteLine("Y touching");
                    return true;
                }
                else
                {
                    //Debug.WriteLine("Y not touching");
                    return false;
                }
            }
            else
            {
                //Debug.WriteLine("X not touching");
                return false;
            }
        }
    }
    public class Rope
    {
        public RopeKnot[] knots = Array.Empty<RopeKnot>();
        public Rope(int knotCount)
        {
            knots= new RopeKnot[knotCount];
            Array.Fill<RopeKnot>(knots,new RopeKnot());
        }
        public void Move(char dir, int spaces)
        {
            for (int i = 0; i < spaces; i++)
            {
                switch (dir)
                {
                    case 'L':
                        knots[0].Move(-1, 0);
                        break;
                    case 'R':
                        knots[0].Move(1, 0);
                        break;
                    case 'U':
                        knots[0].Move(0, 1);
                        break;
                    case 'D':
                        knots[0].Move(0, -1);
                        break;
                }
                for (int j = 1; j < knots.Length; j++)
                {
                    if (knots[j].Touching(knots[j-1]))
                        continue;
                    else
                    {
                        if (knots[j].coordinate.x == knots[j-1].coordinate.x && knots[j].coordinate.y < knots[j - 1].coordinate.y)
                        {
                            knots[j].Move(0,1);
                        }
                        else if (knots[j].coordinate.x == knots[j - 1].coordinate.x && knots[j].coordinate.y > knots[j - 1].coordinate.y)
                        {
                            knots[j].Move(0, -1);
                        }
                        else if (knots[j].coordinate.x < knots[j - 1].coordinate.x && knots[j].coordinate.y ==knots[j - 1].coordinate.y)
                        {
                            knots[j].Move(1, 0);
                        }
                        else if (knots[j].coordinate.x > knots[j - 1].coordinate.x && knots[j].coordinate.y == knots[j - 1].coordinate.y)
                        {
                            knots[j].Move(-1, 0);
                        }
                        else if (knots[j].coordinate.x < knots[j - 1].coordinate.x && knots[j].coordinate.y < knots[j - 1].coordinate.y)
                        {
                            knots[j].Move(1, 1);
                        }
                        else if (knots[j].coordinate.x > knots[j - 1].coordinate.x && knots[j].coordinate.y < knots[j - 1].coordinate.y)
                        {
                            knots[j].Move(-1, 1);
                        }
                        else if (knots[j].coordinate.x < knots[j - 1].coordinate.x && knots[j].coordinate.y > knots[j - 1].coordinate.y)
                        {
                            knots[j].Move(1, -1);
                        }
                        else if (knots[j].coordinate.x > knots[j - 1].coordinate.x && knots[j].coordinate.y > knots[j - 1].coordinate.y)
                        {
                            knots[j].Move(-1, -1);
                        }
                    }
                }
            }
        } 
        public void Visualize(int knotIndex)
        {
            int minX = (from coord in knots[0].history select coord.x).Min();
            int minY = (from coord in knots[0].history select coord.y).Min();
            int maxX = (from coord in knots[0].history select coord.x).Max();
            int maxY = (from coord in knots[0].history select coord.y).Max();
            int xSize = maxX - minX;
            int ySize = maxY - minY;
            int xOffset = 0 - minX;
            int yOffset = 0 - minY;
            char[] chars = new char[xSize+1];
            Array.Fill<char>(chars, 'O');
            string[] str = new string[ySize+1];
            Array.Fill<string>(str, new string(chars));
            foreach(Coords coord in knots[knotIndex].history)
            {
                char[] tempChars = str[coord.y + yOffset].ToCharArray();
                tempChars[coord.x + xOffset] = 'X';
                str[coord.y + yOffset] = new string(tempChars);
            }
            foreach(string line in str.Reverse())
            {
                Debug.WriteLine(line);
            }
        }
    }
}