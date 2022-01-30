using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LotteryCalculator {

	public static int LotteryIntWeight(int[] probs)
	{
		int groupCount = probs.Length;
		int allWeight = 0;

		for (int i = 0; i < probs.Length; i++)
		{
			allWeight = allWeight + probs[i];
		}

		int randWeight = Random.Range(0, allWeight);
		int currentWeight = 0;
		int lotteryNo = 0;

		for (int i = 0; i < groupCount; i++)
		{
			currentWeight = currentWeight + probs[i];

			if (randWeight < currentWeight)
			{
				lotteryNo = i;
				break;
			}
		}

		return lotteryNo;
	}

	public static List<int> LotteryIntWeight(int[] probs, int lotteryCount)
	{
		List<int> lotteryIntList = new List<int>();

		for (int k = 0; k < lotteryCount; k++)
		{
			int groupCount = probs.Length;
			int allWeight = 0;

			for (int i = 0; i < probs.Length; i++)
			{
				allWeight = allWeight + probs[i];
			}

			int randWeight = Random.Range(0, allWeight);
			int currentWeight = 0;
			int lotteryNo = 0;

			for (int i = 0; i < groupCount; i++)
			{
				currentWeight = currentWeight + probs[i];

				if (randWeight < currentWeight)
				{
					lotteryNo = i;
					break;
				}
			}

			lotteryIntList.Add(lotteryNo);

		}

		return lotteryIntList;
	}


	public static int LotteryCalc(List<int> probData)
	{	
		int result = 0;
		int allWeight = 0;

		for (int i = 0; i < probData.Count; i++)
		{
			allWeight = allWeight + probData[i];
		}

		int drawNo = Random.Range(0, allWeight + 1);
		int no = 0;

		for (int i = 0; i < probData.Count; i++)
		{
			if (drawNo > no && drawNo <= no + probData[i])
			{
				return result = i;
			}

			no = no + probData[i];
		}

		return result;
	}

	public static float LotteryCalc(List<float> probData)
	{
		float result = 0;
		float allWeight = 0;

		for (int i = 0; i < probData.Count; i++)
		{
			allWeight = allWeight + probData[i];
		}

		float drawNo = Random.Range(0, allWeight + 1);
		float no = 0;

		for (int i = 0; i < probData.Count; i++)
		{
			if (drawNo > no && drawNo <= no + probData[i])
			{
				return result = i + 1;
			}

			no = no + probData[i];
		}

		return result;
	}

	public static string LotteryCalc(List<string> data, List<int> probData)
	{
		string result = "";
		int allWeight = 0;

		for (int i = 0; i < probData.Count; i++)
		{
			allWeight = allWeight + probData[i];
		}

		int drawNo = Random.Range(0, allWeight + 1);
		int no = 0;

		for (int i = 0; i < probData.Count; i++)
		{
			if (drawNo > no && drawNo <= no + probData[i])
			{
				return result = data[i];
			}

			no = no + probData[i];
		}

		return result;
	}

}
