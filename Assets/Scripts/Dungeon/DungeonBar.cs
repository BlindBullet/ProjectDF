using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.U2D;

public class DungeonBar : MonoBehaviour
{
	public Image EnemyImg;
	public TextMeshProUGUI FloorText;
	public TextMeshProUGUI NameText;	
	public Button DispatchBtn;
	public TextMeshProUGUI DispatchBtnText;

	public void SetBar(DungeonChart chart)
	{
		EnemyImg.sprite = Resources.Load<SpriteAtlas>("Sprites/Characters").GetSprite(chart.AppearEnemy);
		FloorText.text = LanguageManager.Ins.SetString("Basement") + " " + chart.Id.ToString() + LanguageManager.Ins.SetString("Floor");
		NameText.text = LanguageManager.Ins.SetString(chart.Name);		
		DispatchBtnText.text = LanguageManager.Ins.SetString("Dispatch");

		DispatchBtn.onClick.RemoveAllListeners();
		DispatchBtn.onClick.AddListener(() => 
		{ 
			if(StageManager.Ins.DungeonData.DungeonEnterCount <= 0)
			{
				DialogManager.Ins.OpenCautionBar("not_enough_dungeon_ticket");
			}
			else
			{
				DialogManager.Ins.OpenDungeonInfo(chart);
			}
		});
	}

}
