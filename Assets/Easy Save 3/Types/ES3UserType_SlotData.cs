using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("No", "Lv", "Power", "AtkData", "PowerUpStackDatas", "PowerUpListLvs", "PowerUpLists")]
	public class ES3UserType_SlotData : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_SlotData() : base(typeof(SlotData)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (SlotData)obj;
			
			writer.WriteProperty("No", instance.No, ES3Type_int.Instance);
			writer.WriteProperty("Lv", instance.Lv, ES3Type_int.Instance);
			writer.WriteProperty("Power", instance.Power, ES3Type_int.Instance);
			writer.WriteProperty("AtkData", instance.AtkData, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(AttackData)));
			writer.WriteProperty("PowerUpStackDatas", instance.PowerUpStackDatas, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(System.Collections.Generic.List<System.Int32>)));
			writer.WriteProperty("PowerUpListLvs", instance.PowerUpListLvs, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(System.Collections.Generic.List<System.Int32>)));
			writer.WriteProperty("PowerUpLists", instance.PowerUpLists, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(System.Collections.Generic.List<AtkUpgradeType>)));
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (SlotData)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "No":
						instance.No = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "Lv":
						instance.Lv = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "Power":
						instance.Power = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "AtkData":
						instance.AtkData = reader.Read<AttackData>();
						break;
					case "PowerUpStackDatas":
						instance.PowerUpStackDatas = reader.Read<System.Collections.Generic.List<System.Int32>>();
						break;
					case "PowerUpListLvs":
						instance.PowerUpListLvs = reader.Read<System.Collections.Generic.List<System.Int32>>();
						break;
					case "PowerUpLists":
						instance.PowerUpLists = reader.Read<System.Collections.Generic.List<AtkUpgradeType>>();
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new SlotData();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}


	public class ES3UserType_SlotDataArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_SlotDataArray() : base(typeof(SlotData[]), ES3UserType_SlotData.Instance)
		{
			Instance = this;
		}
	}
}