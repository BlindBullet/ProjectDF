using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public Transform PlayerTrf;
    public Animator Anim;
	float playerScaleX = 10f;

	private void Start()
	{
		playerScaleX = PlayerTrf.localScale.x;		
	}

	public void SetDirection(Vector2 dir, JoystickState state)
	{
		float angle = Mathf.Atan2(dir.x, dir.y);
		float degree = (angle * 180) / Mathf.PI;
		
		if (degree < 0)
			degree += 360;

		float reDegree = 360 - degree;

		//switch (state)
		//{
		//	case JoystickState.EndDrag:
		//		if (reDegree >= 315f || reDegree <= 45f)
		//		{
		//			if (!Anim.GetCurrentAnimatorStateInfo(0).IsName("B_Idle"))
		//				Anim.SetTrigger("B_Idle");
		//		}
		//		else if (reDegree >= 45f && reDegree <= 135f)
		//		{
		//			if (!Anim.GetCurrentAnimatorStateInfo(0).IsName("S_Idle"))
		//				Anim.SetTrigger("S_Idle");
		//		}
		//		else if (reDegree >= 135f && reDegree <= 225f)
		//		{
		//			if (!Anim.GetCurrentAnimatorStateInfo(0).IsName("F_Idle"))
		//				Anim.SetTrigger("F_Idle");
		//		}
		//		else if (reDegree >= 225f && reDegree <= 315f)
		//		{
		//			if (!Anim.GetCurrentAnimatorStateInfo(0).IsName("S_Idle"))
		//				Anim.SetTrigger("S_Idle");
		//		}
		//		break;
		//	default:				
		//		if (reDegree >= 315f || reDegree <= 45f)
		//		{
		//			if (!Anim.GetCurrentAnimatorStateInfo(0).IsName("B_Walk"))
		//				Anim.SetTrigger("B_Walk");
		//		}
		//		else if (reDegree >= 45f && reDegree <= 135f)
		//		{
		//			PlayerTrf.localScale = PlayerTrf.localScale.WithX(-playerScaleX);
		//			if (!Anim.GetCurrentAnimatorStateInfo(0).IsName("S_Walk"))
		//				Anim.SetTrigger("S_Walk");
		//		}
		//		else if (reDegree >= 135f && reDegree <= 225f)
		//		{
		//			if (!Anim.GetCurrentAnimatorStateInfo(0).IsName("F_Walk"))
		//				Anim.SetTrigger("F_Walk");
		//		}
		//		else if (reDegree >= 225f && reDegree <= 315f)
		//		{
		//			PlayerTrf.localScale = PlayerTrf.localScale.WithX(playerScaleX);
		//			if (!Anim.GetCurrentAnimatorStateInfo(0).IsName("S_Walk"))
		//				Anim.SetTrigger("S_Walk");
		//		}
		//		break;
		//}

		switch (state)
		{
			case JoystickState.EndDrag:
				if (!Anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
					Anim.SetTrigger("Idle");
				break;
			default:
				if (!Anim.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
					Anim.SetTrigger("Walk");				
				break;
		}

		if (dir.x >= 0)
		{
			PlayerTrf.localScale = PlayerTrf.localScale.WithX(playerScaleX);
		}
		else
		{
			PlayerTrf.localScale = PlayerTrf.localScale.WithX(-playerScaleX);
		}
	}

}
