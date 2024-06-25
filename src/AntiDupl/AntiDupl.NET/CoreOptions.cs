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
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace AntiDupl.NET;

public sealed class CoreOptions
{
	public CoreSearchOptions SearchOptions { get; set; }
	public CoreCompareOptions CompareOptions { get; set; }
	public CoreDefectOptions DefectOptions { get; set; }
	public CoreAdvancedOptions AdvancedOptions { get; set; }

	public CorePathWithSubFolder[] searchPath;
	public CorePathWithSubFolder[] ignorePath;
	public CorePathWithSubFolder[] validPath;
	public CorePathWithSubFolder[] deletePath;

	public CoreOptions()
	{
		SearchOptions = new CoreSearchOptions();
		CompareOptions = new CoreCompareOptions();
		DefectOptions = new CoreDefectOptions();
		AdvancedOptions = new CoreAdvancedOptions();

		searchPath = new CorePathWithSubFolder[1];
		ignorePath = [];
		validPath = [];
		deletePath = [];
	}

	public CoreOptions(CoreLib core, bool onePath) : this()
	{
		SetDefault(core, onePath);
	}

	public CoreOptions(CoreLib core) : this(core, false)
	{
	}

	public CoreOptions(CoreOptions options)
	{
		SearchOptions = options.SearchOptions.Clone();
		CompareOptions = options.CompareOptions.Clone();
		DefectOptions = options.DefectOptions.Clone();
		AdvancedOptions = options.AdvancedOptions.Clone();

		searchPath = PathClone(options.searchPath);
		ignorePath = PathClone(options.ignorePath);
		validPath = PathClone(options.validPath);
		deletePath = PathClone(options.deletePath);
	}

	public void SetDefault(CoreLib core, bool onePath)
	{
		var old = new CoreOptions();
		old.Get(core, onePath);
		core.SetDefaultOptions();
		Get(core, onePath);
		old.Set(core, onePath);

		ignorePath = new CorePathWithSubFolder[1];
		ignorePath[0] = new CorePathWithSubFolder
		{
			Path = Resources.DataPath
		};
	}

	public void Get(CoreLib core, bool onePath)
	{
		SearchOptions = core.SearchOptions.Clone();
		CompareOptions = core.CompareOptions.Clone();
		DefectOptions = core.DefectOptions.Clone();
		AdvancedOptions = core.AdvancedOptions.Clone();
		if (onePath)
		{
			searchPath[0] = core.SearchPath[0];
		}
		else
		{
			searchPath = core.SearchPath;
			ignorePath = core.IgnorePath;
			validPath = core.ValidPath;
			deletePath = core.DeletePath;
		}
	}

	/// <summary>
	/// Устанавливает опции в ядре из текущих, для передачи в dll.
	/// </summary>
	/// <param name="core"></param>
	/// <param name="onePath"></param>
	public void Set(CoreLib core, bool onePath)
	{
		core.SearchOptions = SearchOptions.Clone();
		core.CompareOptions = CompareOptions.Clone();
		core.DefectOptions = DefectOptions.Clone();
		core.AdvancedOptions = AdvancedOptions.Clone();
		if (onePath)
		{
			var tmpSearch = new CorePathWithSubFolder[1];
			var tmpOther = System.Array.Empty<CorePathWithSubFolder>();
			if (searchPath.Length > 0 && Directory.Exists(searchPath[0].Path))
			{
				tmpSearch[0] = searchPath[0];
			}
			else
			{
				tmpSearch[0].Path = Application.StartupPath;
			}

			core.SearchPath = tmpSearch;
			core.IgnorePath = tmpOther;
			core.ValidPath = tmpOther;
			core.DeletePath = tmpOther;
		}
		else
		{
			core.SearchPath = searchPath;
			core.IgnorePath = ignorePath;
			core.ValidPath = validPath;
			core.DeletePath = deletePath;
		}
	}

	public void Validate(CoreLib core, bool onePath)
	{
		Set(core, onePath);
		Get(core, onePath);
	}

	public CoreOptions Clone()
	{
		return new CoreOptions(this);
	}

	public void CopyTo(ref CoreOptions options)
	{
		options.SearchOptions = SearchOptions.Clone();
		options.CompareOptions = CompareOptions.Clone();
		options.DefectOptions = DefectOptions.Clone();
		options.AdvancedOptions = AdvancedOptions.Clone();

		PathCopy(searchPath, ref options.searchPath);
		PathCopy(ignorePath, ref options.ignorePath);
		PathCopy(validPath, ref options.validPath);
		PathCopy(deletePath, ref options.deletePath);
	}

	public static void PathCopy(string[] source, ref string[] destination)
	{
		destination = new string[source.GetLength(0)];
		for (var i = 0; i < source.GetLength(0); ++i)
		{
			destination[i] = (string)source[i].Clone();
		}
	}

	public static void PathCopy(CorePathWithSubFolder[] source, ref CorePathWithSubFolder[] destination)
	{
		destination = new CorePathWithSubFolder[source.GetLength(0)];
		for (var i = 0; i < source.GetLength(0); ++i)
		{
			destination[i] = source[i];
		}
	}

	public static string[] PathClone(string[] path)
	{
		var clone = System.Array.Empty<string>();
		PathCopy(path, ref clone);
		return clone;
	}

	public static CorePathWithSubFolder[] PathClone(CorePathWithSubFolder[] path)
	{
		var clone = System.Array.Empty<CorePathWithSubFolder>();
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

	public bool Equals(CoreOptions options)
	{
		if (!SearchOptions.Equals(options.SearchOptions))
		{
			return false;
		}
		else if (!CompareOptions.Equals(options.CompareOptions))
		{
			return false;
		}
		else if (!DefectOptions.Equals(options.DefectOptions))
		{
			return false;
		}
		else if (!AdvancedOptions.Equals(options.AdvancedOptions))
		{
			return false;
		}
		else if (!Equals(searchPath, options.searchPath))
		{
			return false;
		}
		else if (!Equals(ignorePath, options.ignorePath))
		{
			return false;
		}
		else if (!Equals(validPath, options.validPath))
		{
			return false;
		}
		else if (!Equals(deletePath, options.deletePath))
		{
			return false;
		}
		else
		{
			return true;
		}
	}

	public static CoreOptions Load(string fileName, CoreLib core, bool onePath)
	{
		var fileInfo = new FileInfo(fileName);
		if (fileInfo.Exists)
		{
			FileStream fileStream = null;
			try
			{
				var xmlSerializer = new XmlSerializer(typeof(CoreOptions));
				fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
				var coreOptions = (CoreOptions)xmlSerializer.Deserialize(fileStream);
				fileStream.Close();
				coreOptions.Validate(core, onePath);
				return coreOptions;
			}
			catch
			{
				fileStream?.Close();

				return new CoreOptions(core);
			}
		}
		else
		{
			return new CoreOptions(core);
		}
	}

	public void Save(string fileName)
	{
		try
		{
			using TextWriter writer = new StreamWriter(fileName);
			var xmlSerializer = new XmlSerializer(typeof(CoreOptions));
			xmlSerializer.Serialize(writer, this);
		}
		catch { }
	}

	public string GetImageDataBasePath()
	{
		var directory = $@"{Resources.UserPath}\images\{AdvancedOptions.ReducedImageSize}x{AdvancedOptions.ReducedImageSize}";
		var directoryInfo = new DirectoryInfo(directory);
		if (!directoryInfo.Exists)
		{
			directoryInfo.Create();
		}

		return directory;
	}
}
