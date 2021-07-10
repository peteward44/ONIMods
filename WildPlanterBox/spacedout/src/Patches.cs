
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;



public static class Patches
{
	static bool isPlantWild( KPrefabID plant )
	{
		if (plant.GetComponent<BasicForagePlantPlanted>() != null)
		{
			return true;
		}

		ReceptacleMonitor receptacleMonitor = plant.GetComponent<ReceptacleMonitor>();
		if (receptacleMonitor != null)
		{
			if (!receptacleMonitor.Replanted)
			{
				return true;
			}
			
			var plantablePlot = receptacleMonitor.GetReceptacle();
			if (plantablePlot != null)
			{
				if (plantablePlot.GetComponent<WildPlanterBox>() || plantablePlot.GetComponent<WildFarmTile>())
				{
					return true;
				}
			}
		}
		return false;
	}

	static int countWildPlants(Room room)
	{
		int count = 0;
		foreach (KPrefabID plant in room.cavity.plants)
		{
			if (plant != null && isPlantWild( plant ))
			{
				count++;
			}
		}
		return count;
	}

	static void addBuilding( string category, string ID, string name, string description, string effect)
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
			RoomConstraints.WILDPLANT = new RoomConstraints.Constraint(null, delegate (Room room)
			{
				return countWildPlants(room) >= 2;
			}, 1, STRINGS.ROOMS.CRITERIA.WILDPLANT.NAME, STRINGS.ROOMS.CRITERIA.WILDPLANT.DESCRIPTION);

			RoomConstraints.WILDPLANTS = new RoomConstraints.Constraint(null, delegate (Room room)
			{
				return countWildPlants(room) >= 4;
			}, 1, STRINGS.ROOMS.CRITERIA.WILDPLANTS.NAME, STRINGS.ROOMS.CRITERIA.WILDPLANTS.DESCRIPTION);
		}

		public static void Prefix()
		{
			addBuilding("Food", WildPlanterBoxConfig.ID, WildPlanterBoxConfig.Name, WildPlanterBoxConfig.Description, WildPlanterBoxConfig.Effect);
			addBuilding("Food", WildFarmTileConfig.ID, WildFarmTileConfig.Name, WildFarmTileConfig.Description, WildFarmTileConfig.Effect);
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
	public static class ReceptacleMonitor_SetReceptacle
	{
		public static void Postfix(ReceptacleMonitor __instance)
		{
			// force plants put in the wild receptacles to be wild
			var plantablePlot = __instance.GetReceptacle();
			if (plantablePlot && !plantablePlot.AcceptsIrrigation)
			{
				FieldInfo fi = typeof(ReceptacleMonitor).GetField("replanted", BindingFlags.NonPublic | BindingFlags.Instance);
				fi.SetValue(__instance, false);
			}
		}
	}
}


public class MyMod : KMod.UserMod2
{
	public override void OnLoad(Harmony harmony)
	{
		Patches.GeneratedBuildings_LoadGeneratedBuildings_Patch.OnLoad();
		harmony.PatchAll();
	}
}
