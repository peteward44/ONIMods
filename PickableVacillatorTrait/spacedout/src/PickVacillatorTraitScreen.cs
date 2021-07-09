using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PickVacillatorTraitScreen : KModalButtonMenu
{
	[SerializeField]
	private OptionsMenuScreen optionsScreen;

	[SerializeField]
	private SaveScreen saveScreenPrefab;

	[SerializeField]
	private LoadScreen loadScreenPrefab;

	[SerializeField]
	private KButton closeButton;

	[SerializeField]
	private LocText title;

	[SerializeField]
	private LocText worldSeed;

	private float originalTimeScale;

	private static PickVacillatorTraitScreen instance;

	public static PickVacillatorTraitScreen Instance => instance;

	public static void DestroyInstance()
	{
		instance = null;
	}

	protected override void OnPrefabInit()
	{
		keepMenuOpen = true;
		base.OnPrefabInit();
		buttons = new ButtonInfo[1];
		buttons.Add(new ButtonInfo("TEST", Action.NumActions, OnResume));
		//buttons = new ButtonInfo[8]
		//{
		//	new ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.RESUME, Action.NumActions, OnResume),
		//	new ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.SAVE, Action.NumActions, OnSave),
		//	new ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.SAVEAS, Action.NumActions, OnSaveAs),
		//	new ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.LOAD, Action.NumActions, OnLoad),
		//	new ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.OPTIONS, Action.NumActions, OnOptions),
		//	new ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.COLONY_SUMMARY, Action.NumActions, OnColonySummary),
		//	new ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.QUIT, Action.NumActions, OnQuit),
		//	new ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.DESKTOPQUIT, Action.NumActions, OnDesktopQuit)
		//};

		closeButton.onClick += OnResume;
		instance = this;
		Show(show: false);
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
	//	((TMP_Text)title).SetText((string)"");
		try
		{
//			string settingsCoordinate = CustomGameSettings.Instance.GetSettingsCoordinate();
//			string[] array = CustomGameSettings.ParseSettingCoordinate(settingsCoordinate);
//			((TMP_Text)worldSeed).SetText(string.Format(UI.FRONTEND.PAUSE_SCREEN.WORLD_SEED, settingsCoordinate));
//			((Component)(object)worldSeed).GetComponent<ToolTip>().toolTip = string.Format(UI.FRONTEND.PAUSE_SCREEN.WORLD_SEED_TOOLTIP, array[1], array[2], array[3]);
		}
		catch (Exception arg)
		{
//			Debug.LogWarning($"Failed to load Coordinates on ClusterLayout {arg}, please report this error on the forums");
//			CustomGameSettings.Instance.Print();
//			Debug.Log("ClusterCache: " + string.Join(",", SettingsCache.clusterLayouts.clusterCache.Keys));
//			((TMP_Text)worldSeed).SetText(string.Format(UI.FRONTEND.PAUSE_SCREEN.WORLD_SEED, "0"));
		}
	}

	private void OnResume()
	{
		Show(show: false);
	}

	protected override void OnShow(bool show)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		base.OnShow(show);
		if (show)
		{
			return;
		}
		ToolTipScreen.Instance.ClearToolTip(closeButton.GetComponent<ToolTip>());
	}

	private void OnOptions()
	{
		ActivateChildScreen(optionsScreen.gameObject);
	}

	private void OnSaveAs()
	{
		ActivateChildScreen(saveScreenPrefab.gameObject);
	}

	private void ConfirmDecision(string text, System.Action onConfirm)
	{
		base.gameObject.SetActive(value: false);
		ConfirmDialogScreen confirmDialogScreen = (ConfirmDialogScreen)GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, base.transform.parent.gameObject);
		confirmDialogScreen.PopupConfirmDialog(text, onConfirm, OnCancelPopup);
	}

	private void OnQuit()
	{
	//	ConfirmDecision(UI.FRONTEND.MAINMENU.QUITCONFIRM, OnQuitConfirm);
	}

	private void OnDesktopQuit()
	{
	//	ConfirmDecision(UI.FRONTEND.MAINMENU.DESKTOPQUITCONFIRM, OnDesktopQuitConfirm);
	}

	private void OnCancelPopup()
	{
		base.gameObject.SetActive(value: true);
	}

	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(Action.Escape) || e.TryConsume(Action.MouseRight))
		{
			Show(show: false);
		}
		else
		{
			base.OnKeyDown(e);
		}
	}
}
