using System;

using System.Collections.Generic;

public class OrCondition : ICondition
{
    private List<ICondition> _conditions;

    public OrCondition()
    {
        _conditions = new List<ICondition>();
    }

    public void AddCondition(ICondition condition)
    {
        _conditions.Add(condition);
    }

    public bool Evaluate(Row row)
    {
        foreach (var condition in _conditions)
        {
            if (condition.Evaluate(row))
                return true;
        }

        return false;
    }
}
