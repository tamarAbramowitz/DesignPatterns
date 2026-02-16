using System;
using System.Collections.Generic;

public class Database
{
    public string Name { get; private set; }

    private Dictionary<string, Table> _tables;

    public Database(string name)
    {
        Name = name;
        _tables = new Dictionary<string, Table>();
    }

    // Create Table
    public void RegisterTable(Table table)
    {
        if (table == null)
            throw new ArgumentNullException(nameof(table));

        if (_tables.ContainsKey(table.Name))
            throw new InvalidOperationException($"Table '{table.Name}' already exists.");

        _tables.Add(table.Name, table);
    }

    // Remove Table
    public void RemoveTable(string tableName)
    {
        if (!_tables.ContainsKey(tableName))
            throw new InvalidOperationException($"Table '{tableName}' does not exist.");

        _tables.Remove(tableName);
    }

    // Get Table
    public Table GetTable(string tableName)
    {
        if (!_tables.ContainsKey(tableName))
            throw new InvalidOperationException($"Table '{tableName}' does not exist.");

        return _tables[tableName];
    }

    // Check if table exists
    public bool ContainsTable(string tableName)
    {
        return _tables.ContainsKey(tableName);
    }

    // Get all tables (read-only)
    public IReadOnlyDictionary<string, Table> GetAllTables()
    {
        return _tables;
    }
}
