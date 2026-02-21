public class Remove : Operation
{
    private List<string> _tablesNames;

    public Remove(Database database, List<string> tableNames) : base(database)
    {
        _tablesNames = tableNames;    
    }



    protected override void Validation()
    {
        var tables = _database.GetAllTables();

        foreach (var tableName in _tablesNames)
        {
            if (!tables.ContainsKey(tableName))
            {
                throw new Exception("Table does not exist");
            }

        }
    }
    
    protected override List<Row> Execution()
    {
        foreach (var tableName in _tablesNames)
        {
            _database.RemoveTable(tableName);
        }
        DataChangePublisher publisher = new DataChangePublisher();
        publisher.PublishChange($"Removed tables: {string.Join(", ", _tablesNames)}");
        return new List<Row>();
    }
}
