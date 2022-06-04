using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStep : MonoBehaviour
{
	public void SetStep(int no)
	{
		switch (no)
		{
			case 1:
				break;
			case 2:
				for(int i = 0; i < StageManager.Ins.Slots.Count; i++)
				{
					if(StageManager.Ins.Slots[i].No == 3)
					{
						StageManager.Ins.Slots[i].LevelUp();
					}
				}
				break;
			case 3:
				for (int i = 0; i < StageManager.Ins.Slots.Count; i++)
				{
					if (StageManager.Ins.Slots[i].No == 3)
					{
						DialogManager.Ins.OpenSlotPowerUp(StageManager.Ins.Slots[i].data); 
					}
				}
				break;
		}
	}




}
