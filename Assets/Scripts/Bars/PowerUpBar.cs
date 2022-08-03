using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.U2D;

public class PowerUpBar : MonoBehaviour
{
	public Image IconImg;
	public TextMeshProUGUI Name;
	public TextMeshProUGUI Desc;
	public Button PurchaseBtn;
	public TextMeshProUGUI PurchaseBtnText;
	public Image CostIcon;
	public TextMeshProUGUI CostText;
	Material mat;
	SlotData data;

	public void SetBar(SlotData data, AtkUpgradeType type, CostType costType, double cost)
	{	
		Image uiImage = PurchaseBtn.GetComponent<Image>();
		uiImage.material = new Material(uiImage.materialForRendering);
		mat = PurchaseBtn.GetComponent<Image>().materialForRendering;
		PurchaseBtn.GetComponent<AllIn1SpriteShader.AllIn1Shader>().ApplyMaterialToHierarchy();
		PurchaseBtnText.text = LanguageManager.Ins.SetString("Upgrade");

		this.data = data;
		IconImg.sprite = Resources.Load<SpriteAtlas>("Sprites/Icons").GetSprite(type.ToString());

		switch (type)
		{
			case AtkUpgradeType.AtkUp:
				Name.text = string.Format(LanguageManager.Ins.SetString("name_slot_power_up_" + type.ToString()), data.AtkData.AtkUp + 1);
				break;
			case AtkUpgradeType.Boom:
				Name.text = string.Format(LanguageManager.Ins.SetString("name_slot_power_up_" + type.ToString()), data.AtkData.Boom + 1);
				break;
			case AtkUpgradeType.Bounce:
				Name.text = string.Format(LanguageManager.Ins.SetString("name_slot_power_up_" + type.ToString()), data.AtkData.Bounce + 1);
				break;
			case AtkUpgradeType.Diagonal:
				Name.text = string.Format(LanguageManager.Ins.SetString("name_slot_power_up_" + type.ToString()), data.AtkData.Diagonal + 1);
				break;
			case AtkUpgradeType.Front:
				Name.text = string.Format(LanguageManager.Ins.SetString("name_slot_power_up_" + type.ToString()), data.AtkData.Front + 1);
				break;
			case AtkUpgradeType.Multi:
				Name.text = string.Format(LanguageManager.Ins.SetString("name_slot_power_up_" + type.ToString()), data.AtkData.Multi + 1);
				break;
			case AtkUpgradeType.Piercing:
				Name.text = string.Format(LanguageManager.Ins.SetString("name_slot_power_up_" + type.ToString()), data.AtkData.Piercing + 1);
				break;
			case AtkUpgradeType.Size:
				Name.text = string.Format(LanguageManager.Ins.SetString("name_slot_power_up_" + type.ToString()), data.AtkData.Size + 1);
				break;
			case AtkUpgradeType.Push:
				Name.text = string.Format(LanguageManager.Ins.SetString("name_slot_power_up_" + type.ToString()), data.AtkData.Push + 1);
				break;
		}
		
		Desc.text = LanguageManager.Ins.SetString("desc_slot_power_up_" + type.ToString());
		CostIcon.sprite = Resources.Load<SpriteAtlas>("Sprites/Icons").GetSprite(costType.ToString());

		SetPurchaseBtn(type, costType, cost);
	}

	void SetPurchaseBtn(AtkUpgradeType type, CostType costType, double cost)
	{		
		CostText.text = cost.ToCurrencyString();

		bool canPurchase = false;

		switch (costType)
		{
			case CostType.Gold:
				if (StageManager.Ins.PlayerData.Gold >= cost)
					canPurchase = true;
				break;
			case CostType.Magicite:
				if (StageManager.Ins.PlayerData.Magicite >= cost)
					canPurchase = true;
				break;
			case CostType.SoulStone:
				if (StageManager.Ins.PlayerData.SoulStone >= cost)
					canPurchase = true;
				break;
		}

		if (canPurchase)
		{
			PurchaseBtn.enabled = true;
			mat.SetFloat("_GreyscaleBlend", 0f);

			PurchaseBtn.onClick.RemoveAllListeners();
			PurchaseBtn.onClick.AddListener(() =>
			{
				SoundManager.Ins.PlaySFX("se_button_2");
				data.PowerUp(data.Lv, type);

				switch (costType)
				{
					case CostType.Gold:
						if (StageManager.Ins.PlayerData.Gold >= cost)
							StageManager.Ins.ChangeGold(-cost);
						break;
					case CostType.Magicite:
						if (StageManager.Ins.PlayerData.Magicite >= cost)
							StageManager.Ins.ChangeMagicite(-cost);
						break;
					case CostType.SoulStone:
						if (StageManager.Ins.PlayerData.SoulStone >= cost)
							StageManager.Ins.ChangeSoulStone(-cost);
						break;
				}

				for (int i = 0; i < StageManager.Ins.Slots.Length; i++)
				{
					if (data.No == StageManager.Ins.Slots[i].No)
						StageManager.Ins.Slots[i].AfterPowerUp();
				}

				DialogSlotPowerUp._Dialog.CloseDialog(true);
				StageManager.Ins.PlayerData.Save();
			});
		}
		else
		{
			PurchaseBtn.enabled = false;
			mat.SetFloat("_GreyscaleBlend", 1f);
		}
	}
}
