using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary8
{
    public class TransportException : Exception
    {
        public TransportException() { }

        public TransportException(string message) : base(message) { }
    }

    public class TransportTrainException : Exception
    {
        public TransportTrainException() { }

        public TransportTrainException(string message) : base(message) { }
    }

    public class TransportBusException : Exception
    {
        public TransportBusException() { }

        public TransportBusException(string message) : base(message) { }
    }

    public class TransportAirplaneException : Exception
    {
        public TransportAirplaneException() { }

        public TransportAirplaneException(string message) : base(message) { }
    }
}
