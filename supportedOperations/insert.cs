using System;
using System.Linq;

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

        foreach (var kvp in _row.Values)
        {
            var column = table.Schema.Columns.Find(c => c.Name == kvp.Key);
            if (column == null)
            {
                throw new Exception($"Column {kvp.Key} does not exist in table");
            }

            if (kvp.Value == null || (kvp.Value is string str && string.IsNullOrWhiteSpace(str)))
            {
                throw new Exception($"Value for column {kvp.Key} cannot be null or empty");
            }

            var expectedType = column.Type;
            var actualValue = kvp.Value;

            if (expectedType == DataType.Integer)
            {
                if (!(actualValue is int))
                {
                    throw new Exception($"Column {kvp.Key} expects Integer type");
                }
            }
            else if (expectedType == DataType.String)
            {
                if (!(actualValue is string))
                {
                    throw new Exception($"Column {kvp.Key} expects String type");
                }
            }
            else if (expectedType == DataType.Boolean)
            {
                if (!(actualValue is bool))
                {
                    throw new Exception($"Column {kvp.Key} expects Boolean type");
                }
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
