using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static partial class ExtensionMethods
{
	const string Zero = "0";
	const string One = "1";
	/// <summary> /// 단위 표현 스타일 /// </summary> 
	public enum CurrencyType { Default, SI, } 
	/// <summary> /// 표현 가능한 화폐 단위, double형 e+308까지 자릿수 표현 지원 /// </summary> 
	static readonly string[] CurrencyUnits = new string[] 
	{ "", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "aa", "ab", "ac", "ad", "ae", "af", "ag", "ah", "ai", "aj", "ak", "al", "am", "an", "ao", "ap", "aq", "ar", "as", "at", "au", "av", "aw", "ax", "ay", "az", "ba", "bb", "bc", "bd", "be", "bf", "bg", "bh", "bi", "bj", "bk", "bl", "bm", "bn", "bo", "bp", "bq", "br", "bs", "bt", "bu", "bv", "bw", "bx", "by", "bz", "ca", "cb", "cc", "cd", "ce", "cf", "cg", "ch", "ci", "cj", "ck", "cl", "cm", "cn", "co", "cp", "cq", "cr", "cs", "ct", "cu", "cv", "cw", "cx", }; 
	/// <summary> /// SI 표기법 생략... /// </summary> 
	static readonly string[] SI = new string[] { "", "K", "M", "G", "T", "P", "E", "Z" }; 
	/// <summary> /// double형 데이터를 클리커 화폐 단위로 표현한다. /// 확장 메서드로 제공한다. /// </summary> /// 
	///<param name="number">double형 숫자</param> 
	/// <param name="currencyType">클리커 단위타입</param> 
	/// /// <returns>클리커 표현식</returns> 

	public static string ToCurrencyString(this double number, CurrencyType currencyType = CurrencyType.Default) 
	{ 
		// 정수부가 0인경우 0으로 퉁친다. 
		if (-1d < number && number < 1d) 
		{ 
			return Zero;
		}
		if (true == double.IsInfinity(number)) 
		{ 
			return "Infinity"; 
		} 
		
		// 부호 출력 문자열 
		string significant = (number < 0) ? "-" : string.Empty; 
		
		// 보여줄 숫자 
		string showNumber = string.Empty; 
		
		// 단위 문자열 
		string unitString = string.Empty; 
		
		// 패턴을 단순화 시키기 위해 무조건 지수표현식으로 변경한 후 처리한다. 
		string[] partsSplit = number.ToString("E").Split('+'); 
		
		// 예외 상황이다.... 
		if (partsSplit.Length < 2) 
		{
			UnityEngine.Debug.LogWarning(string.Format("Failed - ToCurrencyString({0})", number)); 
			return Zero; 
		} 
		
		// 지수 (자릿수 표현) 
		if (false == int.TryParse(partsSplit[1], out int exponent)) 
		{ 
			UnityEngine.Debug.LogWarning(string.Format("Failed - ToCurrencyString({0}) : partsSplit[1] = {1}", number, partsSplit[1])); 
			return Zero; 
		} 
		
		// 몫은 문자열 인덱스 
		int quotient = exponent / 3; 
		
		// 나머지는 정수부 자릿수 계산에 사용 (10의 거듭제곱을 사용) 
		int remainder = exponent % 3; 
		
		// 1A 미만은 그냥 표현한다. 
		if (exponent < 3) 
		{ 
			showNumber = Math.Truncate(number).ToString(); 
		} 
		else 
		{
			// 10의 거듭제곱을 구해서 자릿수 표현값을 만들어준다. 
			var temp = double.Parse(partsSplit[0].Replace("E", ""), System.Globalization.CultureInfo.InvariantCulture) * Math.Pow(10, remainder); 
			// 소수 둘째자리까지만 출력한다. 
			showNumber = temp.ToString("F").Replace(".00", ""); 
		} 
  
		if (currencyType == CurrencyType.Default) 
		{
			unitString = CurrencyUnits[quotient]; 
		}
		else 
		{ 
			unitString = SI[quotient]; 
		} 
	
		return string.Format("{0}{1}{2}", significant, showNumber, unitString); 
	} 
	
	/// <summary> /// 문자열로 입력한 클리커 재화를 double형으로 표현한다. 
	/// /// </summary> 
	/// /// <param name="currencyString">재화</param> 
	/// /// <param name="stringType">문자열의 재화 표현 타입</param> 
	/// /// <returns>double형 재화</returns> 
	public static double ToCurrencyDouble(this string currencyString, CurrencyType stringType = CurrencyType.Default) 
	{ 
		double result = 0; 
		bool isNumber = double.TryParse(currencyString, out result); 
		
		if (true == isNumber) 
		{ 
			return result; 
		} 
		else 
		{
			int length = currencyString.Length; 
			int lastNumberIndex = -1; 
			for (int i = length - 1; 0 <= i; --i) 
			{ 
				if (true == char.IsNumber(currencyString, i)) 
				{ 
					lastNumberIndex = i; break; 
				}
			} 
   
			if (lastNumberIndex < 0) 
			{ 
				throw new Exception("Failed currency string"); 
			} 
   
			string number = currencyString.Substring(0, lastNumberIndex + 1); 
			string unit = currencyString.Substring(lastNumberIndex + 1); 
			int index = (CurrencyType.Default == stringType) ? Array.FindIndex(CurrencyUnits, p => p == unit) : Array.FindIndex(SI, p => p == unit); 
			
			if (-1 == index) 
			{ 
				throw new Exception("Failed currency string"); 
			}
			
			string exponentNumber = string.Format("{0}E+{1}", number, index * 3); return double.Parse(exponentNumber, System.Globalization.CultureInfo.InvariantCulture); 
		} 
	}

}
