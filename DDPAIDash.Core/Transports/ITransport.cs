using System;
using Windows.Networking;

namespace DDPAIDash.Core.Transports
{
    internal interface ITransport : IDisposable
    {
        string SessionId { get; set; }

        void Connect(HostName address, int port);

        ResponseMessage Execute(string requestSession);
        ResponseMessage Execute(string requestCertificate, string v);
        void Disconnect();
    }
}
