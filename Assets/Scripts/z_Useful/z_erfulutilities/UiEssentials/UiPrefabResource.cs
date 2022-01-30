using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;
#if UNITY_EDITOR

using UnityEditor;

#endif

//TODO : 에셋 필요없이 잘되도록 수정할 것

//[CreateAssetMenu(menuName = "GameSystem/UiPrefabResource")]
[ResourcePath("GameSystem/UiPrefabResource")]
public class UiPrefabResource : ResourceSingleton<UiPrefabResource>
{
	[ReadOnly]
	public string DialogDirectory = "Assets/Prefabs/UI/";

	[SerializeField]
	public UiCamera CameraPrefab;
	
	[SerializeField]
	private List<UiViewBase> Uis = new List<UiViewBase>();

	
	private Dictionary<Type, UiViewBase> __dict;

	public Dictionary<Type, UiViewBase> Prefabs
	{
		get
		{
			if (__dict == null)
			{
				__dict = new Dictionary<Type, UiViewBase>();
				foreach (var elem in Uis)
				{
					__dict.AddSafe(elem.GetType(), elem);
				}
			}
			return __dict;
		}
	}

	public UiViewBase GetDialogPrefab(Type type)
	{
		return Prefabs.Get(type);
	}

	void OnValidate()
	{
		DialogPrefabSaved();
	}

	public void DialogPrefabSaved()
	{
#if UNITY_EDITOR
		//string[] filePaths = Directory.GetFiles(DialogDirectory.RemoveFromEnd("/"));
		Uis.Clear();
		foreach (var elem in Directory.EnumerateFiles(DialogDirectory.RemoveFromEnd("/"), "*.*", SearchOption.AllDirectories))
		{
			var obj = AssetDatabase.LoadAssetAtPath<GameObject>(elem);
			if (obj == null)
			{
				continue;
			}
			var dialog = obj.GetComponent<UiViewBase>();
			if (dialog != null)
			{
				Uis.Add(dialog);
			}
		}
		EditorUtility.SetDirty(this);
		AssetDatabase.SaveAssets();
#endif
	}
}
