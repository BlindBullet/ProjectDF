using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TutorialStep : MonoBehaviour
{
	public TextMeshProUGUI StepDesc;

	public void SetStep(int no)
	{
		switch (no)
		{
			case 1:
				StepDesc.text = LanguageManager.Ins.SetString("desc_tutorial_step_1");
				break;
			case 2:
				StepDesc.text = LanguageManager.Ins.SetString("desc_tutorial_step_2");
				break;
			case 3:
				StepDesc.text = LanguageManager.Ins.SetString("desc_tutorial_step_3");
				break;
			case 4:
				StepDesc.text = LanguageManager.Ins.SetString("desc_tutorial_step_4");
				break;
			case 5:
				StepDesc.text = LanguageManager.Ins.SetString("desc_tutorial_step_5");
				break;
			case 6:
				StepDesc.text = LanguageManager.Ins.SetString("desc_tutorial_step_6");
				break;
		}
	}

	public void PlayStep(int no)
	{
		switch (no)
		{
			case 1:
				break;
			case 2:
				for (int i = 0; i < HeroBase.Heroes.Count; i++)
				{
					if (HeroBase.Heroes[i].Data.SlotNo == 3)
					{
						HeroBase.Heroes[i].SkillCon.Skill.CoolTime = HeroBase.Heroes[i].SkillCon.Skill._CoolTime;
					}
				}
				break;
			case 3:
				for (int i = 0; i < HeroBase.Heroes.Count; i++)
				{
					if (HeroBase.Heroes[i].Data.SlotNo == 3)
					{
						HeroBase.Heroes[i].SkillCon.UseSkill();
					}
				}
				break;
			case 4:				
				for (int i = 0; i < StageManager.Ins.Slots.Length; i++)
				{
					if (StageManager.Ins.Slots[i].No == 3)
					{
						StageManager.Ins.Slots[i].LevelUp();
					}
				}
				break;
			case 5:				
				for (int i = 0; i < StageManager.Ins.Slots.Length; i++)
				{
					if (StageManager.Ins.Slots[i].No == 3)
					{
						DialogManager.Ins.OpenSlotPowerUp(StageManager.Ins.Slots[i].data);
					}
				}
				break;
			case 6:
				DialogManager.Ins.OpenAscension();
				break;
		}
	}
}
