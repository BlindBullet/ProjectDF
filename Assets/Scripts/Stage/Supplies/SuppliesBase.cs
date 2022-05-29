using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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

	private void Start()
	{	
		SuppliesChart chart = CsvData.Ins.SuppliesChart[Random.Range(1, CsvData.Ins.SuppliesChart.Count).ToString()];
		Setup(chart);
	}

	public void Setup(SuppliesChart chart)
	{
		IconImg.material = new Material(IconImg.material);
		mat = IconImg.material;

		data = chart;
		IconImg.sprite = Resources.Load<Sprite>("Sprites/Cost/" + chart.Icon);
		IconImg.transform.localScale = new Vector2(chart.IconSize, chart.IconSize);
		WingsMove();

		min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
		max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

		mat.DOFloat(6.28f, "_ShineRotate", 1f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);

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
		RightWing.DORotate(new Vector3(0, 0, -20f), 0.5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
		LeftWing.DORotate(new Vector3(0, 0, 20f), 0.5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
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
			transform.DOMove(new Vector2(max.x - 1f, transform.position.y + Random.Range(-1f, 1f)), 2f).SetEase(Ease.InOutQuad);			
		}
		else if (transform.position.x < min.x)
		{
			transform.DOMove(new Vector2(min.x + 1f, transform.position.y + Random.Range(-1f, 1f)), 2f).SetEase(Ease.InOutQuad);			
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

				transform.DOMove(new Vector2(destX, destY), time).SetEase(Ease.Linear);
			}
			else
			{
				time = 8f;
				transform.DOMove(new Vector2(destX, destY), time).SetEase(Ease.Linear);
			}

			yield return new WaitForSeconds(time);
		}

		transform.position = new Vector3(-100f, 0, 0);
		ObjectManager.Ins.Push<SuppliesBase>(this);
	}

	public void GetReward()
	{	
		//±¤°í ÆË¾÷ ¿¬°á
		DialogManager.Ins.OpenAdReward(data);
		ObjectManager.Ins.Push<SuppliesBase>(this);		
	}


}
