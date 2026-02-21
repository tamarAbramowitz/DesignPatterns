public class Create : Operation
{
    private List<Table> _tables;

    public Create(Database database, List<Table> tables) : base(database)
    {
        _tables = tables;    
    }



    protected override void Validation()
    {
        var tables = _database.GetAllTables();

        foreach (var table in _tables)
        {
            var tableName = table.Name;
            if (tables.ContainsKey(tableName))
            {
                throw new Exception("Table already exists");
            }

        }
    }
    
    protected override List<Row> Execution()
    {
        foreach (var table in _tables)
        {
            _database.RegisterTable(table);
        }
        DataChangePublisher publisher = new DataChangePublisher();
        publisher.PublishChange($"Created tables: {string.Join(", ", _tables.Select(t => t.Name))}");

        return new List<Row>();
    }
}
