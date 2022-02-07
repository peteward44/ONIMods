// PlanterBoxConfig
using System;
using System.Collections.Generic;
using System.Reflection;
using TUNING;
using UnityEngine;


namespace PW
{
	public class AutomatedGrillConfig : IBuildingConfig
	{
		public const string ID = "AutomatedGrill";

		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("WildPlanterBox", 1, 1, "wildplanterbox_kanim", 10, 3f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER2, MATERIALS.FARMABLE, 800f, BuildLocationRule.OnFloor, noise: NOISE_POLLUTION.NONE, decor: BUILDINGS.DECOR.PENALTY.TIER1);
			buildingDef.ForegroundLayer = Grid.SceneLayer.BuildingBack;
			buildingDef.Overheatable = false;
			buildingDef.Floodable = false;
			buildingDef.AudioCategory = "Glass";
			buildingDef.AudioSize = "large";

			return buildingDef;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			Storage storage = go.AddOrGet<Storage>();
			PlantablePlot plantablePlot = go.AddOrGet<PlantablePlot>();

			// turn off irrigation
			FieldInfo fi = typeof(PlantablePlot).GetField("accepts_irrigation", BindingFlags.NonPublic | BindingFlags.Instance);
			fi.SetValue(plantablePlot, false);

			plantablePlot.AddDepositTag(GameTags.CropSeed);
			plantablePlot.SetFertilizationFlags(fertilizer: false, liquid_piping: false);
			CopyBuildingSettings copyBuildingSettings = go.AddOrGet<CopyBuildingSettings>();
			copyBuildingSettings.copyGroupTag = GameTags.Farm;
			BuildingTemplates.CreateDefaultStorage(go);
			storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
			go.AddOrGet<DropAllWorkable>();
			go.AddOrGet<AnimTileable>();
			Prioritizable.AddRef(go);
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
		}

		public static readonly LocString Name = "Wild Planter Box";
		public static readonly LocString Description = "Box that simulates wild conditions for plants";
		public static readonly LocString Effect = "Allows duplicants to plant a wild plant from a seed";

	}
}
