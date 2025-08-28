using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A simple 2D integer-based Point class commonly used for grid-based operations.
/// Includes utility methods for arithmetic, conversions, and directional constants.
/// </summary>
[System.Serializable]
public class Point
{
    public int x; // Horizontal grid coordinate
    public int y; // Vertical grid coordinate

    /// <summary>
    /// Constructs a Point with the given x and y values.
    /// </summary>
    public Point(int nx, int ny)
    {
        x = nx;
        y = ny;
    }

    /// <summary>
    /// Multiplies both x and y by a scalar value.
    /// Modifies the current Point.
    /// </summary>
    public void mult(int m)
    {
        x *= m;
        y *= m;
    }

    /// <summary>
    /// Adds another Point's x and y to this Point.
    /// Modifies the current Point.
    /// </summary>
    public void add(Point p)
    {
        x += p.x;
        y += p.y;
    }

    /// <summary>
    /// Converts this Point to a Vector2 (float-based 2D position).
    /// Useful for UI or world-space positioning.
    /// </summary>
    public Vector2 ToVector()
    {
        return new Vector2(x, y);
    }

    /// <summary>
    /// Checks if another Point has the same x and y values.
    /// </summary>
    public bool Equals(Point p)
    {
        return (x == p.x && y == p.y);
    }

    /// <summary>
    /// Creates a Point from a Vector2 by converting its components to integers.
    /// </summary>
    public static Point fromVector(Vector2 v)
    {
        return new Point((int)v.x, (int)v.y);
    }

    /// <summary>
    /// Creates a Point from a Vector3 by converting x and y components to integers.
    /// </summary>
    public static Point fromVector(Vector3 v)
    {
        return new Point((int)v.x, (int)v.y);
    }

    /// <summary>
    /// Returns a new Point by multiplying another Point’s x and y values by a scalar.
    /// </summary>
    public static Point mult(Point p, int m)
    {
        return new Point(p.x * m, p.y * m);
    }

    /// <summary>
    /// Returns a new Point by adding two Points’ coordinates.
    /// </summary>
    public static Point add(Point p, Point o)
    {
        return new Point(p.x + o.x, p.y + o.y);
    }

    /// <summary>
    /// Returns a new Point with the same x and y values as the given Point.
    /// </summary>
    public static Point clone(Point p)
    {
        return new Point(p.x, p.y);
    }

    // ---- Commonly Used Static Directional Constants ----

    /// <summary>Returns a Point at the origin (0, 0).</summary>
    public static Point zero
    {
        get { return new Point(0, 0); }
    }

    /// <summary>Returns a Point (1, 1).</summary>
    public static Point one
    {
        get { return new Point(1, 1); }
    }

    /// <summary>Returns a Point pointing up (0, 1).</summary>
    public static Point up
    {
        get { return new Point(0, 1); }
    }

    /// <summary>Returns a Point pointing down (0, -1).</summary>
    public static Point down
    {
        get { return new Point(0, -1); }
    }

    /// <summary>Returns a Point pointing right (1, 0).</summary>
    public static Point right
    {
        get { return new Point(1, 0); }
    }

    /// Returns a Point pointing left (-1, 0).</summary>
    public static Point left
    {
        get { return new Point(-1, 0); }
    }
}
