using System;
using System.Collections.Generic;
using System.Linq;

public class DatabaseAPI
{
    private Database _database;

    public DatabaseAPI(string databaseName)
    {
        _database = new Database(databaseName);
    }

    public void CreateTable(string tableName, params (string columnName, DataType dataType)[] columns)
    {
        Schema schema = BuildSchema(columns);
        Table table = CreateTableInstance(tableName, schema);
        RegisterTableToDatabase(table);
        DataChangePublisher.Instance.PublishChange($"Created table: {table.Name}");

    }
    public void Insert(string tableName, Dictionary<string, object> values)
    {
        Table table = GetTableSafely(tableName);
        Row row = BuildRow(values);
        AddRowToTable(table, row);
        DataChangePublisher.Instance.PublishChange($"Inserted row into table: {tableName} with values: {string.Join(", ", values)}");
    }

    public List<Row> Query(string tableName, IExpression condition =null)
    {
        Table table = GetTableSafely(tableName);
        return FilterRows(table, condition);
    }

    private Schema BuildSchema((string columnName, DataType dataType)[] columns)
    {
        Schema schema = new Schema();
        foreach (var column in columns)
        {
            schema.AddColumn(column.columnName, column.dataType);
        }
        return schema;
    }

    private Table CreateTableInstance(string tableName, Schema schema)
    {
        return new Table(tableName, schema);
    }

    private void RegisterTableToDatabase(Table table)
    {
        _database.RegisterTable(table);

    }

    private Table GetTableSafely(string tableName)
    {
        return _database.GetTable(tableName);
    }
  
    private Row BuildRow(Dictionary<string, object> values)
    {
        Row row = new Row();
        foreach (var kvp in values)
        {
            row.SetValue(kvp.Key, kvp.Value);
        }
        return row;
    }

    private void AddRowToTable(Table table, Row row)
    {
        table.AddRow(row);
    }

    public List<Row> FilterRows(Table table, IExpression condition)
    {
        if (condition == null)
            return table.Rows.ToList();

        return table.Rows.Where(row => condition.Interpret(row)).ToList();
    }
}
