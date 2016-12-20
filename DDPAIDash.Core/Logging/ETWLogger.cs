
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
using Windows.Foundation.Diagnostics;
using Windows.ApplicationModel.Core;

namespace DDPAIDash.Core.Logging
{
    internal class EtwLogger : ILogger
    {
        private static readonly Lazy<ILogger> InstanceLazy = new Lazy<ILogger>(() => new EtwLogger());

        private FileLoggingSession _filelogSession;
        private LoggingChannel _logChannel;
        
        private EtwLogger()
        {
            InitiateLogger();
        }

        public static ILogger Instance => InstanceLazy.Value;

        private void InitiateLogger()
        {
            _filelogSession = new FileLoggingSession("DDPaiDashSession");
            _logChannel = new LoggingChannel("DDPaiDashChannel");
            _filelogSession.AddLoggingChannel(_logChannel);

            CoreApplication.UnhandledErrorDetected += CoreApplication_UnhandledErrorDetected;
        }
                
        private void CoreApplication_UnhandledErrorDetected(object sender, UnhandledErrorDetectedEventArgs e)
        {
            try
            {
                _logChannel.LogMessage("Caught and unhandled exception");
                e.UnhandledError.Propagate();
            }
            catch (Exception ex)
            {
                _logChannel.LogMessage(string.Format("Exception Message: {0}", ex.Message));
            }
        }

        public void Info(string message)
        {
            _logChannel.LogMessage(message, LoggingLevel.Information);
        }

        public void Verbose(string message)
        {
            _logChannel.LogMessage(message, LoggingLevel.Verbose);
        }

        public void Error(string message)
        {
            _logChannel.LogMessage(message, LoggingLevel.Error);
        }

        public void Critical(string message, Exception exception)
        {
            _logChannel.LogMessage(string.Format("Message: {0} Exception: {1}", message, exception), LoggingLevel.Critical);
        }
    }
}
