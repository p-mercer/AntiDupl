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

public sealed class CoreDefectOptions
{
	public bool CheckOnDefect { get; set; }
	public bool CheckOnBlockiness { get; set; }
	public int BlockinessThreshold { get; set; }
	public bool CheckOnBlockinessOnlyNotJpeg { get; set; }
	public bool CheckOnBlurring { get; set; }
	public int BlurringThreshold { get; set; }

	public CoreDefectOptions()
	{
	}

	public CoreDefectOptions(CoreDefectOptions defectOptions)
	{
		CheckOnDefect = defectOptions.CheckOnDefect;
		CheckOnBlockiness = defectOptions.CheckOnBlockiness;
		BlockinessThreshold = defectOptions.BlockinessThreshold;
		CheckOnBlockinessOnlyNotJpeg = defectOptions.CheckOnBlockinessOnlyNotJpeg;
		CheckOnBlurring = defectOptions.CheckOnBlurring;
		BlurringThreshold = defectOptions.BlurringThreshold;
	}

	public CoreDefectOptions(ref CoreDll.adDefectOptions defectOptions)
	{
		CheckOnDefect = defectOptions.checkOnDefect != CoreDll.FALSE;
		CheckOnBlockiness = defectOptions.checkOnBlockiness != CoreDll.FALSE;
		BlockinessThreshold = defectOptions.blockinessThreshold;
		CheckOnBlockinessOnlyNotJpeg = defectOptions.checkOnBlockinessOnlyNotJpeg != CoreDll.FALSE;
		CheckOnBlurring = defectOptions.checkOnBlurring != CoreDll.FALSE;
		BlurringThreshold = defectOptions.blurringThreshold;
	}

	public void ConvertTo(ref CoreDll.adDefectOptions defectOptions)
	{
		defectOptions.checkOnDefect = CheckOnDefect ? CoreDll.TRUE : CoreDll.FALSE;
		defectOptions.checkOnBlockiness = CheckOnBlockiness ? CoreDll.TRUE : CoreDll.FALSE;
		defectOptions.blockinessThreshold = BlockinessThreshold;
		defectOptions.checkOnBlockinessOnlyNotJpeg = CheckOnBlockinessOnlyNotJpeg ? CoreDll.TRUE : CoreDll.FALSE;
		defectOptions.checkOnBlurring = CheckOnBlurring ? CoreDll.TRUE : CoreDll.FALSE;
		defectOptions.blurringThreshold = BlurringThreshold;
	}

	public CoreDefectOptions Clone()
	{
		return new CoreDefectOptions(this);
	}

	public bool Equals(CoreDefectOptions defectOptions)
	{
		return
			CheckOnDefect == defectOptions.CheckOnDefect &&
			CheckOnBlockiness == defectOptions.CheckOnBlockiness &&
			BlockinessThreshold == defectOptions.BlockinessThreshold &&
			 CheckOnBlockinessOnlyNotJpeg == defectOptions.CheckOnBlockinessOnlyNotJpeg &&
			CheckOnBlurring == defectOptions.CheckOnBlurring &&
			BlurringThreshold == defectOptions.BlurringThreshold;
	}
}
