using System;
using System.Reflection;
using HarmonyLib;
using UI;

namespace abcdcode_CreatureRetry_MOD
{
	// Token: 0x02000002 RID: 2
	public class Harmony_Patch
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public Harmony_Patch()
		{
			Harmony harmony = new Harmony("LOR.abcdcode.CreatureRetry");
			Harmony.DEBUG = true;
			MethodInfo method = typeof(Harmony_Patch).GetMethod("UIReviewStorySephirahSlot_OnClickStory");
			harmony.Patch(typeof(UIReviewStorySephirahSlot).GetMethod("OnClickStory", AccessTools.all), new HarmonyMethod(method), null, null, null);
			FileLog.Log("first patch complete\n");
			method = typeof(Harmony_Patch).GetMethod("CheckCreatureBossBattle");
			harmony.Patch(typeof(LibraryModel).GetMethod("CheckCreatureBossBattle", AccessTools.all), new HarmonyMethod(method), null, null, null);
			FileLog.Log("second patch complete\n");
			harmony.PatchAll();
			FileLog.Log("third patch complete\n");
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020B0 File Offset: 0x000002B0
		public static bool UIReviewStorySephirahSlot_OnClickStory(UIReviewStorySephirahSlot __instance, int epnumber)
		{
			bool flag = epnumber == 1;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				LibraryFloorModel floor = LibraryModel.Instance.GetFloor(__instance.sephirah);
				FloorLevelXmlInfo data = Singleton<FloorLevelXmlList>.Instance.GetData(__instance.sephirah, epnumber - 1);
				string text = string.Empty;
				bool flag2 = LibraryModel.Instance.CheckCreatureBossBattle(floor);
				if (flag2)
				{
					string id = "";
					switch (floor.Sephirah)
					{
						case SephirahType.None:
							id = "";
							break;
						case SephirahType.Malkuth:
							id = "ui_malkuthfloor";
							break;
						case SephirahType.Yesod:
							id = "ui_yesodfloor";
							break;
						case SephirahType.Hod:
							id = "ui_hodfloor";
							break;
						case SephirahType.Netzach:
							id = "ui_netzachfloor";
							break;
						case SephirahType.Tiphereth:
							id = "ui_tipherethfloor";
							break;
						case SephirahType.Gebura:
							id = "ui_geburafloor";
							break;
						case SephirahType.Chesed:
							id = "ui_chesedfloor";
							break;
						case SephirahType.Binah:
							id = "ui_hokmafloor";
							break;
						case SephirahType.Hokma:
							id = "ui_binahfloor";
							break;
						case SephirahType.Keter:
							id = "ui_keterfloor";
							break;
					}
					text = TextDataModel.GetText(id, Array.Empty<object>());
				}
				else
				{
					bool flag3 = data != null;
					if (flag3)
					{
						StageClassInfo data2 = Singleton<StageClassInfoList>.Instance.GetData(data.stageId);
						bool flag4 = data2 != null;
						if (flag4)
						{
							text = Singleton<StageNameXmlList>.Instance.GetName(data2.id);
						}
					}
				}
				bool flag5 = epnumber != 1;
				if (flag5)
				{
					UIAlarmType uialarmType = UIAlarmType.StartCreatureBattle;
					bool flag6 = epnumber == 6;
					if (flag6)
					{
						uialarmType = UIAlarmType.StartCreatureBattleInBoss;
					}
					UIAlarmPopup.instance.SetAlarmText(uialarmType, UIAlarmButtonType.YesNo, delegate (bool b)
					{
						if (b)
						{
							Harmony_Patch.OnClickStartCreatureStage(__instance.sephirah, epnumber);
						}
					}, text);
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002280 File Offset: 0x00000480
		public static void OnClickStartCreatureStage(SephirahType targetSephirah, int level)
		{
			FileLog.Log("전투 시작됨\n");
			LibraryModel.Instance.GetFloor(targetSephirah);
			FloorLevelXmlInfo data = Singleton<FloorLevelXmlList>.Instance.GetData(targetSephirah, level - 1);
			bool flag = data != null;
			if (flag)
			{
				Singleton<StageController>.Instance.SetCurrentSephirah(targetSephirah);
				StageClassInfo data2 = Singleton<StageClassInfoList>.Instance.GetData(data.stageId);
				bool flag2 = data2 != null;
				if (flag2)
				{
					Singleton<StageController>.Instance.InitStageByCreature(data2);
					foreach (UnitBattleDataModel unitBattleDataModel in Singleton<StageController>.Instance.GetCurrentStageFloorModel().GetUnitBattleDataList())
					{
						unitBattleDataModel.IsAddedBattle = true;
					}
					StageStoryInfo startStory = data2.GetStartStory();
					bool flag3 = startStory != null;
					if (flag3)
					{
						string story = startStory.story;
						bool flag4 = !string.IsNullOrWhiteSpace(story);
						if (flag4)
						{
							UI.UIController.Instance.OpenStory(story, delegate ()
							{
								GlobalGameManager.Instance.LoadBattleScene();
							}, false);
						}
						else
						{
							GlobalGameManager.Instance.LoadBattleScene();
						}
					}
					else
					{
						GlobalGameManager.Instance.LoadBattleScene();
					}
				}
			}
		}
		public static bool CheckCreatureBossBattle(LibraryFloorModel floor)
		{
			bool result = false;
			switch (floor.Sephirah)
			{
				case SephirahType.Malkuth:
					{
						bool flag = floor.Level >= 5;
						if (flag)
						{
							result = true;
						}
						break;
					}
				case SephirahType.Yesod:
					{
						bool flag2 = floor.Level >= 5;
						if (flag2)
						{
							result = true;
						}
						break;
					}
				case SephirahType.Hod:
					{
						bool flag3 = floor.Level >= 5;
						if (flag3)
						{
							result = true;
						}
						break;
					}
				case SephirahType.Netzach:
					{
						bool flag4 = floor.Level >= 5;
						if (flag4)
						{
							result = true;
						}
						break;
					}
				case SephirahType.Tiphereth:
					{
						bool flag5 = floor.Level >= 5;
						if (flag5)
						{
							result = true;
						}
						break;
					}
				case SephirahType.Gebura:
					{
						bool flag6 = floor.Level >= 5;
						if (flag6)
						{
							result = true;
						}
						break;
					}
				case SephirahType.Chesed:
					{
						bool flag7 = floor.Level >= 5;
						if (flag7)
						{
							result = true;
						}
						break;
					}
				case SephirahType.Binah:
					{
						bool flag8 = floor.Level >= 5;
						if (flag8)
						{
							result = true;
						}
						break;
					}
				case SephirahType.Hokma:
					{
						bool flag9 = floor.Level >= 5;
						if (flag9)
						{
							result = true;
						}
						break;
					}
			}
			return result;
		}
	}

	[HarmonyPatch(typeof(LibraryModel), "CanLevelUpSephirah")]
}
