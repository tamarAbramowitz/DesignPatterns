public class Delete : Operation
{
    private string _tableName;
    private List<int> _indexes;

    public Delete(Database database, string tableName, List<int> indexes) : base(database)
    {
        _tableName = tableName;
        _indexes = indexes;
    }
    
    protected override void Validation()
    {
        var tables = _database.GetAllTables();

        if (!tables.ContainsKey(_tableName))
        {
            throw new Exception("Table does not exist");
        }

        if (_indexes == null || _indexes.Count == 0)
        {
            throw new Exception("No indexes provided for deletion");
        }

        Table table = tables[_tableName]; 

        foreach (var index in _indexes)
        {
            if (index < 0 || index >= table.Rows.Count)
            {
                throw new Exception($"Index {index} is out of range");
            }
        }

        // Check for duplicate indexes
        if (_indexes.Count != _indexes.Distinct().Count())
        {
            throw new Exception("Duplicate indexes provided");
        }
    }

    protected override List<Row> Execution()
    {
        Table table = _database.GetAllTables()[_tableName];
        List<Row> deletedRows = new List<Row>();

        foreach (var index in _indexes)
        {
            deletedRows.Add(table.Rows[index]);
        }

        foreach (var index in _indexes.OrderByDescending(i => i))
        {
            table.Rows.RemoveAt(index);
        }
        DataChangePublisher.Instance.PublishChange($"Deleted rows in indexes: {string.Join(", ", _indexes)} from table: {_tableName}");
        return deletedRows;
    }
}