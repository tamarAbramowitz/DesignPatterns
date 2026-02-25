

using System.Data.Common;
using System.Windows.Input;

namespace project.supportedOperations
{
    public class Invoker
    {
        private Operation _operation;

        public void SetOperation(Operation operation)
        {
            _operation = operation;
        }

        public void activate()
        {
            _operation.Execute();
        }

    }
}
