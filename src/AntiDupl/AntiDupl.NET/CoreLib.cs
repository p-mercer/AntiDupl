﻿/*
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
using System.Runtime.InteropServices;
using System.Threading;

namespace AntiDupl.NET;

    public class CoreLib : IDisposable
    {
        private const uint VERSION_SIZE = 40;
        private const uint PAGE_SIZE = 16;

        private readonly IntPtr m_handle = IntPtr.Zero;
        private CoreDll m_dll = null;

        //-----------Public functions----------------------------------------------

        public CoreLib(string userPath)
        {
            try
            {
                m_dll = new CoreDll();
            }
            catch
            {
                throw new Exception("Can't load core library!");
            }
            if (Version.Compatible(GetVersion(CoreDll.VersionType.AntiDupl)))
            {
                m_handle = m_dll.adCreateW(userPath);
            }
            else
                throw new Exception("Incompatible core library version!");
        }

        ~CoreLib()
        {
            Dispose();
        }

        public void Release()
        {
            if (m_dll != null && m_handle != IntPtr.Zero)
            {
                if (m_dll.adRelease(m_handle) == CoreDll.Error.AccessDenied)
                {
                    Stop();
                    Thread.Sleep(10);
                    m_dll.adRelease(m_handle);
                }
            }
        }

        public void Dispose()
        {
            Release();
            if (m_dll != null)
            {
                m_dll.Dispose();
                m_dll = null;
            }
            GC.SuppressFinalize(this);
        }

        public CoreVersion GetVersion(CoreDll.VersionType versionType)
        {
            try
            {
                var versionB = new sbyte[VERSION_SIZE];
                var sizeB = new IntPtr[1];
                sizeB[0] = new IntPtr(VERSION_SIZE);
                var versionH = GCHandle.Alloc(versionB, GCHandleType.Pinned);
                var sizeH = GCHandle.Alloc(sizeB, GCHandleType.Pinned);
                try
                {
                    var versionP = versionH.AddrOfPinnedObject();
                    var sizeP = sizeH.AddrOfPinnedObject();
                    if (m_dll.adVersionGet(versionType, versionP, sizeP) == CoreDll.Error.Ok)
                    {
                        return new CoreVersion(versionB);
                    }
                }
                finally 
                {
                    versionH.Free();
                    sizeH.Free();
                }
            }
            catch(Exception)
            {
            }
            return null; 
        }

        public bool IsInited()
        {
            return m_handle != IntPtr.Zero;
        }

        public bool IsWork()
        {
            var status = StatusGet(CoreDll.ThreadType.Main, 0);
            if (status != null)
                return status.state != CoreDll.StateType.None;
            else
                return false;
        }

        public bool Stop()
        {
            return m_dll.adStop(m_handle) == CoreDll.Error.Ok;
        }

        public bool Search()
        {
            return m_dll.adSearch(m_handle) == CoreDll.Error.Ok;
        }

        public bool Load(CoreDll.FileType fileType, string fileName, bool check)
        {
            return m_dll.adLoadW(m_handle, fileType, fileName, check ? CoreDll.TRUE : CoreDll.FALSE) == CoreDll.Error.Ok;
        }

        public bool Save(CoreDll.FileType fileType, string fileName)
        {
            return m_dll.adSaveW(m_handle, fileType, fileName) == CoreDll.Error.Ok;
        }

        public bool Clear(CoreDll.FileType fileType)
        {
            return m_dll.adClear(m_handle, fileType) == CoreDll.Error.Ok;
        }

        public bool SetDefaultOptions()
        {
            return m_dll.adOptionsSet(m_handle, CoreDll.OptionsType.SetDefault, IntPtr.Zero) == CoreDll.Error.Ok;
        }

        public CoreStatistic GetStatistic()
        {
            try
            {
                object statisticO = new CoreDll.adStatistic();
                var statisticB = new byte[Marshal.SizeOf(statisticO)];
                var statisticH = GCHandle.Alloc(statisticB, GCHandleType.Pinned);
                try
                {
                    var statisticP = statisticH.AddrOfPinnedObject();
                    if (m_dll.adStatisticGet(m_handle, statisticP) == CoreDll.Error.Ok)
                    {
                        var statistic = (CoreDll.adStatistic)Marshal.PtrToStructure(statisticP, statisticO.GetType());
                        return new CoreStatistic(ref statistic);
                    }
                }
                finally
                {
                    statisticH.Free();
                }
            }
            catch (Exception)
            {
            }
            return null;
        }

        public CoreStatus StatusGet(CoreDll.ThreadType threadType, int threadId)
        {
            try
            {
                object statusO = new CoreDll.adStatusW();
                var statusB = new byte[Marshal.SizeOf(statusO)];
                var statusH = GCHandle.Alloc(statusB, GCHandleType.Pinned);
                try
                {
                    var statusP = statusH.AddrOfPinnedObject();
                    if (m_dll.adStatusGetW(m_handle, threadType, new IntPtr(threadId), statusP) == CoreDll.Error.Ok)
                    {
                        var statusW = (CoreDll.adStatusW)Marshal.PtrToStructure(statusP, statusO.GetType());
                        return new CoreStatus(ref statusW);
                    }
                }
                finally
                {
                    statusH.Free();
                }
            }
            catch (Exception)
            {
            }
            return null;
        }

        public bool SortResult(CoreDll.SortType sortType, bool increasing)
        {
            return m_dll.adResultSort(m_handle, sortType, increasing ? CoreDll.TRUE : CoreDll.FALSE) == CoreDll.Error.Ok;
        }

        public bool ApplyToResult(CoreDll.GlobalActionType globalActionType)
        {
            return m_dll.adResultApply(m_handle, globalActionType) == CoreDll.Error.Ok;
        }

        public bool ApplyToResult(CoreDll.LocalActionType localActionType, CoreDll.TargetType targetType)
        {
            return m_dll.adResultApplyTo(m_handle, localActionType, targetType) == CoreDll.Error.Ok;
        }

        public bool CanApply(CoreDll.ActionEnableType actionEnableType)
        {
            try 
            {
                var enableB = new int[1];
                var enableH = GCHandle.Alloc(enableB, GCHandleType.Pinned);
                try
                {
                    var enableP = enableH.AddrOfPinnedObject();
                    if (m_dll.adCanApply(m_handle, actionEnableType, enableP) == CoreDll.Error.Ok)
                    {
                        return enableB[0] != CoreDll.FALSE;
                    }
                }
                finally 
                {
                    enableH.Free();
                }
            }
            catch(Exception)
            {
            }
            return false;
        }

        public bool RenameCurrent(CoreDll.RenameCurrentType renameCurrentType, string newFileName)
        {
            return m_dll.adRenameCurrentW(m_handle, renameCurrentType, newFileName) == CoreDll.Error.Ok;
        }

        public bool MoveCurrentGroup(string directory)
        {
            return m_dll.adMoveCurrentGroupW(m_handle, directory) == CoreDll.Error.Ok;
        }

        public bool RenameCurrentGroupAs(string fileName)
        {
            return m_dll.adRenameCurrentGroupAsW(m_handle, fileName) == CoreDll.Error.Ok;
        }

        public CoreResult[] GetResult(uint startFrom, uint size)
        {
            var resultSize = GetResultSize();
            if (resultSize > startFrom)
            {
                object resultObject = new CoreDll.adResultW();
                var sizeOfResult = Marshal.SizeOf(resultObject);
                var buffer = new byte[sizeOfResult * PAGE_SIZE];
                size = Math.Min(resultSize - startFrom, size);
                var results = new CoreResult[size];
                var pageCount = (uint)(size / PAGE_SIZE + (size % PAGE_SIZE > 0 ? 1 : 0));
                for (uint page = 0; page < pageCount; ++page)
                {
                    var pStartFrom = new UIntPtr[1];
                    pStartFrom[0] = new UIntPtr(startFrom + page * PAGE_SIZE);

                    var pSize = new UIntPtr[1];
                    pSize[0] = new UIntPtr(PAGE_SIZE);

                    if (m_dll.adResultGetW(m_handle, Marshal.UnsafeAddrOfPinnedArrayElement(pStartFrom, 0),
                        Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0), 
                        Marshal.UnsafeAddrOfPinnedArrayElement(pSize, 0)) == CoreDll.Error.Ok)
                    {
                        for (uint i = 0; i < pSize[0].ToUInt32(); ++i)
                        {
                            var pResult = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, (int)(i*sizeOfResult));
                            var result = (CoreDll.adResultW)Marshal.PtrToStructure(pResult, resultObject.GetType());
                            results[page * PAGE_SIZE + i] = new CoreResult(ref result);
                        }

                    }
                }
                return results;
            }
            return null;
        }

        public uint GetResultSize()
        {
            try 
            {
                var startFromB = new UIntPtr[1];
                startFromB[0] = new UIntPtr(uint.MaxValue);
                var startFromH = GCHandle.Alloc(startFromB, GCHandleType.Pinned);
                try 
                {
                    var startFromP = startFromH.AddrOfPinnedObject();
                    var resultP = new IntPtr(1);
                    var resultSizeP = new IntPtr(1);
                    if (m_dll.adResultGetW(m_handle, startFromP, resultP, resultSizeP) == CoreDll.Error.InvalidStartPosition)
                    {
                        return startFromB[0].ToUInt32();
                    }
                }
                finally
                {
                    startFromH.Free();
                }
            }
            catch(Exception)
            {
            }
            return 0;
        }

        public bool SetSelection(uint startFrom, uint size, bool value)
        {
            var pStartFrom = new UIntPtr[1];
            pStartFrom[0] = new UIntPtr(startFrom);
            return m_dll.adSelectionSet(m_handle, Marshal.UnsafeAddrOfPinnedArrayElement(pStartFrom, 0), new UIntPtr(size), 
                value ? CoreDll.TRUE : CoreDll.FALSE) == CoreDll.Error.Ok;
        }

        public bool[] GetSelection(uint startFrom, uint size)
        {
            var pSelection = new int[size];
            var pStartFrom = new UIntPtr[1];
            pStartFrom[0] = new UIntPtr(startFrom);
            var pSelectionSize = new UIntPtr[1];
            pSelectionSize[0] = new UIntPtr(size);
            if (m_dll.adSelectionGet(m_handle, Marshal.UnsafeAddrOfPinnedArrayElement(pStartFrom, 0), 
                Marshal.UnsafeAddrOfPinnedArrayElement(pSelection, 0),
                Marshal.UnsafeAddrOfPinnedArrayElement(pSelectionSize, 0)) == CoreDll.Error.Ok)
            {
                var selection = new bool[pSelectionSize[0].ToUInt32()];
                for (var i = 0; i < selection.Length; ++i)
                    selection[i] = pSelection[i] != CoreDll.FALSE;
                return selection;
            }
            return null;
        }

        public bool SetCurrent(int index)
        {
            return m_dll.adCurrentSet(m_handle, new IntPtr(index)) == CoreDll.Error.Ok;
        }

        public int GetCurrent()
        {
            var index = new IntPtr[1];
            index[0] = new IntPtr();
            if (m_dll.adCurrentGet(m_handle, Marshal.UnsafeAddrOfPinnedArrayElement(index, 0)) == CoreDll.Error.Ok)
            {
                return index[0].ToInt32();
            }
            return -1;
        }

        public CoreGroup[] GetGroup(uint startFrom, uint size)
        {
            var groupSize = GetGroupSize();
            if (groupSize > startFrom)
            {
                object groupObject = new CoreDll.adGroup();
                var sizeOfGroup= Marshal.SizeOf(groupObject);
                size = Math.Min(groupSize - startFrom, size);
                var buffer = new byte[sizeOfGroup * size];
                var groups = new CoreGroup[size];
                var pStartFrom = new UIntPtr[1];
                pStartFrom[0] = new UIntPtr(startFrom);
                var pSize = new UIntPtr[1];
                pSize[0] = new UIntPtr(size);
                if (m_dll.adGroupGet(m_handle, Marshal.UnsafeAddrOfPinnedArrayElement(pStartFrom, 0),
                    Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0),
                    Marshal.UnsafeAddrOfPinnedArrayElement(pSize, 0)) == CoreDll.Error.Ok)
                {
                    for (uint i = 0; i < pSize[0].ToUInt32(); ++i)
                    {
                        var pGroup = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, (int)(i * sizeOfGroup));
                        var group = (CoreDll.adGroup)Marshal.PtrToStructure(pGroup, groupObject.GetType());
                        groups[i] = new CoreGroup(ref group, this);
                    }
                }
                return groups;
            }
            return null;
        }

        /// <summary>
        /// Возврашает общее количество групп.
        /// </summary>
        /// <returns></returns>
        public uint GetGroupSize()
        {
            var pStartFrom = new UIntPtr[1];
            pStartFrom[0] = new UIntPtr(uint.MaxValue);
            if (m_dll.adGroupGet(m_handle, Marshal.UnsafeAddrOfPinnedArrayElement(pStartFrom, 0),
                new IntPtr(1), new IntPtr(1)) == CoreDll.Error.InvalidStartPosition)
            {
                return pStartFrom[0].ToUInt32();
            }
            return 0;
        }

        /// <summary>
        /// Возврашает массив CoreImageInfo содержащихся в переданной группе.
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="startFrom"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public CoreImageInfo[] GetImageInfo(int groupId, uint startFrom, uint size)
        {
            var imageInfoSize = GetImageInfoSize(groupId);
            if (imageInfoSize > startFrom)
            {
                object imageInfoObject = new CoreDll.adImageInfoW();
                var sizeOfImageInfo = Marshal.SizeOf(imageInfoObject);
                var buffer = new byte[sizeOfImageInfo * PAGE_SIZE];
                size = Math.Min(imageInfoSize - startFrom, size);
                var imageInfos = new CoreImageInfo[size];
                var pageCount = (uint)(size / PAGE_SIZE + (size % PAGE_SIZE > 0 ? 1 : 0));
                for (uint page = 0; page < pageCount; ++page)
                {
                    var pStartFrom = new UIntPtr[1];
                    pStartFrom[0] = new UIntPtr(startFrom + page * PAGE_SIZE);

                    var pSize = new UIntPtr[1];
                    pSize[0] = new UIntPtr(PAGE_SIZE);

                    if (m_dll.adImageInfoGetW(m_handle, new IntPtr(groupId), Marshal.UnsafeAddrOfPinnedArrayElement(pStartFrom, 0),
                        Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0),
                        Marshal.UnsafeAddrOfPinnedArrayElement(pSize, 0)) == CoreDll.Error.Ok)
                    {
                        for (uint i = 0; i < pSize[0].ToUInt32(); ++i)
                        {
                            var pImageInfo = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, (int)(i * sizeOfImageInfo));
                            var imageInfo = (CoreDll.adImageInfoW)Marshal.PtrToStructure(pImageInfo, imageInfoObject.GetType());
                            imageInfos[page * PAGE_SIZE + i] = new CoreImageInfo(ref imageInfo);
                        }

                    }
                }
                return imageInfos;
            }
            return null;
        }

        /// <summary>
        /// Возвращает количество изображений в переданной группе.
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public uint GetImageInfoSize(int groupId)
        {
            var pStartFrom = new UIntPtr[1];
            pStartFrom[0] = new UIntPtr(uint.MaxValue);
            if (m_dll.adImageInfoGetW(m_handle, new IntPtr(groupId), Marshal.UnsafeAddrOfPinnedArrayElement(pStartFrom, 0),
                new IntPtr(1), new IntPtr(1)) == CoreDll.Error.InvalidStartPosition)
            {
                return pStartFrom[0].ToUInt32();
            }
            return 0;
        }

        public bool SetSelection(int groupId, int index, CoreDll.SelectionType selectionType)
        {
            return m_dll.adImageInfoSelectionSet(m_handle, new IntPtr(groupId), new IntPtr(index), selectionType) == CoreDll.Error.Ok;
        }

        public bool[] GetSelection(int groupId, uint startFrom, uint size)
        {
            var pSelection = new int[size];
            var pStartFrom = new UIntPtr[1];
            pStartFrom[0] = new UIntPtr(startFrom);
            var pSelectionSize = new UIntPtr[1];
            pSelectionSize[0] = new UIntPtr(size);
            if (m_dll.adImageInfoSelectionGet(m_handle, new IntPtr(groupId), Marshal.UnsafeAddrOfPinnedArrayElement(pStartFrom, 0),
                Marshal.UnsafeAddrOfPinnedArrayElement(pSelection, 0),
                Marshal.UnsafeAddrOfPinnedArrayElement(pSelectionSize, 0)) == CoreDll.Error.Ok)
            {
                var selection = new bool[pSelectionSize[0].ToUInt32()];
                for (var i = 0; i < selection.Length; ++i)
                    selection[i] = pSelection[i] != CoreDll.FALSE;
                return selection;
            }
            return null;
        }

        public bool Rename(int groupId, int index, string newFileName)
        {
            return m_dll.adImageInfoRenameW(m_handle, new IntPtr(groupId), new IntPtr(index), newFileName) == CoreDll.Error.Ok;
        }

        public System.Drawing.Bitmap LoadBitmap(int width, int height, string path)
        {
            if (height * width == 0)
                return null;

            System.Drawing.Bitmap bitmap = null;
            try
            {
                bitmap = new System.Drawing.Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            }
            catch (System.Exception)
            {
                GC.Collect();
                try
                {
                    bitmap = new System.Drawing.Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                }
                catch (System.Exception)
                {
                    return null;
                }
            }
            var bitmapData = new System.Drawing.Imaging.BitmapData();
            bitmapData = bitmap.LockBits(
                new System.Drawing.Rectangle(0, 0, width, height),
                System.Drawing.Imaging.ImageLockMode.WriteOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb,
                bitmapData);
            var pBitmap = new CoreDll.adBitmap[1];
            pBitmap[0].width = (uint)bitmapData.Width;
            pBitmap[0].height = (uint)bitmapData.Height;
            pBitmap[0].stride = bitmapData.Stride;
            pBitmap[0].format = CoreDll.PixelFormatType.Argb32;
            pBitmap[0].data = bitmapData.Scan0;
            var error = m_dll.adLoadBitmapW(m_handle, path, Marshal.UnsafeAddrOfPinnedArrayElement(pBitmap, 0));
            bitmap.UnlockBits(bitmapData);
            return error == CoreDll.Error.Ok ? bitmap : null;
        }

        /// <summary>
        /// Возврашает загруженное изображение по заланному пути и заданного размера.
        /// </summary>
        /// <param name="size"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public System.Drawing.Bitmap LoadBitmap(System.Drawing.Size size, string path)
        {
            return LoadBitmap(size.Width, size.Height, path);
        }

        public System.Drawing.Bitmap LoadBitmap(CoreImageInfo imageInfo)
        {
            return LoadBitmap((int)imageInfo.width, (int)imageInfo.height, imageInfo.path);
        }

        //-----------Public properties----------------------------------------------

#region Public properties

        public CoreSearchOptions searchOptions
        {
            get
            {
                var options = new CoreDll.adSearchOptions[1];
                m_dll.adOptionsGet(m_handle, CoreDll.OptionsType.Search, Marshal.UnsafeAddrOfPinnedArrayElement(options, 0));
                return new CoreSearchOptions(options[0]);
            }
            set
            {
                var options = new CoreDll.adSearchOptions[1];
                value.ConvertTo(ref options[0]);
                m_dll.adOptionsSet(m_handle, CoreDll.OptionsType.Search, Marshal.UnsafeAddrOfPinnedArrayElement(options, 0));
            }
        }

        public CoreCompareOptions compareOptions
        {
            get
            {
                var options = new CoreDll.adCompareOptions[1];
                m_dll.adOptionsGet(m_handle, CoreDll.OptionsType.Compare, Marshal.UnsafeAddrOfPinnedArrayElement(options, 0));
                return new CoreCompareOptions(ref options[0]);
            }
            set
            {
                var options = new CoreDll.adCompareOptions[1];
                value.ConvertTo(ref options[0]);
                m_dll.adOptionsSet(m_handle, CoreDll.OptionsType.Compare, Marshal.UnsafeAddrOfPinnedArrayElement(options, 0));
            }
        }

        public CoreDefectOptions defectOptions
        {
            get
            {
                var options = new CoreDll.adDefectOptions[1];
                m_dll.adOptionsGet(m_handle, CoreDll.OptionsType.Defect, Marshal.UnsafeAddrOfPinnedArrayElement(options, 0));
                return new CoreDefectOptions(ref options[0]);
            }
            set
            {
                var options = new CoreDll.adDefectOptions[1];
                value.ConvertTo(ref options[0]);
                m_dll.adOptionsSet(m_handle, CoreDll.OptionsType.Defect, Marshal.UnsafeAddrOfPinnedArrayElement(options, 0));
            }
        }

        public CoreAdvancedOptions advancedOptions
        {
            get
            {
                var options = new CoreDll.adAdvancedOptions[1];
                m_dll.adOptionsGet(m_handle, CoreDll.OptionsType.Advanced, Marshal.UnsafeAddrOfPinnedArrayElement(options, 0));
                return new CoreAdvancedOptions(ref options[0]);
            }
            set
            {
                var options = new CoreDll.adAdvancedOptions[1]; //создаем массив из одного значения
                value.ConvertTo(ref options[0]); //конвертируем переданный класс
                m_dll.adOptionsSet(m_handle, CoreDll.OptionsType.Advanced, Marshal.UnsafeAddrOfPinnedArrayElement(options, 0));
            }
        }

        public CorePathWithSubFolder[] searchPath
        {
            get
            {
                return GetPath(CoreDll.PathType.Search);
            }
            set
            {
                SetPath(CoreDll.PathType.Search, value);
            }
        }

        public CorePathWithSubFolder[] ignorePath
        {
            get
            {
                return GetPath(CoreDll.PathType.Ignore);
            }
            set
            {
                SetPath(CoreDll.PathType.Ignore, value);
            }
        }
        
        public CorePathWithSubFolder[] validPath
        {
            get
            {
                return GetPath(CoreDll.PathType.Valid);
            }
            set
            {
                SetPath(CoreDll.PathType.Valid, value);
            }
        }

        public CorePathWithSubFolder[] deletePath
        {
            get
            {
                return GetPath(CoreDll.PathType.Delete);
            }
            set
            {
                SetPath(CoreDll.PathType.Delete, value);
            }
        }

#endregion

        //-----------Private functions:--------------------------------------------
#region private

        static private string BufferToString(char[] buffer, int startIndex, int maxSize)
        {
            if (startIndex >= buffer.Length)
                return null;
            int i = 0, n = Math.Min(maxSize, buffer.Length - startIndex);
            for (; i < n; ++i)
            {
                if (buffer[startIndex + i] == (char)0)
                    break;
            }
            return new string(buffer, startIndex, i);
        }

        private CorePathWithSubFolder[] GetPath(CoreDll.PathType pathType)
        {
            var pathWSF = new CorePathWithSubFolder[0];
            var size = new IntPtr[1];
            var path = new string[0];
            if (m_dll.adPathGetW(m_handle, pathType, new IntPtr(1), Marshal.UnsafeAddrOfPinnedArrayElement(size, 0)) ==
                            CoreDll.Error.OutputBufferIsTooSmall)
            {
                var buffer = new char[(CoreDll.MAX_PATH_EX + 1) * size[0].ToInt32()];
                if (m_dll.adPathGetW(m_handle, pathType, Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0),
                    Marshal.UnsafeAddrOfPinnedArrayElement(size, 0)) == CoreDll.Error.Ok)
                {
                    pathWSF = new CorePathWithSubFolder[size[0].ToInt32()];
                    for (var i = 0; i < size[0].ToInt32(); ++i)
                    {
                        pathWSF[i] = new CorePathWithSubFolder();
                        pathWSF[i].path = BufferToString(buffer, i * (CoreDll.MAX_PATH_EX + 1), CoreDll.MAX_PATH_EX);
                        if (buffer[(CoreDll.MAX_PATH_EX + 1) * i + CoreDll.MAX_PATH_EX] == (char)1)
                            pathWSF[i].enableSubFolder = true;
                        else
                            pathWSF[i].enableSubFolder = false;
                    }
                }
            }
            return pathWSF;
        }

        private bool SetPath(CoreDll.PathType pathType, CorePathWithSubFolder[] path)
        {
            var buffer = new char[path.Length * (CoreDll.MAX_PATH_EX + 1)];
            for (var i = 0; i < path.Length; i++)
            {
                path[i].path.CopyTo(0, buffer, i * (CoreDll.MAX_PATH_EX + 1), path[i].path.Length);
                buffer[(CoreDll.MAX_PATH_EX + 1) * i + CoreDll.MAX_PATH_EX] = path[i].enableSubFolder ? (char)1 : (char)0;
            }
            
            return m_dll.adPathWithSubFolderSetW(m_handle, 
                pathType, 
                Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0),
                new IntPtr(path.Length)) == CoreDll.Error.Ok;
        }

#endregion
    };
