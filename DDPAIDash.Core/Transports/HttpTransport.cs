using System;
using System.Net;
using System.Net.Http;
using Windows.Networking;

namespace DDPAIDash.Core.Transports
{
    internal class HttpTransport : ITransport
    {
        private readonly HttpClientHandler _httpClientHandler;
        private readonly CookieContainer _cookieContainer;
        private readonly HttpClient _httpClient;

        public HttpTransport()
        {
            _cookieContainer = new CookieContainer();
            _httpClientHandler = new HttpClientHandler() { CookieContainer = _cookieContainer };
            _httpClient = new HttpClient();
        }
         
        public string SessionId
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public void Connect(HostName address, int port)
        {
            _httpClient.BaseAddress = new Uri(string.Format("http://{0}:{1}", address.ToString(), port));
            _cookieContainer.Add(_httpClient.BaseAddress, new Cookie("SessionID", "\r\n"));
        }

        public void Disconnect()
        {
        }

        public ResponseMessage Execute(string requestSession)
        {
            throw new NotImplementedException();
        }

        public ResponseMessage Execute(string requestCertificate, string v)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
        }
    }
}
