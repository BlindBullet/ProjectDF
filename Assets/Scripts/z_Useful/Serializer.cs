/*
 * 스트링으로 시리얼라이즈 & 디시리얼라이즈
 * 바이트배열로 시리얼라이즈 & 디시리얼라이즈
*/

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Serializer
{
	#region serialize

	//오브젝트 시리얼라이즈 후 결과 값을 스트링으로 변환하여 반환
	public static string ObjectToStringSerialize(object obj)
	{
		using (var memoryStream = new MemoryStream())
		{
			BinaryFormatter bf = new BinaryFormatter();
			bf.Serialize(memoryStream, obj);
			memoryStream.Flush();
			memoryStream.Position = 0;

			return Convert.ToBase64String(memoryStream.ToArray());
		}
	}

	//오브젝트를 시리얼라이즈 후 바이트 배열 형태로 반환
	public static byte[] ObjectToByteArraySerialize(object obj)
	{
		using (var memoryStream = new MemoryStream())
		{
			BinaryFormatter bf = new BinaryFormatter();
			bf.Serialize(memoryStream, obj);
			memoryStream.Flush();
			memoryStream.Position = 0;

			return memoryStream.ToArray();
		}
	}

	#endregion

	#region Deserialize

	//스트링 타입의 시리얼라이즈된 데이타를 디시리얼라이즈 후 해당 타입으로 변환하여 반환
	public static T Deserialize<T>(string xmlText)
	{
		if (xmlText != null && xmlText != String.Empty)
		{
			byte[] b = Convert.FromBase64String(xmlText);
			using (var stream = new MemoryStream(b))
			{
				var formatter = new BinaryFormatter();
				stream.Seek(0, SeekOrigin.Begin);
				return (T)formatter.Deserialize(stream);
			}
		}
		else
		{
			return default(T);
		}
	}

	//바이트 배열 형태의 시리얼라이즈된 데이타를 디시리얼라이즈 후 해당 타입으로 변환하여 반환
	public static T Deserialize<T>(byte[] byteData)
	{
		using (var stream = new MemoryStream(byteData))
		{
			var formatter = new BinaryFormatter();
			stream.Seek(0, SeekOrigin.Begin);
			return (T)formatter.Deserialize(stream);
		}
	}

	#endregion
}