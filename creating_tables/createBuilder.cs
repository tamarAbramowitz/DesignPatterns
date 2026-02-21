public class CreateBuilder
{
    private string _name = "default_table_name";
    private List<Column> _columns = new List<Column>();



    public CreateBuilder SetName(string name)
    {
        _name = name;
        return this;
    }

    public CreateBuilder AddColumn(Column column)
    {
        _columns.Add(column);
        return this;
    }


    public CreateTable Build()
    {
        return new CreateTable(_name, _columns);
    }
    
}
