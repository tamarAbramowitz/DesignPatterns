using System;
using System.Collections.Generic;


class DataChangePublisher
{
    private static DataChangePublisher? _instance;
    private List<IObserver> observers = new List<IObserver>();

    // Singleton Pattern
    public static DataChangePublisher Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new DataChangePublisher();
            }
            return _instance;
        }
    }

    private DataChangePublisher() { }

    public void Attach(IObserver observer)
    {
        observers.Add(observer);
    }

    public void Detach(IObserver observer)
    {
        observers.Remove(observer);
    }

    public void PublishChange(string change)
    {
        NotifyObservers(change);
    }

    private void NotifyObservers(string change)
    {
        foreach (var observer in observers)
        {
            observer.Update(change);
        }
    }
}