///// <summary>
///// Stick this on a method
///// </summary>
//[System.AttributeUsage(System.AttributeTargets.Method)]
//public class EditorButtonAttribute : PropertyAttribute
//{
//}

//[CustomEditor(typeof (MonoBehaviour), true)]
//public class EditorButton : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();

//        var mono = target as MonoBehaviour;

//        var methods = mono.GetType()
//            .GetMembers(BindingFlags.Instance | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
//            .Where(o => Attribute.IsDefined(o, typeof (EditorButtonAttribute)));

//        foreach (var memberInfo in methods)
//        {
//            if (GUILayout.Button(memberInfo.Name))
//            {
//                var method = memberInfo as MethodInfo;
//                method.ChangeState(mono, null);
//            }
//        }
//    }
//}

namespace Zutilities
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using UnityEditor;
	using UnityEngine;

	[CustomEditor(typeof(MonoBehaviour), true, isFallback = true)]
	[CanEditMultipleObjects]
	public class BehaviourButtonsEditor : Editor
	{
		private CustomButtonAttributeHelper helper = new CustomButtonAttributeHelper();

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			helper.DrawButtons();
		}

		private void OnEnable()
		{
			helper.Init(this.target);
		}
	}

	/// <summary>
	/// Put this attribute above one of your MonoBehaviour method and it will draw
	/// a button in the inspector automatically.
	///
	/// NOTE: the method must not have any params and can not be static.
	///
	/// <code>
	/// <para>[Button]</para>
	/// <para>public void MyMethod()</para>
	/// <para>{</para>
	/// <para>    Debug.Log( "HELLO HELLO HELLO!!" );</para>
	/// <para>}</para>
	/// </code>
	/// </summary>
	[System.AttributeUsage(System.AttributeTargets.Method)]
	public class CustomButtonAttribute : System.Attribute
	{
	}

	/// <summary>
	/// Searches through a target class in order to find all button attributes (<see cref="CustomButtonAttribute"/>).
	/// </summary>
	public class CustomButtonAttributeHelper
	{
		private static object[] emptyParamList = new object[0];

		private IList<MethodInfo> methods = new List<MethodInfo>();
		private System.Object targetObject;

		public void Init(System.Object targetObject)
		{
			this.targetObject = targetObject;
			methods =
				targetObject.GetType()
					.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
					.Where(m =>
							m.GetCustomAttributes(typeof(CustomButtonAttribute), false).Length == 1 &&
							m.GetParameters().Length == 0 &&
							!m.ContainsGenericParameters
					).ToList();
		}

		public void DrawButtons()
		{
			if (methods.Count > 0)
			{
				EditorGUILayout.HelpBox("Click to execute methods!", MessageType.None);
				foreach (MethodInfo method in methods)
				{
					string buttonText = ObjectNames.NicifyVariableName(method.Name);
					if (GUILayout.Button(buttonText))
					{
						method.Invoke(targetObject, emptyParamList);
					}
				}
			}
		}


	}
}

