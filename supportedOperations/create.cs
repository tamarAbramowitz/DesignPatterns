public class Create : Operation
{
    private Dictionary<string, (string columnName, DataType dataType)[]> _tables;
    private DatabaseAPI _databaseAPI;

    public Create(Database database, Dictionary<string, (string, DataType)[]> tables, DatabaseAPI databaseAPI) 
        : base(database)
    {
        _tables = tables;
        _databaseAPI = databaseAPI;
    }

    protected override void Validation()
    {
        var tables = _database.GetAllTables();

        foreach (var table in _tables)
        {
            if (string.IsNullOrWhiteSpace(table.Key))
            {
                throw new Exception("Table name cannot be empty");
            }

            if (tables.ContainsKey(table.Key))
            {
                throw new Exception($"Table '{table.Key}' already exists");
            }

            if (table.Value == null || table.Value.Length == 0)
            {
                throw new Exception($"Table '{table.Key}' must have at least one column");
            }

            if (table.Value.Any(c => string.IsNullOrWhiteSpace(c.columnName)))
            {
                throw new Exception($"Table '{table.Key}' has column with empty name");
            }

            var columnNames = table.Value.Select(c => c.columnName).ToList();
            if (columnNames.Count != columnNames.Distinct().Count())
            {
                throw new Exception($"Table '{table.Key}' has duplicate column names");
            }
        }
    }
    
    protected override List<Row> Execution()
    {
        foreach (var table in _tables)
        {
            _databaseAPI.CreateTable(
                table.Key,
                table.Value
            );
        }
        
        return new List<Row>();
    }
}
