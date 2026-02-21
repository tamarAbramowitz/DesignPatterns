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

        Table table = tables[_tableName]; 

        foreach (var index in _indexes)
        {
            if (index < 0 || index >= table.Rows.Count)
            {
                throw new Exception("Index out of range");
            }
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
        DataChangePublisher publisher = new DataChangePublisher();
        publisher.PublishChange($"Deleted rows in indexes: {string.Join(", ", _indexes)} from table: {_tableName}");
        return deletedRows;
    }
}