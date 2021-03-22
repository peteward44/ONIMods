
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
		public static void Prefix()
		{
			Strings.Add($"STRINGS.BUILDINGS.PREFABS.{WildPlanterBoxConfig.ID.ToUpperInvariant()}.NAME", "<link=\"" + WildPlanterBoxConfig.ID + "\">" + WildPlanterBoxConfig.Name + "</link>");
			Strings.Add($"STRINGS.BUILDINGS.PREFABS.{WildPlanterBoxConfig.ID.ToUpperInvariant()}.DESC", WildPlanterBoxConfig.Description);
			Strings.Add($"STRINGS.BUILDINGS.PREFABS.{WildPlanterBoxConfig.ID.ToUpperInvariant()}.EFFECT", WildPlanterBoxConfig.Effect);

			int categoryIndex = TUNING.BUILDINGS.PLANORDER.FindIndex(x => x.category == "Food");
			(TUNING.BUILDINGS.PLANORDER[categoryIndex].data as IList<String>)?.Add(WildPlanterBoxConfig.ID);

			Strings.Add($"STRINGS.BUILDINGS.PREFABS.{WildFarmTileConfig.ID.ToUpperInvariant()}.NAME", "<link=\"" + WildFarmTileConfig.ID + "\">" + WildFarmTileConfig.Name + "</link>");
			Strings.Add($"STRINGS.BUILDINGS.PREFABS.{WildFarmTileConfig.ID.ToUpperInvariant()}.DESC", WildFarmTileConfig.Description);
			Strings.Add($"STRINGS.BUILDINGS.PREFABS.{WildFarmTileConfig.ID.ToUpperInvariant()}.EFFECT", WildFarmTileConfig.Effect);

			int categoryIndex2 = TUNING.BUILDINGS.PLANORDER.FindIndex(x => x.category == "Food");
			(TUNING.BUILDINGS.PLANORDER[categoryIndex2].data as IList<String>)?.Add(WildFarmTileConfig.ID);
		}
	}

	[HarmonyPatch(typeof(Db), "Initialize")]
	public static class Database_Techs_Init_Patch
	{
		public static void Postfix()
		{
			Db.Get().Techs.Get("Agriculture").unlockedItemIDs.Add(WildPlanterBoxConfig.ID);
			Db.Get().Techs.Get("Agriculture").unlockedItemIDs.Add(WildFarmTileConfig.ID);
		}
	}

	[HarmonyPatch(typeof(ReceptacleMonitor), "SetReceptacle")]
	public static class PlantablePlot_Wild_Patch
	{
		static AccessTools.FieldRef<ReceptacleMonitor, bool> replanted =
			AccessTools.FieldRefAccess<ReceptacleMonitor, bool>("replanted");

		public static void Postfix(ReceptacleMonitor __instance)
		{
			var plantablePlot = __instance.GetReceptacle();
			WildPlanterBox box = null;
			WildFarmTile tile = null;
			if ( plantablePlot.TryGetComponent<WildPlanterBox>( out box ) || plantablePlot.TryGetComponent<WildFarmTile>(out tile))
			{
				// turn off "replanted" flag which is used by the room constraints to count wild plants
				replanted(__instance) = false;
			}
		}
	}
}

