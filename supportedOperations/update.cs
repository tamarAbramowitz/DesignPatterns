public class Update : Operation
{
    private string _tableName;
    private List<Row> _updatedRows;
    private List<int> _indexes;
    public Update(Database database, string tableName, List<int> indexes, List<Row> updatedRows) : base(database)
    {
        _tableName = tableName;
        _indexes = indexes;
        _updatedRows = updatedRows;
    }
    protected override void Validation()
    {
        var tables = _database.GetAllTables();

        if (!tables.ContainsKey(_tableName))
        {
            throw new Exception("Table does not exist");
        }

        if (_indexes.Count != _updatedRows.Count)
        {
            throw new Exception("Rows count mismatch");
        }
            
        Table table = tables[_tableName];

        foreach (var index in _indexes)
        {
            if (index < 0 || index >= table.Rows.Count)
            {
                throw new Exception("Index out of range");
            }
        }
        
        foreach (var row in _updatedRows)
        {
            if (row.Values.Count != table.Schema.Columns.Count)
            {
                throw new Exception("Number of values does not match number of columns");
            }

            foreach (var kvp in row.Values)
            {
                var column = table.Schema.Columns.Find(c => c.Name == kvp.Key);
                if (column == null)
                {
                    throw new Exception($"Column {kvp.Key} does not exist in table");
                }

                // Validate value is not null or empty
                if (kvp.Value == null || (kvp.Value is string str && string.IsNullOrWhiteSpace(str)))
                {
                    throw new Exception($"Value for column {kvp.Key} cannot be null or empty");
                }

                // Validate data type
                var expectedType = column.Type;
                var actualValue = kvp.Value;

                if (expectedType == DataType.Integer && !(actualValue is int))
                {
                    throw new Exception($"Column {kvp.Key} expects Integer type");
                }
                else if (expectedType == DataType.String && !(actualValue is string))
                {
                    throw new Exception($"Column {kvp.Key} expects String type");
                }
                else if (expectedType == DataType.Boolean && !(actualValue is bool))
                {
                    throw new Exception($"Column {kvp.Key} expects Boolean type");
                }
            }
        }
    }

    protected override List<Row> Execution()
    {
        var x=0;
        Table table = _database.GetAllTables()[_tableName];
        foreach (var index in _indexes)
        {
            table.Rows[index] = _updatedRows[x];
            x++;
            
        }
        DataChangePublisher publisher = new DataChangePublisher();
        publisher.PublishChange($"Updated rows in indexes: {string.Join(", ", _indexes)} in table: {_tableName} with values: {string.Join(", ", _updatedRows.Select(r => string.Join(", ", r.Values)))}");
        return _updatedRows;
    }
}
