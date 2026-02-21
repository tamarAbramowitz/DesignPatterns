using System;
using System.Collections.Generic;


class DataChangePublisher
{
    private List<IObserver> observers = new List<IObserver>();

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