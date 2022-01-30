using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Varible Wrapper(int, double, etc only)
/// </summary>
public class CustomValueType<TCustom, TValue> where TValue : struct
{
    protected readonly TValue _value;

    public CustomValueType(TValue value)
    {
        _value = value;
    }

    public override string ToString()
    {
        return _value.ToString();
    }

    public static bool operator <(CustomValueType<TCustom, TValue> a, CustomValueType<TCustom, TValue> b)
    {
        return Comparer<TValue>.Default.Compare(a._value, b._value) < 0;
    }

    public static bool operator >(CustomValueType<TCustom, TValue> a, CustomValueType<TCustom, TValue> b)
    {
        return !(a < b);
    }

    public static bool operator <=(CustomValueType<TCustom, TValue> a, CustomValueType<TCustom, TValue> b)
    {
        return (a < b) || (a == b);
    }

    public static bool operator >=(CustomValueType<TCustom, TValue> a, CustomValueType<TCustom, TValue> b)
    {
        return (a > b) || (a == b);
    }

    public static bool operator ==(CustomValueType<TCustom, TValue> a, CustomValueType<TCustom, TValue> b)
    {
        return a.Equals((object)b);
    }

    public static bool operator !=(CustomValueType<TCustom, TValue> a, CustomValueType<TCustom, TValue> b)
    {
        return !(a == b);
    }

    protected bool Equals(CustomValueType<TCustom, TValue> other)
    {
        return EqualityComparer<TValue>.Default.Equals(_value, other._value);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((CustomValueType<TCustom, TValue>)obj);
    }

    public override int GetHashCode()
    {
        return EqualityComparer<TValue>.Default.GetHashCode(_value);
    }
}

/*
//Example
public class ServerId : CustomValueType<ServerId, Int32>
{
    private ServerId(int value) : base(value) { }
    public static implicit operator ServerId(int value) { return new ServerId(value); }
    public static implicit operator int(ServerId custom) { return custom._value; }
}

public class TableId : CustomValueType<ServerId, Int32>
{
    private TableId(int value) : base(value) { }
    public static implicit operator TableId(int value) { return new TableId(value); }
    public static implicit operator int(TableId custom) { return custom._value; }
}
*/