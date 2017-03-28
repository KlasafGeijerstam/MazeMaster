package mazemaster;

import javax.imageio.ImageIO;
import java.awt.*;
import java.awt.image.BufferedImage;
import java.io.File;
import java.io.IOException;
import java.util.HashMap;

/**
 * Created by Klas on 2017-03-28.
 */
public class Maze {

    private HashMap<Point, Node> nodes;
    private BufferedImage orgImage;
    private BufferedImage curImage;
    private Graphics imageGraphics;
    private int borderPad;
    private int pathColor = Color.white.getRGB();
    private int borderColor = Color.black.getRGB();
    public Node startNode;
    public Node endNode;

    public Maze(String mazeImage,int borderPad) throws IOException
    {
        orgImage = ImageIO.read(new File(mazeImage));
        curImage = ImageIO.read(new File(mazeImage));
        imageGraphics = curImage.getGraphics();
        nodes = new HashMap<>();
        this.borderPad = borderPad;
    }

    public void analyze()
    {
        for(int y = borderPad; y < orgImage.getHeight()-borderPad; y++)
        {
            for (int x = borderPad; x < orgImage.getWidth()-borderPad; x++)
            {
                int curColor = orgImage.getRGB(x,y);

                if (curColor == borderColor)
                    continue;

                if(curColor == pathColor)
                {
                    if(x == borderPad || x == orgImage.getWidth() - borderPad-1 || y == borderPad || y == orgImage.getHeight() - borderPad-1)
                    {
                        Point pos = new Point(x, y);
                        Node node = new Node(pos);
                        node.endpoint = true;
                        connectNode(node, x, y);
                        if(startNode == null)
                            startNode = node;
                        else
                            endNode = node;

                        nodes.put(pos,node);
                    }
                    else if(isNode(x,y))
                    {
                        Point pos = new Point(x, y);
                        Node node = new Node(pos);
                        connectNode(node,x,y);
                        nodes.put(pos,node);
                    }
                }
            }
        }
    }

    private boolean isNode(int x, int y)
    {
        boolean left = false,right = false,up = false ,down = false;
        int trueCount = 0;

        if (x > borderPad)
            left = orgImage.getRGB(x - 1, y) == pathColor;
        if (x < orgImage.getWidth() - borderPad)
            right = orgImage.getRGB(x + 1, y) == pathColor;
        if (y > borderPad)
            up = orgImage.getRGB(x, y - 1) == pathColor;
        if (y < orgImage.getWidth() - borderPad)
            down = orgImage.getRGB(x, y + 1) == pathColor;

        if (left) trueCount++; if (right) trueCount++; if (up) trueCount++; if (down) trueCount++;

        return ((left || right) && (up || down)) || trueCount == 1;

    }

    private void connectNode(Node node,int x, int y)
    {
        //Up
        for (int i = y; i >= borderPad && processPixel(node, x, i,Direction.Up); i--) ;

        //Down
        for (int i = y; i <= orgImage.getHeight() - borderPad && processPixel(node,x,i,Direction.Down); i++) ;

        //Left
        for (int i = x; i >= borderPad && processPixel(node,i,y,Direction.Left); i--) ;

        //Right
        for (int i = x; i <= orgImage.getWidth() - borderPad && processPixel(node, i, y,Direction.Right); i++) ;
    }

    private boolean processPixel(Node node, int x, int y, Direction dir)
    {
        int pix = orgImage.getRGB(x, y);
        if (pix == borderColor)
            return false;
        Point tmp = new Point(x, y);
        if (nodes.containsKey(tmp))
        {
            switch (dir)
            {
                case Up:
                    node.up = nodes.get(tmp);
                    nodes.get(tmp).down = node;
                    break;
                case Down:
                    node.down = nodes.get(tmp);
                    nodes.get(tmp).up = node;
                    break;
                case Left:
                    node.left = nodes.get(tmp);
                    nodes.get(tmp).right = node;
                    break;
                case Right:
                    node.right = nodes.get(tmp);
                    nodes.get(tmp).left = node;
                    break;
            }
            return false;
        }
        return true;
    }

    public void drawNodes(Color nodeColor, Color endPointColor)
    {
        Color orgCol = imageGraphics.getColor();
        for (Node node : nodes.values())
        {
            if(node.endpoint){
                imageGraphics.setColor(endPointColor);
                imageGraphics.fillRect(node.position.x,node.position.y,1,1);
                imageGraphics.setColor(orgCol);
            }
            else{
                imageGraphics.setColor(nodeColor);
                imageGraphics.fillRect(node.position.x,node.position.y,1,1);
                imageGraphics.setColor(orgCol);
            }
        }
    }

    public void drawLine(Color pathColor,Point p1,Point p2)
    {
        Color orgCol = imageGraphics.getColor();
        imageGraphics.setColor(pathColor);
        imageGraphics.drawLine(p1.x,p1.y,p2.x,p2.y);
        imageGraphics.setColor(orgCol);
    }

    public void drawPixel(Point point, Color color)
    {
        Color orgCol = imageGraphics.getColor();
        imageGraphics.setColor(color);
        imageGraphics.fillRect(point.x, point.y, 1, 1);
        imageGraphics.setColor(orgCol);
    }

    public void resetImage()
    {
        imageGraphics.drawImage(orgImage,0,0,null);
    }

    public BufferedImage getCurrentImage() {
        return curImage;
    }

    enum Direction
    {
        Up,Down,Left,Right
    }
}
