using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Match3 : MonoBehaviour
{
    public ArrayLayout boardLayout;
    public Sprite[] pieces;
    [Header("UI Elements")]
    public RectTransform gameBoard;

    [Header("Prefabs")]
    public GameObject nodePiece;
    int width = 9;
    int height = 14;
    Node[,] board;

    List<NodePiece> update;

    System.Random random;
    // Start is called before the first frame update
    void Start()
    {
        StartGame();
    }

    void Update()
    {
        List<NodePiece> finishedUpdating = new List<NodePiece>();
        for (int i = 0; i < update.Count; i++)
        {
            NodePiece piece = update[i];
            if (!piece.UpdatePiece()) finishedUpdating.Add(piece);
        }
        for (int i = 0; i < finishedUpdating.Count; i++)
        {
            NodePiece piece = finishedUpdating[i];
            update.Remove(piece);
        }
    }

    void StartGame()
    {
        string seed = getRandomSeed();
        random = new System.Random(seed.GetHashCode());
        update = new List<NodePiece>();

        InitialzeBoard();
        VerifyBoard();
        InstantiateBoard();

    }

    void InitialzeBoard()
    {
        board = new Node[width, height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
                board[x, y] = new Node((boardLayout.rows[y].row[x]) ? -1 : fillPiece(), new Point(x, y));
        }
    }

    void VerifyBoard()
    {
        List<int> remove;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Point p = new Point(x, y);
                int val = getValueAtPoint(p);
                if (val <= 0) continue;

                remove = new List<int>();
                while (isConnected(p, true).Count > 0)
                {
                    val = getValueAtPoint(p);
                    if (!remove.Contains(val))
                        remove.Add(val);
                    setValueAtPoint(p, newValue(ref remove));
                }

            }

        }
    }

    void InstantiateBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Node node = getNodeAtPoint(new Point(x, y));
                int val = board[x, y].value;
                if (val <= 0) continue;
                GameObject p = Instantiate(nodePiece, gameBoard);
                NodePiece piece = p.GetComponent<NodePiece>();
                RectTransform rect = p.GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector2(32 + (64 * x), -32 - (64 * y));
                piece.Initialize(val, new Point(x, y), pieces[val - 1]);
                node.SetPiece(piece);

            }
        }
    }

    public void ResetPiece(NodePiece piece)
    {
        piece.ResetPosition();
        piece.flipped = null;
        update.Add(piece);
    }

    public void FlipPieces(Point one, Point two)
    {
        if (getValueAtPoint(one) < 0) return;

        Node nodeOne = getNodeAtPoint(one);
        NodePiece pieceOne = nodeOne.getPiece();

        if (getValueAtPoint(two) > 0)
        {
            Node nodeTwo = getNodeAtPoint(two);
            NodePiece pieceTwo = nodeTwo.getPiece();
            nodeOne.SetPiece(pieceTwo);
            nodeTwo.SetPiece(pieceOne);

            pieceOne.flipped = pieceTwo;
            pieceTwo.flipped = pieceOne;

            update.Add(pieceOne);
            update.Add(pieceTwo);
        }
        else
        ResetPiece(pieceOne);
    }
    List<Point> isConnected(Point p, bool main)
    {
        List<Point> connected = new List<Point>();
        int val = getValueAtPoint(p);
        Point[] directions =
        {
            Point.up, Point.right, Point.down, Point.left
        };

        foreach (Point dir in directions)  //checking if there are two or more shape in the direction
        {
            List<Point> line = new List<Point>();

            int same = 0;
            for (int i = 1; i < 3; i++)
            {
                Point check = Point.add(p, Point.mult(dir, 1));
                if (getValueAtPoint(check) == val)
                {
                    line.Add(check);
                    same++;
                }

            }
            if (same > 1) // if there are more than one of the same shape in the direction then we now  it is a match 
                AddPoints(ref connected, line);    //Add these points to the overarching connected list     

        }

        for (int i = 0; i < 2; i++) //Checking if we are in the middle of two of same shapes
        {
            List<Point> line = new List<Point>();

            int same = 0;
            Point[] check = { Point.add(p, directions[i]), Point.add(p, directions[i + 2]) };
            foreach (Point next in check) // check both side of the piece, if they are same value, add them to the list
            {
                if (getValueAtPoint(next) == val)
                {
                    line.Add(next);
                    same++;
                }
            }

            if (same > 1)
                AddPoints(ref connected, line);


        }

        for (int i = 0; i < 4; i++) //checks for a 2x2
        {
            List<Point> square = new List<Point>();

            int same = 0;
            int next = i + 1;
            if (next >= 4)
                next -= 4;

            Point[] check = { Point.add(p, directions[i]), Point.add(p, directions[next]), Point.add(p, Point.add(directions[i], directions[next])) };

            foreach (Point pnt in check) // check both all of the piece, if they are same value, add them to the list
            {
                if (getValueAtPoint(pnt) == val)
                {
                    square.Add(pnt);
                    same++;
                }
            }
            if (same > 2)
                AddPoints(ref connected, square);


        }

        if (main) //checks for other matches along the current match
        {
            for (int i = 0; i < connected.Count; i++)
                AddPoints(ref connected, isConnected(connected[i], false));

        }
        if (connected.Count > 0)
            connected.Add(p);

        return connected;


    }

    void AddPoints(ref List<Point> points, List<Point> add)
    {
        foreach (Point p in add)
        {
            bool doAdd = true;
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].Equals(p))
                {
                    doAdd = false;
                    break;
                }
            }

            if (doAdd) points.Add(p);
        }
    }

    int fillPiece()
    {
        int val = 1;
        val = (random.Next(0, 100) / (100 / pieces.Length)) + 1;
        return val;
    }

    /*int getValueAtPoint(Point p)
    {
        if (p.x < 0 || p.x >= width || p.y < 0 || p.y >= height) return -1;
        return board[p.x, p.y].value;
    }
    */

    int getValueAtPoint(Point p)
    {
        if (p.x < 0 || p.x >= width || p.y < 0 || p.y >= height)
            return -1;
        if (board[p.x, p.y] == null)
            return -1;
        return board[p.x, p.y].value;
    }


    void setValueAtPoint(Point p, int v)
    {
        board[p.x, p.y].value = v;
    }

    Node getNodeAtPoint(Point p)
    {
        return board[p.x, p.y];
    }
    int newValue(ref List<int> remove)
    {
        List<int> available = new List<int>();
        for (int i = 0; i < pieces.Length; i++)
            available.Add(i + 1);
        foreach (int i in remove)
            available.Remove(i);

        if (available.Count <= 0) return 0;
        return available[random.Next(0, available.Count)];
    }

    // Update is called once per frame


    string getRandomSeed()
    {
        string seed = "";
        string acceptableChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890!@#$%^&*()";
        for (int i = 0; i < 20; i++)

            seed += acceptableChars[Random.Range(0, acceptableChars.Length)];

        return seed;
    }

    public Vector2 getPositionFromPoint(Point p)
    {
        return new Vector2(32 + (64 * p.x), -32 - (64 * p.y));
    }

public bool isWithinBounds(Point p)
{
    return p.x >= 0 && p.x < width && p.y >= 0 && p.y < height;
}

    [System.Serializable]
    public class Node
    {
        public int value; //0 =blank, 1 = cube, 2 = sphere, 3 = cylindder, 4 = pryamid, 5 = diamond, -1 = hole
        public Point index;
        NodePiece piece;
        public Node(int v, Point i)
        {
            value = v;
            index = i;
        }

        public void SetPiece(NodePiece p)
        {
            piece = p;
            value = (piece == null) ? 0 : piece.value;
            if (piece != null) 
            piece.SetIndex(index);
        }

        public NodePiece getPiece()
        {
            return piece;
        }
    }
}
