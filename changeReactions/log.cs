class LogObserver : IObserver
{

    public void Update(string change)
    {
        
        Console.WriteLine("LogObserver received change: " + change);
    }
}