namespace DDPAIDash.Core
{
    public class DeviceException : System.Exception
    {
        public DeviceException() { }
        public DeviceException(string message) : base(message) { }
        public DeviceException(string message, System.Exception inner) : base(message, inner) { }
    }
}
