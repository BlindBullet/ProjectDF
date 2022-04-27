using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoSingleton<QuestManager>
{	
	public void ResetQuest()
	{
		List<QuestData> datas = StageManager.Ins.PlayerData.Quests;
		List<QuestChart> lotteriedQuests = LotteryQuests();

		for(int i = 0; i < datas.Count; i++)
		{
			if (datas[i].IsDiapatch)
				continue;

			datas[i].ChangeQuest(lotteriedQuests[i]);
		}

		StageManager.Ins.PlayerData.Save();
	}

	public void ClearQuest(QuestData data)
	{
		//보상 전달
		QuestChart _chart = CsvData.Ins.QuestChart[data.Id];
		SendReward(_chart);

		//퀘스트를 새로운 퀘스트로 체인지
		QuestChart chart = LotteryQuests()[0];
		data.ChangeQuest(chart);

		StageManager.Ins.PlayerData.Save();
	}

	void SendReward(QuestChart chart)
	{
		double rewardValue = chart.RewardValue;

		switch (chart.RewardType)
		{
			case RewardType.Gold:
				rewardValue = ConstantData.GetGoldFromTime(chart.RewardValue, StageManager.Ins.PlayerData.Stage);
				StageManager.Ins.ChangeGold(rewardValue);
				break;
			case RewardType.SoulStone:				
				StageManager.Ins.ChangeMagicite(chart.RewardValue);
				break;
			case RewardType.GameSpeed:
				
				break;
		}

		DialogManager.Ins.OpenReceiveReward(chart.RewardType, rewardValue);

	}

	List<QuestChart> LotteryQuests()
	{	
		int lv = GetQuestLevel();
		List<QuestChart> questList = new List<QuestChart>();

		foreach (KeyValuePair<string, QuestChart> elem in CsvData.Ins.QuestChart)
		{
			if(elem.Value.Lv == lv || elem.Value.Lv == lv - 1)
			{
				bool alreadyHave = false;

				for(int i = 0; i < StageManager.Ins.PlayerData.Quests.Count; i++)
				{
					if (elem.Value.Id == StageManager.Ins.PlayerData.Quests[i].Id)
					{
						alreadyHave = true;
						break;
					}	
				}

				if(!alreadyHave)
					questList.Add(elem.Value);
			}
		}		

		questList = Shuffle.ShuffleList(questList);
		return questList;
	}

	public int GetQuestLevel()
	{
		int clearCount = StageManager.Ins.PlayerData.ClearQuestCount;
		int lv = 1;

		for (int i = ConstantData.QuestLvPerClearCount.Length - 1; i >= 0; i--)
		{
			if (clearCount >= ConstantData.QuestLvPerClearCount[i])
			{
				lv = i + 2;
				break;
			}	
		}

		return lv;
	}



}
