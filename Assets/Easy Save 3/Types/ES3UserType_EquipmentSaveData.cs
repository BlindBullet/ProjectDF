using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("Datas")]
	public class ES3UserType_EquipmentSaveData : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_EquipmentSaveData() : base(typeof(EquipmentSaveData)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (EquipmentSaveData)obj;
			
			writer.WriteProperty("Datas", instance.Datas, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(System.Collections.Generic.List<EquipmentData>)));
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (EquipmentSaveData)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "Datas":
						instance.Datas = reader.Read<System.Collections.Generic.List<EquipmentData>>();
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new EquipmentSaveData();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}


	public class ES3UserType_EquipmentSaveDataArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_EquipmentSaveDataArray() : base(typeof(EquipmentSaveData[]), ES3UserType_EquipmentSaveData.Instance)
		{
			Instance = this;
		}
	}
}