using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// A simple 2D integer point class for grid-based operations.

[System.Serializable]
public class Point
{
    public int x;
    public int y;

    /// Constructs a Point with the given x and y values.
    public Point(int nx, int ny)
    {
        x = nx;
        y = ny;
    }

    /// Multiplies both x and y by a scalar value.
    public void mult(int m)
    {
        x *= m;
        y *= m;
    }

    /// Adds another Point's x and y to this Point.
    public void add(Point p)
    {
        x += p.x;
        y += p.y;
    }
    /// Converts this Point to a Vector2.
    public Vector2 ToVector()
    {
        return new Vector2(x, y);
    }

    /// Checks if another Point has the same x and y values.
    public bool Equals(Point p)
    {
        return (x == p.x && y == p.y);
    }

    /// Creates a Point from a Vector2 by casting its components to integers.
    public static Point fromVector(Vector2 v)
    {
        return new Point((int)v.x, (int)v.y);
    }

    /// Creates a Point from a Vector3 by casting x and y components to integers.
    public static Point fromVector(Vector3 v)
    {
        return new Point((int)v.x, (int)v.y);
    }

    /// Returns a new Point by multiplying the x and y of an existing Point by a scalar.
    public static Point mult(Point p, int m)
    {
        return new Point(p.x * m, p.y * m);
    }

    /// Returns a new Point by adding the x and y values of two Points.
    public static Point add(Point p, Point o)
    {
        return new Point(p.x + o.x, p.y + o.y);
    }

    /// Returns a new Point with the same values as the given Point.
    public static Point clone(Point p)
    {
        return new Point(p.x, p.y);
    }

    /// Gets a Point at the origin (0, 0).
    public static Point zero
    {
        get { return new Point(0, 0); }
    }

    /// Gets a Point where both x and y are 1.
    public static Point one
    {
        get { return new Point(1, 1); }
    }

    /// Gets a Point representing up direction (0, 1).
    public static Point up
    {
        get { return new Point(0, 1); }
    }

    /// Gets a Point representing down direction (0, -1).
    public static Point down
    {
        get { return new Point(0, -1); }
    }

    /// Gets a Point representing right direction (1, 0).
    public static Point right
    {
        get { return new Point(1, 0); }
    }

    /// Gets a Point representing left direction (-1, 0)   
    public static Point left
    {
        get { return new Point(-1, 0); }
    }
}
