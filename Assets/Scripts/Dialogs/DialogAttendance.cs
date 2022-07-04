using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DialogAttendance : DialogController
{
	public TextMeshProUGUI TitleText;
	public TextMeshProUGUI DescText;
	public TextMeshProUGUI LvText;
	public TextMeshProUGUI ExpText;
	public Image ExpFill;
	public RewardIcon[] RewardIcons;
	public Button ClaimBtn;
	public TextMeshProUGUI ClaimBtnText;
	public TextMeshProUGUI ClaimTimeText;
	
	public void OpenDialog()
	{
		TitleText.text = LanguageManager.Ins.SetString("title_popup_attendance");
		DescText.text = LanguageManager.Ins.SetString("desc_popup_attendance");

		SetProgressBar();
		SetRewards();

		ClaimBtn.onClick.RemoveAllListeners();
		ClaimBtn.onClick.AddListener(() =>
		{
			Claim();
		});

		ClaimBtnText.text = LanguageManager.Ins.SetString("Claim");
		SetClaimBtn();

		Show(false);
	}

	void SetClaimBtn()
	{
		if (StageManager.Ins.PlayerData.CheckLv == 1 && StageManager.Ins.PlayerData.CheckCount == 0)
		{			
			ClaimBtnOn();
		}	
		else
		{
			if (CheckCanClaim())
			{
				ClaimBtnOn();
			}
			else
			{
				ClaimBtnOff();
			}
		}
	}

	bool CheckCanClaim()
	{
		DateTime dateTime = StageManager.Ins.PlayerData.CheckStartTime;
		TimeSpan span = TimeManager.Ins.GetCurrentTime() - dateTime;

		if (span.TotalSeconds >= ConstantData.CheckClaimPossibleSec)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	void ClaimBtnOn()
	{
		ClaimBtn.enabled = true;
		ClaimTimeText.gameObject.SetActive(false);
	}

	void ClaimBtnOff()
	{
		ClaimBtn.enabled = false;
		ClaimTimeText.gameObject.SetActive(true);
		StartCoroutine(ClaimBtnSequence());
	}

	IEnumerator ClaimBtnSequence()
	{
		while (true)
		{
			if (!CheckCanClaim())
			{
				DateTime dateTime = StageManager.Ins.PlayerData.CheckStartTime;
				TimeSpan timeSpan = TimeManager.Ins.GetCurrentTime() - dateTime;
				int time = (ConstantData.CheckClaimPossibleSec - (int)timeSpan.TotalSeconds);				
				int hour = (int)(time / 3600f);
				int min = (int)((time % 3600f) / 60f);
				int sec = (int)((time % 3600f) % 60f);

				string hourStr = hour < 10 ? "0" + hour : hour.ToString();
				string minStr = min < 10 ? "0" + min : min.ToString();
				string secStr = sec < 10 ? "0" + sec : sec.ToString();

				ClaimTimeText.text = hourStr + ":" + minStr + ":" + secStr;
			}
			else
			{
				SetClaimBtn();
				yield break;
			}

			yield return null;
		}
	}

	void Claim()
	{
		//보상 주기
		AttendanceChart chart = CsvData.Ins.AttendanceChart[(StageManager.Ins.PlayerData.CheckCount + 1).ToString()];
		float incP = (StageManager.Ins.PlayerData.CheckLv - 1) * 0.1f > 3f ? 3f : (StageManager.Ins.PlayerData.CheckLv - 1) * 0.1f;
		double value = Math.Round(chart.Amount + (chart.Amount * incP));

		switch (chart.RewardType)
		{
			case RewardType.GainGold:
				break;
			case RewardType.GameSpeed:
				break;
			case RewardType.Gold:
				break;
			case RewardType.SoulStone:
				StageManager.Ins.ChangeSoulStone(value);
				DialogManager.Ins.OpenReceiveReward(RewardType.SoulStone, value);
				break;
			case RewardType.UseAutoSkill:
				break;
		}

		bool isInit = false;

		//체크 카운트(Exp) 증가
		if (StageManager.Ins.PlayerData.IncCheckCount())
		{
			isInit = true;
		}

		SetProgressBar();
		SetRewards(isInit);
		SetClaimBtn();
	}

	void SetProgressBar()
	{
		LvText.text = StageManager.Ins.PlayerData.CheckLv.ToString();
		ExpText.text = StageManager.Ins.PlayerData.CheckCount.ToString() + "/15";
		ExpFill.fillAmount = (float)StageManager.Ins.PlayerData.CheckCount / 15f;
	}

	void SetRewards(bool isInit = false)
	{
		for(int i = 0; i < RewardIcons.Length; i++)
		{
			RewardIcons[i].SetIcon(CsvData.Ins.AttendanceChart[(i + 1).ToString()], StageManager.Ins.PlayerData.CheckLv);

			if(i < StageManager.Ins.PlayerData.CheckCount)
			{
				RewardIcons[i].CheckOn();
				RewardIcons[i].SelectedFrameOff();
			}

			if(i == StageManager.Ins.PlayerData.CheckCount)
			{
				RewardIcons[i].SelectedFrameOn();
			}
		}

		if (isInit)
		{
			InitReward();
		}
	}

	void InitReward()
	{
		for(int i = 0; i < RewardIcons.Length; i++)
		{
			RewardIcons[i].CheckOff();
			RewardIcons[i].SelectedFrameOff();
		}
	}




}
