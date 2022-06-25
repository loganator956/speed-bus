using System;

namespace SpeedBus.Gameplay
{
    [Serializable]
    public class BusStopNotFoundException : Exception
    {
        public BusStopNotFoundException() { }
        public BusStopNotFoundException(string message) : base(message) { }
        public BusStopNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected BusStopNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
