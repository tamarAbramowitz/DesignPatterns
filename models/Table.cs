using System.Collections.Generic;


    public class Table
    {
        public string Name { get; set; }
        public Schema Schema { get; private set; }
        public List<Row> Rows { get; private set; }

        public Table(string name, Schema schema)
        {
            Name = name;
            Schema = schema;
            Rows = new List<Row>();
        }

        public void AddRow(Row row)
        {
            // כאן בעתיד נוכל להוסיף וולידציה שהשורה מתאימה לסכימה
            Rows.Add(row);
        }
    }
