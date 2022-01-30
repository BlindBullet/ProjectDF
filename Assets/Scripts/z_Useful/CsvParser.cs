using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

/// <summary>
/// Defines the <see cref="CsvParser"/>
/// </summary>
public class CsvParser : SingletonObject<CsvParser>
{
	private const int _nameRowIndex = 0;

	private const int _idColumnIndex = 0;

	public List<TV> ParseToList<TV>(string path) where TV : class, new()
	{
		string[] lines = PolishAndTrimText(GetCsvData(path));

		string dataName = typeof(TV).Name;
		if (lines.Length <= _nameRowIndex)
		{
			Debug.LogWarning("CSV 파싱 에러: " + dataName + " 테이블의 데이터가 존재하지 않음");
			return new List<TV>();
		}

		List<string> names = new List<string>(lines[_nameRowIndex].Split(','));
		names = names.ConvertAll(name =>
		{
			string[] words = name.Split('_');
			words = Array.ConvertAll(words, word => word.Length > 1 ? (char.ToUpper(word[0]) + word.Substring(1)) : word.ToUpper());
			return string.Join("", words);
		});

		List<TV> list = new List<TV>();
		for (int i = _nameRowIndex + 1; i < lines.Length; i++)
		{
			if (string.IsNullOrEmpty(lines[i])) continue;

			List<string> columns = new List<string>(lines[i].Split(','));
			/*if (names.Count != columns.Count)
            {
                Debug.LogError(string.Format("{0} 테이블 {1}줄의 열 수가 올바르지 않음", dataName, i + 1));
                continue;
            }*/

			list.Add(SetValues<string, TV>(names, columns, path));
		}
		return list;
	}

	public Dictionary<string, TValue[]> ParseDupeKeyToDict<TValue>(Dictionary<string, TValue[]> dict, string path) where TValue : class, new()
	{
		return ParseDupeKeyToDict(dict, path).ToDictionary(k => k.Key, v => v.Value.ToArray());
	}

	public Dictionary<TKey, List<TValue>> ParseDupeKeyToDict<TKey, TValue>(Dictionary<TKey, List<TValue>> dict, string path)
		where TKey : IComparable
		where TValue : class, new()
	{
		if (dict == null)
		{
			dict = new Dictionary<TKey, List<TValue>>();
		}

		string[] lines = ExtractLines<TValue>(path);
		string[] names = ExtractNames<TValue>(lines.Get(_nameRowIndex));
		for (int i = _nameRowIndex + 1; i < lines.Length; i++)
		{
			List<string> columns = ConvertLineToColumns(lines[i]);
			if (columns == null || string.IsNullOrEmpty(columns[_idColumnIndex]))
			{
				continue;
			}
			TKey key = (TKey)Convert.ChangeType(columns[_idColumnIndex], typeof(TKey));
			if (!dict.ContainsKey(key))
			{
				dict[key] = new List<TValue>();
			}
			dict[key].Add(SetValues<TKey, TValue>(names, columns, typeof(TValue).Name, key));
		}
		return dict;
	}

	public Dictionary<string, TValue> ParseToDict<TValue>(Dictionary<string, TValue> dict, string path) where TValue : class, new()
	{
		return this.ParseToDictInternal<string, TValue>(dict, path);
	}

	public Dictionary<TK, TV> ParseToDictInternal<TK, TV>(Dictionary<TK, TV> dict, string path)
		where TK : IComparable
		where TV : class, new()
	{
		if (dict == null)
		{
			dict = new Dictionary<TK, TV>();
		}

		string[] lines = ExtractLines<TV>(path);
		string[] names = ExtractNames<TV>(lines.Get(_nameRowIndex));

		for (int i = _nameRowIndex + 1; i < lines.Length; i++)
		{
			List<string> columns = ConvertLineToColumns(lines[i]);
			if (columns == null || string.IsNullOrEmpty(columns[_idColumnIndex]))
			{
				continue;
			}
						
			//try
			{
				TK key = default;
				if (typeof(TK).IsEnum) 
				{
					try
					{
						key = (TK)Enum.Parse(typeof(TK), columns[_idColumnIndex]);
					}
					catch
					{
						int k = 0;
					}
				}
				else
				{
					key = (TK)Convert.ChangeType(columns[_idColumnIndex], typeof(TK));
				}
				if (dict.ContainsKey(key))
				{
					Debug.LogWarning(string.Format("{0} 테이블 {1}행의 키가 중복되었음. 의도라면 Dict의 Value를 배열로 바꾸시오.", typeof(TV).Name, i + 1));
				}
				dict[key] = SetValues<TK, TV>(names, columns, typeof(TV).Name, key);
			}
			//catch
			//{
			//	int k = 0;
			//	var type = typeof(TK);
			//	var angas = columns[_idColumnIndex];
			//}

		}
		return dict;
	}

	private static string ReplaceCommaReplacement(string stringWithTilde)
	{
		return Regex.Replace(stringWithTilde, "`", ",");
	}
	
	private string GetCsvData(string path)
	{
		TextAsset data = Resources.Load(path) as TextAsset;
		//Fallback : assetbundle folder
		return data.text;
	}

	private string ConvertSnakeToPascal(string name)
	{
		string[] words = name.Split('_');
		words = Array.ConvertAll(words, word => word.Length > 1 ? (char.ToUpper(word[0]) + word.Substring(1)) : word.ToUpper());
		return string.Join(string.Empty, words);
	}

	private string[] PolishAndTrimText(string s)
	{
		StringBuilder sb = new StringBuilder(s);
		var polished = sb.Replace("\r\n", "\n").Replace("\r", "\n").ToString();
		var lines = polished.Split('\n');
		if (lines.Length <= _nameRowIndex)
		{
			return null;
		}
		return lines;
	}

	private string[] ExtractNames<TValue>(string nameRows)
	{
		if (string.IsNullOrEmpty(nameRows))
		{
			Debug.LogWarning("CSV 파싱 에러: " + typeof(TValue).Name + "의 이름 행이 비어있음");
		}
		return Array.ConvertAll(nameRows.Split(','), name => ConvertSnakeToPascal(name));
	}

	private string[] ExtractLines<TValue>(string path)
	{
		string[] lines = PolishAndTrimText(GetCsvData(path));
		if (lines == null)
		{
			Debug.LogWarning("CSV 파싱 에러: " + typeof(TValue).Name + " 테이블의 데이터가 존재하지 않음");
			return null;
		}
		return lines;
	}

	private TV SetValues<TK, TV>(IList<string> names, IList<string> columns, string targetTable = "", TK key = default)
		where TK : IComparable
		where TV : class, new()
	{
		TV data = new TV();
		for (int i = 0, imax = Math.Min(names.Count, columns.Count); i < imax; i++)
		{
			if (string.IsNullOrEmpty(names[i])) continue;
			BindingFlags flag = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetProperty | BindingFlags.Instance;

			PropertyInfo propertyInfo = data.GetType().GetProperty(names[i], flag);
			if (propertyInfo == null)
			{
				Type inheritedClassType = data.GetType().UnderlyingSystemType;
				Debug.LogWarningFormat("{0}테이블의 아이디:{1} 에서 {2}를 {3}타입으로 파싱할 수 없음",
					targetTable, key, names[i], inheritedClassType.Name);
				continue;
			}

			if (propertyInfo.PropertyType.IsArray)
			{
				List<string> column = new List<string>(columns[i].Split(columns[i].IndexOf(',') == -1 ? '|' : ','));

				for (int k = 0; k < column.Count; k++)
				{
					column[k] = Regex.Replace(column[k], "#", "");
				}

				Type elementType = propertyInfo.PropertyType.GetElementType();
				TypeCode typeCode = Type.GetTypeCode(elementType);
				if (typeCode == TypeCode.String)
				{
					var stringArr = column.Select(elem => ReplaceCommaReplacement(elem)).ToArray();
					propertyInfo.SetValue(data, stringArr, null);
				}
				else if (elementType.IsEnum)
				{
					Array arr = Array.CreateInstance(elementType, column.Count);
					for (int j = 0; j < column.Count; j++)
					{
						object parsedEnum = null;
						if (!TryParseEnum<TK, TV>(ref parsedEnum, elementType, column[j], key.ToString()))
						{
							continue;
						}
						arr.SetValue(parsedEnum, j);
					}
					propertyInfo.SetValue(data, arr, null);
				}
				else
				{
					column.ForEach(col =>
					{
						try { Convert.ChangeType(col, elementType); }
						catch
						{
							if (elementType.IsValueType)
								col = Activator.CreateInstance(elementType).ToString();
							else
								col = "";
						}
					});

					Array arr = Array.CreateInstance(elementType, column.Count);
					switch (typeCode)
					{
						case TypeCode.Int32:
							for (int j = 0; j < column.Count; j++)
							{
								int var = 0;
								if (int.TryParse(column[j], out var))
								{
									arr.SetValue(var, j);
								}
								else
								{
									arr.SetValue(default(TV), j);
								}
							}
							break;

						case TypeCode.Single:
							for (int j = 0; j < column.Count; j++)
							{
								float var = 0;
								if (float.TryParse(column[j], out var))
								{
									arr.SetValue(var, j);
								}
								else
								{
									arr.SetValue(default(TV), j);
								}
							}
							break;

						case TypeCode.Double:
							for (int j = 0; j < column.Count; j++)
							{
								double var = 0;
								if (double.TryParse(column[j], out var))
								{
									arr.SetValue(var, j);
								}
								else
								{
									arr.SetValue(default(TV), j);
								}
							}
							break;
					}
					propertyInfo.SetValue(data, arr, null);
				}
			}
			else if (string.IsNullOrEmpty(columns[i]))
			{
				propertyInfo.SetValue(data, default(TV), null);
			}
			else if (propertyInfo.PropertyType.IsEnum)
			{
				// enum Types
				object parsedEnum = null;
				if (TryParseEnum<TK, TV>(ref parsedEnum, propertyInfo.PropertyType, columns[i], columns[0]))
				{
					propertyInfo.SetValue(data, parsedEnum, null);
				}
			}
			else
			{
				if (!ParsePrimitiveType<TV>(propertyInfo, data, columns[i]))
				{
					Debug.LogWarningFormat("{0}테이블의 아이디:{1} 에서 {2}를 {3}타입으로 파싱할 수 없음",
						typeof(TV).Name, key, names[i], propertyInfo.PropertyType.Name);
				}
			}
		}
		return data;
	}

	private bool ParsePrimitiveType<T>(PropertyInfo propertyInfo, T member, string data)
	{
		// PrimitiveTypes
		switch (Type.GetTypeCode(propertyInfo.PropertyType))
		{
			case TypeCode.Boolean: propertyInfo.SetValue(member, data.ToLower() == "true", null); break;
			case TypeCode.String:
				propertyInfo.SetValue(member, ReplaceCommaReplacement(data), null);
				break;

			default:
				object value;
				try
				{
					value = Convert.ChangeType(data, propertyInfo.PropertyType);
					propertyInfo.SetValue(member, value, null);
				}
				catch
				{
					return false;
				}
				break;
		}
		return true;
	}

	private bool TryParseEnum<TK, TV>(ref object parsedEnum, Type elementType, string input, string id)
	{
		var pacalInput = ConvertSnakeToPascal(input).Replace("|", ", ");
		try
		{
			parsedEnum = Enum.Parse(elementType, pacalInput == string.Empty ? "None" : pacalInput);
			return true;
		}
		catch
		{
			Debug.LogWarningFormat("{0}테이블의 아이디:{1} 에서 {2}를 {3}타입으로 파싱할 수 없음",
				typeof(TV).Name, id, input, elementType.ToString());
			return false;
		}
	}

	/// <summary>
	/// 배열 형태가 포함된 csv도 안전하게 찢어준다
	/// </summary>
	private List<string> ConvertLineToColumns(string line, char splitter = default(char))
	{
		if (string.IsNullOrEmpty(line))
		{
			return null;
		}

		List<string> columns = new List<string>();

		if (splitter != default(char))
		{
			columns.AddRange(line.Split(splitter));
			return columns;
		}

		string[] split = line.Split('"');

		if (split.Length == 0) return columns;
		if (split.Length == 1)
		{
			columns.AddRange(line.Split(','));
			return columns;
		}

		for (int i = 0, imax = split.Length - 1; i <= imax; i++)
		{
			if (split[i] == "") continue;
			if (split[i] == ",")
			{
				if (i == 0 || i == imax)
				{
					columns.Add("");
				}
				continue;
			}
			if (split[i] == ",,")
			{
				columns.Add("");
				continue;
			}
			if (split[i][split[i].Length - 1] == ',')
			{
				var res = (split[i][0] == ',') ?
					split[i].Substring(1, split[i].Length - 2) :
					split[i].Substring(0, split[i].Length - 1);
				columns.AddRange(res.Split(','));
			}
			else if (split[i][0] == ',')
			{
				columns.AddRange(split[i].Substring(1).Split(','));
			}
			else columns.Add(split[i]);
		}

		return columns;
	}
}