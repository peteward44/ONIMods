
using Harmony;
using Klei.AI;
using PeterHan.PLib.UI;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TUNING;
using UnityEngine;

public static class Patches
{

	[HarmonyPatch(typeof(GeneShuffler))]
	[HarmonyPatch("ApplyRandomTrait")]
	public static class GeneShuffler_ApplyRandomTrait_Patch
	{

		
		public static void OnLoad()
		{
		}

		private static void OnDialogClosed(string option)
		{

		}

	
		public static bool Prefix(GeneShuffler __instance, Worker worker)
		{
			var parent = GameScreenManager.Instance.ssOverlayCanvas.gameObject;
			var dialog = new PDialog("ModifyItem")
			{
				Title = "",
				DialogClosed = OnDialogClosed,
				SortKey = 200.0f
			}.AddButton("ok", "OK", null, PUITuning.Colors.ButtonPinkStyle).
				AddButton("close", "Cancel", null, PUITuning.Colors.
				ButtonBlueStyle);
			//var body = new PGridPanel("ModifyBody")
			//{
			//	Margin = new RectOffset(10, 10, 10, 10)
			//}.AddColumn(new GridColumnSpec()).AddColumn(new GridColumnSpec(0.0f, 1.0f));
			//body.AddRow(UI.MODIFYDIALOG.CAPTION, new PTextField("Title")
			//{
			//	Text = editor.Title,
			//	MaxLength = 127,
			//	MinWidth = 512,
			//	BackColor =
			//	PUITuning.Colors.DialogDarkBackground,
			//	TextStyle = PUITuning.Fonts.
			//	TextLightStyle,
			//	TextAlignment = TMPro.TextAlignmentOptions.Left
			//}.AddOnRealize((obj) => titleField = obj0));
			//body.AddRow(UI.MODIFYDIALOG.DESC, new PTextArea("Description")
			//{
			//	LineCount = 8,
			//	Text = editor.Description,
			//	MaxLength = 7999,
			//	MinWidth = 512,
			//	BackColor = PUITuning.Colors.DialogDarkBackground,
			//	TextStyle = PUITuning.Fonts.TextLightStyle
			//}.AddOnRealize((obj) => descriptionField = obj));
			//body.AddRow(UI.MODIFYDIALOG.IMAGE_PATH, CheckGroup(new PCheckBox("UpdateImage")
			//{
			//	CheckSize = new Vector2(16.0f, 16.0f),
			//	OnChecked = ToggleCheckbox,
			//	BackColor =
			//	PUITuning.Colors.DialogDarkBackground,
			//	CheckColor = PUITuning.Colors.
			//	ComponentDarkStyle
			//}.AddOnRealize((obj) => doUpdateImg = obj), new PTextField("PreviewPath")
			//{
			//	Text = editor.PreviewPath,
			//	MaxLength = 512,
			//	MinWidth = 512,
			//	BackColor =
			//	PUITuning.Colors.DialogDarkBackground,
			//	TextStyle = PUITuning.Fonts.
			//	TextLightStyle,
			//	TextAlignment = TMPro.TextAlignmentOptions.Left
			//}.AddOnRealize((obj) => imagePathField = obj)));
			//body.AddRow(UI.MODIFYDIALOG.DATA_PATH, CheckGroup(new PCheckBox("UpdateData")
			//{
			//	CheckSize = new Vector2(16.0f, 16.0f),
			//	OnChecked = ToggleCheckbox,
			//	BackColor =
			//	PUITuning.Colors.DialogDarkBackground,
			//	CheckColor = PUITuning.Colors.
			//	ComponentDarkStyle
			//}.AddOnRealize((obj) => doUpdateData = obj), new PTextField("DataPath")
			//{
			//	Text = editor.DataPath,
			//	MaxLength = 512,
			//	MinWidth = 512,
			//	BackColor =
			//	PUITuning.Colors.DialogDarkBackground,
			//	TextStyle = PUITuning.Fonts.
			//	TextLightStyle,
			//	TextAlignment = TMPro.TextAlignmentOptions.Left
			//}.AddOnRealize((obj) => dataPathField = obj)));
			//body.AddRow(UI.MODIFYDIALOG.PATCHNOTES, new PTextField("PatchNotes")
			//{
			//	Text = editor.PatchInfo,
			//	MaxLength = 512,
			//	MinWidth = 512,
			//	BackColor = PUITuning.Colors.DialogDarkBackground,
			//	TextStyle = PUITuning.Fonts.
			//	TextLightStyle,
			//	TextAlignment = TMPro.TextAlignmentOptions.Left
			//}.AddOnRealize((obj) => patchNotesField = obj));
			//dialog.Body.AddChild(body);
			dialog.Show();


			return true;
		}
	}
}

