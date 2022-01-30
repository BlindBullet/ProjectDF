using System.IO;
using UnityEngine;
public static class DebugUtils
{
	public static void ShowExplorer(string itemPath)
	{
#if UNITY_EDITOR
		itemPath = itemPath.Replace(@"/", @"\");
		System.Diagnostics.Process.Start("explorer.exe", "/select," + itemPath);
#endif
	}
	static void OpenInMacFileBrowser(string path)
	{
#if UNITY_EDITOR
		bool openInsidesOfFolder = false;

		// try mac
		string macPath = path.Replace("\\", "/"); // mac finder doesn't like backward slashes

		if (Directory.Exists(macPath)) // if path requested is a folder, automatically open insides of that folder
		{
			openInsidesOfFolder = true;
		}

		//Debug.Log("macPath: " + macPath);
		//Debug.Log("openInsidesOfFolder: " + openInsidesOfFolder);

		if (!macPath.StartsWith("\""))
		{
			macPath = "\"" + macPath;
		}
		if (!macPath.EndsWith("\""))
		{
			macPath = macPath + "\"";
		}
		string arguments = (openInsidesOfFolder ? "" : "-R ") + macPath;
		//Debug.Log("arguments: " + arguments);
		try
		{
			System.Diagnostics.Process.Start("open", arguments);
		}
		catch (System.ComponentModel.Win32Exception e)
		{
			// tried to open mac finder in windows
			// just silently skip error
			// we currently have no platform define for the current OS we are in, so we resort to this
			e.HelpLink = ""; // do anything with this variable to silence warning about not using it
		}
#endif
	}

	static void OpenInWinFileBrowser(string path)
	{
#if UNITY_EDITOR
		bool openInsidesOfFolder = false;

		// try windows
		string winPath = path.Replace("/", "\\"); // windows explorer doesn't like forward slashes

		if (Directory.Exists(winPath)) // if path requested is a folder, automatically open insides of that folder
		{
			openInsidesOfFolder = true;
		}
		try
		{
			System.Diagnostics.Process.Start("explorer.exe", (openInsidesOfFolder ? "/root," : "/select,") + winPath);
		}
		catch (System.ComponentModel.Win32Exception e)
		{
			// tried to open win explorer in mac
			// just silently skip error
			// we currently have no platform define for the current OS we are in, so we resort to this
			e.HelpLink = ""; // do anything with this variable to silence warning about not using it
		}
#endif
	}

	public static void OpenInFileBrowser(string path)
	{
#if UNITY_EDITOR
		if (Application.platform == RuntimePlatform.OSXEditor)
		{
			OpenInMacFileBrowser(path);

		}
		else
		{
			OpenInWinFileBrowser(path);
		}
#endif
	}

	public static void OpenScriptFile(string path, int lineIndex, string key)
	{
#if UNITY_EDITOR
		string polishedPath = path.Substring(path.IndexOf(key)).Replace("\\", "/");
		Object asset = UnityEditor.AssetDatabase.LoadMainAssetAtPath(polishedPath);
		if (asset != null)
		{
			UnityEditor.AssetDatabase.OpenAsset(asset, lineIndex);
		}
		else
		{
			UnityEngine.Debug.Log("File Not Found:" + polishedPath);
		}
#endif
	}


	static public void DrawGizmoString(string text, Vector3 worldPos, IVector2 pixelOffset, Color? colour = null, int fontSize = 20)
	{

#if UNITY_EDITOR
		UnityEditor.Handles.BeginGUI();

		var restoreColor = GUI.color;

		if (colour.HasValue) GUI.color = colour.Value;
		GUI.skin.label.fontSize = fontSize;
		var view = UnityEditor.SceneView.currentDrawingSceneView;
		if (view == null)
		{
			return;
		}
		Vector3 screenPos = view.camera.WorldToScreenPoint(worldPos);

		if (screenPos.y < 0 || screenPos.y > Screen.height || screenPos.x < 0 || screenPos.x > Screen.width || screenPos.z < 0)
		{
			GUI.color = restoreColor;
			UnityEditor.Handles.EndGUI();
			return;
		}

		UnityEditor.Handles.Label(TransformByPixel(worldPos, pixelOffset.x, pixelOffset.y), text);

		GUI.color = restoreColor;
		UnityEditor.Handles.EndGUI();
#endif
	}

	static Vector3 TransformByPixel(Vector3 position, float x, float y)
	{
		return TransformByPixel(position, new Vector3(x, y));
	}

	static Vector3 TransformByPixel(Vector3 position, Vector3 translateBy)
	{
#if UNITY_EDITOR
		Camera cam = UnityEditor.SceneView.currentDrawingSceneView.camera;
		if (cam)
			return cam.ScreenToWorldPoint(cam.WorldToScreenPoint(position) + translateBy);
		else
			return position;
#endif
		return Vector3.zero;
	}

}