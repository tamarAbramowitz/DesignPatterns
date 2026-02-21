public abstract class Operation
{
    protected Database _database { get; }

    public Operation(Database database)
    {
        _database = database;
    }

    public List<Row> Execute()
    {
        Validation();  
        return Execution();
    }

    protected abstract void Validation();
    
    protected abstract List<Row> Execution();

    
}
