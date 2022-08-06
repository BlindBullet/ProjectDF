using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogDungeonWin : DialogController
{
	public TextMeshProUGUI TitleText;
	public TextMeshProUGUI DescText;
	public RewardIcon[] RewardIcons;
	public Button ReturnBtn;
	public TextMeshProUGUI ReturnBtnText;

	public void OpenDialog(double getSoulStone, double getEnchantStone)
	{
		TitleText.text = LanguageManager.Ins.SetString("Victory");

		//½Â¸® ½Ã°£
		int time = (int)(30f - Mathf.Round(DungeonManager.Ins._time));
		//½Â¸® º¸»ó Áõ°¡
		float rewardInc = Mathf.Round(((30f - time) / 30f) * 100f);

		DescText.text = LanguageManager.Ins.SetString("VictoryTime") + " : " + time.ToString() + "\n"
			+ LanguageManager.Ins.SetString("RewardIncRate") + " : +" + rewardInc.ToString() + "%";

		RewardIcons[0].SetIcon(RewardType.SoulStone, getSoulStone, RewardValueShowType.CalcValue);
		RewardIcons[1].SetIcon(RewardType.EnchantStone, getEnchantStone, RewardValueShowType.CalcValue);

		ReturnBtn.onClick.RemoveAllListeners();
		ReturnBtn.onClick.AddListener(() => { DungeonManager.Ins.WinEndSeq(); CloseDialog(); });

		ReturnBtnText.text = LanguageManager.Ins.SetString("Return");

		Show(false);
	}


}
