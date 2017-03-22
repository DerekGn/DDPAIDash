/**
* MIT License
*
* Copyright (c) 2016 Derek Goslin < http://corememorydump.blogspot.ie/ >
*
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in all
* copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
* SOFTWARE.
*/

using System;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using DDPAIDash.Core.Constants;
using System.Threading.Tasks;
using System.Threading;

namespace DDPAIDash.Core.Transports
{
    internal class HttpTransport : ITransport
    {
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private readonly CookieContainer _cookieContainer;
        private readonly HttpClientHandler _httpClientHandler;
        private readonly HttpClient _httpClient;
        private string _sessionId;

        public HttpTransport()
        {
            _cookieContainer = new CookieContainer();
            _httpClientHandler = new HttpClientHandler { CookieContainer = _cookieContainer };
            _httpClient = new HttpClient();
        }

        public string SessionId
        {
            get { return _sessionId; }

            set
            {
                _sessionId = value;
                _cookieContainer.Add(_httpClient.BaseAddress, new Cookie("SessionID", _sessionId));
                _httpClient.DefaultRequestHeaders.Add("sessionid", _sessionId);
            }
        }

        public Uri BaseAddress => _httpClient.BaseAddress;

        public void Open(string address, int port)
        {
            _httpClient.Timeout = new TimeSpan(0, 0, 5);
            _httpClient.BaseAddress = new Uri($"http://{address}:{port}");
            _httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
        }

        public void Close()
        {
        }

        public Task<ResponseMessage> ExecuteAsync(string apiCommand)
        {
            return ExecuteInternalAsync(apiCommand, null);
        }

        public Task<ResponseMessage> ExecuteAsync(string apiCommand, string payload)
        {
            var sc = new StringContent(payload, Encoding.UTF8, "application/x-www-form-urlencoded");

            return ExecuteInternalAsync(apiCommand, sc);
        }

        public async Task<Stream> GetFileAsync(string fileName)
        {
            await _semaphore.WaitAsync();

            try
            {
                return await _httpClient.GetStreamAsync(fileName);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private async Task<ResponseMessage> ExecuteInternalAsync(string apiCommand, HttpContent content)
        {
            HttpResponseMessage postResult;

            await _semaphore.WaitAsync();

            try
            {
                postResult = await _httpClient.PostAsync($"/vcam/cmd.cgi?cmd={apiCommand}", content);
            }
            finally
            {
                _semaphore.Release();
            }

            string payload;

            if (postResult.IsSuccessStatusCode)
            {
                payload = await postResult.Content.ReadAsStringAsync();

                if (apiCommand == ApiConstants.AvCapReq)
                {
                    payload = string.Concat(
                        payload.Substring(0, 20),
                        "\"",
                        payload.Substring(20)
                        .Replace("\"", "\\\"")
                        .Replace("}}", "}\"}"));
                }
            }
            else
            {
                throw new HttpTransportException(
                    $"Error occured sending request to Device: [{_httpClient.BaseAddress}] Result: [{postResult}]");
            }

            return JsonConvert.DeserializeObject<ResponseMessage>(payload);
        }
        
        #region IDisposable Support

        private bool _disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                _disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~HttpTransport() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}