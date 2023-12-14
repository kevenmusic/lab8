using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary8
{
    public class TransportException : Exception
    {
        public object Value;
        public TransportException() { }

        public TransportException(string message, object value1, object value2) : base(message) 
        {
            Value = new { Значение1 = value1, Значение2 = value2 };
        }

        public TransportException(string message, object value) : base(message)
        {
            Value = new { Значение = value};
        }
    }

    public class TransportTrainException : Exception
    {
        public object Value;
        public TransportTrainException() { }

        public TransportTrainException(string message, object value1, object value2) : base(message)
        {
            Value = new { Значение1 = value1, Значение2 = value2 };
        }

        public TransportTrainException(string message, object value) : base(message)
        {
            Value = new { Значение = value };
        }
    }

    public class TransportBusException : Exception
    {
        public object Value;
        public TransportBusException() { }

        public TransportBusException(string message, object value1, object value2) : base(message)
        {
            Value = new { Значение1 = value1, Значение2 = value2 };
        }

        public TransportBusException(string message, object value) : base(message)
        {
            Value = new { Значение = value };
        }
    }

    public class TransportAirplaneException : Exception
    {
        public object Value;
        public TransportAirplaneException() { }

        public TransportAirplaneException(string message, object value1, object value2) : base(message)
        {
            Value = new { Значение1 = value1, Значение2 = value2 };
        }

        public TransportAirplaneException(string message, object value) : base(message)
        {
            Value = new { Значение = value };
        }
    }
}
