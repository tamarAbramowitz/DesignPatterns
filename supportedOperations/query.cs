using System;
using System.Linq;

public class Query : Operation
{
    private string _tableName;
    private IExpression _condition;

    public Query(Database database, string tableName, IExpression condition) : base(database)
    {
        _tableName = tableName;
        _condition = condition;
    }

    protected override void Validation()
    {
        var tables = _database.GetAllTables();

        if (string.IsNullOrWhiteSpace(_tableName))
        {
            throw new Exception("Table name cannot be null or empty");
        }

        if (!tables.ContainsKey(_tableName))
        {
            throw new Exception($"Table '{_tableName}' does not exist");
        }

        if (_condition == null)
        {
            throw new Exception("Query condition cannot be null");
        }
    }

    protected override List<Row> Execution()
    {
        Table table = _database.GetAllTables()[_tableName];
        
        if (_condition == null)
            return table.Rows.ToList();

        return table.Rows.Where(row => _condition.Interpret(row)).ToList();
    }
}
