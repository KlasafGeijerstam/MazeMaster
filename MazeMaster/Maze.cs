using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeMaster
{
    class Maze
    {
        private Dictionary<Point, Node> nodes;
        private Bitmap orgImage;
        private Bitmap curImage;
        private Graphics imageGraphics;
        private int borderPad;
        private Color pathColor = Color.White;
        private Color borderColor = Color.Black;
        private Node startNode;
        private Node endNode;
        public Bitmap CurrentImage { get { return curImage; } }
        public Dictionary<Point, Node> Nodes { get { return nodes; } }
        #region Properties
        public Color PathColor
        {
            get{ return pathColor; }
            set{ pathColor = value; }
        }

        public Color BorderColor
        {
            get{ return borderColor; }
            set{ borderColor = value; }
        }

        internal Node StartNode
        {
            get { return startNode; }
            set { startNode = value; }
        }

        internal Node EndNode
        {
            get { return endNode; }
            set { endNode = value; }
        }

        public Bitmap OrgImage
        {
            get { return orgImage; }
            set{ orgImage = value; }
        }
        #endregion
        public Maze(string mazeImage,int borderPad = 1)
        {
            orgImage = (Bitmap)Image.FromFile(mazeImage);
            curImage = (Bitmap)orgImage.Clone();
            imageGraphics = Graphics.FromImage(curImage);
            nodes = new Dictionary<Point, Node>();
            this.borderPad = borderPad;
        }
        
        public void Analyze()
        {
            for(int y = borderPad; y < orgImage.Height-borderPad; y++)
            {
                for (int x = borderPad; x < orgImage.Width-borderPad; x++)
                {
                    var curColor = orgImage.GetPixel(x, y);

                    if (curColor.ToArgb() == borderColor.ToArgb())
                        continue;

                    if(curColor.ToArgb() == pathColor.ToArgb())
                    {
                        if(x == borderPad || x == orgImage.Width - borderPad-1 || y == borderPad || y == orgImage.Height - borderPad-1)
                        {
                            var pos = new Point(x, y);
                            Node node = new Node(pos) { Endpoint = true };
                            ConnectNode(node, x, y);
                            nodes.Add(pos,node);
                        }
                        else if(IsNode(x,y))
                        {
                            var pos = new Point(x, y);
                            Node node = new Node(pos);
                            ConnectNode(node,x,y);
                            nodes.Add(pos,node);
                        }
                    }
                }
            }

            startNode = nodes.First(x => x.Value.Endpoint).Value;
            endNode = nodes.Last(x => x.Value.Endpoint).Value;

        }

        private bool IsNode(int x, int y)
        {
            bool left = false,right = false,up = false ,down = false;
            int trueCount = 0;
            if (x > borderPad)
                left = orgImage.GetPixel(x - 1, y).ToArgb() == pathColor.ToArgb();
            if (x < orgImage.Width - borderPad)
                right = orgImage.GetPixel(x + 1, y).ToArgb() == pathColor.ToArgb();
            if (y > borderPad)
                up = orgImage.GetPixel(x, y - 1).ToArgb() == pathColor.ToArgb();
            if (y < orgImage.Height - borderPad)
                down = orgImage.GetPixel(x, y + 1).ToArgb() == pathColor.ToArgb();

            if (left) trueCount++; if (right) trueCount++; if (up) trueCount++; if (down) trueCount++;

            return ((left || right) && (up || down)) || trueCount == 1;

        }

        private void ConnectNode(Node node,int x, int y)
        {
            //Up
            for (var i = y; i >= borderPad && ProccessPixel(node, x, i,Direction.Up); i--) ;

            //Down
            for (var i = y; i <= orgImage.Height - borderPad && ProccessPixel(node,x,i,Direction.Down); i++) ;

            //Left
            for (var i = x; i >= borderPad && ProccessPixel(node,i,y,Direction.Left); i--) ;

            //Right
            for (var i = x; i <= orgImage.Width - borderPad && ProccessPixel(node, i, y,Direction.Right); i++) ;
        }

        private bool ProccessPixel(Node node, int x, int y,Direction dir)
        {
            var pix = orgImage.GetPixel(x, y);
            if (pix.ToArgb() == borderColor.ToArgb())
                return false;
            Point tmp = new Point(x, y);
            if (nodes.ContainsKey(tmp))
            {  
                switch (dir)
                {
                    case Direction.Up:
                        node.Up = nodes[tmp];
                        nodes[tmp].Down = node;
                            break;
                    case Direction.Down:
                        node.Down = nodes[tmp];
                        nodes[tmp].Up = node;
                            break;
                    case Direction.Left:
                        node.Left = nodes[tmp];
                        nodes[tmp].Right = node;
                            break;
                    case Direction.Right:
                        node.Right = nodes[tmp];
                        nodes[tmp].Left = node;
                        break;
                }
                return false;
            }

            return true;
        }

        public void DrawNodes(Color nodeColor, Color endPointColor)
        {
            var brush = new Pen(nodeColor).Brush;
            var endPointBrush = new Pen(endPointColor).Brush;
            foreach (var node in nodes.Values)
            {
                if(node.Endpoint)
                    imageGraphics.FillRectangle(endPointBrush,node.Position.X,node.Position.Y,1,1);
                else
                    imageGraphics.FillRectangle(brush, node.Position.X, node.Position.Y, 1, 1);
            }
        }

        public void DrawLine(Color pathColor,Point p1,Point p2)
        {
            imageGraphics.DrawLine(new Pen(pathColor), p1, p2);
        }

        public void DrawPixel(Point point, Color color)
        {
            imageGraphics.FillRectangle(new Pen(color).Brush, point.X, point.Y, 1, 1);
        }

        public void ResetImage()
        {
            imageGraphics.DrawImage(orgImage, 0, 0);
        }

        ~Maze()
        {
            orgImage.Dispose();
            curImage.Dispose();
            imageGraphics.Dispose();
        }
    }

    enum Direction
    {
        Up,Down,Left,Right
    }
}
