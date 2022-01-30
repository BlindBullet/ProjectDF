using System;
using System.Collections.Generic;
using System.Text;

public class Reusable<T> : IDisposable where T : class, new()
{
    private static readonly Stack<Reusable<T>> Pool = new Stack<Reusable<T>>();
    public readonly T Value = new T();

    protected Reusable()
    {
    }

    public void Dispose()
    {
        Pool.Push(this);
    }

    protected static Reusable<T> DoPop() => Pool.Count == 0 ? new Reusable<T>() : Pool.Pop();

    public static implicit operator T(Reusable<T> reusable) => reusable.Value;
}

public class ReusableStringBuilder : Reusable<StringBuilder>
{
    public static Reusable<StringBuilder> Pop()
    {
        Reusable<StringBuilder> reusable = DoPop();
        reusable.Value.Length = 0;
        return reusable;
    }
}

public class ReusableList<T> : Reusable<List<T>>
{
    public static Reusable<List<T>> Pop()
    {
        Reusable<List<T>> reusable = DoPop();
        reusable.Value.Clear();
        return reusable;
    }
}

public class ReusableStack<T> : Reusable<Stack<T>>
{
    public static Reusable<Stack<T>> Pop()
    {
        Reusable<Stack<T>> reusable = DoPop();
        reusable.Value.Clear();
        return reusable;
    }
}