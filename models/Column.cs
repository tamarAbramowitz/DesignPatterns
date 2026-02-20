using System;


    public class Column
    {
        public string Name { get; set; }
        public DataType Type { get; set; }

        public Column(string name, DataType type)
        {
            Name = name;
            Type = type;
        }
    }

