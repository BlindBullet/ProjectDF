using UnityEngine;

[System.Serializable]
public struct IVector3
{
    public int x, y, z;
    public static readonly IVector3 zero = new IVector3(0, 0, 0);
    public static readonly IVector3 one = new IVector3(1, 1, 1);
    public static readonly IVector3 forward = new IVector3(0, 0, 1);
    public static readonly IVector3 back = new IVector3(0, 0, -1);
    public static readonly IVector3 up = new IVector3(0, 1, 0);
    public static readonly IVector3 down = new IVector3(0, -1, 0);
    public static readonly IVector3 left = new IVector3(-1, 0, 0);
    public static readonly IVector3 right = new IVector3(1, 0, 0);

	public IVector3(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }


    public IVector3(int x, int y)
    {
        this.x = x;
        this.y = y;
		this.z = 0;
	}


    public IVector3(IVector3 a)
    {
        this.x = a.x;
        this.y = a.y;
        this.z = a.z;
    }


    public IVector3(Vector3 a)
    {
        this.x = (int)a.x;
        this.y = (int)a.y;
        this.z = (int)a.z;
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
                case 2:
                    {
                        return z;
                    }
                default:
                    {
                        Debug.LogError("Invalid Vector3i index value of: " + key);
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
                case 2:
                    {
                        z = value;
                        return;
                    }
                default:
                    {
                        Debug.LogError("Invalid Vector3i index value of: " + key);
                        return;
                    }
            }
        }
    }


    public static IVector3 Scale(IVector3 a, IVector3 b)
    {
        return new IVector3(a.x * b.x, a.y * b.y, a.z * b.z);
    }


    public static int DistanceSquared(IVector3 a, IVector3 b)
    {
        int dx = b.x - a.x;
        int dy = b.y - a.y;
        int dz = b.z - a.z;
        return dx * dx + dy * dy + dz * dz;
    }


    public int DistanceSquared(IVector3 v)
    {
        return DistanceSquared(this, v);
    }


    public override int GetHashCode()
    {
        return x.GetHashCode() ^ y.GetHashCode() << 2 ^ z.GetHashCode() >> 2;
    }


    public override bool Equals(object other)
    {
        if (!(other is IVector3))
        {
            return false;
        }
        IVector3 vector = (IVector3)other;
        return x == vector.x &&
               y == vector.y &&
               z == vector.z;
    }


    public override string ToString()
    {
        return string.Format("Vector3i({0} {1} {2})", x, y, z);
    }


    public static bool operator ==(IVector3 a, IVector3 b)
    {
        return a.x == b.x &&
               a.y == b.y &&
               a.z == b.z;
    }


    public static bool operator !=(IVector3 a, IVector3 b)
    {
        return a.x != b.x ||
               a.y != b.y ||
               a.z != b.z;
    }


    public static IVector3 operator -(IVector3 a, IVector3 b)
    {
        return new IVector3(a.x - b.x, a.y - b.y, a.z - b.z);
    }


    public static IVector3 operator +(IVector3 a, IVector3 b)
    {
        return new IVector3(a.x + b.x, a.y + b.y, a.z + b.z);
    }


    public static IVector3 operator *(IVector3 a, int d)
    {
        return new IVector3(a.x * d, a.y * d, a.z * d);
    }


    public static IVector3 operator *(int d, IVector3 a)
    {
        return new IVector3(a.x * d, a.y * d, a.z * d);
    }


    public static explicit operator Vector3(IVector3 v)
    {
        return new Vector3(v.x, v.y, v.z);
    }

    public static explicit operator IVector3(Vector3 v)
    {
        return new IVector3(v);
    }


    public static IVector3 Min(IVector3 lhs, IVector3 rhs)
    {
        return new IVector3(Mathf.Min(lhs.x, rhs.x), Mathf.Min(lhs.y, rhs.y), Mathf.Min(lhs.z, rhs.z));
    }


    public static IVector3 Max(IVector3 lhs, IVector3 rhs)
    {
        return new IVector3(Mathf.Max(lhs.x, rhs.x), Mathf.Max(lhs.y, rhs.y), Mathf.Max(lhs.z, rhs.z));
    }


    public static IVector3 Clamp(IVector3 value, IVector3 min, IVector3 max)
    {
        return new IVector3(Mathf.Clamp(value.x, min.x, max.x), Mathf.Clamp(value.y, min.y, max.y), Mathf.Clamp(value.z, min.z, max.z));
    }
}

public static class IVector3Extension
{
	public static IVector3 Clamp(this IVector3 value, IVector3 min, IVector3 max)
	{
		return new IVector3(Mathf.Clamp(value.x, min.x, max.x), Mathf.Clamp(value.y, min.y, max.y), Mathf.Clamp(value.z, min.z, max.z));
	}

	public static IVector3 WithX(this IVector3 v, int n)
	{
		return new IVector3(n, v.y, v.z);
	}

	public static IVector3 WithY(this IVector3 v, int n)
	{
		return new IVector3(v.x, n, v.z);
	}

	public static IVector3 WithZ(this IVector3 v, int n)
	{
		return new IVector3(v.x, v.y, n);
	}
}