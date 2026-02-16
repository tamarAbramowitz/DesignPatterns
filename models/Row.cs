public class Row
{
    public Dictionary<string, object> Values { get; private set; }

    public Row()
    {
        Values = new Dictionary<string, object>();
    }

    public void SetValue(string columnName, object val)
    {
        Values[columnName] = val;
    }

    public object GetValue(string columnName)
    {
        return Values.ContainsKey(columnName) ? Values[columnName] : null;
    }
}
