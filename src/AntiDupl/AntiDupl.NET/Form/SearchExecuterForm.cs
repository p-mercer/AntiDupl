/*
* AntiDupl.NET Program (http://ermig1979.github.io/AntiDupl).
*
* Copyright (c) 2002-2018 Yermalayeu Ihar.
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
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace AntiDupl.NET;

public class SearchExecuterForm : Form
{
	private enum State
	{
		Start,
		ClearResults,
		ClearTemporary,
		LoadImages,
		Search,
		SetGroup,
		SetHint,
		SaveImages,
		Stopped,
		Finish
	}

	private State m_state = State.Start;

	private readonly CoreLib m_core;
	private readonly Options m_options;
	private readonly CoreOptions m_coreOptions;
	private readonly MainSplitContainer m_mainSplitContainer;
	private readonly MainForm m_mainForm;
	private System.Windows.Forms.Timer m_timer;
	private DateTime m_startDateTime;
	private FormWindowState m_mainFormWindowState;

	private Button m_stopButton;
	private Button m_minimizeToTaskbarButton;
	private Button m_minimizeToSystrayButton;
	private ProgressPanel m_progressPanel;
	private NotifyIcon m_notifyIcon;

	public SearchExecuterForm(CoreLib core, Options options, CoreOptions coreOptions, MainSplitContainer mainSplitContainer, MainForm mainForm)
	{
		m_core = core;
		m_options = options;
		m_coreOptions = coreOptions;
		m_mainSplitContainer = mainSplitContainer;
		m_mainForm = mainForm;
		m_mainFormWindowState = m_mainForm.WindowState;
		InitializeComponent();
		UpdateStrings();
		Owner = m_mainForm;
		m_mainForm.Resize += new EventHandler(OnMainFormResize);
	}

	private void InitializeComponent()
	{
		var mainTableLayoutPanel = InitFactory.Layout.Create(1, 2);
		mainTableLayoutPanel.Padding = new Padding(1, 5, 1, 0);
		Controls.Add(mainTableLayoutPanel);

		m_progressPanel = new ProgressPanel();
		mainTableLayoutPanel.Controls.Add(m_progressPanel, 0, 0);

		var buttonsTableLayoutPanel = InitFactory.Layout.Create(5, 1);
		buttonsTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
		buttonsTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
		buttonsTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
		buttonsTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
		buttonsTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
		mainTableLayoutPanel.Controls.Add(buttonsTableLayoutPanel, 0, 1);

		m_stopButton = new Button();
		m_stopButton.Click += new EventHandler(OnStopButtonClick);
		m_stopButton.AutoSize = true;
		buttonsTableLayoutPanel.Controls.Add(m_stopButton, 1, 0);

		m_minimizeToTaskbarButton = new Button();
		m_minimizeToTaskbarButton.Click += new EventHandler(OnMinimizeToTaskbarButtonClick);
		m_minimizeToTaskbarButton.AutoSize = true;
		buttonsTableLayoutPanel.Controls.Add(m_minimizeToTaskbarButton, 2, 0);

		m_minimizeToSystrayButton = new Button();
		m_minimizeToSystrayButton.Click += new EventHandler(OnMinimizeToSystrayButtonClick);
		m_minimizeToSystrayButton.AutoSize = true;
		buttonsTableLayoutPanel.Controls.Add(m_minimizeToSystrayButton, 3, 0);

		m_notifyIcon = new NotifyIcon
		{
			Icon = Resources.Icons.Get(),
			Text = Application.ProductName
		};
		m_notifyIcon.DoubleClick += new EventHandler(OnNotifyIconDoubleClick);

		FormBorderStyle = FormBorderStyle.FixedDialog;
		StartPosition = FormStartPosition.CenterScreen;
		ShowInTaskbar = false;
		ControlBox = false;
		MaximizeBox = false;
		MinimizeBox = false;
		KeyPreview = true;

		var width = 800;
		var height = m_progressPanel.Height + mainTableLayoutPanel.Margin.Vertical +
		  m_stopButton.Height + m_stopButton.Padding.Vertical + mainTableLayoutPanel.Margin.Vertical +
		  mainTableLayoutPanel.Padding.Vertical;
		ClientSize = new Size(width, height);


		m_timer = new System.Windows.Forms.Timer
		{
			Interval = 100
		};
		m_timer.Tick += new EventHandler(TimerCallback);
		m_timer.Start();

		KeyDown += new KeyEventHandler(OnKeyDown);
	}

	private void CoreThreadTask()
	{
		m_startDateTime = DateTime.Now;
		m_coreOptions.Set(m_core, m_options.OnePath);
		m_state = State.ClearResults;
		m_core.Clear(CoreDll.FileType.Result);
		m_state = State.ClearTemporary;
		m_core.Clear(CoreDll.FileType.Temporary);
		if (m_options.UseImageDataBase)
		{
			m_state = State.LoadImages;
			m_core.Load(CoreDll.FileType.ImageDataBase, m_coreOptions.GetImageDataBasePath(), false);
		}
		m_state = State.Search;
		m_core.Search();
		m_state = State.SetGroup;
		m_core.ApplyToResult(CoreDll.GlobalActionType.SetGroup);
		m_state = State.SetHint;
		m_core.ApplyToResult(CoreDll.GlobalActionType.SetHint);
		if (m_options.UseImageDataBase)
		{
			m_state = State.SaveImages;
			m_core.Save(CoreDll.FileType.ImageDataBase, m_coreOptions.GetImageDataBasePath());
		}
		m_core.Clear(CoreDll.FileType.ImageDataBase);
		m_core.SortResult((CoreDll.SortType)m_options.resultsOptions.SortTypeDefault, m_options.resultsOptions.IncreasingDefault);
		m_state = State.Finish;
		LogPerformance(DateTime.Now - m_startDateTime, m_core.GetStatistic());
	}

	public void Execute()
	{
		m_mainSplitContainer.ClearResults();
		m_state = State.Start;
		var searchThread = new Thread(CoreThreadTask);
		searchThread.Start();
		m_stopButton.Enabled = true;
		ShowDialog();
	}

	private void TimerCallback(object obj, EventArgs eventArgs)
	{
		if (m_state == State.Finish)
		{
			if (m_notifyIcon.Visible)
			{
				OnNotifyIconDoubleClick(null, null);
			}

			m_timer.Stop();
			Close();
			m_mainForm.Resize -= new EventHandler(OnMainFormResize);
			m_mainForm.WindowState = m_mainFormWindowState;
			m_mainForm.UpdateCaption();
			m_mainForm.Activate();
			m_mainSplitContainer.UpdateResults();
		}
		else
		{
			var builder = new StringBuilder();
			if (m_notifyIcon.Visible || WindowState == FormWindowState.Minimized)
			{
				builder.Append(Application.ProductName);
				builder.Append(" - ");
			}
			var s = Resources.Strings.Current;
			switch (m_state)
			{
				case State.Start:
				case State.ClearResults:
				case State.ClearTemporary:
					{
						m_stopButton.Enabled = false;
						builder.Append(s.StartFinishForm_ClearTemporary_Text);
						builder.Append("...");
						EstimateOtherProgress();
					}
					break;
				case State.LoadImages:
					{
						m_stopButton.Enabled = false;
						builder.Append(s.StartFinishForm_LoadImages_Text);
						builder.Append("...");
						EstimateOtherProgress();
					}
					break;
				case State.Search:
					{
						m_stopButton.Enabled = true;
						builder.Append(s.SearchExecuterForm_Search);
						var progress = EstimateSearchProgress();
						builder.AppendFormat(" ({0})...", ProgressUtils.GetProgressString(progress, m_startDateTime));
					}
					break;
				case State.SetGroup:
				case State.SetHint:
					{
						m_stopButton.Enabled = false;
						builder.Append(s.SearchExecuterForm_Result);
						builder.Append("...");
						EstimateOtherProgress();
					}
					break;
				case State.SaveImages:
					{
						m_stopButton.Enabled = false;
						builder.Append(s.StartFinishForm_SaveImages_Text);
						builder.Append("...");
						EstimateOtherProgress();
					}
					break;
				case State.Stopped:
					{
						builder.Append(s.SearchExecuterForm_Stopped);
						builder.Append("...");
					}
					break;
			}
			if (m_notifyIcon.Visible)
			{
				m_notifyIcon.Text = builder.ToString();
			}
			else if (WindowState == FormWindowState.Minimized)
			{
				m_mainForm.Text = builder.ToString();
			}
			else
			{
				Text = builder.ToString();
				m_mainForm.UpdateCaption();
			}
		}
	}

	private void OnStopButtonClick(object obj, EventArgs eventArgs)
	{
		if (m_state == State.Search)
		{
			m_core.Stop();
			m_state = State.Stopped;
			m_stopButton.Enabled = false;
		}
	}

	private void OnMinimizeToTaskbarButtonClick(object obj, EventArgs eventArgs)
	{
		m_mainFormWindowState = m_mainForm.WindowState;
		WindowState = FormWindowState.Minimized;
		m_mainForm.WindowState = FormWindowState.Minimized;
	}

	private void OnMainFormResize(object obj, EventArgs eventArgs)
	{
		if (!Modal)
		{
			m_mainForm.Hide();
			m_mainForm.WindowState = m_mainFormWindowState;
			m_mainForm.Show();
			WindowState = FormWindowState.Normal;
			ShowDialog();
			Activate();
		}
	}

	private void OnMinimizeToSystrayButtonClick(object obj, EventArgs eventArgs)
	{
		m_mainFormWindowState = m_mainForm.WindowState;
		m_notifyIcon.Visible = true;
		Hide();
		m_mainForm.Hide();
		Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.Idle;
	}

	private void OnNotifyIconDoubleClick(object obj, EventArgs eventArgs)
	{
		Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.Normal;
		m_notifyIcon.Visible = false;
		m_mainForm.WindowState = m_mainFormWindowState;
		m_mainForm.Show();
		WindowState = FormWindowState.Normal;
		ShowDialog();
		Activate();
	}

	private void UpdateStrings()
	{
		var s = Resources.Strings.Current;

		m_stopButton.Text = s.StopButton_Text;
		m_minimizeToTaskbarButton.Text = s.SearchExecuterForm_MinimizeToTaskbarButton_Text;
		m_minimizeToSystrayButton.Text = s.SearchExecuterForm_MinimizeToSystrayButton_Text;
	}

	private double EstimateSearchProgress()
	{
		double progress = 0;
		int total = 0, currentFirst = 0, currentSecond = 0;
		var path = "";

		var mainThreadStatus = m_core.StatusGet(CoreDll.ThreadType.Main, 0);
		if (mainThreadStatus != null)
		{
			total = mainThreadStatus.Total;
			if (mainThreadStatus.Current > 0)
			{
				if (m_coreOptions.CompareOptions.CheckOnEquality)
				{
					for (var i = 0; ; i++)
					{
						var compareThreadStatus = m_core.StatusGet(CoreDll.ThreadType.Compare, i);
						if (compareThreadStatus == null)
						{
							break;
						}

						if (i == 0)
						{
							path = compareThreadStatus.Path;
						}
						currentFirst += compareThreadStatus.Current;
						currentSecond += compareThreadStatus.Total;
					}
				}
				else
				{
					currentFirst = mainThreadStatus.Current;
					for (var i = 0; ; i++)
					{
						var collectThreadStatus = m_core.StatusGet(CoreDll.ThreadType.Collect, i);
						if (collectThreadStatus == null)
						{
							break;
						}

						if (i == 0)
						{
							path = collectThreadStatus.Path;
						}
						currentFirst += collectThreadStatus.Current;
						currentFirst -= collectThreadStatus.Total;
					}
				}
			}
			else
			{
				path = mainThreadStatus.Path;
			}
		}

		m_progressPanel.UpdateStatus(total, currentFirst, currentSecond, path);

		if (total > 0)
		{
			progress = currentFirst / (double)total;
		}

		return progress;
	}

	private void EstimateOtherProgress()
	{
		var status = m_core.StatusGet(CoreDll.ThreadType.Main, 0);
		if (status != null)
		{
			m_progressPanel.UpdateStatus(status.Total, status.Current, status.Current, "");
		}
		else
		{
			m_progressPanel.UpdateStatus(0, 0, 0, "");
		}
	}

	private void OnKeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyData == Keys.Escape)
		{
			if (m_state == State.Search)
			{
				m_core.Stop();
				m_state = State.Stopped;
				m_stopButton.Enabled = false;
			}
		}
		else
		{
			e.Handled = true;
		}
	}

	private void LogPerformance(TimeSpan time, CoreStatistic statistic)
	{
		var writer = File.AppendText(Resources.Logs.Performance);

		writer.WriteLine("---------------------------------------------------------------");
		writer.WriteLine($"Search start time: {m_startDateTime}");
		writer.WriteLine($"Elapsed time: {time}");
		writer.WriteLine($"Found {MemoryString(statistic.SearchedImageSize)} of {statistic.SearchedImageNumber} images in {statistic.ScanedFolderNumber} folders.");
		writer.WriteLine($"Processed {statistic.ComparedImageNumber} images.");
		writer.WriteLine($"Found {statistic.DefectImageNumber} defects and {statistic.DuplImagePairNumber} duples.");
		writer.WriteLine($"Used {statistic.CollectThreadCount} load and {statistic.CompareThreadCount} compare threads.");
		writer.WriteLine($"Use image database: {m_options.UseImageDataBase}.");
		writer.WriteLine($"Use libjpeg-turbo: {m_coreOptions.AdvancedOptions.UseLibJpegTurbo}.");

		writer.Close();
	}

	private static string MemoryString(ulong size)
	{
		const ulong KB = 1024;
		const ulong MB = 1024 * 1024;
		const ulong GB = 1024 * 1024 * 1024;
		if (size > GB * 0.977)
		{
			return $"{(double)size / GB:F1} GB";
		}
		else if (size > MB * 0.977)
		{
			return $"{(double)size / MB:F1} MB";
		}
		else if (size > KB * 0.977)
		{
			return $"{(double)size / KB:F1} KB";
		}
		else
		{
			return $"{size} B";
		}
	}
}
