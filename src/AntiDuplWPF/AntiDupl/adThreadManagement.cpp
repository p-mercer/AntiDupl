/*
* AntiDupl Dynamic-Link Library.
*
* Copyright (c) 2002-2015 Yermalayeu Ihar.
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
#include "adFileUtils.h"
#include "adEngine.h"
#include "adImageData.h"
#include "adOptions.h"
#include "adStatus.h"
#include "adImageComparer.h"
#include "adDataCollector.h"
#include "adThreadManagement.h"
#include "adResult.h"
#include "adResultStorage.h"
#include "adPerformance.h"

namespace ad
{
    TThreadManager::TThreadManager(TEngine *pEngine)
        :m_pEngine(pEngine),
        m_pOptions(pEngine->Options()),
        m_pThreads(NULL)
    {
    }

    TThreadManager::~TThreadManager()
    {
        if(m_pThreads != NULL)
            delete m_pThreads; 
    }

    void TThreadManager::Finish()
    {
        for(std::vector<TThread>::iterator i = m_pThreads->begin(); i != m_pThreads->end(); i++)
        {
            i->task->Queue()->Finish();
            WaitForSingleObject(i->thread->Handle(), INFINITE);
            delete i->thread;
            delete i->task;
        }
        delete m_pThreads;
        m_pThreads = NULL;
    }

    bool TThreadManager::SetPriority(int priority)
    {
        bool result = true;
        for(std::vector<TThread>::iterator it = m_pThreads->begin(); it != m_pThreads->end(); it++)
            result = result && it->thread->SetPriority(priority);
        return result;
    }

    void TThreadManager::SetSleepInterval(TUInt32 sleepInterval)
    {
        for(std::vector<TThread>::iterator it = m_pThreads->begin(); it != m_pThreads->end(); it++)
            it->task->SetSleepInterval(sleepInterval);
    }

    size_t TThreadManager::GetProcessorCount()
    {
        SYSTEM_INFO systemInfo;
        GetSystemInfo(&systemInfo); 
        return (size_t)systemInfo.dwNumberOfProcessors;
    }
    //-------------------------------------------------------------------------
    TCompareManager::TCompareManager(TEngine *pEngine)
        :TThreadManager(pEngine)
    {
        m_pCS = new TCriticalSection();
    }

    TCompareManager::~TCompareManager()
    {
        delete m_pCS;
    }

    void TCompareManager::Start(size_t imageCount)
    {
        size_t threadCount;
        if(m_pOptions->advanced.compareThreadCount <= 0)
            threadCount = DefaultThreadCount(imageCount);
        else
            threadCount = m_pOptions->advanced.compareThreadCount;

        m_pThreads = new std::vector<TThread>(threadCount);
        m_pEngine->Status()->SetThreadCount(AD_THREAD_TYPE_COMPARE, threadCount);

        for(size_t i = 0; i < threadCount; i++)
        {
            TThread& thread = m_pThreads->at(i);
            thread.task = new TCompareTask(i, m_pEngine);
			thread.thread = new ad::TThread(thread.task);
            thread.thread->Resume();
        }

        m_addCounter = 0;
    }

    void TCompareManager::Add(TImageData *pImageData)
    {
        if(CanCompare(pImageData))
        {
            TCriticalSection::TLocker locker(m_pCS);
            size_t threadId = m_addCounter%m_pThreads->size();
            for(std::vector<TThread>::iterator i = m_pThreads->begin(); i != m_pThreads->end(); i++)
                i->task->Queue()->Push(pImageData, threadId);
            m_pEngine->Status()->Assign(AD_THREAD_TYPE_COMPARE, threadId);
            m_addCounter++;
        }
    }

    size_t TCompareManager::DefaultThreadCount(size_t imageCount)
    {
        size_t threadCountMax = GetProcessorCount();
        if(imageCount > (size_t)LARGE_IMAGE_COLLECTION_SIZE_MIN || m_pOptions->compare.transformedImage == TRUE)
            return threadCountMax;
        else
            return Simd::Max((size_t)1, threadCountMax/2);
    }

    bool TCompareManager::CanCompare(TImageData *pImageData) const
    {
		const adCompareOptions & compare = m_pOptions->compare;
        return 
            compare.checkOnEquality == TRUE && pImageData->type > AD_IMAGE_NONE &&
            pImageData->width >= (TUInt32)compare.minimalImageSize && pImageData->width <= (TUInt32)compare.maximalImageSize &&
            pImageData->height >= (TUInt32)compare.minimalImageSize && pImageData->height <= (TUInt32)compare.maximalImageSize;
    }
    //-------------------------------------------------------------------------
    TCollectManager::TCollectManager(TEngine *pEngine, TCompareManager* pCompareManager)
        :TThreadManager(pEngine),
        m_pCompareManager(pCompareManager)
    {
    }

    void TCollectManager::Start()
    {
        size_t threadCount;
        if(m_pOptions->advanced.collectThreadCount <= 0)
            threadCount = DefaultThreadCount();
        else
            threadCount = m_pOptions->advanced.collectThreadCount;

        m_pThreads = new std::vector<TThread>(threadCount);
        m_pEngine->Status()->SetThreadCount(AD_THREAD_TYPE_COLLECT, threadCount);

        for(size_t i = 0; i < threadCount; i++)
        {
            TThread& thread = m_pThreads->at(i);
            thread.task = new TCollectTask(i, m_pEngine, m_pCompareManager);
            thread.thread = new ad::TThread(thread.task);
            thread.thread->Resume();
        }

        m_addCounter = 0;
    }

    void TCollectManager::Add(TImageData *pImageData)
    {
        if(pImageData->DefectCheckingNeed(m_pOptions) || pImageData->PixelDataFillingNeed(m_pOptions) || pImageData->crc32c == 0)
        {
            pImageData->hGlobal = LoadFileToMemory(pImageData->path.Original().c_str());
            size_t threadId = GetThreadId();
            m_pThreads->at(threadId).task->Queue()->Push(pImageData, threadId);
            m_pEngine->Status()->Assign(AD_THREAD_TYPE_COLLECT, threadId);
        }
        else
        {
			TDefectType defect = pImageData->GetDefect(m_pOptions);
			if(defect > AD_DEFECT_NONE)
				m_pEngine->Result()->AddDefectImage(pImageData, defect);
            pImageData->FillOther(m_pOptions);
            m_pCompareManager->Add(pImageData);
        }
    }

    size_t TCollectManager::DefaultThreadCount()
    {
        size_t threadCountMax = GetProcessorCount();
        return Simd::Min(Simd::Max((size_t)2, threadCountMax - 1), threadCountMax);
    }
    
    size_t TCollectManager::GetThreadId() const
    {
        AD_FUNCTION_PERFORMANCE_TEST
        size_t threadId = 0;
        while(!m_pEngine->Status()->Stopped())
        {
            size_t sizeMin = COLLECT_THREAD_QUEUE_SIZE_MAX;
            for(std::vector<TThread>::iterator it = m_pThreads->begin(); it != m_pThreads->end(); it++)
            {
                if(it->task->Queue()->Size() < sizeMin)
                {
                    sizeMin = it->task->Queue()->Size();
                    threadId = it->task->Queue()->Id();
                }
            }
            if(sizeMin < COLLECT_THREAD_QUEUE_SIZE_MAX)
                break;
            m_pEngine->Status()->Wait(AD_THREAD_TYPE_MAIN, 0); 
            ::Sleep(DEAFAULT_THREAD_SLEEP_INTERVAL);
        }
        return threadId;
    }
    //-------------------------------------------------------------------------
}
