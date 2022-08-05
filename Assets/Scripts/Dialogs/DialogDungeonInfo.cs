using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.U2D;

public class DialogDungeonInfo : DialogController
{
	public TextMeshProUGUI NameText;
	public Image EnemyImg;
	public RewardIcon[] RewardIcons;
	public TextMeshProUGUI DescText;
	public Button EnterBtn;
	public TextMeshProUGUI EnterBtnText;

	public void OpenDialog(DungeonChart chart)
	{
		NameText.text = LanguageManager.Ins.SetString(chart.Name);
		EnemyImg.sprite = Resources.Load<SpriteAtlas>("Sprites/Characters").GetSprite(chart.AppearEnemy);

		RewardIcons[0].SetIcon(RewardType.SoulStone, chart.RewardSoulStone, RewardValueShowType.CalcValue);
		RewardIcons[1].SetIcon(RewardType.EnchantStone, chart.RewardEnchantStone, RewardValueShowType.CalcValue);
		
		DescText.text = LanguageManager.Ins.SetString("desc_dungeon_info");

		EnterBtn.onClick.RemoveAllListeners();
		EnterBtn.onClick.AddListener(() => EnterDungeon(int.Parse(chart.Id)));

		Show(true);
	}

	void EnterDungeon(int no)
	{		
		StageManager.Ins.DungeonData.UseTicket();
		StageManager.Ins.DungeonData.SetCurDungeonLv(no);
		StageManager.Ins.EnterDungeon();
		SoundManager.Ins.DissolveBGMVolume(0f, 0.5f);
		MySceneManager.Ins.ChangeScene("Dungeon");
	}
}
