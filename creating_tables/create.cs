public class CreateTable
{
    private string name { get; set; }
    private List<Column> columns { get; set;}

    public CreateTable(string name, List<Column> columns)
    {
        this.name = name;
        this.columns = new List<Column>(columns);
    }

    public Table Execute(Database database)
    {
        Schema schema = new Schema();
        foreach (var column in columns)
        {
            schema.AddColumn(column.Name, column.Type);
        }
        return new Table(name, schema);
    }
}
