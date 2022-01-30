public struct Pair<T1, T2>
{
    public Pair(T1 item1, T2 item2)
    {
        Key = item1;
        Val = item2;
    }

    public T1 Key { get; }

    public T2 Val { get; }

    public override string ToString()
    {
        return string.Format("Pair({0}, {1})", Key, Val);
    }
}