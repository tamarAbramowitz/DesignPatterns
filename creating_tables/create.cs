public class CreateTable
{
    private string name { get; set; }
    private List<Column> columns { get; set;}

    

    public CreateTable(string name, List<Column> columns)
    {
        this.name = name;
        this.columns = new List<Column>(columns);
        
    }

}
    

    
