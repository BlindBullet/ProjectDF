using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.U2D;

public class SuppliesBase : MonoBehaviour
{
	public Transform IconTrf;
	public SpriteRenderer IconImg;
	public Transform RightWing;
	public Transform LeftWing;
	SuppliesChart data;
	Vector2 min;
	Vector2 max;
	Coroutine cMove = null;
	Material mat;
	bool isClick = false;

	public void Setup(SuppliesChart chart)
	{
		isClick = false;
		data = chart;
		IconImg.sprite = Resources.Load<SpriteAtlas>("Sprites/Icons").GetSprite(chart.Icon);
		IconImg.transform.localScale = new Vector2(chart.IconSize, chart.IconSize);
		WingsMove();

		min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
		max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

		SoundManager.Ins.PlaySFX("AppearAngel");

		SetPos();
		Move();
	}

	void SetPos()
	{
		int randDir = Random.Range(0, 2);

		if(randDir == 0)
		{
			this.transform.position = new Vector2(min.x - 5f, Random.Range(6.6f, 7.7f));
		}
		else
		{
			this.transform.position = new Vector2(max.x + 5f, Random.Range(6.6f, 7.7f));
		}
	}

	void WingsMove()
	{
		RightWing.DORotate(new Vector3(0, 0, -20f), 0.5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo).SetId("SupWingRight");
		LeftWing.DORotate(new Vector3(0, 0, 20f), 0.5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo).SetId("SupWingLeft");
	}
	
	void Move()
	{
		if (cMove == null)
			cMove = StartCoroutine(MoveSequence());
	}

	void StopMove()
	{
		if (cMove != null)
		{
			StopCoroutine(cMove);
			cMove = null;
		}	
	}

	IEnumerator MoveSequence()
	{
		IconTrf.DOLocalMoveY(IconTrf.localPosition.y + 0.1f, 0.5f).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
		
		if (transform.position.x > max.x)
		{
			transform.DOMove(new Vector2(max.x - 1f, transform.position.y + Random.Range(-1f, 1f)), 2f).SetEase(Ease.InOutQuad).SetId("SupMoveInRight");			
		}
		else if (transform.position.x < min.x)
		{
			transform.DOMove(new Vector2(min.x + 1f, transform.position.y + Random.Range(-1f, 1f)), 2f).SetEase(Ease.InOutQuad).SetId("SupMoveInLeft");			
		}

		yield return new WaitForSeconds(1.8f);

		int count = Random.Range(3,5);

		for (int i = 0; i < count; i++)
		{	
			float destX = 0f;
			float destY = 0f;

			if (transform.position.x < 0f)
			{	
				destX = Random.Range(max.x - 1.5f, max.x - 0.5f);				
			}
			else if (transform.position.x > 0f)
			{	
				destX = Random.Range(min.x + 0.5f, min.x + 1.5f);				
			}

			if (transform.position.y + 2f > 8f)
			{
				destY = Random.Range(transform.position.y - 2f, transform.position.y - 1f);
			}
			else if (transform.position.y - 2f < 3f)
			{
				destY = Random.Range(transform.position.y + 1f, transform.position.y + 2f);
			}
			else
			{
				destY = Random.Range(transform.position.y - 1f, transform.position.y + 1f);
			}

			float time = 0f;

			if (i == count - 1)
			{
				time = 11f;

				if (transform.position.x < 0f)
				{
					destX = destX + 4f;
				}
				else if (transform.position.x > 0f)
				{
					destX = destX - 4f;
				}

				transform.DOMove(new Vector2(destX, destY), time).SetEase(Ease.Linear).SetId("");
			}
			else
			{
				time = 8f;
				transform.DOMove(new Vector2(destX, destY), time).SetEase(Ease.Linear);
			}

			yield return new WaitForSeconds(time);
		}

		StopMove();			
		transform.position = new Vector3(-100f, 0, 0);
		this.gameObject.SetActive(false);
	}

	public void GetReward()
	{	
		StopMove();

		if (!isClick)
		{
			DialogManager.Ins.OpenAdReward(data);
			isClick = true;
		}

		this.gameObject.SetActive(false);		
	}

	private void OnDisable()
	{
		ObjectPooler.ReturnToPool(this.gameObject);
	}

}
