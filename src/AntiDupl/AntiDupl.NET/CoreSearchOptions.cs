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
using System.Collections.Generic;

namespace AntiDupl.NET;

public sealed class CoreSearchOptions 
{
	private static readonly string[] s_jpegExtensions = { "JPEG", "JFIF", "JPG", "JPE", "JIFF", "JIF", "J", "JNG", "JFF" };
	private static readonly string[] s_tiffExtensions = { "TIF", "TIFF" };
	private static readonly string[] s_bmpExtensions = { "BMP", "DIB", "RLE" };
	private static readonly string[] s_gifExtensions = { "GIF" };
	private static readonly string[] s_pngExtensions = { "PNG" };
	private static readonly string[] s_emfExtensions = { "EMF", "EMZ" };
	private static readonly string[] s_wmfExtensions = { "WMF" };
	private static readonly string[] s_iconExtensions = { "ICON", "ICO", "ICN" };
	private static readonly string[] s_jp2Extensions = { "JP2", "J2K", "J2C", "JPC", "JPF", "JPX" };
	private static readonly string[] s_psdExtensions = { "PSD" };
	private static readonly string[] s_ddsExtensions = { "DDS" };
	private static readonly string[] s_tgaExtensions = { "TGA", "TPIC" };
	private static readonly string[] s_heifExtensions = { "HEIF", "HEIC" };

	public bool SubFolders { get; set; }
	public bool System { get; set; }
	public bool Hidden { get; set; }
	public bool JPEG { get; set; }
	public bool BMP { get; set; }
	public bool GIF { get; set; }
	public bool PNG { get; set; }
	public bool TIFF { get; set; }
	public bool EMF { get; set; }
	public bool WMF { get; set; }
	public bool EXIF { get; set; }
	public bool ICON { get; set; }
	public bool JP2 { get; set; }
	public bool PSD { get; set; }
	public bool DDS { get; set; }
	public bool TGA { get; set; }
	public bool WEBP { get; set; }
	public bool HEIF { get; set; }

	public CoreSearchOptions()
	{
	}

	public CoreSearchOptions(CoreSearchOptions searchOptions)
	{
		System = searchOptions.System;
		Hidden = searchOptions.Hidden;
		JPEG = searchOptions.JPEG;
		BMP = searchOptions.BMP;
		GIF = searchOptions.GIF;
		PNG = searchOptions.PNG;
		TIFF = searchOptions.TIFF;
		EMF = searchOptions.EMF;
		WMF = searchOptions.WMF;
		EXIF = searchOptions.EXIF;
		ICON = searchOptions.ICON;
		JP2 = searchOptions.JP2;
		PSD = searchOptions.PSD;
		DDS = searchOptions.DDS;
		TGA = searchOptions.TGA;
		WEBP = searchOptions.WEBP;
		HEIF = searchOptions.HEIF;
	}

	public CoreSearchOptions(CoreDll.adSearchOptions searchOptions)
	{
		System = searchOptions.system != CoreDll.FALSE;
		Hidden = searchOptions.hidden != CoreDll.FALSE;
		JPEG = searchOptions.JPEG != CoreDll.FALSE;
		BMP = searchOptions.BMP != CoreDll.FALSE;
		GIF = searchOptions.GIF != CoreDll.FALSE;
		PNG = searchOptions.PNG != CoreDll.FALSE;
		TIFF = searchOptions.TIFF != CoreDll.FALSE;
		EMF = searchOptions.EMF != CoreDll.FALSE;
		WMF = searchOptions.WMF != CoreDll.FALSE;
		EXIF = searchOptions.EXIF != CoreDll.FALSE;
		ICON = searchOptions.ICON != CoreDll.FALSE;
		JP2 = searchOptions.JP2 != CoreDll.FALSE;
		PSD = searchOptions.PSD != CoreDll.FALSE;
		DDS = searchOptions.DDS != CoreDll.FALSE;
		TGA = searchOptions.TGA != CoreDll.FALSE;
		WEBP = searchOptions.WEBP != CoreDll.FALSE;
		HEIF = searchOptions.HEIF != CoreDll.FALSE;
	}

	public void ConvertTo(ref CoreDll.adSearchOptions searchOptions)
	{
		searchOptions.system = System ? CoreDll.TRUE : CoreDll.FALSE;
		searchOptions.hidden = Hidden ? CoreDll.TRUE : CoreDll.FALSE;
		searchOptions.JPEG = JPEG ? CoreDll.TRUE : CoreDll.FALSE;
		searchOptions.BMP = BMP ? CoreDll.TRUE : CoreDll.FALSE;
		searchOptions.GIF = GIF ? CoreDll.TRUE : CoreDll.FALSE;
		searchOptions.PNG = PNG ? CoreDll.TRUE : CoreDll.FALSE;
		searchOptions.TIFF = TIFF ? CoreDll.TRUE : CoreDll.FALSE;
		searchOptions.EMF = EMF ? CoreDll.TRUE : CoreDll.FALSE;
		searchOptions.WMF = WMF ? CoreDll.TRUE : CoreDll.FALSE;
		searchOptions.EXIF = EXIF ? CoreDll.TRUE : CoreDll.FALSE;
		searchOptions.ICON = ICON ? CoreDll.TRUE : CoreDll.FALSE;
		searchOptions.JP2 = JP2 ? CoreDll.TRUE : CoreDll.FALSE;
		searchOptions.PSD = PSD ? CoreDll.TRUE : CoreDll.FALSE;
		searchOptions.DDS = DDS ? CoreDll.TRUE : CoreDll.FALSE;
		searchOptions.TGA = TGA ? CoreDll.TRUE : CoreDll.FALSE;
		searchOptions.WEBP = WEBP ? CoreDll.TRUE : CoreDll.FALSE;
		searchOptions.HEIF = HEIF ? CoreDll.TRUE : CoreDll.FALSE;
	}

	public CoreSearchOptions Clone()
	{
		return new CoreSearchOptions(this);
	}

	public bool Equals(CoreSearchOptions searchOptions)
	{
		return
			System == searchOptions.System &&
			Hidden == searchOptions.Hidden &&
			JPEG == searchOptions.JPEG &&
			BMP == searchOptions.BMP &&
			GIF == searchOptions.GIF &&
			PNG == searchOptions.PNG &&
			TIFF == searchOptions.TIFF &&
			EMF == searchOptions.EMF &&
			WMF == searchOptions.WMF &&
			EXIF == searchOptions.EXIF &&
			ICON == searchOptions.ICON &&
			JP2 == searchOptions.JP2 &&
			PSD == searchOptions.PSD &&
			DDS == searchOptions.DDS &&
			TGA == searchOptions.TGA &&
			WEBP == searchOptions.WEBP &&
			HEIF == searchOptions.HEIF;
	}

	public string[] GetActualExtensions()
	{
		var extensions = new List<string>();
		if (JPEG)
		{
			extensions.AddRange(s_jpegExtensions);
		}

		if (TIFF)
		{
			extensions.AddRange(s_tiffExtensions);
		}

		if (BMP)
		{
			extensions.AddRange(s_bmpExtensions);
		}

		if (GIF)
		{
			extensions.AddRange(s_gifExtensions);
		}

		if (PNG)
		{
			extensions.AddRange(s_pngExtensions);
		}

		if (EMF)
		{
			extensions.AddRange(s_emfExtensions);
		}

		if (WMF)
		{
			extensions.AddRange(s_wmfExtensions);
		}

		if (ICON)
		{
			extensions.AddRange(s_iconExtensions);
		}

		if (JP2)
		{
			extensions.AddRange(s_jp2Extensions);
		}

		if (PSD)
		{
			extensions.AddRange(s_psdExtensions);
		}

		if (DDS)
		{
			extensions.AddRange(s_ddsExtensions);
		}

		if (TGA)
		{
			extensions.AddRange(s_tgaExtensions);
		}

		if (WEBP)
		{
			extensions.AddRange(s_tgaExtensions);
		}

		if (HEIF)
		{
			extensions.AddRange(s_heifExtensions);
		}

		return extensions.ToArray();
	}
}
