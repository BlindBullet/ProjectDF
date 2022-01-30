using System.Collections.Generic;
using UnityEngine;

public class IVector2Comparer : IEqualityComparer<IVector2>

{

	public bool Equals(IVector2 x, IVector2 y)

	{

		return x.x == y.x && x.y == y.y;

	}



	public int GetHashCode(IVector2 vector)

	{

		return vector.x.GetHashCode() ^ vector.y.GetHashCode() << 2;

	}

}

[System.Serializable]
public struct IVector2
{
    public int x, y;
    public static readonly IVector2 zero = new IVector2(0, 0);
    public static readonly IVector2 up = new IVector2(0, 1);
    public static readonly IVector2 down = new IVector2(0, -1);
    public static readonly IVector2 left = new IVector2(-1, 0);
    public static readonly IVector2 right = new IVector2(1, 0);
    public static readonly IVector2 topLeft = new IVector2(-1, 1);
    public static readonly IVector2 topRight = new IVector2(1, 1);
    public static readonly IVector2 bottomLeft = new IVector2(-1, -1);
    public static readonly IVector2 bottomRight = new IVector2(1, -1);

    public IVector2(IVector3 v)
    {
        this.x = v.x;
        this.y = v.y;
    }

    public IVector2(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public IVector2(Vector2 a)
    {
        this.x = Mathf.RoundToInt(a.x);
        this.y = Mathf.RoundToInt(a.y);
    }

    public IVector2(Vector3 a)
    {
        this.x = Mathf.RoundToInt(a.x);
        this.y = Mathf.RoundToInt(a.y);
    }

    public int this[int key]
    {
        get
        {
            switch (key)
            {
                case 0:
                    {
                        return x;
                    }
                case 1:
                    {
                        return y;
                    }
                default:
                    {
                        Debug.LogError("Invalid Vector2i index value of: " + key);
                        return 0;
                    }
            }
        }
        set
        {
            switch (key)
            {
                case 0:
                    {
                        x = value;
                        return;
                    }
                case 1:
                    {
                        y = value;
                        return;
                    }
                default:
                    {
                        Debug.LogError("Invalid Vector2i index value of: " + key);
                        return;
                    }
            }
        }
    }

    public static IVector2 Scale(IVector2 a, IVector2 b)
    {
        return new IVector2(a.x * b.x, a.y * b.y);
    }

    public static int DistanceSquared(IVector2 a, IVector2 b)
    {
        int dx = b.x - a.x;
        int dy = b.y - a.y;
        return dx * dx + dy * dy;
    }

    public int DistanceSquared(IVector2 v)
    {
        return DistanceSquared(this, v);
    }

    /// <summary>
    /// Divide Round Down
    /// 음수에 나눗셈에서 + 1 한 결과가 나오지 않도록 함
    /// </summary>
    public IVector2 DivideRoundDown(int d)
    {
        return new IVector2((x / d) + ((x % d) >> 31),
            (y / d) + ((y % d) >> 31));
    }

    public IVector2 Mod(int m)
    {
        return new IVector2((this.x % m + m) % m, (this.y % m + m) % m);
    }

    public override int GetHashCode()
    {
        return x.GetHashCode() ^ y.GetHashCode() << 2;
    }

    public override bool Equals(object other)
    {
        if (!(other is IVector2))
        {
            return false;
        }
        IVector2 vector = (IVector2)other;
        return x == vector.x &&
               y == vector.y;
    }

    public override string ToString()
    {
        return string.Format("Vector2i({0} {1}", x, y);
    }

    public static bool operator ==(IVector2 a, IVector2 b)
    {
        if (ReferenceEquals(a, b)) return true;
        if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return false;

        return a.x == b.x &&
               a.y == b.y;
    }

    public static bool operator !=(IVector2 a, IVector2 b)
    {
        if (ReferenceEquals(a, b)) return false;
        if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return true;

        return a.x != b.x ||
               a.y != b.y;
    }

    public static IVector2 operator -(IVector2 a, IVector2 b)
    {
        return new IVector2(a.x - b.x, a.y - b.y);
    }

    public static IVector2 operator +(IVector2 a, IVector2 b)
    {
        return new IVector2(a.x + b.x, a.y + b.y);
    }

    public static IVector2 operator +(IVector2 a, int b)
    {
        return new IVector2(a.x + b, a.y + b);
    }

    public static IVector2 operator *(IVector2 a, int d)
    {
        return new IVector2(a.x * d, a.y * d);
    }

    public static IVector2 operator *(int d, IVector2 a)
    {
        return new IVector2(a.x * d, a.y * d);
    }

    public static IVector2 operator /(IVector2 a, int d)
    {
        return new IVector2(a.x / d, a.y / d);
    }

    public static IVector2 operator %(IVector2 a, int d)
    {
        return new IVector2(a.x % d, a.y % d);
    }

    public static explicit operator Vector2(IVector2 v)
    {
        return new Vector2(v.x, v.y);
    }

    public static explicit operator Vector3(IVector2 v)
    {
        return new Vector3(v.x, v.y, 0);
    }

    public static explicit operator IVector2(Vector3 v)
    {
        return new IVector2(v);
    }

    public static IVector2 Min(IVector2 lhs, IVector2 rhs)
    {
        return new IVector2(Mathf.Min(lhs.x, rhs.x), Mathf.Min(lhs.y, rhs.y));
    }

    public static IVector2 Max(IVector2 lhs, IVector2 rhs)
    {
        return new IVector2(Mathf.Max(lhs.x, rhs.x), Mathf.Max(lhs.y, rhs.y));
    }

    public static IVector2 Clamp(IVector2 value, IVector2 min, IVector2 max)
    {
        return new IVector2(Mathf.Clamp(value.x, min.x, max.x), Mathf.Clamp(value.y, min.y, max.y));
    }
}

public static class IVector2Extension
{
	public static IVector2 Clamp(this IVector2 value, IVector2 min, IVector2 max)
	{
		return new IVector2(Mathf.Clamp(value.x, min.x, max.x), Mathf.Clamp(value.y, min.y, max.y));
	}

	public static IVector2 WithX(this IVector2 v, int x)
    {
        return new IVector2(x, v.y);
    }

    public static IVector2 WithY(this IVector2 v, int y)
    {
        return new IVector2(v.x, y);
    }
}