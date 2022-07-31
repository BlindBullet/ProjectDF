using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("Datas", "WeaponLv", "WeaponExp", "AccLv", "AccExp", "EnchantStone")]
	public class ES3UserType_EquipmentSaveData : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_EquipmentSaveData() : base(typeof(EquipmentSaveData)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (EquipmentSaveData)obj;
			
			writer.WriteProperty("Datas", instance.Datas, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(System.Collections.Generic.List<EquipmentData>)));
			writer.WriteProperty("WeaponLv", instance.WeaponLv, ES3Type_int.Instance);
			writer.WriteProperty("WeaponExp", instance.WeaponExp, ES3Type_int.Instance);
			writer.WriteProperty("AccLv", instance.AccLv, ES3Type_int.Instance);
			writer.WriteProperty("AccExp", instance.AccExp, ES3Type_int.Instance);
			writer.WriteProperty("EnchantStone", instance.EnchantStone, ES3Type_double.Instance);
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
					case "WeaponLv":
						instance.WeaponLv = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "WeaponExp":
						instance.WeaponExp = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "AccLv":
						instance.AccLv = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "AccExp":
						instance.AccExp = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "EnchantStone":
						instance.EnchantStone = reader.Read<System.Double>(ES3Type_double.Instance);
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