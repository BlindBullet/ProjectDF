using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("DungeonEnterCount", "TicketChargeStartTime", "TicketChargeLeftTime", "IsDungeonOpen", "CurDungeonLv", "TopDungeonLv")]
	public class ES3UserType_DungeonSaveData : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_DungeonSaveData() : base(typeof(DungeonSaveData)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (DungeonSaveData)obj;
			
			writer.WriteProperty("DungeonEnterCount", instance.TicketCount, ES3Type_int.Instance);
			writer.WriteProperty("TicketChargeStartTime", instance.TicketChargeStartTime, ES3Type_DateTime.Instance);
			writer.WriteProperty("TicketChargeLeftTime", instance.TicketChargeLeftTime, ES3Type_float.Instance);
			writer.WriteProperty("IsDungeonOpen", instance.IsDungeonOpen, ES3Type_bool.Instance);
			writer.WriteProperty("CurDungeonLv", instance.CurDungeonLv, ES3Type_int.Instance);
			writer.WriteProperty("TopDungeonLv", instance.TopDungeonLv, ES3Type_int.Instance);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (DungeonSaveData)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "DungeonEnterCount":
						instance.TicketCount = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "TicketChargeStartTime":
						instance.TicketChargeStartTime = reader.Read<System.DateTime>(ES3Type_DateTime.Instance);
						break;
					case "TicketChargeLeftTime":
						instance.TicketChargeLeftTime = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "IsDungeonOpen":
						instance.IsDungeonOpen = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "CurDungeonLv":
						instance.CurDungeonLv = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "TopDungeonLv":
						instance.TopDungeonLv = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new DungeonSaveData();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}


	public class ES3UserType_DungeonSaveDataArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_DungeonSaveDataArray() : base(typeof(DungeonSaveData[]), ES3UserType_DungeonSaveData.Instance)
		{
			Instance = this;
		}
	}
}