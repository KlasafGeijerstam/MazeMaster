package mazemaster;

import javax.swing.*;
import java.awt.*;
import java.util.ArrayList;
import java.util.Stack;

/**
 * Created by Klas on 2017-03-28.
 */
public class MazeMaster extends JFrame {

    private Maze maze;
    private Stack<Node> stack;
    private ArrayList<Node> toDraw;

    public MazeMaster() throws Exception{

        maze = new Maze("./img/maze2.bmp",1);
        maze.analyze();
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        setSize(900,900);
        setPreferredSize(new Dimension(1200,1200));
        pack();
        setVisible(true);


        stack = new Stack<>();
        toDraw = new ArrayList<>();
        //maze.drawNodes(Color.BLUE,Color.RED);

        Thread t = new Thread(() -> {
           depthFirst(maze.startNode);
        });
        t.run();

    }

    @Override
    public void paint(Graphics g){
        g.drawImage(maze.getCurrentImage(),10,50,900,900,null);
    }

    @Override
    public void update(Graphics g){
        super.update(g);
    }

    private boolean depthFirst(Node n){
        stack.push(n);
        toDraw.add(n);
        //Draw
        drawNodesLines();
        try {
            Thread.sleep(100);
        } catch (InterruptedException e) {
            e.printStackTrace();
        }
        n.visited = true;

        if (n == maze.endNode)
            return true;

        if (n.up != null && ! n.up.visited)
            if (depthFirst(n.up))
                return true;
        if (n.down != null && !n.down.visited)
            if (depthFirst(n.down))
                return true;
        if (n.left != null && !n.left.visited)
            if(depthFirst(n.left))
                return true;
        if (n.right != null && !n.right.visited)
            if (depthFirst(n.right))
                return true;

        toDraw.remove(stack.pop());
        return false;
    }

    private void drawNodesLines(){
        maze.resetImage();
        for(int i = 0; i < toDraw.size(); i++)
        {
            if (toDraw.size() - i < 2)
                break;
            maze.drawLine(Color.blue,toDraw.get(i).position,toDraw.get(i+1).position);
        }
        maze.drawPixel(maze.startNode.position, Color.green);
        maze.drawPixel(maze.endNode.position, Color.green);
        repaint();
    }
}
