public class AndExpression : IExpression
{
    private IExpression _left;
    private IExpression _right;

    public AndExpression(IExpression left, IExpression right)
    {
        _left = left;
        _right = right;
    }

    public bool Interpret(Row context)
    {
        return _left.Interpret(context) && _right.Interpret(context);
    }
}
