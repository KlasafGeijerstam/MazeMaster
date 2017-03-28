package mazemaster;

import java.awt.*;

/**
 * Created by Klas on 2017-03-28.
 */
public class Node {

    public boolean visited;
    public Node up;
    public Node down;
    public Node left;
    public Node right;
    public Point position;
    public boolean endpoint;

    public Node(Point position)
    {
        this.position = position;
    }

    public Node(Point position, Node up, Node down, Node left, Node right)
    {
        this.position = position;
        this.up = up;
        this.down = down;
        this.left = left;
        this.right = right;
    }

    public Node(Node up, Node down, Node left, Node right, Point position, boolean endpoint)
    {
        this.position = position;
        this.up = up;
        this.down = down;
        this.left = left;
        this.right = right;
        this.endpoint = endpoint;
    }
}
