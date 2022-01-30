using UnityEngine;
using UnityEngine.UI;


#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UI;

[CanEditMultipleObjects, CustomEditor(typeof(NonDrawingGraphic), false)]
public class NonDrawingGraphicEditor : GraphicEditor
{
	public override void OnInspectorGUI()
	{
		base.serializedObject.Update();
		EditorGUILayout.PropertyField(base.m_Script, new GUILayoutOption[0]);
		// skipping AppearanceControlsGUI
		base.RaycastControlsGUI();
		base.serializedObject.ApplyModifiedProperties();
	}
}
#endif

/// A concrete subclass of the Unity UI `Graphic` class that just skips drawing.
/// Useful for providing a raycast target without actually drawing anything.
public class NonDrawingGraphic : Graphic
{
	public override void SetMaterialDirty() { return; }
	public override void SetVerticesDirty() { return; }

	/// Probably not necessary since the chain of calls `Rebuild()`->`UpdateGeometry()`->`DoMeshGeneration()`->`OnPopulateMesh()` won't happen; so here really just as a fail-safe.
	protected override void OnPopulateMesh(VertexHelper vh)
	{
		vh.Clear();
		return;
	}
}

