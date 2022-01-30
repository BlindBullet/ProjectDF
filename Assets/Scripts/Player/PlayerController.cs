using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public Transform PlayerTrf;
    public Animator Anim;
	float playerScaleX;

	private void Start()
	{
		playerScaleX = PlayerTrf.localScale.x;
		//Anim.SetTrigger("S_Idle");
	}

	public void SetDirection(Vector2 dir)
	{
		float angle = Mathf.Atan2(dir.x, dir.y);
		float degree = (angle * 180) / Mathf.PI;
		//Debug.Log(degree);
		if (degree < 0)
			degree += 360;

		float reDegree = 360 - degree;
				
		if (reDegree >= 315f || reDegree <= 45f)
		{
			if (!Anim.GetCurrentAnimatorStateInfo(0).IsName("B_Walk"))
				Anim.SetTrigger("B_Walk");
		}
		else if (reDegree >= 45f && reDegree <= 135f)
		{
			PlayerTrf.localScale = PlayerTrf.localScale.WithX(-playerScaleX);
			if (!Anim.GetCurrentAnimatorStateInfo(0).IsName("S_Walk"))
				Anim.SetTrigger("S_Walk");
		}
		else if (reDegree >= 135f && reDegree <= 225f)
		{
			if (!Anim.GetCurrentAnimatorStateInfo(0).IsName("F_Walk"))
				Anim.SetTrigger("F_Walk");
		}
		else if (reDegree >= 225f && reDegree <= 315f)
		{
			PlayerTrf.localScale = PlayerTrf.localScale.WithX(playerScaleX);
			if (!Anim.GetCurrentAnimatorStateInfo(0).IsName("S_Walk"))
				Anim.SetTrigger("S_Walk");
		}
	}

}
