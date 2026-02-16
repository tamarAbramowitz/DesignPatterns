using System;

using System;

public class SimpleCondition : ICondition
{
    private string _columnName;
    private string _operator;
    private object _value;

    public SimpleCondition(string columnName, string op, object value)
    {
        _columnName = columnName;
        _operator = op;
        _value = value;
    }

    public bool Evaluate(Row row)
    {
        object rowValue = row.GetValue(_columnName);

        if (rowValue == null)
            return false;

        int comparison = Comparer<object>.Default.Compare(rowValue, _value);

        return _operator switch
        {
            "=" => Equals(rowValue, _value),
            "!=" => !Equals(rowValue, _value),
            ">" => comparison > 0,
            "<" => comparison < 0,
            ">=" => comparison >= 0,
            "<=" => comparison <= 0,
            _ => throw new InvalidOperationException("Invalid operator")
        };
    }
}

