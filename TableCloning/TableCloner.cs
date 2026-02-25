using System.Collections.Generic;

public static class TableCloner
{
    public static Table Clone(Table original)
    {
        Schema clonedSchema = CloneSchema(original.Schema);
        Table clonedTable = new Table(original.Name, clonedSchema);
        
        foreach (var row in original.Rows)
        {
            Row clonedRow = CloneRow(row);
            clonedTable.AddRow(clonedRow);
        }
        //DataChangePublisher publisher = new DataChangePublisher();
        //publisher.PublishChange($"table cloned : {original.Name}");
        return clonedTable;
    }

    private static Schema CloneSchema(Schema original)
    {
        Schema cloned = new Schema();
        foreach (var column in original.Columns)
        {
            cloned.AddColumn(column.Name, column.Type);
        }
        return cloned;
    }

    private static Row CloneRow(Row original)
    {
        Row cloned = new Row();
        foreach (var kvp in original.Values)
        {
            cloned.SetValue(kvp.Key, CloneValue(kvp.Value));
        }
        return cloned;
    }

    private static object CloneValue(object value)
    {
        if (value == null)
            return null;

        if (value is string || value.GetType().IsValueType)
            return value;

        if (value is System.ICloneable cloneable)
            return cloneable.Clone();

        return value;
    }
}
