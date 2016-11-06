using System.ServiceModel;

namespace Microsoft.ServiceBus.Samples
{
    // Define a service contract.

    [ServiceContract(Name = "Calculator", Namespace = "http://Microsoft.ServiceModel.Samples")]
    public interface ICalculator
    {
        [OperationContract(Action = "Add", ReplyAction = "AddResponse")]
        double Add(double n1, double n2);

        [OperationContract(Action = "Subtract", ReplyAction = "SubtractResponse")]
        double Subtract(double n1, double n2);

        [OperationContract(Action = "Multiply", ReplyAction = "MultiplyResponse")]
        double Multiply(double n1, double n2);

        [OperationContract(Action = "Divide", ReplyAction = "DivideResponse")]
        double Divide(double n1, double n2);
    }


    public interface ICalculatorChannel : ICalculator, IClientChannel { }


    // Service class which implements the service contract.
    public class CalculatorService : ICalculator
    {
        public double Add(double n1, double n2)
        {
            return n1 + n2;
        }

        public double Subtract(double n1, double n2)
        {
            return n1 - n2;
        }

        public double Multiply(double n1, double n2)
        {
            return n1 * n2;

        }

        public double Divide(double n1, double n2)
        {
            return n1 / n2;
        }
    }
}

