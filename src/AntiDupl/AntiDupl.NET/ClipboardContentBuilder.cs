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
using System.Text;
using TypeHorizontal = AntiDupl.NET.ResultsListView.ColumnsTypeHorizontal;
using TypeVertical = AntiDupl.NET.ResultsListView.ColumnsTypeVertical;

namespace AntiDupl.NET;

public class ClipboardContentBuilder
{
	private readonly StringBuilder m_builder;
	private readonly ResultsOptions m_options;
	private bool m_insertTab;

	public ClipboardContentBuilder(ResultsOptions options)
	{
		m_options = options;
		m_builder = new StringBuilder();
	}

	public void Add(CoreResult result)
	{
		m_insertTab = false;
		switch (m_options.viewMode)
		{
			case ResultsOptions.ViewMode.VerticalPairTable:
				AddCommon(result, m_options.columnOptionsVertical);
				AddVertical(result, m_options.columnOptionsVertical);
				break;
			case ResultsOptions.ViewMode.HorizontalPairTable:
				AddCommon(result, m_options.columnOptionsHorizontal);
				AddHorizontal(result, m_options.columnOptionsHorizontal);
				break;
		}
		m_builder.AppendLine("");
	}

	public override string ToString()
	{
		return m_builder.ToString();
	}

	private void AddCommon(CoreResult result, ResultsOptions.ColumnOptions[] options)
	{
		if (options[(int)TypeVertical.Type].visible)
		{
			Append(result.Type);
		}

		if (options[(int)TypeVertical.Group].visible)
		{
			Append(result.Group);
		}

		if (options[(int)TypeVertical.Difference].visible)
		{
			Append(result.Difference.ToString("F2"));
		}

		if (options[(int)TypeVertical.Defect].visible)
		{
			Append(result.Defect);
		}

		if (options[(int)TypeVertical.Transform].visible)
		{
			Append(result.Transform);
		}

		if (options[(int)TypeVertical.Hint].visible)
		{
			Append(result.Hint);
		}
	}

	private void AddVertical(CoreResult result, ResultsOptions.ColumnOptions[] options)
	{
		if (options[(int)TypeVertical.FileName].visible ||
			options[(int)TypeVertical.FileDirectory].visible)
		{
			Append(result.First.Path);
		}

		if (options[(int)TypeVertical.ImageSize].visible)
		{
			Append(result.First.GetImageSizeString());
		}

		if (options[(int)TypeVertical.ImageType].visible)
		{
			Append(result.First.GetImageTypeString());
		}

		if (options[(int)TypeVertical.FileSize].visible)
		{
			Append(result.First.GetFileSizeString());
		}

		if (options[(int)TypeVertical.FileTime].visible)
		{
			Append(result.First.GetFileTimeString());
		}

		if (options[(int)TypeVertical.FileName].visible ||
				options[(int)TypeVertical.FileDirectory].visible)
		{
			Append(result.Second.Path);
		}

		if (options[(int)TypeVertical.ImageSize].visible)
		{
			Append(result.Second.GetImageSizeString());
		}

		if (options[(int)TypeVertical.ImageType].visible)
		{
			Append(result.Second.GetImageTypeString());
		}

		if (options[(int)TypeVertical.FileSize].visible)
		{
			Append(result.Second.GetFileSizeString());
		}

		if (options[(int)TypeVertical.FileTime].visible)
		{
			Append(result.Second.GetFileTimeString());
		}
	}

	private void AddHorizontal(CoreResult result, ResultsOptions.ColumnOptions[] options)
	{
		if (options[(int)TypeHorizontal.FirstFileName].visible ||
			options[(int)TypeHorizontal.FirstFileDirectory].visible)
		{
			Append(result.First.Path);
		}

		if (options[(int)TypeHorizontal.FirstImageSize].visible)
		{
			Append(result.First.GetImageSizeString());
		}

		if (options[(int)TypeHorizontal.FirstImageType].visible)
		{
			Append(result.First.GetImageTypeString());
		}

		if (options[(int)TypeHorizontal.FirstFileSize].visible)
		{
			Append(result.First.GetFileSizeString());
		}

		if (options[(int)TypeHorizontal.FirstFileTime].visible)
		{
			Append(result.First.GetFileTimeString());
		}

		if (options[(int)TypeHorizontal.SecondFileName].visible ||
				options[(int)TypeHorizontal.SecondFileDirectory].visible)
		{
			Append(result.Second.Path);
		}

		if (options[(int)TypeHorizontal.SecondImageSize].visible)
		{
			Append(result.Second.GetImageSizeString());
		}

		if (options[(int)TypeHorizontal.SecondImageType].visible)
		{
			Append(result.Second.GetImageTypeString());
		}

		if (options[(int)TypeHorizontal.SecondFileSize].visible)
		{
			Append(result.Second.GetFileSizeString());
		}

		if (options[(int)TypeHorizontal.SecondFileTime].visible)
		{
			Append(result.Second.GetFileTimeString());
		}
	}

	private void Append(object value)
	{
		if (m_insertTab)
		{
			m_builder.Append('\t');
		}

		m_builder.Append(value.ToString());
		m_insertTab = true;
	}
}
