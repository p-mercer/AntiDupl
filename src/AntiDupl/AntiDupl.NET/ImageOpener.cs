using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace AntiDupl.NET;

public static class ImageOpener
{
	/// <summary>
	/// Opens the specified file using its default application.
	/// </summary>
	public static void OpenFile(string filePath)
	{
		var startInfo = new ProcessStartInfo
		{
			UseShellExecute = true,
			FileName = filePath
		};
		try
		{
			var process = Process.Start(startInfo);
			Thread.Sleep(System.TimeSpan.FromMilliseconds(100));
		}
		catch (System.Exception exeption)
		{
			MessageBox.Show(exeption.Message);
		}
	}
}
