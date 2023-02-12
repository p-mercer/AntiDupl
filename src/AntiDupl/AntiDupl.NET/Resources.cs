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
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace AntiDupl.NET;

public static class Resources
{
	private static string GetPath(string path, string name, string extension)
	{
		return $"{path}\\{name}{extension}";
	}

	private static string CreateIfNotExists(string path)
	{
		var directoryInfo = new DirectoryInfo(path);
		if (!directoryInfo.Exists)
		{
			directoryInfo.Create();
		}

		return path;
	}

	public static string GetDefaultUserPath()
	{
		return CreateIfNotExists($"{Application.StartupPath}\\user");
	}

	public static string UserPath { get; set; }

	public static string ProfilesPath { get { return CreateIfNotExists($"{UserPath}\\profiles"); } }

	public static string DataPath { get { return $"{Application.StartupPath}\\data"; } }

	public static string Path { get { return $"{DataPath}\\resources"; } }

	public static class Images
	{
		public static Image GetNullImage()
		{
			var bitmap = new Bitmap(1, 1);
			bitmap.SetPixel(0, 0, Color.FromArgb(0, 0, 0, 0));
			return bitmap;
		}

		public static Image GetImageWithBlackCircle(int width, int height, double radius)
		{
			var bitmap = new Bitmap(width, height);
			for (var x = 0; x < width; x++)
			{
				var xx = x - width / 2;
				for (var y = 0; y < height; y++)
				{
					var yy = y - height / 2;
					if (xx * xx + yy * yy < radius * radius)
					{
						bitmap.SetPixel(x, y, Color.FromArgb(0xFF, 0, 0, 0));
					}
					else
					{
						bitmap.SetPixel(x, y, Color.FromArgb(0, 0, 0, 0));
					}
				}
			}
			return bitmap;
		}

		public static Image Get(string name)
		{
			Image image;
			try
			{
				var extension = System.IO.Path.GetExtension(name);
				if (string.IsNullOrEmpty(extension))
				{
					extension = Extension;
				}

				image = Image.FromFile(GetPath(Path, System.IO.Path.GetFileNameWithoutExtension(name), extension));
			}
			catch
			{
				image = GetNullImage();
			}
			return image;
		}

		private static string Path { get { return $"{Resources.Path}\\images"; } }

		private static string Extension { get { return ".img"; } }
	}

	public static class Icons
	{
		public static Icon Get(Size size)
		{
			var icon = Get();
			return new Icon(icon, size);
		}

		public static Icon Get()
		{
			Icon icon = null;
			try
			{
				icon = new Icon(GetPath(Path, "Icon", Extension));
			}
			catch
			{
				try
				{
					icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
				}
				catch
				{
				}
			}
			return icon;
		}

		private static string Path { get { return $"{Resources.Path}\\icons"; } }

		private static string Extension { get { return ".ico"; } }
	}

	public static class Strings
	{
		public delegate void CurrentChangeHandler();
		/// <summary>
		/// Событие вызываемое при смене языка
		/// </summary>
		public static event CurrentChangeHandler OnCurrentChange;

		public static void Initialize()
		{
			m_strings.Clear();

			m_strings.Add(StringsDefaultEnglish.Get());
			m_strings.Add(StringsDefaultRussian.Get());

			var directoryInfo = new DirectoryInfo(Path);
			if (directoryInfo.Exists)
			{
				var fileInfos = directoryInfo.GetFiles(Filter, SearchOption.TopDirectoryOnly);
				for (var i = 0; i < fileInfos.Length; i++)
				{
					var strings = Load(fileInfos[i].FullName);
					if (strings != null)
					{
						var name = System.IO.Path.GetFileNameWithoutExtension(fileInfos[i].FullName);
						if (name.CompareTo(StringsDefaultRussian.Get().Name) != 0 &&
							name.CompareTo(StringsDefaultEnglish.Get().Name) != 0)
						{
							strings.Name = name;
							m_strings.Add(strings);
						}
					}
				}
			}

			try
			{
				CreateIfNotExists(Path);
				Save(StringsDefaultEnglish.Get());
				Save(StringsDefaultRussian.Get());
			}
			catch (Exception)
			{
			}
		}

		private static NET.Strings Load(string path)
		{
			var fileInfo = new FileInfo(path);
			if (fileInfo.Exists)
			{
				try
				{
					var xmlSerializer = new XmlSerializer(typeof(NET.Strings));
					var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
					var strings = (NET.Strings)xmlSerializer.Deserialize(fileStream);
					fileStream.Close();
					return strings;
				}
				catch
				{
					return null;
				}
			}
			else
			{
				return null;
			}
		}

		private static void Save(NET.Strings strings)
		{
			try
			{
				TextWriter writer = new StreamWriter(GetPath(Path, strings.Name, Extension));
				var xmlSerializer = new XmlSerializer(typeof(NET.Strings));
				xmlSerializer.Serialize(writer, strings);
				writer.Close();
			}
			catch
			{
			}
		}

		private static string Path { get { return $"{Resources.Path}\\strings"; } }

		private static string Extension { get { return ".xml"; } }

		private static string Filter { get { return "*.xml"; } }

		public static int Count
		{
			get
			{
				return m_strings.Count;
			}
		}

		public static int CurrentIndex { get; private set; }

		public static NET.Strings Current
		{
			get
			{
				return CurrentIndex < Count && CurrentIndex >= 0 ? (NET.Strings)m_strings[CurrentIndex] : null;
			}
		}

		public static NET.Strings Get(int index)
		{
			return index < Count && index >= 0 ? (NET.Strings)m_strings[index] : null;
		}

		public static bool SetCurrent(int index)
		{
			if (index != CurrentIndex && index < Count && index >= 0)
			{
				CurrentIndex = index;
				OnCurrentChange?.Invoke();

				return true;
			}
			else
			{
				return false;
			}
		}

		public static bool SetCurrent(string name)
		{
			for (var i = 0; i < Count; i++)
			{
				if (Get(i).Name.CompareTo(name) == 0)
				{
					return SetCurrent(i);
				}
			}
			for (var i = 0; i < Count; i++)
			{
				if (Get(i).Name.CompareTo(StringsDefaultEnglish.Get().Name) == 0)
				{
					return SetCurrent(i);
				}
			}
			return false;
		}

		public static bool IsCurrentRussian()
		{
			return Current.Name.CompareTo(StringsDefaultRussian.Get().Name) == 0;
		}

		public static bool IsCurrentEnglish()
		{
			return Current.Name.CompareTo(StringsDefaultEnglish.Get().Name) == 0;
		}

		public static bool IsCurrentRussianFamily()
		{
			return IsCurrentRussian() ||
				Current.Name.CompareTo("Belarusian") == 0 ||
				Current.Name.CompareTo("Ukrainian") == 0;
		}

		private static readonly ArrayList m_strings = new();

		public static void Update()
		{
			OnCurrentChange?.Invoke();
		}
	}

	public static class WebLinks
	{
		public const string GithubComAntidupl = "http://ermig1979.github.io/AntiDupl";
		public const string GithubComAntiduplEnglish = "http://ermig1979.github.io/AntiDupl/english/index.html";
		public const string GithubComAntiduplRussian = "http://ermig1979.github.io/AntiDupl/russian/index.html";
		public const string Version = "http://ermig1979.github.io/AntiDupl/version.xml";

		public const string Simd = "http://ermig1979.github.io/Simd";
		public const string OpenJpeg = "http://www.openjpeg.org";
		public const string LibWebp = "https://github.com/webmproject/libwebp";
		public const string LibJpegTurbo = "http://www.libjpeg-turbo.org";
		public const string LibHeif = "http://www.libheif.org";

		public static string GithubComAntiduplCurrent
		{
			get
			{
				return Strings.IsCurrentRussianFamily() ? GithubComAntiduplRussian : GithubComAntiduplEnglish;
			}
		}
	}

	public static class Help
	{
		private static string GetUrl(string page)
		{
			var builder = new StringBuilder(WebLinks.GithubComAntidupl);
			builder.Append("/data/help");
			if (Strings.IsCurrentRussianFamily())
			{
				builder.Append("/russian");
			}
			else
			{
				builder.Append("/english");
			}

			builder.Append("/index.html");
			if (page != null)
			{
				builder.Append("?page=");
				builder.Append(page);
			}
			return builder.ToString();
		}

		private class Starter
		{
			private readonly string m_url;

			public Starter(Form form, string url)
			{
				m_url = url;
				form.HelpButton = true;
				form.HelpButtonClicked += new CancelEventHandler(OnHelpButtonClicked);
			}

			private void OnHelpButtonClicked(object sender, CancelEventArgs e)
			{
				Show(m_url);
			}
		}

		public static void Show(string url)
		{
			try
			{
				if (url[..4].ToUpper() != "HTTP")
				{
					var keyName = @"HTTP\shell\open\command";
					var registryKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(keyName, false);
					if (registryKey != null)
					{
						var defaultBrouserPath = ((string)registryKey.GetValue(null, null)).Split('"')[1];
						Process.Start(defaultBrouserPath, url);
					}
				}
				else
				{
					Process.Start(url);
				}
			}
			catch
			{
			}
		}

		public static void Bind(Form form, string path)
		{
			form.Tag = new Starter(form, path);
		}

		public static string Index { get { return GetUrl(null); } }
		public static string Options { get { return GetUrl("options.html"); } }
		public static string Paths { get { return GetUrl("paths.html"); } }
		public static string HotKeys { get { return GetUrl("hotkeys.html"); } }
	}

	public static class Logs
	{
		public static string Performance { get { return $"{UserPath}\\performance.txt"; } }
	}
}

