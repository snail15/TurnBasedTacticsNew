using System;

public struct GridPosition : IEquatable<GridPosition>
{
    public int X { get; set; }
    public int Z { get; set; }

    public GridPosition(int x, int z)
    {
        X = x;
        Z = z;
    }

    public bool Equals(GridPosition other)
    {
        return this == other;
    }
    public override bool Equals(object obj)
    {
        return obj is GridPosition position && X == position.X && Z == position.Z;
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(X, Z);
    }
    public override string ToString()
    {
        return $"X: {X}, Z: {Z}";
    }

    public static bool operator == (GridPosition a, GridPosition b)
    {
        return a.X == b.X && a.Z == b.Z;
    }

    public static bool operator !=(GridPosition a, GridPosition b)
    {
        return !(a == b);
    }

    public static GridPosition operator +(GridPosition a, GridPosition b)
    {
        return new GridPosition(a.X + b.X, a.Z + b.Z);
    }
    
    public static GridPosition operator -(GridPosition a, GridPosition b)
    {
        return new GridPosition(a.X - b.X, a.Z - b.Z);
    }
}