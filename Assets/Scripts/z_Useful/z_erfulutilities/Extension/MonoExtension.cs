using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
#endif
public static class MonoExtension
{
	public static bool IsGameMode(this MonoBehaviour obj)
	{
#if UNITY_EDITOR
		if (!Application.isPlaying)
		{
			return false;
		}

		if (obj.gameObject.scene.rootCount == 0)
		{
			return false;
		}
		return PrefabStageUtility.GetCurrentPrefabStage() == null;
#endif
		return true;
	}
}