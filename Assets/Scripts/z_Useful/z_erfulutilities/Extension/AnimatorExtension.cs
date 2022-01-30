using UnityEngine;

public static class AnimatorExtension
{
	public static Vector3 X(float v)
	{
		return new Vector3(v, 0, 0);
	}

	public static Vector3 Y(float v)
	{
		return new Vector3(0, v, 0);
	}

	public static void ApplyTrigger(this Animator animator, string key)
	{
		foreach (AnimatorControllerParameter p in animator.parameters)
			if (p.type == AnimatorControllerParameterType.Trigger)
				animator.ResetTrigger(p.name);
		animator.SetTrigger(key);
	}

	public static void ApplyTrigger(this Animator animator, int key)
	{
		foreach (AnimatorControllerParameter p in animator.parameters)
			if (p.type == AnimatorControllerParameterType.Trigger)
				animator.ResetTrigger(p.name);
		animator.SetTrigger(key);
	}
}