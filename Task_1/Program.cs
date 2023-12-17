using System;
using System.Threading;

class Resource
{
    private Semaphore semaphore;
    private Mutex mutex;
    private int highPriorityCount;

    public Resource()
    {
        semaphore = new Semaphore(1, 1);
        mutex = new Mutex();
        highPriorityCount = 0;
    }

    public void AccessResource(bool isHighPriority)
    {
        if (isHighPriority)
        {
            mutex.WaitOne();
            highPriorityCount++;
            mutex.ReleaseMutex();
        }

        semaphore.WaitOne();

       

        semaphore.Release();

        if (isHighPriority)
        {
            mutex.WaitOne();
            highPriorityCount--;
            if (highPriorityCount == 0)
            {
                
            }
            mutex.ReleaseMutex();
        }
    }
}

class Program
{
    static void Main()
    {
        Resource resource = new Resource();

        
        Thread highPriorityThread1 = new Thread(() => resource.AccessResource(true));
        Thread highPriorityThread2 = new Thread(() => resource.AccessResource(true));
        highPriorityThread1.Start();
        highPriorityThread2.Start();

       
        Thread lowPriorityThread1 = new Thread(() => resource.AccessResource(false));
        Thread lowPriorityThread2 = new Thread(() => resource.AccessResource(false));
        lowPriorityThread1.Start();
        lowPriorityThread2.Start();
    }
}