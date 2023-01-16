/*
* AntiDupl.NET Program (http://ermig1979.github.io/AntiDupl).
*
* Copyright (c) 2002-2018 Yermalayeu Ihar, 2013-2018 Borisov Dmitry.
*
* Permission is hereby granted, free of charge, to any person obtaining a copy 
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
* copies of the Software, and to permit persons to whom the Software is 
* furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in 
* all copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
* SOFTWARE.
*/

namespace AntiDupl.NET;

public class Strings
{
	public Strings()
	{
		StringsDefaultEnglish.CopyTo(this);
	}

	public string Name { get; set; }
	public string OriginalLanguageName { get; set; }

	public string OkButton_Text { get; set; }
	public string CancelButton_Text { get; set; }
	public string StopButton_Text { get; set; }
	public string SetDefaultButton_Text { get; set; }

	public string ErrorMessage_FileAlreadyExists { get; set; }

	public string WarningMessage_ChangeFileExtension { get; set; }

	public string AboutProgramPanel_CopyrightLabel0_Text { get; set; }
	public string AboutProgramPanel_CopyrightLabel1_Text { get; set; }
	public string AboutProgramPanel_ComponentLabel_Text { get; set; }
	public string AboutProgramPanel_VersionLabel_Text { get; set; }

	public string AboutProgramForm_Text { get; set; }

	public string StartFinishForm_LoadImages_Text { get; set; }
	public string StartFinishForm_LoadMistakes_Text { get; set; }
	public string StartFinishForm_LoadResults_Text { get; set; }
	public string StartFinishForm_SaveImages_Text { get; set; }
	public string StartFinishForm_SaveMistakes_Text { get; set; }
	public string StartFinishForm_SaveResults_Text { get; set; }
	public string StartFinishForm_ClearTemporary_Text { get; set; }

	public string CoreOptionsForm_Text { get; set; }

	public string CoreOptionsForm_SearchTabPage_Text { get; set; }
	public string CoreOptionsForm_SearchFileTypeGroupBox_Text { get; set; }
	public string CoreOptionsForm_BmpCheckBox_Text { get; set; }
	public string CoreOptionsForm_GifCheckBox_Text { get; set; }
	public string CoreOptionsForm_JpegCheckBox_Text { get; set; }
	public string CoreOptionsForm_PngCheckBox_Text { get; set; }
	public string CoreOptionsForm_TiffCheckBox_Text { get; set; }
	public string CoreOptionsForm_EmfCheckBox_Text { get; set; }
	public string CoreOptionsForm_WmfCheckBox_Text { get; set; }
	public string CoreOptionsForm_ExifCheckBox_Text { get; set; }
	public string CoreOptionsForm_IconCheckBox_Text { get; set; }
	public string CoreOptionsForm_Jp2CheckBox_Text { get; set; }
	public string CoreOptionsForm_PsdCheckBox_Text { get; set; }
	public string CoreOptionsForm_DdsCheckBox_Text { get; set; }
	public string CoreOptionsForm_TgaCheckBox_Text { get; set; }
	public string CoreOptionsForm_WebpCheckBox_Text { get; set; }
	public string CoreOptionsForm_HeifCheckBox_Text { get; set; }
	public string CoreOptionsForm_SearchSystemCheckBox_Text { get; set; }
	public string CoreOptionsForm_SearchHiddenCheckBox_Text { get; set; }

	public string CoreOptionsForm_CompareTabPage_Text { get; set; }
	public string CoreOptionsForm_CheckOnEqualityCheckBox_Text { get; set; }
	public string CoreOptionsForm_TransformedImageCheckBox_Text { get; set; }
	public string CoreOptionsForm_SizeControlCheckBox_Text { get; set; }
	public string CoreOptionsForm_TypeControlCheckBox_Text { get; set; }
	public string CoreOptionsForm_RatioControlCheckBox_Text { get; set; }
	public string CoreOptionsForm_AlgorithmComparingLabeledComboBox_Text { get; set; }
	public string CoreOptionsForm_AlgorithmComparingLabeledComboBox_SquaredSum { get; set; }
	public string CoreOptionsForm_ThresholdDifferenceLabeledComboBox_Text { get; set; }
	public string CoreOptionsForm_MinimalImageSizeLabeledIntegerEdit_Text { get; set; }
	public string CoreOptionsForm_MaximalImageSizeLabeledIntegerEdit_Text { get; set; }
	public string CoreOptionsForm_CompareInsideOneFolderCheckBox_Text { get; set; }
	public string CoreOptionsForm_CompareInsideOneSearchPathCheckBox_Text { get; set; }

	public string CoreOptionsForm_DefectTabPage_Text { get; set; }
	public string CoreOptionsForm_CheckOnDefectCheckBox_Text { get; set; }
	public string CoreOptionsForm_CheckOnBlockinessCheckBox_Text { get; set; }
	public string CoreOptionsForm_BlockinessThresholdLabeledComboBox_Text { get; set; }
	public string CoreOptionsForm_CheckOnBlockinessOnlyNotJpegCheckBox_Text { get; set; }
	public string CoreOptionsForm_CheckOnBlurringCheckBox_Text { get; set; }
	public string CoreOptionsForm_BlurringThresholdLabeledComboBox_Text { get; set; }

	public string CoreOptionsForm_AdvancedTabPage_Text { get; set; }
	public string CoreOptionsForm_DeleteToRecycleBinCheckBox_Text { get; set; }
	public string CoreOptionsForm_MistakeDataBaseCheckBox_Text { get; set; }
	public string CoreOptionsForm_RatioResolutionLabeledComboBox_Text { get; set; }
	public string CoreOptionsForm_CompareThreadCountLabeledComboBox_Text { get; set; }
	public string CoreOptionsForm_CompareThreadCountLabeledComboBox_Description_0 { get; set; }
	public string CoreOptionsForm_CollectThreadCountLabeledComboBox_Text { get; set; }
	public string CoreOptionsForm_CollectThreadCountLabeledComboBox_Description_0 { get; set; }
	public string CoreOptionsForm_ReducedImageSizeLabeledComboBox_Text { get; set; }
	public string CoreOptionsForm_UndoQueueSizeLabeledIntegerEdit_Text { get; set; }
	public string CoreOptionsForm_ResultCountMaxLabeledIntegerEdit_Text { get; set; }
	public string CoreOptionsForm_IgnoreFrameWidthLabeledComboBox_Text { get; set; }
	public string CoreOptionsForm_UseLibJpegTurboCheckBox_Text { get; set; }

	public string CoreOptionsForm_HighlightTabPage_Text { get; set; }
	public string CoreOptionsForm_HighlightDifferenceCheckBox_Text { get; set; }
	public string CoreOptionsForm_DifrentValue_Text { get; set; }
	public string CoreOptionsForm_NotHighlightIfFragmentsMoreThemCheckBox_Text { get; set; }
	public string CoreOptionsForm_MaxFragmentsForDisableHighlightLabeledIntegerEdit_Text { get; set; }
	public string CoreOptionsForm_HighlightAllDifferencesCheckBox_Text { get; set; }
	public string CoreOptionsForm_MaxFragmentsForHighlightLabeledIntegerEdit_Text { get; set; }
	public string CoreOptionsForm_AmountOfFragmentsOnXLabeledIntegerEdit_Text { get; set; }
	public string CoreOptionsForm_AmountOfFragmentsOnYLabeledIntegerEdit_Text { get; set; }
	public string CoreOptionsForm_NormalizedSizeOfImageLabeledIntegerEdit_Text { get; set; }
	public string CoreOptionsForm_PenThicknessLabeledIntegerEdit_Text { get; set; }

	public string CorePathsForm_Text { get; set; }
	public string CorePathsForm_SearchTabPage_Text { get; set; }
	public string CorePathsForm_IgnoreTabPage_Text { get; set; }
	public string CorePathsForm_ValidTabPage_Text { get; set; }
	public string CorePathsForm_DeleteTabPage_Text { get; set; }
	public string CorePathsForm_AddFolderButton_Text { get; set; }
	public string CorePathsForm_AddFilesButton_Text { get; set; }
	public string CorePathsForm_ChangeButton_Text { get; set; }
	public string CorePathsForm_RemoveButton_Text { get; set; }
	public string CorePathsForm_SearchCheckedListBox_ToolTip_Text { get; set; }

	public string ProgressUtils_Completed { get; set; }
	public string ProgressUtils_5HoursRemaining { get; set; }
	public string ProgressUtils_2HoursRemaining { get; set; }
	public string ProgressUtils_5MinutesRemaining { get; set; }
	public string ProgressUtils_2MinutesRemaining { get; set; }
	public string ProgressUtils_5SecondsRemaining { get; set; }

	public string ProgressForm_DeleteDefect { get; set; }
	public string ProgressForm_DeleteFirst { get; set; }
	public string ProgressForm_DeleteSecond { get; set; }
	public string ProgressForm_DeleteBoth { get; set; }
	public string ProgressForm_PerformHint { get; set; }
	public string ProgressForm_Mistake { get; set; }
	public string ProgressForm_RenameCurrent { get; set; }
	public string ProgressForm_RefreshResults { get; set; }
	public string ProgressForm_Undo { get; set; }
	public string ProgressForm_Redo { get; set; }

	public string SearchExecuterForm_Result { get; set; }
	public string SearchExecuterForm_Search { get; set; }
	public string SearchExecuterForm_Stopped { get; set; }
	public string SearchExecuterForm_MinimizeToTaskbarButton_Text { get; set; }
	public string SearchExecuterForm_MinimizeToSystrayButton_Text { get; set; }

	public string ResultsPreviewBase_NextButton_ToolTip_Text { get; set; }
	public string ResultsPreviewBase_PreviousButton_ToolTip_Text { get; set; }

	public string ResultsPreviewDuplPair_DeleteFirstButton_ToolTip_Text { get; set; }
	public string ResultsPreviewDuplPair_DeleteSecondButton_ToolTip_Text { get; set; }
	public string ResultsPreviewDuplPair_DeleteBothButton_ToolTip_Text { get; set; }
	public string ResultsPreviewDuplPair_RenameFirstToSecondButton_ToolTip_Text { get; set; }
	public string ResultsPreviewDuplPair_RenameSecondToFirstButton_ToolTip_Text { get; set; }
	public string ResultsPreviewDuplPair_MistakeButton_ToolTip_Text { get; set; }
	public string ResultsPreviewDuplPair_OpenBothFoldersButton_ToolTip_Text { get; set; }
	public string ResultsPreviewDuplPair_OpenBothImagesButton_ToolTip_Text { get; set; }

	public string ResultsPreviewDefect_DeleteButton_ToolTip_Text { get; set; }
	public string ResultsPreviewDefect_MistakeButton_ToolTip_Text { get; set; }

	public string ResultRowSetter_DefectIcon_ToolTip_Text { get; set; }
	public string ResultRowSetter_DuplPairIcon_ToolTip_Text { get; set; }

	public string ResultRowSetter_UnknownDefectIcon_ToolTip_Text { get; set; }
	public string ResultRowSetter_JpegEndMarkerIsAbsentIcon_ToolTip_Text { get; set; }
	public string ResultRowSetter_blockinessIcon_ToolTip_Text { get; set; }
	public string ResultRowSetter_blurringIcon_ToolTip_Text { get; set; }

	public string ResultRowSetter_DeleteDefectIcon_ToolTip_Text { get; set; }
	public string ResultRowSetter_DeleteFirstIcon_ToolTip_Text { get; set; }
	public string ResultRowSetter_DeleteSecondIcon_ToolTip_Text { get; set; }
	public string ResultRowSetter_RenameFirstToSecondIcon_ToolTip_Text { get; set; }
	public string ResultRowSetter_RenameSecondToFirstIcon_ToolTip_Text { get; set; }

	public string ResultRowSetter_Turn_0_Icon_ToolTip_Text { get; set; }
	public string ResultRowSetter_Turn_90_Icon_ToolTip_Text { get; set; }
	public string ResultRowSetter_Turn_180_Icon_ToolTip_Text { get; set; }
	public string ResultRowSetter_Turn_270_Icon_ToolTip_Text { get; set; }
	public string ResultRowSetter_MirrorTurn_0_Icon_ToolTip_Text { get; set; }
	public string ResultRowSetter_MirrorTurn_90_Icon_ToolTip_Text { get; set; }
	public string ResultRowSetter_MirrorTurn_180_Icon_ToolTip_Text { get; set; }
	public string ResultRowSetter_MirrorTurn_270_Icon_ToolTip_Text { get; set; }

	public string ResultsListView_Type_Column_Text { get; set; }
	public string ResultsListView_Group_Column_Text { get; set; }
	public string ResultsListView_GroupSize_Column_Text { get; set; }
	public string ResultsListView_Difference_Column_Text { get; set; }
	public string ResultsListView_Defect_Column_Text { get; set; }
	public string ResultsListView_Transform_Column_Text { get; set; }
	public string ResultsListView_Hint_Column_Text { get; set; }

	public string ResultsListView_FileName_Column_Text { get; set; }
	public string ResultsListView_FileDirectory_Column_Text { get; set; }
	public string ResultsListView_ImageSize_Column_Text { get; set; }
	public string ResultsListView_ImageType_Column_Text { get; set; }
	public string ResultsListView_Blockiness_Column_Text { get; set; }
	public string ResultsListView_Blurring_Column_Text { get; set; }
	public string ResultsListView_FileSize_Column_Text { get; set; }
	public string ResultsListView_FileTime_Column_Text { get; set; }

	public string ResultsListView_FirstFileName_Column_Text { get; set; }
	public string ResultsListView_FirstFileDirectory_Column_Text { get; set; }
	public string ResultsListView_FirstImageSize_Column_Text { get; set; }
	public string ResultsListView_FirstImageType_Column_Text { get; set; }
	public string ResultsListView_FirstBlockiness_Column_Text { get; set; }
	public string ResultsListView_FirstBlurring_Column_Text { get; set; }
	public string ResultsListView_FirstFileSize_Column_Text { get; set; }
	public string ResultsListView_FirstFileTime_Column_Text { get; set; }
	public string ResultsListView_SecondFileName_Column_Text { get; set; }
	public string ResultsListView_SecondFileDirectory_Column_Text { get; set; }
	public string ResultsListView_SecondImageSize_Column_Text { get; set; }
	public string ResultsListView_SecondImageType_Column_Text { get; set; }
	public string ResultsListView_SecondBlockiness_Column_Text { get; set; }
	public string ResultsListView_SecondBlurring_Column_Text { get; set; }
	public string ResultsListView_SecondFileSize_Column_Text { get; set; }
	public string ResultsListView_SecondFileTime_Column_Text { get; set; }

	public string ResultsListViewContextMenu_DeleteDefectItem_Text { get; set; }
	public string ResultsListViewContextMenu_DeleteFirstItem_Text { get; set; }
	public string ResultsListViewContextMenu_DeleteSecondItem_Text { get; set; }
	public string ResultsListViewContextMenu_DeleteBothItem_Text { get; set; }
	public string ResultsListViewContextMenu_RenameFirstToSecondIcon_ToolTip_Text { get; set; }
	public string ResultsListViewContextMenu_RenameSecondToFirstIcon_ToolTip_Text { get; set; }
	public string ResultsListViewContextMenu_RenameFirstLikeSecondButton_ToolTip_Text { get; set; }
	public string ResultsListViewContextMenu_RenameSecondLikeFirstButton_ToolTipText { get; set; }
	public string ResultsListViewContextMenu_MoveFirstToSecondButton_ToolTipText { get; set; }
	public string ResultsListViewContextMenu_MoveSecondToFirstButton_ToolTipText { get; set; }
	public string ResultsListViewContextMenu_MistakeItem_Text { get; set; }
	public string ResultsListViewContextMenu_PerformHintItem_Text { get; set; }

	public string MainStatusStrip_TotalLabel_Text { get; set; }
	public string MainStatusStrip_CurrentLabel_Text { get; set; }
	public string MainStatusStrip_SelectedLabel_Text { get; set; }

	public string MainMenu_FileMenuItem_Text { get; set; }
	public string MainMenu_File_OpenProfileMenuItem_Text { get; set; }
	public string MainMenu_File_SaveProfileAsMenuItem_Text { get; set; }
	public string MainMenu_File_LoadProfileOnLoadingMenuItem_Text { get; set; }
	public string MainMenu_File_SaveProfileOnClosingMenuItem_Text { get; set; }
	public string MainMenu_File_ExitMenuItem_Text { get; set; }

	public string MainMenu_EditMenuItem_Text { get; set; }
	public string MainMenu_Edit_UndoMenuItem_Text { get; set; }
	public string MainMenu_Edit_RedoMenuItem_Text { get; set; }
	public string MainMenu_Edit_SelectAllMenuItem_Text { get; set; }

	public string MainMenu_ViewMenuItem_Text { get; set; }
	public string MainMenu_View_ToolMenuItem_Text { get; set; }
	public string MainMenu_View_StatusMenuItem_Text { get; set; }
	public string MainMenu_View_SelectColumnsMenuItem_Text { get; set; }
	public string MainMenu_View_HotKeysMenuItem_Text { get; set; }
	public string MainMenu_View_StretchSmallImagesMenuItem_Text { get; set; }
	public string MainMenu_View_ProportionalImageSizeMenuItem_Text { get; set; }
	public string MainMenu_View_ShowNeighbourImageMenuItem_Text { get; set; }

	public string MainMenu_SearchMenuItem_Text { get; set; }
	public string MainMenu_Search_StartMenuItem_Text { get; set; }
	public string MainMenu_Search_RefreshResultsMenuItem_Text { get; set; }
	public string MainMenu_Search_RefreshImagesMenuItem_Text { get; set; }
	public string MainMenu_Search_PathsMenuItem_Text { get; set; }
	public string MainMenu_Search_OptionsMenuItem_Text { get; set; }
	public string MainMenu_Search_OnePathMenuItem_Text { get; set; }
	public string MainMenu_Search_UseImageDataBaseMenuItem_Text { get; set; }
	public string MainMenu_Search_CheckResultsAtLoadingMenuItem_Text { get; set; }
	public string MainMenu_Search_CheckMistakesAtLoadingMenuItem_Text { get; set; }

	public string MainMenu_HelpMenuItem_Text { get; set; }
	public string MainMenu_Help_HelpMenuItem_Text { get; set; }
	public string MainMenu_Help_AboutProgramMenuItem_Text { get; set; }
	public string MainMenu_Help_CheckingForUpdatesMenuItem_Text { get; set; }

	public string MainMenu_NewVersionMenuItem_Text { get; set; }
	public string MainMenu_NewVersionMenuItem_Tooltip { get; set; }

	public string SelectHotKeysForm_InvalidHotKeyToolTipText { get; set; }

	public string LanguageMenuItem_Text { get; set; }

	public string ViewModeMenuItem_Text { get; set; }
	public string ViewModeMenuItem_VerticalPairTableMenuItem_Text { get; set; }
	public string ViewModeMenuItem_HorizontalPairTableMenuItem_Text { get; set; }
	public string ViewModeMenuItem_GroupedThumbnailsMenuItem_Text { get; set; }

	public string ImagePreviewContextMenu_CopyPathItem_Text { get; set; }
	public string ImagePreviewContextMenu_CopyFileNameItem_Text { get; set; }
	public string ImagePreviewContextMenu_OpenImageItem_Text { get; set; }
	public string ImagePreviewContextMenu_OpenFolderItem_Text { get; set; }
	public string ImagePreviewContextMenu_AddToIgnore_Text { get; set; }
	public string ImagePreviewContextMenu_AddToIgnoreDirectory_Text { get; set; }
	public string ImagePreviewContextMenu_RenameImageItem_Text { get; set; }
	public string ImagePreviewContextMenu_RenameImageLikeNeighbour_Text { get; set; }
	public string ImagePreviewContextMenu_MoveImageToNeighbourItem_Text { get; set; }
	public string ImagePreviewContextMenu_MoveAndRenameImageToNeighbourItem_Text { get; set; }
	public string ImagePreviewContextMenu_MoveGroupToNeighbourItem_Text { get; set; }
	public string ImagePreviewContextMenu_RenameGroupAsNeighbourItem_Text { get; set; }

	public string ImagePreviewPanel_EXIF_Text { get; set; }
	public string ImagePreviewPanel_EXIF_Tooltip_ImageDescription { get; set; }
	public string ImagePreviewPanel_EXIF_Tooltip_EquipMake { get; set; }
	public string ImagePreviewPanel_EXIF_Tooltip_EquipModel { get; set; }
	public string ImagePreviewPanel_EXIF_Tooltip_SoftwareUsed { get; set; }
	public string ImagePreviewPanel_EXIF_Tooltip_DateTime { get; set; }
	public string ImagePreviewPanel_EXIF_Tooltip_Artist { get; set; }
	public string ImagePreviewPanel_EXIF_Tooltip_UserComment { get; set; }
}
