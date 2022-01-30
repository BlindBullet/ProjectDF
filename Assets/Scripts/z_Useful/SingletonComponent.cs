using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class SingletonComponent<T> where T:class, new()
{
    static private T instance;

    static public T Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new T();
            }
            return instance;
        }
    }
}
