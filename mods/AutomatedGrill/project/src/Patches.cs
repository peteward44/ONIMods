
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using PeterHan.PLib.Core;
using PeterHan.PLib.Database;
using PeterHan.PLib.Options;
using PeterHan.PLib.PatchManager;
using PeterHan.PLib.UI;

namespace PW
{
	public static class Patches
	{
		static void addBuilding(string category, string ID, string name, string description, string effect)
		{
			Strings.Add($"STRINGS.BUILDINGS.PREFABS.{ID.ToUpperInvariant()}.NAME", "<link=\"" + ID + "\">" + name + "</link>");
			Strings.Add($"STRINGS.BUILDINGS.PREFABS.{ID.ToUpperInvariant()}.DESC", description);
			Strings.Add($"STRINGS.BUILDINGS.PREFABS.{ID.ToUpperInvariant()}.EFFECT", effect);

			int categoryIndex = TUNING.BUILDINGS.PLANORDER.FindIndex(x => x.category == category);
			(TUNING.BUILDINGS.PLANORDER[categoryIndex].data as IList<String>)?.Add(ID);
		}

		[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
		public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
		{
			public static void OnLoad()
			{
			}

			public static void Prefix()
			{
				addBuilding("Food", AutomatedGrillConfig.ID, AutomatedGrillConfig.Name, AutomatedGrillConfig.Description, AutomatedGrillConfig.Effect);
			}
		}

		[HarmonyPatch(typeof(Db), "Initialize")]
		public static class Database_Techs_Init_Patch
		{
			public static void Postfix()
			{
				Db.Get().Techs.Get("Agriculture").unlockedItemIDs.Add(AutomatedGrillConfig.ID);
			}
		}
	}


	public class MyMod : KMod.UserMod2
	{
		public override void OnLoad(Harmony harmony)
		{
			base.OnLoad(harmony);
			PUtil.InitLibrary();
			LocString.CreateLocStringKeys(typeof(AutomatedGrillConfig));
			new PLocalization().Register();
			new POptions().RegisterOptions(this, typeof(AutomatedGrillOptions));
			new PPatchManager(harmony).RegisterPatchClass(typeof(MyMod));
			Patches.GeneratedBuildings_LoadGeneratedBuildings_Patch.OnLoad();
		}
	}
}
