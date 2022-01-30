#if UNITY_EDITOR

using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

internal static class EditorMenus
{
	//[MenuItem("Shortcuts/Apply Prefab Changes %#g")]
	//static public void ApplyPrefabChanges()
	//{
	//	var obj = Selection.activeGameObject;
	//	if (obj != null)
	//	{
	//		var prefab_root = PrefabUtility.FindPrefabRoot(obj);
	//		var prefab_src = PrefabUtility.GetCorrespondingObjectFromSource(prefab_root);
	//		if (prefab_src != null)
	//		{
	//			PrefabUtility.ReplacePrefab(prefab_root, prefab_src, ReplacePrefabOptions.ConnectToPrefab);
	//		}
	//	}
	//}

	[MenuItem("Tools/Apply all selected prefabs %#g", false, 5)]
	static void ApplyPrefabs()
	{
		foreach (GameObject go in Selection.gameObjects)
		{
			PrefabUtility.ApplyPrefabInstance(go, InteractionMode.UserAction);
		}
	}

	[MenuItem("Tools/Revert all selected prefabs %#z", false, 6)]
	static void ResetPrefabs()
	{
		foreach (GameObject go in Selection.gameObjects)
		{
			PrefabUtility.RevertObjectOverride(go, InteractionMode.UserAction);
		}
	}

	[MenuItem("Tools/Raycast ui graphic recursively %#r")]
	static void SetRaycastOffRecursively()
	{
		foreach (GameObject sel in Selection.gameObjects)
		{
			
			foreach (var rf in sel.GetComponentsInChildren<Graphic>())
			{
				rf.raycastTarget = false;
				Debug.Log("didit");
			}
		}
	}

	[MenuItem("Shortcuts/Find selected asset in project window &#b")]
	static public void PingSelectedObject()
	{
		EditorGUIUtility.PingObject(Selection.activeObject);
	}

	// taken from: http://answers.unity3d.com/questions/282959/set-inspector-lock-by-code.html
	[MenuItem("Shortcuts/Toggle Inspector Lock %h")] // Ctrl + H
	private static void ToggleInspectorLock()
	{
		ActiveEditorTracker.sharedTracker.isLocked = !ActiveEditorTracker.sharedTracker.isLocked;
		ActiveEditorTracker.sharedTracker.ForceRebuild();
	}

	[MenuItem("Shortcuts/ActiveToggle %g")]
	private static void ToggleActivationSelection()
	{
		var go = Selection.activeGameObject;
		go.SetActive(!go.activeSelf);
	}

	[MenuItem("Shortcuts/Run _F5")]
	private static void SetEditorPlayMode()
	{
		EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), "", false);
		EditorApplication.ExecuteMenuItem("Edit/Play");
	}

	[MenuItem("Shortcuts/Pause #_F5")]
	private static void SetEditorPauseMode()
	{
		Debug.Break();
	}

	[MenuItem("Shortcuts/OpenDataSourceFolder %#u")] // Ctrl + Shift
	private static void OpenDataExcelSourceFolder()
	{
		DebugUtils.OpenInFileBrowser(Application.dataPath + "/DataTables~");
	}


	[MenuItem("Shortcuts/ClearSearchBar %&#b")] 
	private static void ClearSearchBar()
	{
		var pb = Type.GetType("UnityEditor.ProjectBrowser,UnityEditor");
		var ins = pb.GetField("s_LastInteractedProjectBrowser", BindingFlags.Static | BindingFlags.Public).GetValue(null);
		var method = pb.GetMethod("ClearSearch", BindingFlags.NonPublic | BindingFlags.Instance);
		method.Invoke(ins, null);
	}



}

#endif