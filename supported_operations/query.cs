using System;



public class Query : Operation
{
    private string _tableName;
    private string _columnName;
    private DataType _value;
    private ComparisonOperators _operator;


    public Query(Database database, string tableName, string columnName, DataType value, ComparisonOperators op) : base(database)
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
        if(!column.Type.Equals( _value.GetType()))
        {
            throw new Exception("Value type does not match column type");
        }
        
    }

    // protected override List<Row> Execution()
    // {
    //     Table table = _database.GetAllTables()[_tableName];
    //     var columnIndex = table.Schema.Columns.FindIndex(c => c.Name == _columnName);
    //     List<Row> result = new List<Row>();

    //     foreach (var row in table.Rows)
    //     {
    //         var cellValue = row.Values[columnIndex];
    //         bool conditionMet = false;

    //         switch (_operator)
    //         {
    //             case "==":
    //                 conditionMet = cellValue.Equals(_value);
    //                 break;
    //             case "!=":
    //                 conditionMet = !cellValue.Equals(_value);
    //                 break;
    //             case ">":
    //                 conditionMet = Convert.ToDouble(cellValue) > Convert.ToDouble(_value);
    //                 break;
    //             case "<":
    //                 conditionMet = Convert.ToDouble(cellValue) < Convert.ToDouble(_value);
    //                 break;
    //             case ">=":
    //                 conditionMet = Convert.ToDouble(cellValue) >= Convert.ToDouble(_value);
    //                 break;
    //             case "<=":
    //                 conditionMet = Convert.ToDouble(cellValue) <= Convert.ToDouble(_value);
    //                 break;
    //         }

    //         if (conditionMet)
    //         {
    //             result.Add(row);
    //         }
    //     }

    //     return result;
    // }

    protected override List<Row> Execution()
    {
        Table table = _database.GetAllTables()[_tableName];
        int columnIndex = table.Schema.Columns.FindIndex(c => c.Name == _columnName);
        List<Row> result = new List<Row>();

        foreach (var row in table.Rows)
        {
            var cellValue = row.Values.ElementAt(columnIndex).Value;

            if (!(cellValue is IComparable))
                continue; 
            int cmp = ((IComparable)cellValue).CompareTo(_value);

            bool conditionMet = _operator switch
            {
                ComparisonOperators.Equal => cmp == 0,
                ComparisonOperators.NotEqual => cmp != 0,
                ComparisonOperators.Greater => cmp > 0,
                ComparisonOperators.Less => cmp < 0,
                ComparisonOperators.GreaterOrEqual => cmp >= 0,
                ComparisonOperators.LessOrEqual => cmp <= 0,
                _ => false
            };

            if (conditionMet)
                result.Add(row);
        }
        // DataChangePublisher publisher = new DataChangePublisher();
        // publisher.PublishChange($"Queried table: {_tableName} with column: {_columnName}, operator: {_operator}, value: {_value}");


        return result;
    }

}