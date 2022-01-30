using System;
using System.Reflection;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
namespace ER.ScriptWizard
{
    public static class GenericUtility
    {
        /// <summary>
        /// can clone public field only
        /// </summary>
        public static T CloneJson<T>(this T source)
        {
            string temp = JsonUtility.ToJson(source);
            return JsonUtility.FromJson<T>(temp);
        }

        public static object CloneMemory(this object obj)
        {
            object objResult = null;
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter  bf =   new BinaryFormatter();
                bf.Serialize(ms, obj);

                ms.Position = 0;
                objResult = bf.Deserialize(ms);
            }
            return objResult;
        }

        public static T CloneField<T>(this T obj) where T : class
        {
            if (obj == null)
                throw new ArgumentNullException();
            return (T)FieldProcess(obj);
        }

        private static object FieldProcess(object obj)
        {
            if (obj == null)
                return null;
            Type type=obj.GetType();
            if (type.IsValueType || type == typeof(string))
            {
                return obj;
            }
            else if (type.IsArray)
            {
                Type elementType=Type.GetType(
                     type.FullName.Replace("[]",string.Empty));
                var array=obj as Array;
                Array copied=Array.CreateInstance(elementType,array.Length);
                for (int i = 0; i < array.Length; i++)
                {
                    copied.SetValue(FieldProcess(array.GetValue(i)), i);
                }
                return Convert.ChangeType(copied, obj.GetType());
            }
            else if (type.IsClass)
            {
                object toret = Activator.CreateInstance(obj.GetType());
                FieldInfo[] fields=type.GetFields(BindingFlags.Public| BindingFlags.NonPublic|BindingFlags.Instance);
                foreach (FieldInfo field in fields)
                {
                    object fieldValue=field.GetValue(obj);
                    if (fieldValue == null)
                        continue;
                    field.SetValue(toret, FieldProcess(fieldValue));
                }
                return toret;
            }
            else
                throw new ArgumentException("Unknown type");
        }

        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            else if (val.CompareTo(max) > 0) return max;
            else return val;
        }

    }
}