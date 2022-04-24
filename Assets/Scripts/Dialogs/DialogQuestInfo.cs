using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogQuestInfo : DialogController
{
	public TextMeshProUGUI Title;
	public RewardIcon RewardIcon;
	public DispatchEmptyIcon[] EmptyIcons;
	public QuestInfoGradeIcon GradeIcon;
	public QuestInfoAttrIcon[] AttrIcons;
	public Button DispatchBtn;

	public void OpenDialog(QuestData data)
	{
		QuestChart chart = CsvData.Ins.QuestChart[data.Id];
		Title.text = LanguageManager.Ins.SetString(chart.Name);
		RewardIcon.SetIcon(chart);

		Show(false);
	}

	void SetEmptyIcons(QuestChart chart)
	{
		for(int i = 0; i < chart.NeedAttr.Length; i++)
		{
			EmptyIcons[i].gameObject.SetActive(true);
			//EmptyIcons[i].SetIcon();
		}
	}

	void SetHeroes()
	{

	}



}
