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
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;

namespace AntiDupl.NET;

    static public class FolderOpener
    {
        private static readonly bool m_canOpenFolderWithExplorer = CanOpenFolderWithExplorer();

        static private bool CanOpenFolderWithExplorer()
        {
            var rkFolder = Registry.ClassesRoot.OpenSubKey("Folder");
            if (rkFolder != null)
            {
                var rkShell = rkFolder.OpenSubKey("shell");
                if (rkShell != null)
                {
                    var rkExplore = rkShell.OpenSubKey("explore");
                    if (rkExplore != null)
                    {
                        var rkCommand = rkExplore.OpenSubKey("command");
                        if (rkCommand != null)
                        {
                            var defaultValue = (string)rkCommand.GetValue("");
                            if (defaultValue != null)
                            {
                                if (defaultValue.ToLowerInvariant().Contains("explorer.exe"))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                    var rkOpen = rkShell.OpenSubKey("open");
                    if (rkOpen != null)
                    {
                        var rkCommand = rkOpen.OpenSubKey("command");
                        if (rkCommand != null)
                        {
                            var defaultValue = (string)rkCommand.GetValue("");
                            if (defaultValue != null)
                            {
                                if (defaultValue.ToLowerInvariant().Contains("explorer.exe"))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        static public void OpenContainingFolder(CoreImageInfo imageInfo)
        {
            try
            {
                var startInfo = new ProcessStartInfo();
                if (m_canOpenFolderWithExplorer)
                {
                    startInfo.FileName = "explorer.exe";
                    startInfo.Arguments = string.Format("/e, /select, \"{0}\"", imageInfo.path);
                }
                else
                {
                    startInfo.FileName = imageInfo.GetDirectoryString();
                }
                var process = Process.Start(startInfo);
                Thread.Sleep(System.TimeSpan.FromMilliseconds(100));
            }
            catch (System.Exception exeption)
            {
                MessageBox.Show(exeption.Message);
            }
        }
    }
