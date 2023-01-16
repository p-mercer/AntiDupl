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

public class CoreResult
{
	public CoreDll.ResultType Type { get; set; }
	public CoreImageInfo First { get; set; }
	public CoreImageInfo Second { get; set; }
	public CoreDll.DefectType Defect { get; set; }
	public double Difference { get; set; }
	public CoreDll.TransformType Transform { get; set; }
	public int Group { get; set; }
	public int GroupSize { get; set; }
	public CoreDll.HintType Hint { get; set; }

	public CoreResult(ref CoreDll.adResultW result)
	{
		Type = result.type;
		First = new CoreImageInfo(ref result.first);
		Second = new CoreImageInfo(ref result.second);
		Defect = result.defect;
		Difference = result.difference;
		Transform = result.transform;
		Group = result.group.ToInt32();
		GroupSize = result.groupSize.ToInt32();
		Hint = result.hint;
	}
}
