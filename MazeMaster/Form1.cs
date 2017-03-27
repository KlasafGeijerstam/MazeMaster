using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MazeMaster
{
    public partial class Form1 : Form
    {
        private InterpolationPictureBox pictureBox;
        private Stack<Node> nodeStack = new Stack<Node>();
        private List<Node> toDraw = new List<Node>();
        private Queue<Node> nodeQueue = new Queue<Node>();

        public Form1()
        {
            InitializeComponent();
            Load += Form1_Load;
            Width = 900;
            Height = 900;
        }
        private Maze maze;

        private void Form1_Load(object sender, EventArgs e)
        {

            pictureBox = new InterpolationPictureBox();
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox.Dock = DockStyle.Fill;
            Controls.Add(pictureBox);

            maze = new Maze("img/maze2.bmp", 1);
            maze.Analyze();
            pictureBox.Image = maze.CurrentImage;
            Task.Factory.StartNew(new Action(() => { DepthFirst(maze.StartNode); }));
            
        }
        
        private void DrawNodes()
        {
            maze.ResetImage();
            foreach(var n in toDraw){
                maze.DrawPixel(n.Position, Color.Red);
            }
            BeginInvoke(new Action(()=> {
                pictureBox.Image = maze.CurrentImage;
            }));
        }

        private void DrawNodesLines()
        {
            maze.ResetImage(); //4
            for(int i = 0; i < toDraw.Count; i++)
            {
                if (toDraw.Count - i < 2)
                    break;
                maze.DrawLine(Color.Blue,toDraw[i].Position,toDraw[i+1].Position);
            }
            maze.DrawPixel(maze.StartNode.Position, Color.Purple);
            maze.DrawPixel(maze.EndNode.Position, Color.Purple);
            BeginInvoke(new Action(() => {
                pictureBox.Image = maze.CurrentImage;
            }));
        }

        private bool DepthFirst(Node n)
        {
            nodeStack.Push(n);
            toDraw.Add(n);
            //DrawNodes();
            DrawNodesLines();
            Thread.Sleep(10);
            n.Visited = true;
            if (n == maze.EndNode)
                return true;

            if (n.Up != null && ! n.Up.Visited)
                if (DepthFirst(n.Up))
                    return true;
            if (n.Down != null && !n.Down.Visited)
                if (DepthFirst(n.Down))
                    return true;
            if (n.Left != null && !n.Left.Visited)
                if(DepthFirst(n.Left))
                    return true;
            if (n.Right != null && !n.Right.Visited)
                if (DepthFirst(n.Right))
                    return true;

            toDraw.Remove(nodeStack.Pop());
            return false;
        }
        //https://hastebin.com/emugafegew.tex
        //TODO - Implement BreadthFirst
        private bool BreadthFirst(Node n)
        {
            nodeQueue.Enqueue(n);
            toDraw.Add(n);
            return true;
            Thread.Sleep(100);
            n.Visited = true;

            if (n == maze.EndNode)
                return true;
            
            if (n.Up != null && !n.Up.Visited)
                nodeQueue.Enqueue(n.Up);
            if (n.Down != null && !n.Down.Visited)
                nodeQueue.Enqueue(n.Down);
            if (n.Left != null && !n.Left.Visited)
                nodeQueue.Enqueue(n.Left);
            if (n.Right != null && !n.Right.Visited)
                nodeQueue.Enqueue(n.Right);

            toDraw.Remove(nodeStack.Pop());
        }

    }
}
