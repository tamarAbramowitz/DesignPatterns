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

        if (_tables == null || _tables.Count == 0)
        {
            throw new Exception("No tables provided to create");
        }

        foreach (var table in _tables)
        {
            if (string.IsNullOrWhiteSpace(table.Name))
            {
                throw new Exception("Table name cannot be empty");
            }

            if (tables.ContainsKey(table.Name))
            {
                throw new Exception($"Table '{table.Name}' already exists");
            }

            if (table.Schema == null || table.Schema.Columns.Count == 0)
            {
                throw new Exception($"Table '{table.Name}' must have at least one column");
            }

            // Validate column names are unique
            var columnNames = table.Schema.Columns.Select(c => c.Name).ToList();
            if (columnNames.Count != columnNames.Distinct().Count())
            {
                throw new Exception($"Table '{table.Name}' has duplicate column names");
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
