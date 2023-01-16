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

namespace AntiDupl.NET;

public class CoreStatistic
{
	public uint ScanedFolderNumber { get; set; }
	public uint SearchedImageNumber { get; set; }
	public ulong SearchedImageSize { get; set; }
	public uint CollectedImageNumber { get; set; }
	public uint ComparedImageNumber { get; set; }
	public uint CollectThreadCount { get; set; }
	public uint CompareThreadCount { get; set; }
	public uint DefectImageNumber { get; set; }
	public uint DuplImagePairNumber { get; set; }
	public uint RenamedImageNumber { get; set; }
	public uint DeletedImageNumber { get; set; }
	public ulong DeletedImageSize { get; set; }

	public CoreStatistic(ref CoreDll.adStatistic statistic)
	{
		ScanedFolderNumber = statistic.scanedFolderNumber.ToUInt32();
		SearchedImageNumber = statistic.searchedImageNumber.ToUInt32();
		SearchedImageSize = statistic.searchedImageSize;
		CollectedImageNumber = statistic.collectedImageNumber.ToUInt32();
		ComparedImageNumber = statistic.comparedImageNumber.ToUInt32();
		CollectThreadCount = statistic.collectThreadCount.ToUInt32();
		CompareThreadCount = statistic.compareThreadCount.ToUInt32();
		DefectImageNumber = statistic.defectImageNumber.ToUInt32();
		DuplImagePairNumber = statistic.duplImagePairNumber.ToUInt32();
		RenamedImageNumber = statistic.renamedImageNumber.ToUInt32();
		DeletedImageNumber = statistic.deletedImageNumber.ToUInt32();
		DeletedImageSize = statistic.deletedImageSize;
	}
}
