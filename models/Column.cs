using System;


    public class Column
    {
        public string Name { get; private set; }
        public DataType Type { get; private set; }

        public Column(string name, DataType type)
        {
            Name = name;
            Type = type;
        }
    }

