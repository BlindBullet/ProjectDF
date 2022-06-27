using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("IsFirstPlay", "PlayAppCount", "Gold", "Magicite", "SoulStone", "Stage", "TopStage", "AscensionCount", "TouchAttackLv", "Slots", "Heroes", "Relics", "Castles", "Quests", "PlayerBuffs", "QuestLv", "ClearQuestCount", "TotalClearQuestCount", "OnBGM", "OnSFX", "OfflineStartTime", "TutorialStep", "QuestResetStartTime")]
	public class ES3UserType_PlayerData : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_PlayerData() : base(typeof(PlayerData)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (PlayerData)obj;
			
			writer.WriteProperty("IsFirstPlay", instance.IsFirstPlay, ES3Type_bool.Instance);
			writer.WriteProperty("PlayAppCount", instance.PlayAppCount, ES3Type_int.Instance);
			writer.WriteProperty("Gold", instance.Gold, ES3Type_double.Instance);
			writer.WriteProperty("Magicite", instance.Magicite, ES3Type_double.Instance);
			writer.WriteProperty("SoulStone", instance.SoulStone, ES3Type_double.Instance);
			writer.WriteProperty("Stage", instance.Stage, ES3Type_int.Instance);
			writer.WriteProperty("TopStage", instance.TopStage, ES3Type_int.Instance);
			writer.WriteProperty("AscensionCount", instance.AscensionCount, ES3Type_int.Instance);
			writer.WriteProperty("TouchAttackLv", instance.TouchAttackLv, ES3Type_int.Instance);
			writer.WriteProperty("Slots", instance.Slots, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(System.Collections.Generic.List<SlotData>)));
			writer.WriteProperty("Heroes", instance.Heroes, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(System.Collections.Generic.List<HeroData>)));
			writer.WriteProperty("Relics", instance.Relics, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(System.Collections.Generic.List<RelicData>)));
			writer.WriteProperty("Castles", instance.Castles, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(System.Collections.Generic.List<RelicData>)));
			writer.WriteProperty("Quests", instance.Quests, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(System.Collections.Generic.List<QuestData>)));
			writer.WriteProperty("PlayerBuffs", instance.PlayerBuffs, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(System.Collections.Generic.List<PlayerBuffData>)));
			writer.WriteProperty("QuestLv", instance.QuestLv, ES3Type_int.Instance);
			writer.WriteProperty("ClearQuestCount", instance.ClearQuestCount, ES3Type_int.Instance);
			writer.WriteProperty("TotalClearQuestCount", instance.TotalClearQuestCount, ES3Type_int.Instance);
			writer.WriteProperty("OnBGM", instance.OnBGM, ES3Type_bool.Instance);
			writer.WriteProperty("OnSFX", instance.OnSFX, ES3Type_bool.Instance);
			writer.WriteProperty("OfflineStartTime", instance.OfflineStartTime, ES3Type_DateTime.Instance);
			writer.WriteProperty("TutorialStep", instance.TutorialStep, ES3Type_int.Instance);
			writer.WriteProperty("QuestResetStartTime", instance.QuestResetStartTime, ES3Type_DateTime.Instance);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (PlayerData)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "IsFirstPlay":
						instance.IsFirstPlay = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "PlayAppCount":
						instance.PlayAppCount = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "Gold":
						instance.Gold = reader.Read<System.Double>(ES3Type_double.Instance);
						break;
					case "Magicite":
						instance.Magicite = reader.Read<System.Double>(ES3Type_double.Instance);
						break;
					case "SoulStone":
						instance.SoulStone = reader.Read<System.Double>(ES3Type_double.Instance);
						break;
					case "Stage":
						instance.Stage = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "TopStage":
						instance.TopStage = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "AscensionCount":
						instance.AscensionCount = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "TouchAttackLv":
						instance.TouchAttackLv = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "Slots":
						instance.Slots = reader.Read<System.Collections.Generic.List<SlotData>>();
						break;
					case "Heroes":
						instance.Heroes = reader.Read<System.Collections.Generic.List<HeroData>>();
						break;
					case "Relics":
						instance.Relics = reader.Read<System.Collections.Generic.List<RelicData>>();
						break;
					case "Castles":
						instance.Castles = reader.Read<System.Collections.Generic.List<RelicData>>();
						break;
					case "Quests":
						instance.Quests = reader.Read<System.Collections.Generic.List<QuestData>>();
						break;
					case "PlayerBuffs":
						instance.PlayerBuffs = reader.Read<System.Collections.Generic.List<PlayerBuffData>>();
						break;
					case "QuestLv":
						instance.QuestLv = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "ClearQuestCount":
						instance.ClearQuestCount = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "TotalClearQuestCount":
						instance.TotalClearQuestCount = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "OnBGM":
						instance.OnBGM = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "OnSFX":
						instance.OnSFX = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "OfflineStartTime":
						instance.OfflineStartTime = reader.Read<System.DateTime>(ES3Type_DateTime.Instance);
						break;
					case "TutorialStep":
						instance.TutorialStep = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "QuestResetStartTime":
						instance.QuestResetStartTime = reader.Read<System.DateTime>(ES3Type_DateTime.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new PlayerData();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}


	public class ES3UserType_PlayerDataArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_PlayerDataArray() : base(typeof(PlayerData[]), ES3UserType_PlayerData.Instance)
		{
			Instance = this;
		}
	}
}