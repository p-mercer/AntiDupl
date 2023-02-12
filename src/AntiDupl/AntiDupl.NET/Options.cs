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
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace AntiDupl.NET;

public class Options
{
	public delegate void VisibleChangeHandler(bool visible);

	public delegate void ChangeHandler();
	public event ChangeHandler OnChange;

	public void Change()
	{
		OnChange?.Invoke();
	}

	private string m_language = StringsDefaultEnglish.Get().Name;
	public string Language
	{
		get { return m_language; }
		set
		{
			if (m_language != value)
			{
				m_language = value;
				Resources.Strings.SetCurrent(m_language);
			}
		}
	}

	public bool OnePath { get; set; }
	public bool CheckingForUpdates { get; set; } = true;
	public bool UseImageDataBase { get; set; } = true;
	public bool CheckResultsAtLoading { get; set; } = true;
	public bool CheckMistakesAtLoading { get; set; } = true;
	public bool LoadProfileOnLoading { get; set; } = true;
	public bool SaveProfileOnClosing { get; set; } = true;

	public MainFormOptions mainFormOptions = new();
	public ResultsOptions resultsOptions = new();
	public HotKeyOptions hotKeyOptions = new();

	public string CoreOptionsFileName { get; set; } = GetDefaultCoreOptionsFileName();

	public string GetResultsFileName()
	{
		return Path.ChangeExtension(CoreOptionsFileName, ".adr");
	}

	public static Options Load()
	{
		var fileInfo = new FileInfo(GetOptionsFileName());
		if (fileInfo.Exists)
		{
			try
			{
				var xmlSerializer = new XmlSerializer(typeof(Options));
				using var fileStream = new FileStream(GetOptionsFileName(), FileMode.Open, FileAccess.Read);
				var options = (Options)xmlSerializer.Deserialize(fileStream);
				options.resultsOptions.Check();

				return options;
			}
			catch
			{
				return new Options();
			}
		}
		else
		{
			return new Options();
		}
	}

	public Options()
	{
	}

	public Options(Options options)
	{
		resultsOptions = new ResultsOptions(options.resultsOptions);
		mainFormOptions = new MainFormOptions(options.mainFormOptions);
		hotKeyOptions = new HotKeyOptions(options.hotKeyOptions);
		CoreOptionsFileName = (string)options.CoreOptionsFileName.Clone();

		Language = options.Language;
		OnePath = options.OnePath;
		CheckingForUpdates = options.CheckingForUpdates;
		UseImageDataBase = options.UseImageDataBase;
		CheckResultsAtLoading = options.CheckResultsAtLoading;
		CheckMistakesAtLoading = options.CheckMistakesAtLoading;
		LoadProfileOnLoading = options.LoadProfileOnLoading;
		SaveProfileOnClosing = options.SaveProfileOnClosing;
	}

	public void Save()
	{
		try
		{
			using TextWriter writer = new StreamWriter(GetOptionsFileName());
			var xmlSerializer = new XmlSerializer(typeof(Options));
			xmlSerializer.Serialize(writer, this);
		}
		catch
		{
		}
	}

	public Options Clone()
	{
		return new Options(this);
	}

	public void CopyTo(ref Options options)
	{
		resultsOptions.CopyTo(ref options.resultsOptions);
		mainFormOptions.CopyTo(ref options.mainFormOptions);
		hotKeyOptions.CopyTo(ref options.hotKeyOptions);
		options.CoreOptionsFileName = (string)CoreOptionsFileName.Clone();

		options.Language = Language;
		options.OnePath = OnePath;
		options.CheckingForUpdates = CheckingForUpdates;
		options.UseImageDataBase = UseImageDataBase;
		options.CheckResultsAtLoading = CheckResultsAtLoading;
		options.CheckMistakesAtLoading = CheckMistakesAtLoading;
		options.LoadProfileOnLoading = LoadProfileOnLoading;
		options.SaveProfileOnClosing = SaveProfileOnClosing;
	}

	public static void PathCopy(string[] source, ref string[] destination)
	{
		destination = new string[source.GetLength(0)];
		for (var i = 0; i < source.GetLength(0); ++i)
		{
			destination[i] = (string)source[i].Clone();
		}
	}

	public static string[] PathClone(string[] path)
	{
		var clone = System.Array.Empty<string>();
		PathCopy(path, ref clone);
		return clone;
	}

	public static bool Equals(string[] path1, string[] path2)
	{
		if (path1.Length != path2.Length)
		{
			return false;
		}

		for (var i = 0; i < path1.Length; ++i)
		{
			if (path1[i].CompareTo(path2[i]) != 0)
			{
				return false;
			}
		}

		return true;
	}

	public static string GetOptionsFileName()
	{
		var builder = new StringBuilder();
		builder.Append(Resources.UserPath);
		builder.Append(@"\options.xml");
		return builder.ToString();
	}

	public static string GetMistakeDataBaseFileName()
	{
		var builder = new StringBuilder();
		builder.Append(Resources.UserPath);
		builder.Append(@"\mistakes.adm");
		return builder.ToString();
	}

	public static string GetDefaultCoreOptionsFileName()
	{
		return $@"{Resources.ProfilesPath}\default.xml";
	}

	public bool Equals(Options options)
	{
		if (CheckingForUpdates != options.CheckingForUpdates)
		{
			return false;
		}

		if (OnePath != options.OnePath)
		{
			return false;
		}

		if (UseImageDataBase != options.UseImageDataBase)
		{
			return false;
		}

		if (CheckResultsAtLoading != options.CheckResultsAtLoading)
		{
			return false;
		}

		if (CheckMistakesAtLoading != options.CheckMistakesAtLoading)
		{
			return false;
		}

		if (LoadProfileOnLoading != options.LoadProfileOnLoading)
		{
			return false;
		}

		if (SaveProfileOnClosing != options.SaveProfileOnClosing)
		{
			return false;
		}

		if (m_language != options.m_language)
		{
			return false;
		}

		if (!resultsOptions.Equals(options.resultsOptions))
		{
			return false;
		}

		if (!mainFormOptions.Equals(options.mainFormOptions))
		{
			return false;
		}

		if (!hotKeyOptions.Equals(options.hotKeyOptions))
		{
			return false;
		}

		if (!CoreOptionsFileName.Equals(options.CoreOptionsFileName))
		{
			return false;
		}

		return true;
	}
}
