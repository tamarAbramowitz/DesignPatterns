using System;

public class ComparisonExpression : IExpression
{
    private string _columnName;
    private ComparisonOperator _operator;
    private object _value;

    public ComparisonExpression(string columnName,
                                ComparisonOperator op,
                                object value)
    {
        _columnName = columnName;
        _operator = op;
        _value = value;
    }

    public bool Interpret(Row context)
    {
        object rowValue = context.GetValue(_columnName);

        if (rowValue == null)
            return false;

        int comparison = Comparer<object>.Default.Compare(rowValue, _value);

        return _operator switch
        {
            ComparisonOperator.Equal => Equals(rowValue, _value),
            ComparisonOperator.NotEqual => !Equals(rowValue, _value),
            ComparisonOperator.GreaterThan => comparison > 0,
            ComparisonOperator.LessThan => comparison < 0,
            ComparisonOperator.GreaterOrEqual => comparison >= 0,
            ComparisonOperator.LessOrEqual => comparison <= 0,
            _ => false
        };
    }
}
