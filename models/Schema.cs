using System.Collections.Generic;


    public class Schema
    {
        public List<Column> Columns { get; private set; }

        public Schema()
        {
            Columns = new List<Column>();
        }

        public void AddColumn(string name, DataType type)
        {
            Columns.Add(new Column(name, type));
        }
    }
