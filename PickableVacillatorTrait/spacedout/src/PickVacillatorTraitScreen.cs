using System;
using System.Collections.Generic;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PickVacillatorTraitScreen : KScreen, ISim4000ms, ISim1000ms
{
	public KButton closeButton;
	public static PickVacillatorTraitScreen Instance;
	private bool initialized = false;


	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Instance = this;
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.ConsumeMouseScroll = true;
		Init();
	}

	public void Init()
	{
		if (!initialized)
		{
			initialized = true;
			Populate();
			closeButton.onClick += delegate
			{
				Show(show: false);
			};
			Show(show: false);
		}
	}

	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (show)
		{
			ManagementMenu.Instance.CloseAll();
			AllDiagnosticsScreen.Instance.Show(show: false);
			RefreshRows();
		}
	}

	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(Action.Escape))
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close"));
			Show(show: false);
			e.Consumed = true;
		}
		base.OnKeyDown(e);
	}

	public override void OnKeyUp(KButtonEvent e)
	{
		if (PlayerController.Instance.ConsumeIfNotDragging(e, Action.MouseRight))
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close"));
			Show(show: false);
			e.Consumed = true;
		}
		if (!e.Consumed)
		{
			base.OnKeyUp(e);
		}
	}

	public override float GetSortKey()
	{
		return 50f;
	}

	public void Populate(object data = null)
	{
		SpawnRows();
	}

	private void SpawnRows()
	{
		WorldInventory worldInventory = ClusterManager.Instance.GetWorld(ClusterManager.Instance.activeWorldId).worldInventory;
		allowDisplayCategories.Add(GameTags.MaterialCategories);
		allowDisplayCategories.Add(GameTags.CalorieCategories);
		allowDisplayCategories.Add(GameTags.UnitCategories);
		foreach (Tag materialCategory in GameTags.MaterialCategories)
		{
			SpawnCategoryRow(materialCategory, GameUtil.MeasureUnit.mass);
		}
		foreach (Tag calorieCategory in GameTags.CalorieCategories)
		{
			SpawnCategoryRow(calorieCategory, GameUtil.MeasureUnit.kcal);
		}
		foreach (Tag unitCategory in GameTags.UnitCategories)
		{
			SpawnCategoryRow(unitCategory, GameUtil.MeasureUnit.quantity);
		}
		List<Tag> list = new List<Tag>();
		foreach (KeyValuePair<Tag, GameObject> categoryRow in categoryRows)
		{
			list.Add(categoryRow.Key);
		}
		list.Sort((Tag a, Tag b) => a.ProperNameStripLink().CompareTo(b.ProperNameStripLink()));
		foreach (Tag item in list)
		{
			categoryRows[item].transform.SetAsLastSibling();
		}
	}

	private void SpawnCategoryRow(Tag categoryTag, GameUtil.MeasureUnit unit)
	{
		GameObject gameObject = null;
		if (!categoryRows.ContainsKey(categoryTag))
		{
			GameObject gameObject2 = Util.KInstantiateUI(categoryLinePrefab, rootListContainer, force_active: true);
			((TMP_Text)gameObject2.GetComponent<HierarchyReferences>().GetReference<LocText>("NameLabel")).SetText(categoryTag.ProperNameStripLink());
			categoryRows.Add(categoryTag, gameObject2);
			currentlyDisplayedRows.Add(categoryTag, value: true);
			units.Add(categoryTag, unit);
			HierarchyReferences component = gameObject2.GetComponent<HierarchyReferences>();
			component.GetReference<SparkLayer>("Chart").GetComponent<GraphBase>().axis_x.min_value = 0f;
			component.GetReference<SparkLayer>("Chart").GetComponent<GraphBase>().axis_x.max_value = 600f;
			component.GetReference<SparkLayer>("Chart").GetComponent<GraphBase>().axis_x.guide_frequency = 120f;
			component.GetReference<SparkLayer>("Chart").GetComponent<GraphBase>().RefreshGuides();
		}
		gameObject = categoryRows[categoryTag].GetComponent<FoldOutPanel>().container;
		foreach (Tag item in DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(categoryTag))
		{
			if (resourceRows.ContainsKey(item))
			{
				continue;
			}
			GameObject gameObject3 = Util.KInstantiateUI(resourceLinePrefab, gameObject, force_active: true);
			HierarchyReferences component2 = gameObject3.GetComponent<HierarchyReferences>();
			Tuple<Sprite, Color> uISprite = Def.GetUISprite(item);
			component2.GetReference<Image>("Icon").set_sprite(uISprite.first);
			((Graphic)component2.GetReference<Image>("Icon")).set_color(uISprite.second);
			((TMP_Text)component2.GetReference<LocText>("NameLabel")).SetText(item.ProperNameStripLink());
			Tag targetTag = item;
			MultiToggle pinToggle = component2.GetReference<MultiToggle>("PinToggle");
			MultiToggle multiToggle = pinToggle;
			multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, (System.Action)delegate
			{
				if (ClusterManager.Instance.activeWorld.worldInventory.pinnedResources.Contains(targetTag))
				{
					ClusterManager.Instance.activeWorld.worldInventory.pinnedResources.Remove(targetTag);
				}
				else
				{
					ClusterManager.Instance.activeWorld.worldInventory.pinnedResources.Add(targetTag);
					if (DiscoveredResources.Instance.newDiscoveries.ContainsKey(targetTag))
					{
						DiscoveredResources.Instance.newDiscoveries.Remove(targetTag);
					}
				}
				RefreshPinnedState(targetTag);
				pinToggle.ChangeState(ClusterManager.Instance.activeWorld.worldInventory.pinnedResources.Contains(targetTag) ? 1 : 0);
			});
			gameObject3.GetComponent<MultiToggle>().onClick = pinToggle.onClick;
			MultiToggle notifyToggle = component2.GetReference<MultiToggle>("NotificationToggle");
			MultiToggle multiToggle2 = notifyToggle;
			multiToggle2.onClick = (System.Action)Delegate.Combine(multiToggle2.onClick, (System.Action)delegate
			{
				if (ClusterManager.Instance.activeWorld.worldInventory.notifyResources.Contains(targetTag))
				{
					ClusterManager.Instance.activeWorld.worldInventory.notifyResources.Remove(targetTag);
				}
				else
				{
					ClusterManager.Instance.activeWorld.worldInventory.notifyResources.Add(targetTag);
				}
				RefreshPinnedState(targetTag);
				notifyToggle.ChangeState(ClusterManager.Instance.activeWorld.worldInventory.notifyResources.Contains(targetTag) ? 1 : 0);
			});
			component2.GetReference<SparkLayer>("Chart").GetComponent<GraphBase>().axis_x.min_value = 0f;
			component2.GetReference<SparkLayer>("Chart").GetComponent<GraphBase>().axis_x.max_value = 600f;
			component2.GetReference<SparkLayer>("Chart").GetComponent<GraphBase>().axis_x.guide_frequency = 120f;
			component2.GetReference<SparkLayer>("Chart").GetComponent<GraphBase>().RefreshGuides();
			resourceRows.Add(item, gameObject3);
			currentlyDisplayedRows.Add(item, value: true);
			if (units.ContainsKey(item))
			{
				Debug.LogError(string.Concat("Trying to add ", item.ToString(), ":UnitType ", units[item], " but units dictionary already has key ", item.ToString(), " with unit type:", unit));
			}
			else
			{
				units.Add(item, unit);
			}
		}
	}

	private void FilterRowBySearch(Tag tag, string filter)
	{
		currentlyDisplayedRows[tag] = PassesSearchFilter(tag, filter);
	}

	private void SearchFilter(string search)
	{
		foreach (KeyValuePair<Tag, GameObject> resourceRow in resourceRows)
		{
			FilterRowBySearch(resourceRow.Key, search);
		}
		foreach (KeyValuePair<Tag, GameObject> categoryRow in categoryRows)
		{
			if (PassesSearchFilter(categoryRow.Key, search))
			{
				currentlyDisplayedRows[categoryRow.Key] = true;
				foreach (Tag item in DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(categoryRow.Key))
				{
					if (currentlyDisplayedRows.ContainsKey(item))
					{
						currentlyDisplayedRows[item] = true;
					}
				}
			}
			else
			{
				currentlyDisplayedRows[categoryRow.Key] = false;
			}
		}
		EnableCategoriesByActiveChildren();
		SetRowsActive();
	}

	private bool PassesSearchFilter(Tag tag, string filter)
	{
		filter = filter.ToUpper();
		string text = tag.ProperName().ToUpper();
		if (filter != "" && !text.Contains(filter) && !tag.Name.ToUpper().Contains(filter))
		{
			return false;
		}
		return true;
	}

	private void EnableCategoriesByActiveChildren()
	{
		foreach (KeyValuePair<Tag, GameObject> categoryRow in categoryRows)
		{
			if (DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(categoryRow.Key).Count == 0)
			{
				currentlyDisplayedRows[categoryRow.Key] = false;
				continue;
			}
			GameObject container = categoryRow.Value.GetComponent<FoldOutPanel>().container;
			foreach (KeyValuePair<Tag, GameObject> resourceRow in resourceRows)
			{
				if (!(resourceRow.Value.transform.parent.gameObject != container))
				{
					currentlyDisplayedRows[categoryRow.Key] = currentlyDisplayedRows[categoryRow.Key] || currentlyDisplayedRows[resourceRow.Key];
				}
			}
		}
	}

	public void RefreshRows()
	{
	}

	private void RefreshCharts()
	{
		float time = GameClock.Instance.GetTime();
		float num = 3000f;
		foreach (KeyValuePair<Tag, GameObject> categoryRow in categoryRows)
		{
			HierarchyReferences component = categoryRow.Value.GetComponent<HierarchyReferences>();
			ResourceTracker resourceStatistic = TrackerTool.Instance.GetResourceStatistic(ClusterManager.Instance.activeWorldId, categoryRow.Key);
			if (resourceStatistic != null)
			{
				SparkLayer reference = component.GetReference<SparkLayer>("Chart");
				Tuple<float, float>[] array = resourceStatistic.ChartableData(num);
				if (array.Length != 0)
				{
					reference.graph.axis_x.max_value = array[array.Length - 1].first;
				}
				else
				{
					reference.graph.axis_x.max_value = 0f;
				}
				reference.graph.axis_x.min_value = time - num;
				reference.RefreshLine(array, "resourceAmount");
			}
			else
			{
				DebugUtil.DevLogError("DevError: No tracker found for resource category " + categoryRow.Key);
			}
		}
		foreach (KeyValuePair<Tag, GameObject> resourceRow in resourceRows)
		{
			HierarchyReferences component = resourceRow.Value.GetComponent<HierarchyReferences>();
			ResourceTracker resourceStatistic2 = TrackerTool.Instance.GetResourceStatistic(ClusterManager.Instance.activeWorldId, resourceRow.Key);
			if (resourceStatistic2 != null)
			{
				SparkLayer reference2 = component.GetReference<SparkLayer>("Chart");
				Tuple<float, float>[] array2 = resourceStatistic2.ChartableData(num);
				if (array2.Length != 0)
				{
					reference2.graph.axis_x.max_value = array2[array2.Length - 1].first;
				}
				else
				{
					reference2.graph.axis_x.max_value = 0f;
				}
				reference2.graph.axis_x.min_value = time - num;
				reference2.RefreshLine(array2, "resourceAmount");
			}
			else
			{
				DebugUtil.DevLogError("DevError: No tracker found for resource " + resourceRow.Key);
			}
		}
	}

	private void SetRowsActive()
	{
		foreach (KeyValuePair<Tag, GameObject> categoryRow in categoryRows)
		{
			if (categoryRow.Value.activeSelf != currentlyDisplayedRows[categoryRow.Key])
			{
				categoryRow.Value.SetActive(currentlyDisplayedRows[categoryRow.Key]);
			}
		}
		foreach (KeyValuePair<Tag, GameObject> resourceRow in resourceRows)
		{
			if (resourceRow.Value.activeSelf != currentlyDisplayedRows[resourceRow.Key])
			{
				resourceRow.Value.SetActive(currentlyDisplayedRows[resourceRow.Key]);
			}
		}
	}

	public void Sim4000ms(float dt)
	{
		RefreshCharts();
	}

	public void Sim1000ms(float dt)
	{
		RefreshRows();
	}
}