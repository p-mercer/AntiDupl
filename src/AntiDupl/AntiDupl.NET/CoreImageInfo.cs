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
using System;
using System.Text;

namespace AntiDupl.NET;

public class CoreImageInfo
{
	public ulong Id { get; set; }
	public string Path { get; set; }
	public ulong Size { get; set; }
	public ulong Time { get; set; }
	public CoreDll.ImageType Type { get; set; }
	public uint Width { get; set; }
	public uint Height { get; set; }
	public double Blockiness { get; set; }
	public double Blurring { get; set; }
	public CoreDll.adImageExifW ExifInfo { get; set; }

	public CoreImageInfo(ref CoreDll.adImageInfoW imageInfo)
	{
		Id = (ulong)imageInfo.id;
		Path = imageInfo.path;
		Size = imageInfo.size;
		Time = imageInfo.time;
		Type = imageInfo.type;
		Width = imageInfo.width;
		Height = imageInfo.height;
		Blockiness = imageInfo.blockiness;
		Blurring = imageInfo.blurring;
		ExifInfo = imageInfo.exifInfo;
	}

	public string GetImageSizeString()
	{
		var builder = new StringBuilder();
		builder.Append(Width);
		builder.Append(" x ");
		builder.Append(Height);
		return builder.ToString();
	}

	public string GetImageTypeString()
	{
		var builder = new StringBuilder();
		switch (Type)
		{
			case CoreDll.ImageType.None:
				builder.Append("");
				break;
			case CoreDll.ImageType.Bmp:
				builder.Append("BMP");
				break;
			case CoreDll.ImageType.Gif:
				builder.Append("GIF");
				break;
			case CoreDll.ImageType.Jpeg:
				builder.Append("JPG");
				break;
			case CoreDll.ImageType.Png:
				builder.Append("PNG");
				break;
			case CoreDll.ImageType.Tiff:
				builder.Append("TIFF");
				break;
			case CoreDll.ImageType.Emf:
				builder.Append("EMF");
				break;
			case CoreDll.ImageType.Wmf:
				builder.Append("WMF");
				break;
			case CoreDll.ImageType.Exif:
				builder.Append("EXIF");
				break;
			case CoreDll.ImageType.Icon:
				builder.Append("ICON");
				break;
			case CoreDll.ImageType.Jp2:
				builder.Append("JP2");
				break;
			case CoreDll.ImageType.Psd:
				builder.Append("PSD");
				break;
			case CoreDll.ImageType.Dds:
				builder.Append("DDS");
				break;
			case CoreDll.ImageType.Heif:
				builder.Append("HEIF");
				break;
		}
		return builder.ToString();
	}

	public string GetBlockinessString()
	{
		return Blockiness.ToString("F2");
	}

	public string GetBlurringString()
	{
		return Blurring.ToString("F2");
	}

	public string GetFileTimeString()
	{
		return DateTime.FromFileTime((long)Time).ToString();
	}

	public string GetFileSizeString()
	{
		var builder = new StringBuilder();
		var str = Math.Ceiling(Size / 1024.0).ToString();
		var start = str.Length % 3;
		switch (start)
		{
			case 0:
				break;
			case 1:
				builder.Append(str[0]);
				builder.Append(' ');
				break;
			case 2:
				builder.Append(str[0]);
				builder.Append(str[1]);
				builder.Append(' ');
				break;
		}
		for (var i = start; i < str.Length; i += 3)
		{
			builder.Append(str[i + 0]);
			builder.Append(str[i + 1]);
			builder.Append(str[i + 2]);
			builder.Append(' ');
		}
		builder.Append("KB");
		return builder.ToString();
	}

	public string GetTipString()
	{
		var s = Resources.Strings.Current;
		var builder = new StringBuilder();

		builder.AppendLine(Path);

		builder.Append(s.ResultsListView_ImageSize_Column_Text);
		builder.Append(": ");
		builder.AppendLine(GetImageSizeString());

		builder.Append(s.ResultsListView_ImageType_Column_Text);
		builder.Append(": ");
		builder.AppendLine(GetImageTypeString());

		builder.Append(s.ResultsListView_FileSize_Column_Text);
		builder.Append(": ");
		builder.Append(GetFileSizeString());

		return builder.ToString();
	}

	public string GetDirectoryString()
	{
		var i = Path.Length - 1;
		while (i >= 0 && Path[i] != '\\')
		{
			i--;
		}

		return i < 0 ? "" : Path[..i];
	}

	public string GetFileNameWithoutExtensionString()
	{
		return System.IO.Path.GetFileNameWithoutExtension(Path);
	}
}
