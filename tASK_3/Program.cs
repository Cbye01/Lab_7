using System;
using System.Collections.Generic;
using System.Threading;

public class Operation
{
    public int ThreadId { get; set; }
    public DateTime Timestamp { get; set; }
    public string Description { get; set; }
}

public class ConflictLogger
{
    private List<Operation> operations;

    public ConflictLogger()
    {
        operations = new List<Operation>();
    }

    public void AddOperation(int threadId, string description)
    {
        var operation = new Operation
        {
            ThreadId = threadId,
            Timestamp = DateTime.Now,
            Description = description
        };

        lock (operations)
        {
            operations.Add(operation);
        }
    }

    public void DetectConflicts()
    {
        lock (operations)
        {
            for (int i = 0; i < operations.Count - 1; i++)
            {
                for (int j = i + 1; j < operations.Count; j++)
                {
                    if (operations[i].Timestamp == operations[j].Timestamp)
                    {
                        Console.WriteLine($"Conflict detected between Thread {operations[i].ThreadId} and Thread {operations[j].ThreadId}");
                        Console.WriteLine($"Operation 1: {operations[i].Description}");
                        Console.WriteLine($"Operation 2: {operations[j].Description}");
                    }
                }
            }
        }
    }
}

public class Program
{
    static void Main(string[] args)
    {
        ConflictLogger logger = new ConflictLogger();

        Thread thread1 = new Thread(() =>
        {
            logger.AddOperation(1, "Operation 1");
            Thread.Sleep(100);
            logger.AddOperation(1, "Operation 2");
        });

        Thread thread2 = new Thread(() =>
        {
            logger.AddOperation(2, "Operation 3");
            Thread.Sleep(100);
            logger.AddOperation(2, "Operation 4");
        });

        thread1.Start();
        thread2.Start();

        thread1.Join();
        thread2.Join();

        logger.DetectConflicts();
    }
}