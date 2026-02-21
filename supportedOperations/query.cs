using System;

public class Query : Operation
{
    private string _tableName;
    private string _columnName;
    private DataType _value;
    private ComparisonOperator _operator;

    public Query(Database database, string tableName, string columnName, DataType value, ComparisonOperator op) : base(database)
    {
        _tableName = tableName;
        _columnName = columnName;
        _value = value;
        _operator = op;
    }

    protected override void Validation()
    {
       var tables = _database.GetAllTables();

        if (!tables.ContainsKey(_tableName))
        {
            throw new Exception("Table does not exist");
        }
        Table table = tables[_tableName];
        var column = table.Schema.Columns.FirstOrDefault(c => c.Name == _columnName);

        if (column == null)
        {
            throw new Exception("Column does not exist");
        }
        if(column.Type != _value)
        {
            throw new Exception("Value type does not match column type");
        }
    }

    protected override List<Row> Execution()
    {
        Table table = _database.GetAllTables()[_tableName];
        List<Row> result = new List<Row>();

        foreach (var row in table.Rows)
        {
            var cellValue = row.GetValue(_columnName);

            if (!(cellValue is IComparable))
                continue; 
            int cmp = ((IComparable)cellValue).CompareTo(_value);

            bool conditionMet = _operator switch
            {
                ComparisonOperator.Equal => cmp == 0,
                ComparisonOperator.NotEqual => cmp != 0,
                ComparisonOperator.GreaterThan => cmp > 0,
                ComparisonOperator.LessThan => cmp < 0,
                ComparisonOperator.GreaterOrEqual => cmp >= 0,
                ComparisonOperator.LessOrEqual => cmp <= 0,
                _ => false
            };

            if (conditionMet)
                result.Add(row);
        }

        return result;
    }
}
