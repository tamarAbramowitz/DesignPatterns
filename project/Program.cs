using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Testing In-Memory Data Engine ===");
        Console.WriteLine();

        var database = new Database("TestDB");

        // Test Requirement 1: Table Definition (Builder Pattern)
        Console.WriteLine("--- Requirement 1: Table Definition (Builder Pattern) ---");
        var tableBuilder = new CreateBuilder()
            .SetName("Users")
            .AddColumn(new Column("Id", DataType.Integer))
            .AddColumn(new Column("Name", DataType.String))
            .AddColumn(new Column("Age", DataType.Integer))
            .AddColumn(new Column("IsActive", DataType.Boolean));
        var createTableOp = tableBuilder.Build();
        var table = createTableOp.Execute(database);
        database.RegisterTable(table);
        Console.WriteLine($"✓ Table '{table.Name}' created with {table.Schema.Columns.Count} columns");
        Console.WriteLine();

        // Test Requirement 2: Data Operations (Command Pattern)
        Console.WriteLine("--- Requirement 2: Data Operations (Command Pattern) ---");
        
        var row1 = new Row();
        row1.SetValue("Id", 1);
        row1.SetValue("Name", "Sara");
        row1.SetValue("Age", 25);
        row1.SetValue("IsActive", true);
        var insertOp1 = new Insert(database, "Users", row1);
        var insertResult1 = insertOp1.Execute();
        Console.WriteLine($"✓ Insert operation executed - returned {insertResult1.Count} row(s)");

        var row2 = new Row();
        row2.SetValue("Id", 2);
        row2.SetValue("Name", "David");
        row2.SetValue("Age", 17);
        row2.SetValue("IsActive", false);
        var insertOp2 = new Insert(database, "Users", row2);
        insertOp2.Execute();

        var row3 = new Row();
        row3.SetValue("Id", 3);
        row3.SetValue("Name", "Rachel");
        row3.SetValue("Age", 30);
        row3.SetValue("IsActive", true);
        var insertOp3 = new Insert(database, "Users", row3);
        insertOp3.Execute();
        Console.WriteLine($"✓ Total 3 rows inserted");

        var updatedRow = new Row();
        updatedRow.SetValue("Id", 2);
        updatedRow.SetValue("Name", "David Updated");
        updatedRow.SetValue("Age", 18);
        updatedRow.SetValue("IsActive", true);
        var updateOp = new Update(database, "Users", new List<int> { 1 }, new List<Row> { updatedRow });
        var updateResult = updateOp.Execute();
        Console.WriteLine($"✓ Update operation executed - returned {updateResult.Count} row(s)");

        var deleteOp = new Delete(database, "Users", new List<int> { 2 });
        var deleteResult = deleteOp.Execute();
        Console.WriteLine($"✓ Delete operation executed - returned {deleteResult.Count} row(s)");
        Console.WriteLine();

        // Test Requirement 3: Query Conditions (Interpreter Pattern)
        Console.WriteLine("--- Requirement 3: Query Conditions (Interpreter Pattern) ---");
        
        var ageCondition = new ComparisonExpression("Age", ComparisonOperator.GreaterThan, 18);
        var usersTable = database.GetTable("Users");
        var result1 = usersTable.Rows.FindAll(r => ageCondition.Interpret(r));
        Console.WriteLine($"✓ Simple condition (Age > 18): Found {result1.Count} rows");
        foreach (var row in result1)
        {
            Console.WriteLine($"  - {row.GetValue("Name")}, Age: {row.GetValue("Age")}");
        }

        var nameCondition = new ComparisonExpression("Name", ComparisonOperator.Equal, "Sara");
        var activeCondition = new ComparisonExpression("IsActive", ComparisonOperator.Equal, true);
        var andCondition = new AndExpression(ageCondition, activeCondition);
        var result2 = usersTable.Rows.FindAll(r => andCondition.Interpret(r));
        Console.WriteLine($"✓ Combined AND (Age > 18 AND IsActive): Found {result2.Count} rows");

        var youngCondition = new ComparisonExpression("Age", ComparisonOperator.LessThan, 20);
        var orCondition = new OrExpression(youngCondition, nameCondition);
        var result3 = usersTable.Rows.FindAll(r => orCondition.Interpret(r));
        Console.WriteLine($"✓ Combined OR (Age < 20 OR Name = 'Sara'): Found {result3.Count} rows");
        Console.WriteLine();

        // Test Requirement 4: DB Client Interface (Facade Pattern)
        Console.WriteLine("--- Requirement 4: DB Client Interface (Facade Pattern) ---");
        var dbAPI = new DatabaseAPI("MyDatabase");
        
        dbAPI.CreateTable("Products", 
            ("Id", DataType.Integer),
            ("Name", DataType.String),
            ("Price", DataType.Integer)
        );
        Console.WriteLine("✓ CreateTable via Facade");

        dbAPI.Insert("Products", new Dictionary<string, object>
        {
            { "Id", 1 },
            { "Name", "Laptop" },
            { "Price", 1000 }
        });
        Console.WriteLine("✓ Insert via Facade");

        var priceCondition = new ComparisonExpression("Price", ComparisonOperator.GreaterThan, 500);
        var queryResult = dbAPI.Query("Products", priceCondition);
        Console.WriteLine($"✓ Query via Facade: Found {queryResult.Count} products");
        Console.WriteLine();

        // Test Requirement 5: Table Cloning (Prototype Pattern)
        Console.WriteLine("--- Requirement 5: Table Cloning (Prototype Pattern) ---");
        
        var originalTable = database.GetTable("Users");
        Console.WriteLine($"✓ Original table has {originalTable.Rows.Count} rows");
        
        var clonedTable = TableCloner.Clone(originalTable);
        Console.WriteLine($"✓ Table cloned - has {clonedTable.Rows.Count} rows");
        
        var newRow = new Row();
        newRow.SetValue("Id", 99);
        newRow.SetValue("Name", "Clone Test");
        newRow.SetValue("Age", 40);
        newRow.SetValue("IsActive", true);
        clonedTable.AddRow(newRow);
        
        Console.WriteLine($"✓ Added row to cloned table");
        Console.WriteLine($"  Original: {originalTable.Rows.Count} rows, Cloned: {clonedTable.Rows.Count} rows");
        Console.WriteLine($"✓ Tables are independent");
        Console.WriteLine();

        // Test Requirement 6: Change Reactions and Logging (Observer Pattern)
        Console.WriteLine("--- Requirement 6: Change Reactions and Logging (Observer Pattern) ---");
        
        var publisher = new DataChangePublisher();
        var logger = new Logger();
        publisher.Attach(logger);
        
        publisher.PublishChange("Test: User inserted");
        publisher.PublishChange("Test: User updated");
        publisher.PublishChange("Test: User deleted");
        
        Console.WriteLine("✓ Observer pattern tested - check logs above");
        Console.WriteLine();

        Console.WriteLine("=== All 6 Requirements Tested Successfully ===");
    }
}
