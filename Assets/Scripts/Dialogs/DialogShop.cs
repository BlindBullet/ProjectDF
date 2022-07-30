using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogShop : DialogController
{
	public TextMeshProUGUI TitleText;
	public Button SoulStone1Btn;
	public TextMeshProUGUI SoulStone1CostText;
	public Button SoulStone2Btn;
	public TextMeshProUGUI SoulStone2CostText;
	public Button SoulStone3Btn;
	public TextMeshProUGUI SoulStone3CostText;
	public Button SoulStone4Btn;
	public TextMeshProUGUI SoulStone4CostText;
	public WeaponGachaBar WeaponGachaBar;
	public AccGachaBar AccGachaBar;

	public void OpenDialog()
	{
		TitleText.text = LanguageManager.Ins.SetString("Shop");

		SoulStone1Btn.onClick.RemoveAllListeners();
		SoulStone1Btn.onClick.AddListener(() => IAPManager.Ins.Purchase("soulstone1"));
		SoulStone2Btn.onClick.RemoveAllListeners();
		SoulStone2Btn.onClick.AddListener(() => IAPManager.Ins.Purchase("soulstone2"));
		SoulStone3Btn.onClick.RemoveAllListeners();
		SoulStone3Btn.onClick.AddListener(() => IAPManager.Ins.Purchase("soulstone3"));
		SoulStone4Btn.onClick.RemoveAllListeners();
		SoulStone4Btn.onClick.AddListener(() => IAPManager.Ins.Purchase("soulstone4"));

		SoulStone1CostText.text = string.Format("{0} {1}", IAPManager.Ins.GetProduct("soulstone1").metadata.localizedPrice, IAPManager.Ins.GetProduct("soulstone1").metadata.isoCurrencyCode);
		SoulStone2CostText.text = string.Format("{0} {1}", IAPManager.Ins.GetProduct("soulstone2").metadata.localizedPrice, IAPManager.Ins.GetProduct("soulstone2").metadata.isoCurrencyCode);
		SoulStone3CostText.text = string.Format("{0} {1}", IAPManager.Ins.GetProduct("soulstone3").metadata.localizedPrice, IAPManager.Ins.GetProduct("soulstone3").metadata.isoCurrencyCode);
		SoulStone4CostText.text = string.Format("{0} {1}", IAPManager.Ins.GetProduct("soulstone4").metadata.localizedPrice, IAPManager.Ins.GetProduct("soulstone4").metadata.isoCurrencyCode);

		WeaponGachaBar.Setup();
		AccGachaBar.Setup();

		Show(true);
	}
}
