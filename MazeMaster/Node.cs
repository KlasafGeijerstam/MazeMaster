using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeMaster
{
    class Node
    {
        public bool Visited { get; set; }
        internal Node Up { get; set; }
        internal Node Down { get; set; }
        internal Node Left { get; set; }
        internal Node Right { get; set; }
        public Point Position { get; set; }
        public bool Endpoint { get; set; }


        public Node(Point position)
        {
            Position = position;
        }

        public Node(Point position, Node up, Node down, Node left, Node right)
        {
            Position = position;
            Up = up;
            Down = down;
            Left = left;
            Right = right;
        }

        public Node(Node up, Node down, Node left, Node right, Point position, bool endpoint)
        {
            Up = up;
            Down = down;
            Left = left;
            Right = right;
            Position = position;
            Endpoint = endpoint;
        }
    }
}
