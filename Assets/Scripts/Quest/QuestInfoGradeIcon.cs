using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestInfoGradeIcon : MonoBehaviour
{
	public TextMeshProUGUI GradeText;
	public TextMeshProUGUI CountText;

	public void Init(QuestChart chart)
	{
		GradeText.text = chart.NeedGrade.ToString();
		SetCountText(0, chart.NeedGradeCount);		
	}

	public void SetCountText(int count, int maxCount)
	{
		CountText.text = count + "/" + maxCount;
	}
}
