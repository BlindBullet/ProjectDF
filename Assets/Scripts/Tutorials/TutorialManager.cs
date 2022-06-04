using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoSingleton<TutorialManager>
{	
	public int TotalStep;
	[HideInInspector] public int CurStep;
	public List<TutorialCondition> Tutorials = new List<TutorialCondition>();
	public GameObject[] StepsObj;

	public void InitTutorial()
	{
		StageManager.Ins.GoldChanged += GoldChanged;		
		StageManager.Ins.StageChanged += StageChanged;

		for (int i = 0; i < TotalStep; i++)
		{
			Tutorials.Add(new TutorialCondition(i + 1));
		}
	}

	public void SetTutorial()
	{		
		int curStep = StageManager.Ins.PlayerData.TutorialStep;

		if (curStep >= TotalStep)
			return;

		if(CheckCondition(Tutorials[curStep]))
		{			
			StepsObj[curStep].SetActive(true);
			StepsObj[curStep].GetComponent<TutorialStep>().SetStep(curStep + 1);
		}
		else
		{
			return;
		}
	}

	public void IncTutorialStep()
	{
		int curStep = StageManager.Ins.PlayerData.TutorialStep;
		StepsObj[curStep].GetComponent<TutorialStep>().PlayStep(curStep + 1);
		StepsObj[curStep].SetActive(false);

		StageManager.Ins.PlayerData.IncTutorialStep();		
		SetTutorial();
	}

	bool CheckCondition(TutorialCondition con)
	{
		switch (con.Condition)
		{
			case TutorialConType.None:
				return true;
			case TutorialConType.Gold:
				if (StageManager.Ins.PlayerData.Gold >= con.Value)
					return true;
				else
					return false;
			case TutorialConType.Stage:
				if (StageManager.Ins.PlayerData.Stage >= con.Value)
					return true;
				else
					return false;				
			default:
				return true;
		}
	}

	void GoldChanged(double value)
	{
		SetTutorial();
	}

	void StageChanged()
	{
		SetTutorial();
	}

}

public class TutorialCondition
{
	public int Step;
	public TutorialConType Condition;
	public double Value;

	public TutorialCondition(int no)
	{
		Step = no;

		switch (no)
		{
			case 1:
				Condition = TutorialConType.None;
				break;
			case 2:
				Condition = TutorialConType.None;
				break;
			case 3:
				Condition = TutorialConType.Gold;
				Value = 10f;
				break;
			case 4:
				Condition = TutorialConType.None;
				break;
			case 5:
				Condition = TutorialConType.Stage;
				Value = ConstantData.PossibleAscensionStage;
				break;			
		}
	}


}

public enum TutorialConType
{
	None,
	Gold,
	Stage,

}