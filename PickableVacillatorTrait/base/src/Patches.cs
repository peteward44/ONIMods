
using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



public static class Patches
{

	[HarmonyPatch(typeof(GeneratedBuildings))]
	[HarmonyPatch("LoadGeneratedBuildings")]
	public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
	{
		public static void OnLoad()
		{
		}

		public static void Prefix()
		{
	//		addBuilding("Food", WildPlanterBoxConfig.ID, WildPlanterBoxConfig.Name, WildPlanterBoxConfig.Description, WildPlanterBoxConfig.Effect);
	//		addBuilding("Food", WildFarmTileConfig.ID, WildFarmTileConfig.Name, WildFarmTileConfig.Description, WildFarmTileConfig.Effect);
		}
	}

	[HarmonyPatch(typeof(Db), "Initialize")]
	public static class Database_Techs_Init_Patch
	{
		public static void Postfix()
		{
	//		Db.Get().Techs.Get("Agriculture").unlockedItemIDs.Add(WildPlanterBoxConfig.ID);
	//		Db.Get().Techs.Get("Agriculture").unlockedItemIDs.Add(WildFarmTileConfig.ID);
		}
	}
}

