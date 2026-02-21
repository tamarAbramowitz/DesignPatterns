public class Insert : Operation
{
    private string _tableName;
    private Row _row;

    public Insert(Database database, string tableName, Row row) : base(database)
    {
        _tableName = tableName;
        _row = row;
    }

    protected override void Validation()
    {
        var tables = _database.GetAllTables();

        if (!tables.ContainsKey(_tableName))
        {
            throw new Exception("Table does not exist");
        }
        Table table = tables[_tableName];

        if (_row.Values.Count != table.Schema.Columns.Count)
        {
            throw new Exception("Number of values does not match number of columns");
        }

        for (int i = 0; i < _row.Values.Count; i++)
        {
            if (_row.Values[object][i].GetType() != table.Schema.Columns[i].Type)
            {
                throw new Exception($"Value type does not match column type for column {table.Schema.Columns[i].Name}");
            }
        }
        
    
    }

    protected override List<Row> Execution()
    {
        Table table = _database.GetAllTables()[_tableName];
        table.Rows.Add(_row);
        DataChangePublisher publisher = new DataChangePublisher();
        publisher.PublishChange($"Inserted row into table: {_tableName} with values: {string.Join(", ", _row.Values)}");
        return new List<Row> { _row };

    }
}