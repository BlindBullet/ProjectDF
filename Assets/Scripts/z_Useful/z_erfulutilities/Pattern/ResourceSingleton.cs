using System;
using UnityEngine;

public abstract class ResourceSingleton<T> : ScriptableObject where T : ScriptableObject
{
	private static T _instance;

	public static string GetPath()
	{
		Type type = typeof(T);
		object[] pathAttrs = type.GetCustomAttributes(typeof(ResourcePathAttribute), true);
		for (int i = 0, iMax = pathAttrs.Length; i < iMax; i++)
		{
			var pathAttr = pathAttrs[i] as ResourcePathAttribute;
			if (pathAttr != null)
			{
				return pathAttr.Path;
			}
		}
		return string.Empty;
	}

	public static string GetResourcePath()
	{
		var path = GetPath();
		var lastIndex = path.LastIndexOf("/");
		if (lastIndex != 0)
		{
			path = path.Remove(lastIndex);
		}
		return path;
	}

	public static string GetDirectory()
	{
		return "Assets/Resources/" + GetResourcePath() + "/";
	}

	public static T Ins
	{
		get
		{
			if (_instance != null)
			{
				return _instance;
			}

			Type type = typeof(T);
			var path = GetPath();

			if (string.IsNullOrEmpty(path))
			{
				Debug.LogError(string.Format("{0}: Empty Resource Path\n. Use " +
					"<color=white>[<color=#4EC9B0FF>ResourcePath</color>(<color=#D69D62FF>\"Path\"</color>)]</color>", type.Name));
				return null;
			}

			var comp = Resources.Load<T>(path);
			if (comp == null)
			{
				Debug.LogError(string.Format("{0}: Wrong Resource Path - {1}", type.Name, path));
				return null;
			}

			_instance = comp;
			_instance.hideFlags = HideFlags.DontUnloadUnusedAsset;
			return _instance;
		}
	}


	public virtual void OnUpdateAsset() { }
}

public class ResourcePathAttribute : Attribute
{
	public ResourcePathAttribute(string path)
	{
		Path = path;
	}

	public string Path
	{
		get;
		private set;
	}
}