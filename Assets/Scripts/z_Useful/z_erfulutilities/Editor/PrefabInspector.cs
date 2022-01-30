using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEngine;

public static class EditorObjectStatus
{
	public static bool CheckPrefabInstance(GameObject gameObject)
	{
		return PrefabUtility.GetPrefabParent(gameObject) != null && PrefabUtility.GetPrefabObject(gameObject.transform) != null;
	}

	public static bool CheckPrefabOriginal(GameObject gameObject)
	{
		return PrefabUtility.GetPrefabParent(gameObject) == null && PrefabUtility.GetPrefabObject(gameObject.transform) != null;
	}

	public static bool CheckDisconnectedPrefabInstance(GameObject gameObject)
	{
		return PrefabUtility.GetPrefabParent(gameObject) != null && PrefabUtility.GetPrefabObject(gameObject.transform) == null;
	}
}

public class PrefabInspector : EditorWindow
{
	private bool _isFocused = false;

	private static bool _isInspectorSelected = true;

	private string prefabPath = "";

	private GameObject inspectObject;

	private Editor goEditor;

	private List<Editor> componentEditors = new List<Editor>();

	//[MenuItem("Shortcuts/FocusPrefabInspector &#v")]
	//static void FocusPrefabInspector()
	//{
	//	_isInspectorSelected = !_isInspectorSelected;
	//	if (_isInspectorSelected)
	//	{
	//		EditorWindowUtility.ShowEditorWindow(EditorWindowUtility.Inspector);
	//	}
	//	else
	//	{
	//		var inspector = EditorWindow.GetWindow<PrefabInspector>();
	//		inspector.Focus();
	//		inspector.OnSelectionChange();
	//	}
	//}

	private void OnEnable()
	{
		//Selection.selectionChanged += SelectionChanged;
		OnSelectionChange();
	}

	private void OnDisable()
	{
		//Selection.selectionChanged -= SelectionChanged;
		CleanupExisting();
	}

	private void SelectionChanged()
	{
		//Automatically focus window (Buggy)
		var focusedWindow = EditorWindow.focusedWindow.GetType().ToString();
		var selection = Selection.gameObjects;
		if (selection == null || selection.Length > 1 || selection.Length == 0 || !EditorObjectStatus.CheckPrefabOriginal(selection[0]))
		{
			// To default inspector
			EditorWindowUtility.ShowEditorWindow(EditorWindowUtility.Inspector);
			EditorWindowUtility.ShowEditorWindow(focusedWindow);
		}
		else
		{
			EditorWindow.GetWindow<PrefabInspector>().Focus();
			if (!_isFocused)
			{
				OnSelectionChange();
			}
			EditorWindowUtility.ShowEditorWindow(focusedWindow);
		}
	}

	private void CleanupExisting()
	{//release memory etc
		if (goEditor != null)
		{
			DestroyImmediate(goEditor);
		}
		if (componentEditors != null)
		{
			foreach (Editor componentEditor in componentEditors)
			{
				if (componentEditor != null)
				{
					DestroyImmediate(componentEditor);
				}
			}
			componentEditors.Clear();
		}
		if (inspectObject != null)
		{
			if (prefabPath != "")
			{//prefab utility save it
				PrefabUtility.SaveAsPrefabAsset(inspectObject, prefabPath);
			}
			PrefabUtility.UnloadPrefabContents(inspectObject);
		}
		prefabPath = "";
		inspectObject = null;
		_isFocused = false;
	}

	private void OnSelectionChange()
	{
		CleanupExisting();
		if (Selection.activeObject == null || !(Selection.activeObject is GameObject))
		{
			return;
		}
		foreach (GameObject go in Selection.GetFiltered(typeof(GameObject), SelectionMode.Assets))
		{
			prefabPath = AssetDatabase.GetAssetPath(go);
			if (!string.IsNullOrEmpty(prefabPath) && File.Exists(prefabPath) && prefabPath.EndsWith(".prefab"))
			{//selected is a prefab, ideally in project view?
			 //inspectObject = go; //For a read-only viewer, uncomment this and instead comment out the line below as well as the saving/unloading code in CleanupExisting
				inspectObject = PrefabUtility.LoadPrefabContents(prefabPath); //Something like this to load, then code to save in CleanupExisting
				if (inspectObject != null)
				{
					goEditor = Editor.CreateEditor(inspectObject);
					if (goEditor == null)
					{
						Debug.LogError("NullEditor...");
					}
					else
					{
						if (componentEditors == null)
						{//todo: reinit
							break;
						}
						foreach (Component c in inspectObject.GetComponents<Component>())
						{
							if (c is UnityEngine.UI.Image)
							{
								Editor componentEditor = Editor.CreateEditor(c as UnityEngine.UI.Image);
								if (componentEditor != null)
								{
									componentEditors.Add(componentEditor);
									_isFocused = true;
								}
							}
							else
							{
								Editor componentEditor = Editor.CreateEditor(c);
								if (componentEditor != null)
								{
									componentEditors.Add(componentEditor);
									_isFocused = true;
								}
							}
						}
					}
					break;
				}
			}
		}
		Repaint();//force OnGUI redraw
	}

	private void OnGUI()
	{
		if (goEditor == null)
		{
			EditorGUILayout.LabelField("No prefab selected");
			return;
		}
		else
		{
			goEditor.DrawHeader();
			// goEditor.OnInspectorGUI(); //seems to do nothing
			//goEditor.DrawDefaultInspector(); //lists just object properties with no proper format
			if (componentEditors == null)
			{
				return;
			}
			foreach (Editor componentEditor in componentEditors)
			{
				componentEditor.DrawHeader();//seems to draw component icon but gameobject's name, and wider than normal.
				EditorGUILayout.LabelField("Component: " + componentEditor.target.GetType().Name); //Show component type, since DrawHeader doesn't list it.
				componentEditor.OnInspectorGUI();
				//componentEditor.DrawDefaultInspector(); //lists with no proper format
			}
		}
	}
}

internal static class EditorWindowUtility
{

	//UnityEditor.ProjectBrowser

	public const string Project = "UnityEditor.ProjectBrowser";
	public const string Inspector = "UnityEditor.InspectorWindow";
	public const string Scene = "UnityEditor.SceneView";

	public static void ShowEditorWindow(string windowTypeName)
	{
		var windowType = typeof(Editor).Assembly.GetType(windowTypeName);
		EditorWindow.GetWindow(windowType);
	}
}