using System;
using System.Collections.Generic;
using System.Linq;

class UserInterface
{
    private DatabaseAPI db;
    private DataChangePublisher publisher;

    public UserInterface()
    {
        db = new DatabaseAPI("MyDatabase");
        publisher = new DataChangePublisher();
        publisher.Attach(new LogObserver());
    }

    public void Run()
    {
        Console.WriteLine("╔════════════════════════════════════════╗");
        Console.WriteLine("║   In-Memory Database Management System ║");
        Console.WriteLine("╚════════════════════════════════════════╝");
        Console.WriteLine();

        while (true)
        {
            ShowMenu();
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    CreateTable();
                    break;
                case "2":
                    InsertRow();
                    break;
                case "3":
                    QueryTable();
                    break;
                case "4":
                    ShowAllTables();
                    break;
                case "5":
                    CloneTable();
                    break;
                case "0":
                    Console.WriteLine("Goodbye!");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Try again.");
                    break;
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }
    }

    private void ShowMenu()
    {
        Console.WriteLine("┌─────────────────────────────┐");
        Console.WriteLine("│         MAIN MENU           │");
        Console.WriteLine("├─────────────────────────────┤");
        Console.WriteLine("│ 1. Create Table             │");
        Console.WriteLine("│ 2. Insert Row               │");
        Console.WriteLine("│ 3. Query Table              │");
        Console.WriteLine("│ 4. Show All Tables          │");
        Console.WriteLine("│ 5. Clone Table              │");
        Console.WriteLine("│ 0. Exit                     │");
        Console.WriteLine("└─────────────────────────────┘");
        Console.Write("\nYour choice: ");
    }

    private void CreateTable()
    {
        Console.WriteLine("\n=== CREATE TABLE ===");
        Console.Write("Table name: ");
        string tableName = Console.ReadLine() ?? "";

        var columns = new List<(string, DataType)>();
        
        while (true)
        {
            Console.Write("\nColumn name (or press Enter to finish): ");
            string colName = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(colName)) break;

            Console.WriteLine("Data type: 1=Integer, 2=String, 3=Boolean");
            Console.Write("Choice: ");
            string typeChoice = Console.ReadLine() ?? "";

            DataType dataType = typeChoice switch
            {
                "1" => DataType.Integer,
                "2" => DataType.String,
                "3" => DataType.Boolean,
                _ => DataType.String
            };

            columns.Add((colName, dataType));
            Console.WriteLine($"✓ Added column: {colName} ({dataType})");
        }

        if (columns.Count > 0)
        {
            db.CreateTable(tableName, columns.ToArray());
            publisher.PublishChange($"Table '{tableName}' created with {columns.Count} columns");
            Console.WriteLine($"\n✓ Table '{tableName}' created successfully!");
        }
        else
        {
            Console.WriteLine("\n✗ No columns added. Table not created.");
        }
    }

    private void InsertRow()
    {
        Console.WriteLine("\n=== INSERT ROW ===");
        Console.Write("Table name: ");
        string tableName = Console.ReadLine() ?? "";

        try
        {
            var values = new Dictionary<string, object>();

            Console.WriteLine("\nEnter values for each column:");
            Console.Write("Column name (or press Enter to finish): ");
            
            while (true)
            {
                string colName = Console.ReadLine() ?? "";
                if (string.IsNullOrWhiteSpace(colName)) break;

                Console.Write($"Value for {colName}: ");
                string value = Console.ReadLine() ?? "";

                if (int.TryParse(value, out int intVal))
                    values[colName] = intVal;
                else if (bool.TryParse(value, out bool boolVal))
                    values[colName] = boolVal;
                else
                    values[colName] = value;

                Console.Write("Next column name (or press Enter to finish): ");
            }

            if (values.Count > 0)
            {
                db.Insert(tableName, values);
                publisher.PublishChange($"Row inserted into '{tableName}'");
                Console.WriteLine($"\n✓ Row inserted successfully!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n✗ Error: {ex.Message}");
        }
    }

    private void QueryTable()
    {
        Console.WriteLine("\n=== QUERY TABLE ===");
        Console.Write("Table name: ");
        string tableName = Console.ReadLine() ?? "";

        try
        {
            Console.WriteLine("\n1. Show all rows");
            Console.WriteLine("2. Query with condition");
            Console.Write("Choice: ");
            string choice = Console.ReadLine() ?? "";

            List<Row> results;

            if (choice == "2")
            {
                Console.Write("\nColumn name: ");
                string colName = Console.ReadLine() ?? "";

                Console.WriteLine("Operator: 1=Equal, 2=NotEqual, 3=Greater, 4=Less");
                Console.Write("Choice: ");
                string opChoice = Console.ReadLine() ?? "";

                Console.Write("Value: ");
                string value = Console.ReadLine() ?? "";

                ComparisonOperator op = opChoice switch
                {
                    "1" => ComparisonOperator.Equal,
                    "2" => ComparisonOperator.NotEqual,
                    "3" => ComparisonOperator.GreaterThan,
                    "4" => ComparisonOperator.LessThan,
                    _ => ComparisonOperator.Equal
                };

                object compareValue = int.TryParse(value, out int intVal) ? intVal : value;
                var condition = new ComparisonExpression(colName, op, compareValue);
                results = db.Query(tableName, condition);
            }
            else
            {
                results = db.Query(tableName);
            }

            Console.WriteLine($"\n✓ Found {results.Count} row(s):");
            Console.WriteLine(new string('─', 50));
            
            foreach (var row in results)
            {
                foreach (var kvp in row.Values)
                {
                    Console.Write($"{kvp.Key}: {kvp.Value}  ");
                }
                Console.WriteLine();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n✗ Error: {ex.Message}");
        }
    }

    private void ShowAllTables()
    {
        Console.WriteLine("\n=== ALL TABLES ===");
        Console.WriteLine("(This feature requires access to internal database structure)");
        Console.WriteLine("Tables created in this session are stored in the database.");
    }

    private void CloneTable()
    {
        Console.WriteLine("\n=== CLONE TABLE ===");
        Console.WriteLine("(This feature requires direct database access)");
        Console.WriteLine("Use the DatabaseAPI to access tables for cloning.");
    }
}
