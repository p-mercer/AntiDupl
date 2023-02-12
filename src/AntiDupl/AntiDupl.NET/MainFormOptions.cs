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
using System.Drawing;
using System.Windows.Forms;

namespace AntiDupl.NET;

public class MainFormOptions
{
	public event Options.VisibleChangeHandler OnToolStripVisibleChange;
	private bool m_toolStripView = true;
	public bool ToolStripView
	{
		get { return m_toolStripView; }
		set
		{
			if (m_toolStripView != value)
			{
				m_toolStripView = value;
				OnToolStripVisibleChange?.Invoke(m_toolStripView);
			}
		}
	}

	public event Options.VisibleChangeHandler OnStatusStripVisibleChange;
	private bool m_statusStripView = true;
	public bool StatusStripView
	{
		get { return m_statusStripView; }
		set
		{
			if (m_statusStripView != value)
			{
				m_statusStripView = value;
				OnStatusStripVisibleChange?.Invoke(m_statusStripView);
			}
		}
	}

	public bool Maximized { get; set; }

	public Point Location { get; set; } = DefaultLocation();
	public Size Size { get; set; } = new(MainForm.MIN_WIDTH, MainForm.MIN_HEIGHT);

	public MainFormOptions()
	{
		SetDefault();
	}

	public MainFormOptions(MainFormOptions options)
	{
		Location = options.Location;
		Size = options.Size;
		Maximized = options.Maximized;
		m_toolStripView = options.m_toolStripView;
		m_statusStripView = options.m_statusStripView;
	}

	public void CopyTo(ref MainFormOptions options)
	{
		options.Location = Location;
		options.Size = Size;
		options.Maximized = Maximized;
		options.m_toolStripView = m_toolStripView;
		options.m_statusStripView = m_statusStripView;
	}

	public bool Equals(MainFormOptions options)
	{
		if (Location != options.Location)
		{
			return false;
		}

		if (Size != options.Size)
		{
			return false;
		}

		if (Maximized != options.Maximized)
		{
			return false;
		}

		if (m_toolStripView != options.m_toolStripView)
		{
			return false;
		}

		if (m_statusStripView != options.m_statusStripView)
		{
			return false;
		}

		return true;
	}

	public void SetDefault()
	{
		Location = DefaultLocation();
		Size = new Size(MainForm.MIN_WIDTH, MainForm.MIN_HEIGHT);
		Maximized = false;
		m_toolStripView = true;
		m_statusStripView = true;
	}

	private static Point DefaultLocation()
	{
		var rect = Screen.PrimaryScreen.WorkingArea;
		var left = (rect.Left + rect.Width - MainForm.MIN_WIDTH) / 2;
		var top = (rect.Top + rect.Height - MainForm.MIN_HEIGHT) / 2;
		return new Point(left, top);
	}
}
